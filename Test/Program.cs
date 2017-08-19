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

namespace Hef.Math.Test
{
    class Program
    {
        private static string Format = "{0} = {1}";

        private static Player player;
        private static Hef.Math.Interpreter interpreter;

        static void Main(string[] args)
        {
            player = new Player();
            interpreter = new Interpreter(player);
            interpreter.SetVar("Foo", 40d);
            interpreter.SetVar("bar", 2d);

            Calc("'1", -1d);
            Calc("1-1", 0d);
            Calc("!1", 0d);
            Calc("!0", 1d);
            Calc("!2", 0d);
            Calc("!0.5", 0D);
            Calc("2 + 2", 4);
            Calc("2+2", 4);
            Calc("(2+2)", 4);
            Calc("sqrt4+3*4", 14d);
            Calc("(sqrt4+3)*4", 20d);
            Calc("5 * '1", -5d);
            Calc("abs '1", 1d);
            Calc("sin(1+2)", System.Math.Sin(1 + 2));
            Calc("sin1+2", System.Math.Sin(1) + 2);
            Calc("sin1*cos2+cos1*sin2", System.Math.Sin(1) * System.Math.Cos(2) + System.Math.Cos(1) * System.Math.Sin(2));
            Calc("(2 * 5 == 10) * 5", 5d);
            Calc("min 4 6", 4d);
            Calc("max 4 6", 6d);
            Calc("(4 gte 4)", 1d);
            Calc("(4 gte 3)", 1d);
            Calc("(3 gte 4)", 0d);
            Calc("$Health / $MaxHealth", .5d);
            Calc("$bar", 2d);
            Calc("$Foo + $bar", 42d);
            Calc("round (rand * 10 + 90)");
            Calc("1d4+1 + 1D6+1");
        }

        private static void Calc(string infix)
        {
            System.Console.WriteLine("{0} = {1}", infix, interpreter.Calculate(infix));
        }

        private static void Calc(string infix, double intendedResult)
        {
            double result = interpreter.Calculate(infix);
            bool match = System.Math.Abs(intendedResult - result) < double.Epsilon;
            System.Console.WriteLine("{0} = {1} -> {2}", infix, result, match);
        }
    }

    public class Player : Hef.Math.IInterpreterContext
    {
        public double MaxHealth
        {
            get
            {
                return 50d;
            }
        }

        public double GetHealth()
        {
            return 50d;
        }

        public bool TryGetVariable(string name, out double value)
        {
            value = 0d;
            if (name == "Health")
            {
                value = this.GetHealth();
                return true;
            }
            else if (name == "MaxHealth")
            {
                value = this.MaxHealth;
                return true;
            }

            return false;
        }
    }
}
