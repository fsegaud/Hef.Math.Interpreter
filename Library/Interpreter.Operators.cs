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
    // TODO: Move interface somewhere else.
    public partial class Interpreter : IVariableProvider
    {
        #region Static

        private static readonly System.Collections.Generic.Dictionary<string, OperatorDescriptor> operators
            = new System.Collections.Generic.Dictionary<string, OperatorDescriptor>();

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
        
        #region ZeroNode

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

        [Operator("pi", 90)]
        private class PiNode : ZeroNode
        {
            public override double GetValue(IVariableProvider variableProvider)
            {
                return System.Math.PI;
            }
        }

        [Operator("rand", 90)]
        private class RandNode : ZeroNode
        {
            public override double GetValue(IVariableProvider variableProvider)
            {
                return Interpreter.Random.NextDouble();
            }
        }

        [Operator("true", 90)]
        private class TrueNode : ZeroNode
        {
            public override double GetValue(IVariableProvider variableProvider)
            {
                return TRUE;
            }
        }

        [Operator("false", 90)]
        private class FalseNode : ZeroNode
        {
            public override double GetValue(IVariableProvider variableProvider)
            {
                return FALSE;
            }
        }

        #endregion

        #region UnaryNode

        [Operator("±", 99)]
        private class SignNode : UnaryNode
        {
            public SignNode(Node input) : 
                base(input)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return -this.input.GetValue(variableProvider);
            }
        }

        [Operator("sqrt", 15)]
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

        [Operator("cos", 12)]
        private class CosNode : UnaryNode
        {
            public CosNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return System.Math.Cos(this.input.GetValue(variableProvider));
            }
        }

        [Operator("sin", 12)]
        private class SinNode : UnaryNode
        {
            public SinNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return System.Math.Sin(this.input.GetValue(variableProvider));
            }
        }

        [Operator("tan", 12)]
        private class TanNode : UnaryNode
        {
            public TanNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return System.Math.Tan(this.input.GetValue(variableProvider));
            }
        }

        [Operator("acos", 12)]
        private class AcosNode : UnaryNode
        {
            public AcosNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return System.Math.Acos(this.input.GetValue(variableProvider));
            }
        }

        [Operator("asin", 12)]
        private class AsinNode : UnaryNode
        {
            public AsinNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return System.Math.Asin(this.input.GetValue(variableProvider));
            }
        }

        [Operator("atan", 12)]
        private class AtanNode : UnaryNode
        {
            public AtanNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return System.Math.Atan(this.input.GetValue(variableProvider));
            }
        }

        [Operator("cosh", 12)]
        private class CoshNode : UnaryNode
        {
            public CoshNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return System.Math.Cosh(this.input.GetValue(variableProvider));
            }
        }

        [Operator("sinh", 12)]
        private class SinhNode : UnaryNode
        {
            public SinhNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return System.Math.Sinh(this.input.GetValue(variableProvider));
            }
        }

        [Operator("tanh", 12)]
        private class TanhNode : UnaryNode
        {
            public TanhNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return System.Math.Tanh(this.input.GetValue(variableProvider));
            }
        }

        [Operator("degrad", 13)]
        private class Deg2RadNode : UnaryNode
        {
            public Deg2RadNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return (this.input.GetValue(variableProvider) * System.Math.PI) / 180d;
            }
        }

        [Operator("raddeg", 13)]
        private class Rad2DegNode : UnaryNode
        {
            public Rad2DegNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return (this.input.GetValue(variableProvider) * 180d) / System.Math.PI;
            }
        }

        [Operator("abs", 8)]
        private class AbsNode : UnaryNode
        {
            public AbsNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return System.Math.Abs(this.input.GetValue(variableProvider));
            }
        }

        [Operator("round", 8)]
        private class RoundNode : UnaryNode
        {
            public RoundNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return System.Math.Round(this.input.GetValue(variableProvider));
            }
        }

        [Operator("!", 50)]
        private class NegNode : UnaryNode
        {
            public NegNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return System.Math.Abs(this.input.GetValue(variableProvider)) < double.Epsilon ? TRUE : FALSE;
            }
        }

        #endregion

        #region  BinaryNode

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

        [Operator("-", 2)]
        private class SubNode : BinaryNode
        {
            public SubNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return this.leftInput.GetValue(variableProvider) - this.rightInput.GetValue(variableProvider);
            }
        }

        [Operator("*", 5)]
        private class MultNode : BinaryNode
        {
            public MultNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return this.leftInput.GetValue(variableProvider) * this.rightInput.GetValue(variableProvider);
            }
        }

        [Operator("/", 5)]
        private class DivNode : BinaryNode
        {
            public DivNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return this.leftInput.GetValue(variableProvider) / this.rightInput.GetValue(variableProvider);
            }
        }

        [Operator("%", 10)]
        private class ModNode : BinaryNode
        {
            public ModNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return (int)this.leftInput.GetValue(variableProvider) % (int)this.rightInput.GetValue(variableProvider);
            }
        }

        [Operator("^", 15)]
        private class PowNode : BinaryNode
        {
            public PowNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return System.Math.Pow(this.leftInput.GetValue(variableProvider), this.rightInput.GetValue(variableProvider));
            }
        }

        [Operator("min", 80)]
        private class MinNode : BinaryNode
        {
            public MinNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return System.Math.Min(this.leftInput.GetValue(variableProvider), this.rightInput.GetValue(variableProvider));
            }
        }

        [Operator("max", 90)]
        private class MaxNode : BinaryNode
        {
            public MaxNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return System.Math.Max(this.leftInput.GetValue(variableProvider), this.rightInput.GetValue(variableProvider));
            }
        }

        [Operator("==", 0)]
        [Operator("eq", 0)]
        private class EqualNode : BinaryNode
        {
            public EqualNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return System.Math.Abs(this.leftInput.GetValue(variableProvider) - this.rightInput.GetValue(variableProvider)) < double.Epsilon ? TRUE : FALSE;
            }
        }

        [Operator("lt", 0)]
        private class LtNode : BinaryNode
        {
            public LtNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return this.leftInput.GetValue(variableProvider) < this.rightInput.GetValue(variableProvider) ? TRUE : FALSE;
            }
        }

        [Operator("lte", 0)]
        private class LteNode : BinaryNode
        {
            public LteNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return this.leftInput.GetValue(variableProvider) <= this.rightInput.GetValue(variableProvider) ? TRUE : FALSE;
            }
        }

        [Operator("gt", 0)]
        private class GtNode : BinaryNode
        {
            public GtNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return this.leftInput.GetValue(variableProvider) > this.rightInput.GetValue(variableProvider) ? TRUE : FALSE;
            }
        }

        [Operator("gte", 0)]
        private class GteNode : BinaryNode
        {
            public GteNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return this.leftInput.GetValue(variableProvider) >= this.rightInput.GetValue(variableProvider) ? TRUE : FALSE;
            }
        }

        [Operator("d", 90)]
        [Operator("D", 90)]
        private class DiceNode : BinaryNode
        {
            public DiceNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                int left = (int)this.leftInput.GetValue(variableProvider);
                int right = (int)this.rightInput.GetValue(variableProvider);

                int value = 0;
                for (int i = 0; i < left; ++i)
                {
                    value += Interpreter.Random.Next(1, right + 1);
                }

                return value;
            }
        }

        [Operator("&", 0)]
        [Operator("and", 0)]
        private class AndNode : BinaryNode
        {
            public AndNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return BoolToDouble(DoubleToBool(this.leftInput.GetValue(variableProvider)) && DoubleToBool(this.rightInput.GetValue(variableProvider)));
            }
        }

        [Operator("|", 0)]
        [Operator("or", 0)]
        private class OrNode : BinaryNode
        {
            public OrNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(IVariableProvider variableProvider)
            {
                return BoolToDouble(DoubleToBool(this.leftInput.GetValue(variableProvider)) || DoubleToBool(this.rightInput.GetValue(variableProvider)));
            }
        }

        #endregion

        [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
        private class OperatorAttribute : System.Attribute
        {
            public string Symbol;
            public int Priority;

            public OperatorAttribute(string symbol, int priority)
            {
                Symbol = symbol;
                Priority = priority;
            }
        }
    }
}
