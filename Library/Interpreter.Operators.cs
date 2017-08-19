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
    public partial class Interpreter
    {
        #region Static

        private static readonly System.Collections.Generic.Dictionary<string, OperatorDescriptor> operators
            = new System.Collections.Generic.Dictionary<string, OperatorDescriptor>
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
            {"D",       new OperatorDescriptor(Operator.Dice,  OperatorType.Binary,    90)},
            {"'",       new OperatorDescriptor(Operator.Minus, OperatorType.Unary,     99)}

            /* Add your own operator description here ... */
        };

        #endregion

        #region Enumerations

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
            Dice,
            Minus

            /* Add your own operator here ... */
        }

        #endregion

        #region Functions

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
                    return System.Math.Abs(left - right) < double.Epsilon ? 1f : 0f;

                case Operator.Pow:
                    return System.Math.Pow(left, right);

                case Operator.Sqrt:
                    return System.Math.Sqrt(left);

                case Operator.Cos:
                    return System.Math.Cos(left);

                case Operator.Sin:
                    return System.Math.Sin(left);

                case Operator.Abs:
                    return System.Math.Abs(left);

                case Operator.Round:
                    return System.Math.Round(left);

                case Operator.Neg:
                    //return -left;
                    return System.Math.Abs(left) < double.Epsilon ? 1d : 0d;

                case Operator.PI:
                    return System.Math.PI;

                case Operator.Min:
                    return System.Math.Min(left, right);

                case Operator.Max:
                    return System.Math.Max(left, right);

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
                    {
                        int value = 0;
                        for (int i = 0; i < left; ++i)
                        {
                            value += Interpreter.Random.Next(1, (int)right + 1);
                        }

                        return value;
                    }

                case Operator.Minus:
                    return -left;

                /* Add your own operator computations here ... */

                default:
                    throw new System.InvalidOperationException(string.Format("Operator '{0}' not supported.", op));
            }
        }

        #endregion
    }
}
