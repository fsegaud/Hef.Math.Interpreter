namespace Hef.Math
{
    using System;
    using System.Collections.Generic;

    public class Interpreter
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

        private static readonly Dictionary<string, OperatorDescriptor> operators = new Dictionary<string, OperatorDescriptor>
        {
            {"+",       new OperatorDescriptor(Operator.Add,   OperatorType.Binary,    2) },
            {"-",       new OperatorDescriptor(Operator.Sub,   OperatorType.Binary,    2) },
            {"*",       new OperatorDescriptor(Operator.Mult,  OperatorType.Binary,    5) },
            {"/",       new OperatorDescriptor(Operator.Div,   OperatorType.Binary,    5) },
            {"%",       new OperatorDescriptor(Operator.Mod,   OperatorType.Binary,    10)},
            {"^",       new OperatorDescriptor(Operator.Pow,   OperatorType.Binary,    15)},
            {"sqrt",    new OperatorDescriptor(Operator.Sqrt,  OperatorType.Unary,     15)},
            {"cos",     new OperatorDescriptor(Operator.Cos,   OperatorType.Unary,     12)},
            {"sin",     new OperatorDescriptor(Operator.Sin,   OperatorType.Unary,     12)},
            {"abs",     new OperatorDescriptor(Operator.Abs,   OperatorType.Unary,     8) },
            {"round",   new OperatorDescriptor(Operator.Round, OperatorType.Unary,     8) },
            {"!",       new OperatorDescriptor(Operator.Neg,   OperatorType.Unary,     50)},
            {"pi",      new OperatorDescriptor(Operator.PI,    OperatorType.Const,     90)},
            {"min",     new OperatorDescriptor(Operator.Min,   OperatorType.Binary,    80)},
            {"max",     new OperatorDescriptor(Operator.Max,   OperatorType.Binary,    90)},
            {"==",      new OperatorDescriptor(Operator.Equal, OperatorType.Binary,    0) },
            {"eq",      new OperatorDescriptor(Operator.Equal, OperatorType.Binary,    0) },
            {"lt",      new OperatorDescriptor(Operator.LT,    OperatorType.Binary,    0) },
            {"lte",     new OperatorDescriptor(Operator.LTE,   OperatorType.Binary,    0) },
            {"gt",      new OperatorDescriptor(Operator.GT,    OperatorType.Binary,    0) },
            {"gte",     new OperatorDescriptor(Operator.GTE,   OperatorType.Binary,    0) },
            {"rand",    new OperatorDescriptor(Operator.Rand,  OperatorType.Const,     90)},
            {"d",       new OperatorDescriptor(Operator.Dice,  OperatorType.Binary,    90)},
            {"D",       new OperatorDescriptor(Operator.Dice,  OperatorType.Binary,    90)}
        };

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

        private enum Operator
        {
            Add = 0,
            Sub,
            Mult,
            Div,
            Mod,
            Equal,
            Pow,
            Sqrt,
            Cos,
            Sin,
            Abs,
            Round,
            Neg,
            PI,
            Min,
            Max,
            LT,
            LTE,
            GT,
            GTE,
            Rand,
            Dice
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

        public void SetVar(string name, double value)
        {
            if (!this.variables.ContainsKey(name))
            {
                name = name.StartsWith(Interpreter.VarPrefixStr) ? name : string.Format("{0}{1}", Interpreter.VarPrefixStr, name);
                this.variables.Add(name, value);
            }
        }

        public void SetContext(IInterpreterContext interpreterContext)
        {
            this.interpreterContext = interpreterContext;
        }

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
        
        private static double ComputeOperation(double left, double right, Operator op)
        {
            switch (op)
            {
                case Operator.Add:
                    return left + right;

                case Operator.Sub:
                    return left - right;

                case Operator.Mult:
                    return left * right;

                case Operator.Div:
                    return left / right;

                case Operator.Mod:
                    return (int)left % (int)right;

                case Operator.Equal:
                    return Math.Abs(left - right) < double.Epsilon ? 1f : 0f;

                case Operator.Pow:
                    return Math.Pow(left, right);

                case Operator.Sqrt:
                    return Math.Sqrt(left);

                case Operator.Cos:
                    return Math.Cos(left);

                case Operator.Sin:
                    return Math.Sin(left);

                case Operator.Abs:
                    return Math.Abs(left);

                case Operator.Round:
                    return Math.Round(left);

                case Operator.Neg:
                    return -left;

                case Operator.PI:
                    return Math.PI;

                case Operator.Min:
                    return Math.Min(left, right);

                case Operator.Max:
                    return Math.Max(left, right);

                case Operator.LT:
                    return left < right ? 1d : 0d;

                case Operator.LTE:
                    return left <= right ? 1d : 0d;

                case Operator.GT:
                    return left > right ? 1d : 0d;

                case Operator.GTE:
                    return left >= right ? 1d : 0d;

                case Operator.Rand:
                    return Interpreter.Random.NextDouble();

                case Operator.Dice:
                    int value = 0;
                    for (int i = 0; i < left; ++i)
                        value += Interpreter.Random.Next(1, (int)right + 1);
                    return value;

                default:
                    throw new InvalidOperationException(string.Format("Operator '{0}' not supported.", op));
            }
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
                            left = values.Pop();
                            break;

                        case OperatorType.Binary:
                            right = values.Pop();
                            left = values.Pop();
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
