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
    using System;
    using System.Collections.Generic;

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

        private static Random Random;

        #endregion

        #region Members

        private readonly Dictionary<string, double> variables;
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
            this.variables = new Dictionary<string, double>();

            if (Interpreter.Random == null)
            {
                Interpreter.Random = new Random();
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
            for (int idx = 0; idx < infix.Length; ++idx)
            {
                if (infix[idx] == Interpreter.VarPrefixChar)
                {
                    idx = Interpreter.SkipString(infix, idx + 2);
                }
                else if (Interpreter.IsAlpha(infix[idx]))
                {
                    infix = infix.Insert(idx, Interpreter.LongOpMark0Str);
                    idx = Interpreter.SkipString(infix, idx + 2);
                    infix = infix.Insert(idx, Interpreter.LongOpMark1Str);
                }
            }

            // Add blank spaces where needed.
            for (int idx = 0; idx < infix.Length; ++idx)
            {
                if (Interpreter.operators.ContainsKey(infix[idx].ToString()) || infix[idx] == Interpreter.OpMarkChar
                    || infix[idx] == Interpreter.OpenBracketChar || infix[idx] == Interpreter.ClosingBracketChar)
                {
                    if (idx != 0 && infix[idx - 1] != Interpreter.WhiteSpaceChar)
                    {
                        infix = infix.Insert(idx, Interpreter.WhiteSpaceStr);
                    }

                    // Handle long operators.
                    int jdx = idx;
                    if (infix[idx] == Interpreter.OpMarkChar)
                    {
                        jdx = infix.IndexOf(Interpreter.OpMarkChar, idx + 1);
                    }

                    if (jdx != infix.Length - 1 && infix[jdx + 1] != Interpreter.OpMarkChar)
                    {
                        infix = infix.Insert(jdx + 1, Interpreter.WhiteSpaceStr);
                    }

                    idx = jdx;
                }
            }

            // Trim long op mark and white spaces.
            infix = System.Text.RegularExpressions.Regex.Replace(infix.Replace(Interpreter.OpMarkStr, string.Empty), @"\s+", " ");

            string[] tokens = infix.Split(Interpreter.WhiteSpaceChar);
            List<string> list = new List<string>();     //TODO: static
            Stack<string> stack = new Stack<string>();  //TODO: static

            foreach (string token in tokens)
            {
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
            Stack<double> values = new Stack<double>();

            foreach (string token in tokens)
            {
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
                    else if (this.interpreterContext.TryGetVariable(token.TrimStart(Interpreter.VarPrefixChar), out value))
                    {
                        values.Push(value);
                    }
                    else
                    {
                        throw new InvalidOperationException(string.Format("Error parsing '{0}'", token));
                    }
                }
            }

            if (values.Count != 1)
            {
                throw new InvalidOperationException("Cannot calculate formula");
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
