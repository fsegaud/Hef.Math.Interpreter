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
    public partial class Interpreter
    {
        #region Static

        private static readonly System.Collections.Generic.Dictionary<string, OperatorDescriptor> operators
            = new System.Collections.Generic.Dictionary<string, OperatorDescriptor>();

        #endregion

        private abstract class Node
        {
            public abstract double GetValue(Interpreter interpreter);
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

            public override double GetValue(Interpreter interpreter)
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

            public override double GetValue(Interpreter interpreter)
            {
                double value = 0;
                if (interpreter.TryGetVariableValue(this.varName, out value))
                {
                    return value;
                }

                throw new System.Exception(string.Format("Could not parse variable '{0}'", this.varName));
            }
        }

        [Operator("pi", 0)]
        private class PiNode : ZeroNode
        {
            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.PI;
            }
        }

        [Operator("rand", 0)]
        private class RandNode : ZeroNode
        {
            public override double GetValue(Interpreter interpreter)
            {
                return Interpreter.Random.NextDouble();
            }
        }

        [Operator("true", 0)]
        private class TrueNode : ZeroNode
        {
            public override double GetValue(Interpreter interpreter)
            {
                return TRUE;
            }
        }

        [Operator("false", 0)]
        private class FalseNode : ZeroNode
        {
            public override double GetValue(Interpreter interpreter)
            {
                return FALSE;
            }
        }

        #endregion

        #region UnaryNode

        [Operator("±", 1)]
        private class SignNode : UnaryNode
        {
            public SignNode(Node input) : 
                base(input)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return -this.input.GetValue(interpreter);
            }
        }

        [Operator("sqrt")]
        private class SqrtNode : UnaryNode
        {
            public SqrtNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Sqrt(this.input.GetValue(interpreter));
            }
        }

        [Operator("cos")]
        private class CosNode : UnaryNode
        {
            public CosNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Cos(this.input.GetValue(interpreter));
            }
        }

        [Operator("sin")]
        private class SinNode : UnaryNode
        {
            public SinNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Sin(this.input.GetValue(interpreter));
            }
        }

        [Operator("tan")]
        private class TanNode : UnaryNode
        {
            public TanNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Tan(this.input.GetValue(interpreter));
            }
        }

        [Operator("acos")]
        private class AcosNode : UnaryNode
        {
            public AcosNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Acos(this.input.GetValue(interpreter));
            }
        }

        [Operator("asin")]
        private class AsinNode : UnaryNode
        {
            public AsinNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Asin(this.input.GetValue(interpreter));
            }
        }

        [Operator("atan")]
        private class AtanNode : UnaryNode
        {
            public AtanNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Atan(this.input.GetValue(interpreter));
            }
        }

        [Operator("cosh")]
        private class CoshNode : UnaryNode
        {
            public CoshNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Cosh(this.input.GetValue(interpreter));
            }
        }

        [Operator("sinh")]
        private class SinhNode : UnaryNode
        {
            public SinhNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Sinh(this.input.GetValue(interpreter));
            }
        }

        [Operator("tanh")]
        private class TanhNode : UnaryNode
        {
            public TanhNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Tanh(this.input.GetValue(interpreter));
            }
        }

        [Operator("deg2rad")]
        [Operator("degrad")]
        private class Deg2RadNode : UnaryNode
        {
            public Deg2RadNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return (this.input.GetValue(interpreter) * System.Math.PI) / 180d;
            }
        }

        [Operator("rad2deg")]
        [Operator("raddeg")]
        private class Rad2DegNode : UnaryNode
        {
            public Rad2DegNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return (this.input.GetValue(interpreter) * 180d) / System.Math.PI;
            }
        }

        [Operator("abs")]
        private class AbsNode : UnaryNode
        {
            public AbsNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Abs(this.input.GetValue(interpreter));
            }
        }

        [Operator("round")]
        private class RoundNode : UnaryNode
        {
            public RoundNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Round(this.input.GetValue(interpreter));
            }
        }

        [Operator("!", 3)]
        [Operator("not", 3)]
        private class NegNode : UnaryNode
        {
            public NegNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Abs(this.input.GetValue(interpreter)) < double.Epsilon ? TRUE : FALSE;
            }
        }

        [Operator("ceil")]
        private class CeilNode : UnaryNode
        {
            public CeilNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Ceiling(this.input.GetValue(interpreter));
            }
        }

        [Operator("floor")]
        private class FlorrNode : UnaryNode
        {
            public FlorrNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Floor(this.input.GetValue(interpreter));
            }
        }

        [Operator("trunc")]
        private class TruncNode : UnaryNode
        {
            public TruncNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Truncate(this.input.GetValue(interpreter));
            }
        }

        [Operator("log")]
        private class LogNode : UnaryNode
        {
            public LogNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Log(this.input.GetValue(interpreter));
            }
        }

        [Operator("log10")]
        private class Log10Node : UnaryNode
        {
            public Log10Node(Node input)
                : base(input)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Log10(this.input.GetValue(interpreter));
            }
        }

        [Operator("e")]
        [Operator("exp")]
        private class ExpNode : UnaryNode
        {
            public ExpNode(Node input)
                : base(input)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Exp(this.input.GetValue(interpreter));
            }
        }

        #endregion

        #region  BinaryNode

        [Operator("+", 6)]
        private class AddNode : BinaryNode
        {
            public AddNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return this.leftInput.GetValue(interpreter) + this.rightInput.GetValue(interpreter);
            }
        }

        [Operator("-", 6)]
        private class SubNode : BinaryNode
        {
            public SubNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return this.leftInput.GetValue(interpreter) - this.rightInput.GetValue(interpreter);
            }
        }

        [Operator("*", 5)]
        private class MultNode : BinaryNode
        {
            public MultNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return this.leftInput.GetValue(interpreter) * this.rightInput.GetValue(interpreter);
            }
        }

        [Operator("/", 5)]
        private class DivNode : BinaryNode
        {
            public DivNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return this.leftInput.GetValue(interpreter) / this.rightInput.GetValue(interpreter);
            }
        }

        [Operator("%", 5)]
        private class ModNode : BinaryNode
        {
            public ModNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return (int)this.leftInput.GetValue(interpreter) % (int)this.rightInput.GetValue(interpreter);
            }
        }

        [Operator("^")]
        [Operator("pow")]
        private class PowNode : BinaryNode
        {
            public PowNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Pow(this.leftInput.GetValue(interpreter), this.rightInput.GetValue(interpreter));
            }
        }

        [Operator("min")]
        private class MinNode : BinaryNode
        {
            public MinNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Min(this.leftInput.GetValue(interpreter), this.rightInput.GetValue(interpreter));
            }
        }

        [Operator("max")]
        private class MaxNode : BinaryNode
        {
            public MaxNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Max(this.leftInput.GetValue(interpreter), this.rightInput.GetValue(interpreter));
            }
        }

        [Operator("==", 9)]
        [Operator("eq", 9)]
        private class EqualNode : BinaryNode
        {
            public EqualNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Abs(this.leftInput.GetValue(interpreter) - this.rightInput.GetValue(interpreter)) < double.Epsilon ? TRUE : FALSE;
            }
        }

        [Operator("!=", 9)]
        [Operator("ne", 9)]
        private class NonEqualNode : BinaryNode
        {
            public NonEqualNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return System.Math.Abs(this.leftInput.GetValue(interpreter) - this.rightInput.GetValue(interpreter)) < double.Epsilon ? FALSE : TRUE;
            }
        }

        [Operator("lt", 8)]
        [Operator("<", 8)]
        private class LtNode : BinaryNode
        {
            public LtNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return this.leftInput.GetValue(interpreter) < this.rightInput.GetValue(interpreter) ? TRUE : FALSE;
            }
        }

        [Operator("lte", 8)]
        [Operator("<=", 8)]
        private class LteNode : BinaryNode
        {
            public LteNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return this.leftInput.GetValue(interpreter) <= this.rightInput.GetValue(interpreter) ? TRUE : FALSE;
            }
        }

        [Operator("gt", 8)]
        [Operator(">", 8)]
        private class GtNode : BinaryNode
        {
            public GtNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return this.leftInput.GetValue(interpreter) > this.rightInput.GetValue(interpreter) ? TRUE : FALSE;
            }
        }

        [Operator("gte", 8)]
        [Operator(">=", 8)]
        private class GteNode : BinaryNode
        {
            public GteNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return this.leftInput.GetValue(interpreter) >= this.rightInput.GetValue(interpreter) ? TRUE : FALSE;
            }
        }

        [Operator("d")]
        [Operator("D")]
        private class DiceNode : BinaryNode
        {
            public DiceNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                int left = (int)this.leftInput.GetValue(interpreter);
                int right = (int)this.rightInput.GetValue(interpreter);

                int value = 0;
                for (int i = 0; i < left; ++i)
                {
                    value += Interpreter.Random.Next(1, right + 1);
                }

                return value;
            }
        }

        [Operator("&&", 13)]
        [Operator("and", 13)]
        private class AndNode : BinaryNode
        {
            public AndNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return BoolToDouble(DoubleToBool(this.leftInput.GetValue(interpreter)) && DoubleToBool(this.rightInput.GetValue(interpreter)));
            }
        }

        [Operator("||", 14)]
        [Operator("or", 14)]
        private class OrNode : BinaryNode
        {
            public OrNode(Node leftInput, Node rightInput)
                : base(leftInput, rightInput)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return BoolToDouble(DoubleToBool(this.leftInput.GetValue(interpreter)) || DoubleToBool(this.rightInput.GetValue(interpreter)));
            }
        }

        [Operator("<<", 7)]
        private class LeftShiftNode : BinaryNode
        {
            public LeftShiftNode(Node leftInput, Node rightInput) : base(leftInput, rightInput)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return (double)((int)this.leftInput.GetValue(interpreter) << (int)this.rightInput.GetValue(interpreter));
            }
        }

        [Operator(">>", 7)]
        private class RightShiftNode : BinaryNode
        {
            public RightShiftNode(Node leftInput, Node rightInput) : base(leftInput, rightInput)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return (double)((int)this.leftInput.GetValue(interpreter) >> (int)this.rightInput.GetValue(interpreter));
            }
        }

        [Operator("|", 12)]
        private class BitOrNode : BinaryNode
        {
            public BitOrNode(Node leftInput, Node rightInput) : base(leftInput, rightInput)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return (double)((int)this.leftInput.GetValue(interpreter) | (int)this.rightInput.GetValue(interpreter));
            }
        }

        [Operator("&", 10)]
        private class BitAndNode : BinaryNode
        {
            public BitAndNode(Node leftInput, Node rightInput) : base(leftInput, rightInput)
            {
            }

            public override double GetValue(Interpreter interpreter)
            {
                return (double)((int)this.leftInput.GetValue(interpreter) & (int)this.rightInput.GetValue(interpreter));
            }
        }

        #endregion

        [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = true)]
        private class OperatorAttribute : System.Attribute
        {
            private const int FunctionPriority = 2;

            public string Symbol;
            public int Priority;

            public OperatorAttribute(string symbol, int priority)
            {
                Symbol = symbol;
                Priority = priority;
            }

            public OperatorAttribute(string symbol)
                : this(symbol, OperatorAttribute.FunctionPriority)
            {
            }
        }
    }
}
