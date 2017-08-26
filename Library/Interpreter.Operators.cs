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

using System;
using System.Configuration;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Windows.Forms;

namespace Hef.Math
{
    // TODO: Move interface somewhere else.
    public partial class Interpreter : IVariableProvider
    {
        #region Static

        private static readonly System.Collections.Generic.Dictionary<string, OperatorDescriptor> operators
            = new System.Collections.Generic.Dictionary<string, OperatorDescriptor>();
        /*{
            {"±",       new OperatorDescriptor(Operator.Sign,    OperatorType.Unary,     99, null)},
            {"+",       new OperatorDescriptor(Operator.Add,     OperatorType.Binary,    2, typeof(AddNode)) },
            {"-",       new OperatorDescriptor(Operator.Sub,     OperatorType.Binary,    2, null) },
            {"*",       new OperatorDescriptor(Operator.Mult,    OperatorType.Binary,    5, null) },
            {"/",       new OperatorDescriptor(Operator.Div,     OperatorType.Binary,    5, null) },
            {"%",       new OperatorDescriptor(Operator.Mod,     OperatorType.Binary,    10, null)},
            {"^",       new OperatorDescriptor(Operator.Pow,     OperatorType.Binary,    15, null)},
            {"sqrt",    new OperatorDescriptor(Operator.Sqrt,    OperatorType.Unary,     15, typeof(SqrtNode))},
            {"cos",     new OperatorDescriptor(Operator.Cos,     OperatorType.Unary,     12, null)},
            {"sin",     new OperatorDescriptor(Operator.Sin,     OperatorType.Unary,     12, null)},
            {"tan",     new OperatorDescriptor(Operator.Tan,     OperatorType.Unary,     12, null)},
            {"acos",    new OperatorDescriptor(Operator.Acos,    OperatorType.Unary,     12, null)},
            {"asin",    new OperatorDescriptor(Operator.Asin,    OperatorType.Unary,     12, null)},
            {"atan",    new OperatorDescriptor(Operator.Atan,    OperatorType.Unary,     12, null)},
            {"cosh",    new OperatorDescriptor(Operator.Cosh,    OperatorType.Unary,     12, null)},
            {"sinh",    new OperatorDescriptor(Operator.Sinh,    OperatorType.Unary,     12, null)},
            {"tanh",    new OperatorDescriptor(Operator.Tanh,    OperatorType.Unary,     12, null)},
            {"degrad",  new OperatorDescriptor(Operator.Deg2Rad, OperatorType.Unary,     13, null)},
            {"raddeg",  new OperatorDescriptor(Operator.Rad2Deg, OperatorType.Unary,     13, null)},
            {"abs",     new OperatorDescriptor(Operator.Abs,     OperatorType.Unary,     8, null) },
            {"round",   new OperatorDescriptor(Operator.Round,   OperatorType.Unary,     8, null) },
            {"!",       new OperatorDescriptor(Operator.Neg,     OperatorType.Unary,     50, null)},
            {"pi",      new OperatorDescriptor(Operator.PI,      OperatorType.Const,     90, typeof(PiNode))},
            {"min",     new OperatorDescriptor(Operator.Min,     OperatorType.Binary,    80, null)},
            {"max",     new OperatorDescriptor(Operator.Max,     OperatorType.Binary,    90, null)},
            {"==",      new OperatorDescriptor(Operator.Equal,   OperatorType.Binary,    0, null) },
            {"eq",      new OperatorDescriptor(Operator.Equal,   OperatorType.Binary,    0, null) },
            {"lt",      new OperatorDescriptor(Operator.LT,      OperatorType.Binary,    0, null) },
            {"lte",     new OperatorDescriptor(Operator.LTE,     OperatorType.Binary,    0, null) },
            {"gt",      new OperatorDescriptor(Operator.GT,      OperatorType.Binary,    0, null) },
            {"gte",     new OperatorDescriptor(Operator.GTE,     OperatorType.Binary,    0, null) },
            {"rand",    new OperatorDescriptor(Operator.Rand,    OperatorType.Const,     90, null)},
            {"d",       new OperatorDescriptor(Operator.Dice,    OperatorType.Binary,    90, null)},
            {"D",       new OperatorDescriptor(Operator.Dice,    OperatorType.Binary,    90, null)},
            {"true",    new OperatorDescriptor(Operator.True,    OperatorType.Const,     90, null)},
            {"false",   new OperatorDescriptor(Operator.False,   OperatorType.Const,     90, null)},
            {"&",       new OperatorDescriptor(Operator.And,     OperatorType.Binary,    0, null) },
            {"and",     new OperatorDescriptor(Operator.And,     OperatorType.Binary,    0, null) },
            {"|",       new OperatorDescriptor(Operator.Or,      OperatorType.Binary,    1, null) },
            {"or",      new OperatorDescriptor(Operator.Or,      OperatorType.Binary,    1, null) },

            // Add your own operator description here ...
        };*/

        #endregion

        #region Enumerations

        /*private enum Operator
        {
            Sign = 0,
            Add,
            Sub,
            Mult,
            Div,
            Mod,
            Equal,
            Pow,
            Sqrt,
            Cos,
            Sin,
            Tan,
            Cosh,
            Sinh,
            Tanh,
            Acos,
            Asin,
            Atan,
            Deg2Rad,
            Rad2Deg,
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
            Dice,
            True,
            False,
            And,
            Or

            // Add your own operator here ...
        }*/

        #endregion

        #region Functions

        /*private static double ComputeOperation(double left, double right, Operator op)
        {
            switch (op)
            {
                case Operator.Sign:
                    return -left;

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
                    return System.Math.Abs(left - right) < double.Epsilon ? TRUE : FALSE;

                case Operator.Pow:
                    return System.Math.Pow(left, right);

                case Operator.Sqrt:
                    return System.Math.Sqrt(left);

                case Operator.Cos:
                    return System.Math.Cos(left);

                case Operator.Sin:
                    return System.Math.Sin(left);

                case Operator.Tan:
                    return System.Math.Tan(left);

                case Operator.Acos:
                    return System.Math.Acos(left);

                case Operator.Asin:
                    return System.Math.Asin(left);

                case Operator.Atan:
                    return System.Math.Atan(left);

                case Operator.Cosh:
                    return System.Math.Cosh(left);

                case Operator.Sinh:
                    return System.Math.Sinh(left);

                case Operator.Tanh:
                    return System.Math.Tanh(left);

                case Operator.Deg2Rad:
                    return (left * System.Math.PI) / 180d;

                case Operator.Rad2Deg:
                    return (left * 180d) / System.Math.PI;

                case Operator.Abs:
                    return System.Math.Abs(left);

                case Operator.Round:
                    return System.Math.Round(left);

                case Operator.Neg:
                    //return -left;
                    return System.Math.Abs(left) < double.Epsilon ? TRUE : FALSE;

                case Operator.PI:
                    return System.Math.PI;

                case Operator.Min:
                    return System.Math.Min(left, right);

                case Operator.Max:
                    return System.Math.Max(left, right);

                case Operator.LT:
                    return left < right ? TRUE : FALSE;

                case Operator.LTE:
                    return left <= right ? TRUE : FALSE;

                case Operator.GT:
                    return left > right ? TRUE : FALSE;

                case Operator.GTE:
                    return left >= right ? TRUE : FALSE;

                case Operator.Rand:
                    return Interpreter.Random.NextDouble();

                case Operator.Dice:
                    {
                        int value = 0;
                        for (int i = 0; i < left; ++i)
                        {
                            value += Interpreter.Random.Next(1, (int)right + 1);
                        }

                        return value;
                    }

                case Operator.True:
                    return 1d;

                case Operator.False:
                    return 0d;

                case Operator.And:
                    return BoolToDouble(DoubleToBool(left) && DoubleToBool(right));

                case Operator.Or:
                    return BoolToDouble(DoubleToBool(left) || DoubleToBool(right));

                // Add your own operator computations here ...

                default:
                    throw new System.InvalidOperationException(string.Format("Operator '{0}' not supported.", op));
            }
        }*/

        #endregion

        private abstract class Node
        {
            public abstract double GetValue(IVariableProvider variableProvider);
        }

        private abstract class ZeroNode : Node
        {
        }

        private abstract class UnaryNode : Node
        {
            protected Node input;

            protected UnaryNode(Node input)
            {
                this.input = input;
            }
        }

        private abstract class BinaryNode : Node
        {
            protected Node leftInput;
            protected Node rightInput;

            protected BinaryNode(Node leftInput, Node rightInput)
            {
                this.leftInput = leftInput;
                this.rightInput = rightInput;
            }
        }

        private class ValueNode : ZeroNode
        {
            private double value;

            public ValueNode(double value)
            {
                this.value = value;
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return value;
            }
        }

        private class VarNode : ZeroNode
        { 
            private string varName;

            public VarNode(string varName)
            {
                this.varName = varName;
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                double value = 0;
                if (variableProvider.TryGerVariableValue(this.varName, out value))
                {
                    return value;
                }

                throw new System.Exception(string.Format("Could not parse variable '{0}'", this.varName));
            }
        }

        [Operator("pi", 99)]
        private class PiNode : ZeroNode
        {
            public override double GetValue(IVariableProvider variableProvider)
            {
                return System.Math.PI;
            }
        }

        [Operator("sqrt", 50)]
        private class SqrtNode : UnaryNode
        {
            public SqrtNode(Node input) 
                : base(input)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return System.Math.Sqrt(this.input.GetValue(variableProvider));
            }
        }

        [Operator("+", 2)]
        private class AddNode : BinaryNode
        {
            public AddNode(Node leftInput, Node rightInput) 
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return this.leftInput.GetValue(variableProvider) + this.rightInput.GetValue(variableProvider);
            }
        }

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
        internal class OperatorAttribute : System.Attribute
        {
            public string Symbol;
            public int Priority;

            public OperatorAttribute(string symbol, int priority)
            {
                Symbol = symbol;
                Priority = priority;
            }
        }

        bool IVariableProvider.TryGerVariableValue(string varName, out double value)
        {
            value = 0;
            if (this.variables.TryGetValue(varName, out value))
            {
                return true;
            }

            if (this.interpreterContext != null &&
                this.interpreterContext.TryGetVariable(varName.TrimStart(Interpreter.VarPrefixChar), out value))
            {
                return true;
            }

            if (System.Text.RegularExpressions.Regex.IsMatch(varName, @"\$\w+.\w+"))
            {
                string contextName = varName.Substring(varName.IndexOf('$') + 1, varName.IndexOf('.') - 1);
                string variableName = varName.Substring(varName.IndexOf('.') + 1);

                if (this.namedContext.ContainsKey(contextName) &&
                    this.namedContext[contextName].TryGetVariable(variableName, out value))
                {
                    return true;
                }
            }

            return false;
        }

        public static void TEST_DynamicLoad()
        {
            System.Type nodeType = typeof (Node);
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetAssembly(nodeType);
            System.Type[] allTypes = assembly.GetTypes();

            for (int i = 0; i < allTypes.Length; i++)
            {
                System.Type type = allTypes[i];
                if (type.IsSubclassOf(nodeType) && !type.IsAbstract)
                {
                    OperatorAttribute[] attributes = (OperatorAttribute[]) type.GetCustomAttributes(typeof (OperatorAttribute), true);
                    if (attributes != null)
                    {
                        for (int attrIndex = 0; attrIndex < attributes.Length; attrIndex++)
                        {
                            OperatorAttribute operatorAttribute = attributes[attrIndex];
                            System.Console.WriteLine("{1} {0} ({2})", type.FullName, operatorAttribute.Symbol, operatorAttribute.Priority);

                            Interpreter.operators.Add(operatorAttribute.Symbol, new OperatorDescriptor(operatorAttribute.Priority, type));
                        }
                    }
                }
            }
        }

        public void TEST_Rpn2Node()
        {
            Node root = RpnToNode(InfixToRpn("pi + 6 + sqrt 16 + $hundred"));
            Console.WriteLine(root.GetValue(this));
        }
    }

    internal interface IVariableProvider
    {
        bool TryGerVariableValue(string varName, out double value);
    }
}
