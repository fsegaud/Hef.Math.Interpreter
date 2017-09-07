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
    public partial class Interpreter : System.IDisposable
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
        private const char   CommaSeparatorChar = ',';

        #endregion

        #region Static

        private static Hef.Collection.Cache<string, Node> cache;
        private static System.Collections.Generic.Dictionary<string, double> globalVariables;
        private static System.Random Random;

        #endregion

        #region Members

        private System.Collections.Generic.Dictionary<string, double> variables;
        private System.Collections.Generic.Dictionary<string, IInterpreterContext> namedContext;
        private bool disposed = false;

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

        static Interpreter()
        {
            Interpreter.cache = new Hef.Collection.Cache<string, Node>(64);
            Interpreter.globalVariables = new System.Collections.Generic.Dictionary<string, double>();
            Interpreter.Random = new System.Random();

            Interpreter.LoadOperators();
        }

        /// <summary>
        /// Instantiates a new instance of Interpreter.
        /// </summary>
        public Interpreter()
        {
            this.variables = new System.Collections.Generic.Dictionary<string, double>();
            this.namedContext = new System.Collections.Generic.Dictionary<string, IInterpreterContext>();
        }

        #endregion

        #region Destructor

        /// <summary>
        /// Destructor.
        /// </summary>
        ~Interpreter()
        {
            this.Dispose(false);
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
        /// Sets a variable to be used in the formula. This variable will be global to ALL interpreters.
        /// </summary>
        /// <param name="name">The variable name.</param>
        /// <param name="value">The variable value.</param>
        public static void SetGlobalVar(string name, double value)
        {
            if (!Interpreter.globalVariables.ContainsKey(name))
            {
                name = name.StartsWith(Interpreter.VarPrefixStr) ? name : string.Format("{0}{1}", Interpreter.VarPrefixStr, name);
                Interpreter.globalVariables.Add(name, value);
            }
        }

        /// <summary>
        /// Sets an interpreter context to be use un variables resolution.
        /// </summary>
        /// <param name="name">The name of the context..</param>
        /// <param name="interpreterContext">An object that implements Hef.Math.IInterpreterContext. Null to re;ove context.</param>
        public void SetContext(string name, IInterpreterContext interpreterContext)
        {
            if (interpreterContext == null)
            {
                this.namedContext.Remove(name);
            }
            else
            {
                this.namedContext.Add(name, interpreterContext);
            }
        }

        /// <summary>
        /// Compute the formula passed as argument.
        /// </summary>
        /// <param name="infix">The formula to resolve.</param>
        /// <returns></returns>
        public double Calculate(string infix)
        {
            Node root = Interpreter.cache.GetOrInitializeValue(infix, Interpreter.InfixToNode);
            return root.GetValue(this);
        }

        /// <summary>
        /// Forces the cache to be cleard.
        /// </summary>
        public static void ForceClearCache()
        {
            Interpreter.cache.Clear();
        }

        /// <summary>
        /// Dispose this instance of Interpreter.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            System.GC.SuppressFinalize(this);
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

            return Interpreter.operators[b].Priority - Interpreter.operators[a].Priority;
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
            // Also alow dots for names context cariable access `$xxx.yyy`.
            // [#12] Operators with names containing digits fail -> Fixed by adding IsNumeric().
            while (index < value.Length && (Interpreter.IsAlpha(value[index]) || Interpreter.IsNumeric(value[index]) || value[index] == '.'))
            {
                ++index;
            }

            return index;
        }

        private static bool IsSpecial(char c)
        {
            return !IsNumeric(c) && !IsAlpha(c) && c != OpenBracketChar && c != ClosingBracketChar && c != VarPrefixChar && c != WhiteSpaceChar && c != '.' && c != '±';
        }

        private static int SkipSpecial(string value, int index)
        {
            while (index < value.Length && IsSpecial(value[index]))
            {
                ++index;
            }

            return index;
        }

        private static string InfixToRpn(string infix)
        {
            // Replace comma separator with white space for function-like use of operators.
            infix = infix.Replace(Interpreter.CommaSeparatorChar, Interpreter.WhiteSpaceChar);

            // Add operator markers.
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
                else if (Interpreter.IsSpecial(infix[index]))
                {
                    infix = infix.Insert(index, Interpreter.LongOpMark0Str);
                    index = Interpreter.SkipSpecial(infix, index + 2);
                    infix = infix.Insert(index, Interpreter.LongOpMark1Str);
                }
            }

            // Add blank spaces where needed.
            for (int index = 0; index < infix.Length; ++index)
            {
                if (Interpreter.operators.ContainsKey(infix[index].ToString()) || infix[index] == Interpreter.VarPrefixChar || infix[index] == Interpreter.OpMarkChar
                    || infix[index] == Interpreter.OpenBracketChar || infix[index] == Interpreter.ClosingBracketChar)
                {
                    // Ignore variable. It would be a mess to find an operator in the middle of a variable name...
                    if (infix[index] == Interpreter.VarPrefixChar)
                    {
                        index = Interpreter.SkipString(infix, index + 2);
                        //continue;
                    }

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
            infix = infix.TrimStart(WhiteSpaceChar);
            infix = infix.TrimEnd(WhiteSpaceChar);

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

        private static Node RpnToNode(string rpn)
        {
            string[] tokens = rpn.Split(Interpreter.WhiteSpaceChar);
            System.Collections.Generic.Stack<Node> values = new System.Collections.Generic.Stack<Node>();

            for (int tokenIndex = 0; tokenIndex < tokens.Length; ++tokenIndex)
            {
                string token = tokens[tokenIndex];

                if (Interpreter.operators.ContainsKey(token))
                {
                    OperatorDescriptor opeDesc = Interpreter.operators[token];

                    if (opeDesc.NodeType != null)
                    {
                        if (opeDesc.NodeType.IsSubclassOf(typeof (ZeroNode)))
                        {
                            System.Reflection.ConstructorInfo constructorInfo = opeDesc.NodeType.GetConstructor(new System.Type[0]);
                            if (constructorInfo != null)
                            {
                                Node node = (Node) constructorInfo.Invoke(new object[0]);
                                values.Push(node);
                            }
                        }

                        if (opeDesc.NodeType.IsSubclassOf(typeof (UnaryNode)))
                        {
                            System.Reflection.ConstructorInfo constructorInfo = opeDesc.NodeType.GetConstructor(new [] {typeof (Node)});
                            if (constructorInfo != null)
                            {
                                Node node = (Node) constructorInfo.Invoke(new object[] {values.Pop()});
                                values.Push(node);
                            }
                        }

                        if (opeDesc.NodeType.IsSubclassOf(typeof (BinaryNode)))
                        {
                            System.Reflection.ConstructorInfo constructorInfo = opeDesc.NodeType.GetConstructor(new [] {typeof (Node), typeof (Node)});
                            if (constructorInfo != null)
                            {
                                Node right = values.Pop();
                                Node left = values.Pop();
                                Node node = (Node) constructorInfo.Invoke(new object[] {left, right});
                                values.Push(node);
                            }
                        }
                    }
                }
                else
                {
                    double value;
                    if (double.TryParse(token, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out value))
                    {
                        Node node = new ValueNode(value);
                        values.Push(node);
                    }
                    else
                    {
                        Node node = new VarNode(token);
                        values.Push(node);
                    }
                }
            }

            if (values.Count != 1)
            {
                throw new System.InvalidOperationException("Cannot calculate formula");
            }

            return values.Pop();
        }

        private static Node InfixToNode(string infix)
        {
            return Interpreter.RpnToNode(Interpreter.InfixToRpn(infix));
        }

        private static void LoadOperators()
        {
            System.Type nodeType = typeof(Node);
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(nodeType);
            System.Type[] allTypes = assembly.GetTypes();

            for (int i = 0; i < allTypes.Length; i++)
            {
                System.Type type = allTypes[i];
                if (type.IsSubclassOf(nodeType) && !type.IsAbstract)
                {
                    OperatorAttribute[] attributes = (OperatorAttribute[])type.GetCustomAttributes(typeof(OperatorAttribute), true);
                    if (attributes != null)
                    {
                        for (int attrIndex = 0; attrIndex < attributes.Length; attrIndex++)
                        {
                            OperatorAttribute operatorAttribute = attributes[attrIndex];
                            Interpreter.operators.Add(operatorAttribute.Symbol, new OperatorDescriptor(operatorAttribute.Priority, type));
                        }
                    }
                }
            }
        }

        private bool TryGetVariableValue(string varName, out double value)
        {
            // Look in local variables.
            if (this.variables.TryGetValue(varName, out value))
            {
                return true;
            }

            // Look in named contexts.
            // Fixed #23 (bad regex).
            if (System.Text.RegularExpressions.Regex.IsMatch(varName, @"\$\w+\.\w+"))
            {
                string contextName = varName.Substring(varName.IndexOf('$') + 1, varName.IndexOf('.') - 1);
                string variableName = varName.Substring(varName.IndexOf('.') + 1);

                if (this.namedContext.ContainsKey(contextName) &&
                    this.namedContext[contextName].TryGetVariable(variableName, out value))
                {
                    return true;
                }
            }

            // Look in global variables;
            if (Interpreter.globalVariables.TryGetValue(varName, out value))
            {
                return true;
            }

            return false;
        }

        private void Dispose(bool disposing)
        {
            if (this.disposed)
            {
                return;
            }

            if (disposing)
            {
                if (this.variables != null)
                {
                    this.variables.Clear();
                    this.variables = null;
                }

                if (this.namedContext != null)
                {
                    this.namedContext.Clear();
                    this.namedContext = null;
                }
            }

            this.disposed = true;
        }

        #endregion

        #region Inner Types

        private struct OperatorDescriptor
        {
            public readonly int Priority;
            public readonly System.Type NodeType;

            public OperatorDescriptor(int priority, System.Type nodeType)
            {
                Priority = priority;
                NodeType = nodeType;
            }
        }

        #endregion
    }
}
