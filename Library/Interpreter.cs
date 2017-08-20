#region License
// Copyright(c) 2017 François Ségaud
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
#endregion

namespace Hef.Math
{
    /// <summary>
    /// An interpreter able to resolve a mathmatical formula.
    /// </summary>
    public partial class Interpreter
    {
        #region Constants

        private const string VarPrefixStr       = "$";
        private const char   VarPrefixChar      = '$';
        private const string OpMarkStr          = "_";
        private const char   OpMarkChar         = '_';
        private const string LongOpMark0Str     = " _";
        private const string LongOpMark1Str     = "_ ";
        private const string OpenBracketStr     = "(";
        private const string ClosingBracketStr  = ")";
        private const char   OpenBracketChar    = '(';
        private const char   ClosingBracketChar = ')';
        private const string WhiteSpaceStr      = " ";
        private const char   WhiteSpaceChar     = ' ';

        #endregion

        #region Static

        private static System.Random Random;

        #endregion

        #region Members

        private readonly System.Collections.Generic.Dictionary<string, double> variables;
        private IInterpreterContext interpreterContext;

        #endregion

        #region Enumerations

        private enum OperatorType
        {
            Const = 0,
            Unary,
            Binary
        }

        #endregion

        #region Constructors

        public Interpreter()
        {
            this.variables = new System.Collections.Generic.Dictionary<string, double>();

            if (Interpreter.Random == null)
            {
                Interpreter.Random = new System.Random();
            }
        }

        public Interpreter(IInterpreterContext interpreterContext)
            : this()
        {
            this.SetContext(interpreterContext);
        }

        #endregion

        #region Public Functions

        /// <summary>
        /// Sets a variable to be used in the formula.
        /// </summary>
        /// <param name="name">The variable name.</param>
        /// <param name="value">The variable value.</param>
        public void SetVar(string name, double value)
        {
            if (!this.variables.ContainsKey(name))
            {
                name = name.StartsWith(Interpreter.VarPrefixStr) ? name : string.Format("{0}{1}", Interpreter.VarPrefixStr, name);
                this.variables.Add(name, value);
            }
        }

        /// <summary>
        /// Sets an interpreter context to be use un variables resolution.
        /// </summary>
        /// <param name="interpreterContext">An object that implements Hef.Math.IInterpreterContext.</param>
        public void SetContext(IInterpreterContext interpreterContext)
        {
            this.interpreterContext = interpreterContext;
        }

        /// <summary>
        /// Compute the formula passed as argument.
        /// </summary>
        /// <param name="infix">The formula to resolve.</param>
        /// <returns></returns>
        public double Calculate(string infix)
        {
            return this.CalculateRpn(Interpreter.InfixToRpn(infix));
        }

        #endregion

        #region Private functions

        private static int ComparePrecedence(string a, string b)
        {
            if (!Interpreter.operators.ContainsKey(a))
            {
                throw new System.Exception(string.Format("Operator '{0}' is not registered.", a));
            }

            if (!Interpreter.operators.ContainsKey(b))
            {
                throw new System.Exception(string.Format("Operator '{0}' is not registered.", b));
            }

            return Interpreter.operators[a].Priority - Interpreter.operators[b].Priority;
        }

        private static bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z');
        }

        private static bool IsNumeric(char c)
        {
            return c >= '0' && c <= '9';
        }

        private static int SkipString(string value, int index)
        {
            while (index < value.Length && Interpreter.IsAlpha(value[index]))
            {
                ++index;
            }

            return index;
        }
        
        private static string InfixToRpn(string infix)
        {
            for (int index = 0; index < infix.Length; ++index)
            {
                if (infix[index] == Interpreter.VarPrefixChar)
                {
                    index = Interpreter.SkipString(infix, index + 2);
                }
                else if (Interpreter.IsAlpha(infix[index]))
                {
                    infix = infix.Insert(index, Interpreter.LongOpMark0Str);
                    index = Interpreter.SkipString(infix, index + 2);
                    infix = infix.Insert(index, Interpreter.LongOpMark1Str);
                }
            }

            // Add blank spaces where needed.
            for (int index = 0; index < infix.Length; ++index)
            {
                if (Interpreter.operators.ContainsKey(infix[index].ToString()) || infix[index] == Interpreter.OpMarkChar
                    || infix[index] == Interpreter.OpenBracketChar || infix[index] == Interpreter.ClosingBracketChar)
                {
                    if (index != 0 && infix[index - 1] != Interpreter.WhiteSpaceChar)
                    {
                        infix = infix.Insert(index, Interpreter.WhiteSpaceStr);
                    }

                    // Handle long operators.
                    int jndex = index;
                    if (infix[index] == Interpreter.OpMarkChar)
                    {
                        jndex = infix.IndexOf(Interpreter.OpMarkChar, index + 1);
                    }

                    if (jndex != infix.Length - 1 && infix[jndex + 1] != Interpreter.OpMarkChar)
                    {
                        infix = infix.Insert(jndex + 1, Interpreter.WhiteSpaceStr);
                    }

                    index = jndex;
                }
            }

            // Trim long op mark and white spaces.
            infix = System.Text.RegularExpressions.Regex.Replace(infix.Replace(Interpreter.OpMarkStr, string.Empty), @"\s+", " ");

            string[] tokens = infix.Split(Interpreter.WhiteSpaceChar);
            System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();     //TODO: static
            System.Collections.Generic.Stack<string> stack = new System.Collections.Generic.Stack<string>();  //TODO: static

            for (int tokenIndex = 0; tokenIndex < tokens.Length; ++tokenIndex)
            {
                string token = tokens[tokenIndex];

                if (string.IsNullOrEmpty(token) || token == Interpreter.WhiteSpaceStr)
                {
                    continue;
                }

                if (Interpreter.operators.ContainsKey(token))
                {
                    while (stack.Count > 0 && Interpreter.operators.ContainsKey(stack.Peek()))
                    {
                        if (ComparePrecedence(token, stack.Peek()) < 0)
                        {
                            list.Add(stack.Pop());
                            continue;
                        }

                        break;
                    }

                    stack.Push(token);
                }
                else if (token == Interpreter.OpenBracketStr)
                {
                    stack.Push(token);
                }
                else if (token == Interpreter.ClosingBracketStr)
                {
                    while (stack.Count > 0 && stack.Peek() != Interpreter.OpenBracketStr)
                    {
                        list.Add(stack.Pop());
                    }

                    stack.Pop();
                }
                else
                {
                    list.Add(token);
                }
            }

            while (stack.Count > 0)
            {
                list.Add(stack.Pop());
            }

            string rpn = string.Join(Interpreter.WhiteSpaceStr, list.ToArray());

            return rpn;
        }

        private double CalculateRpn(string rpn)
        {
            string[] tokens = rpn.Split(Interpreter.WhiteSpaceChar);
            System.Collections.Generic.Stack<double> values = new System.Collections.Generic.Stack<double>();

            for (int tokenIndex = 0; tokenIndex < tokens.Length; ++tokenIndex)
            {
                string token = tokens[tokenIndex];

                if (Interpreter.operators.ContainsKey(token))
                {
                    double right = 0d;
                    double left = 0d;

                    switch (Interpreter.operators[token].Type)
                    {
                        case OperatorType.Const:
                            break;

                        case OperatorType.Unary:
                            {
                                if (values.Count < 1)
                                {
                                    throw new System.InvalidOperationException(
                                        string.Format("Operator '{0}' ({1}) is unary.", token, Interpreter.operators[token].Operator));
                                }

                                left = values.Pop();
                            }

                            break;

                        case OperatorType.Binary:
                            {
                                if (values.Count < 2)
                                {
                                    throw new System.InvalidOperationException(
                                        string.Format("Operator '{0}' ({1}) is binary.", token, Interpreter.operators[token].Operator));
                                }

                                right = values.Pop();
                                left = values.Pop();
                            }

                            break;
                    }

                    values.Push(Interpreter.ComputeOperation(left, right, Interpreter.operators[token].Operator));
                }
                else
                {
                    double value;
                    if (double.TryParse(token, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out value))
                    {
                        values.Push(value);
                    }
                    else if (this.variables.TryGetValue(token, out value))
                    {
                        values.Push(value);
                    }
                    else if (this.interpreterContext != null &&
                        this.interpreterContext.TryGetVariable(token.TrimStart(Interpreter.VarPrefixChar), out value))
                    {
                        values.Push(value);
                    }
                    else
                    {
                        throw new System.InvalidOperationException(string.Format("Error parsing '{0}'", token));
                    }
                }
            }

            if (values.Count != 1)
            {
                throw new System.InvalidOperationException("Cannot calculate formula");
            }

            return values.Pop();
        }

        #endregion

        #region Inner Types

        private struct OperatorDescriptor
        {
            public readonly Operator Operator;
            public readonly OperatorType Type;
            public readonly int Priority;

            public OperatorDescriptor(Operator op, OperatorType type, int priority)
            {
                this.Operator = op;
                this.Type = type;
                this.Priority = priority;
            }
        }

        #endregion
    }
}
