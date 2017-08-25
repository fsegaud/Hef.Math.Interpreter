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
            {"±",       new OperatorDescriptor(Operator.Sign,    OperatorType.Unary,     99)},
            {"+",       new OperatorDescriptor(Operator.Add,     OperatorType.Binary,    2) },
            {"-",       new OperatorDescriptor(Operator.Sub,     OperatorType.Binary,    2) },
            {"*",       new OperatorDescriptor(Operator.Mult,    OperatorType.Binary,    5) },
            {"/",       new OperatorDescriptor(Operator.Div,     OperatorType.Binary,    5) },
            {"%",       new OperatorDescriptor(Operator.Mod,     OperatorType.Binary,    10)},
            {"^",       new OperatorDescriptor(Operator.Pow,     OperatorType.Binary,    15)},
            {"sqrt",    new OperatorDescriptor(Operator.Sqrt,    OperatorType.Unary,     15)},
            {"cos",     new OperatorDescriptor(Operator.Cos,     OperatorType.Unary,     12)},
            {"sin",     new OperatorDescriptor(Operator.Sin,     OperatorType.Unary,     12)},
            {"tan",     new OperatorDescriptor(Operator.Tan,     OperatorType.Unary,     12)},
            {"acos",    new OperatorDescriptor(Operator.Acos,    OperatorType.Unary,     12)},
            {"asin",    new OperatorDescriptor(Operator.Asin,    OperatorType.Unary,     12)},
            {"atan",    new OperatorDescriptor(Operator.Atan,    OperatorType.Unary,     12)},
            {"cosh",    new OperatorDescriptor(Operator.Cosh,    OperatorType.Unary,     12)},
            {"sinh",    new OperatorDescriptor(Operator.Sinh,    OperatorType.Unary,     12)},
            {"tanh",    new OperatorDescriptor(Operator.Tanh,    OperatorType.Unary,     12)},
            {"degrad",  new OperatorDescriptor(Operator.Deg2Rad, OperatorType.Unary,     13)},
            {"raddeg",  new OperatorDescriptor(Operator.Rad2Deg, OperatorType.Unary,     13)},
            {"abs",     new OperatorDescriptor(Operator.Abs,     OperatorType.Unary,     8) },
            {"round",   new OperatorDescriptor(Operator.Round,   OperatorType.Unary,     8) },
            {"!",       new OperatorDescriptor(Operator.Neg,     OperatorType.Unary,     50)},
            {"pi",      new OperatorDescriptor(Operator.PI,      OperatorType.Const,     90)},
            {"min",     new OperatorDescriptor(Operator.Min,     OperatorType.Binary,    80)},
            {"max",     new OperatorDescriptor(Operator.Max,     OperatorType.Binary,    90)},
            {"==",      new OperatorDescriptor(Operator.Equal,   OperatorType.Binary,    0) },
            {"eq",      new OperatorDescriptor(Operator.Equal,   OperatorType.Binary,    0) },
            {"lt",      new OperatorDescriptor(Operator.LT,      OperatorType.Binary,    0) },
            {"lte",     new OperatorDescriptor(Operator.LTE,     OperatorType.Binary,    0) },
            {"gt",      new OperatorDescriptor(Operator.GT,      OperatorType.Binary,    0) },
            {"gte",     new OperatorDescriptor(Operator.GTE,     OperatorType.Binary,    0) },
            {"rand",    new OperatorDescriptor(Operator.Rand,    OperatorType.Const,     90)},
            {"d",       new OperatorDescriptor(Operator.Dice,    OperatorType.Binary,    90)},
            {"D",       new OperatorDescriptor(Operator.Dice,    OperatorType.Binary,    90)},
            {"true",    new OperatorDescriptor(Operator.True,    OperatorType.Const,     90)},
            {"false",   new OperatorDescriptor(Operator.False,   OperatorType.Const,     90)},
            {"&",       new OperatorDescriptor(Operator.And,     OperatorType.Binary,    0) },
            {"and",     new OperatorDescriptor(Operator.And,     OperatorType.Binary,    0) },
            {"|",       new OperatorDescriptor(Operator.Or,      OperatorType.Binary,    1) },
            {"or",      new OperatorDescriptor(Operator.Or,      OperatorType.Binary,    1) },

            /* Add your own operator description here ... */
        };

        #endregion

        #region Enumerations

        private enum Operator
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

            /* Add your own operator here ... */
        }

        #endregion

        #region Functions

        private static double ComputeOperation(double left, double right, Operator op)
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

                /* Add your own operator computations here ... */

                default:
                    throw new System.InvalidOperationException(string.Format("Operator '{0}' not supported.", op));
            }
        }

        #endregion
    }
}
