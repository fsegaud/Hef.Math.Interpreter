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
        private static Player player;
        private static Hef.Math.Interpreter interpreter;

        static void Main(string[] args)
        {
            double foo = 40d;
            double bar = 2d;

            player = new Player();
            interpreter = new Interpreter(player);
            interpreter.SetVar("Foo", foo);
            interpreter.SetVar("bar", bar);

            bool success = true;

            success &= Calc("±1", -1d);
            success &= Calc("1-1", 1d - 1d);
            success &= Calc("1-±1", 1d - -1d);
            success &= Calc("!1", 0d);
            success &= Calc("!0", 1d);
            success &= Calc("!2", 0d);
            success &= Calc("!0.5", 0d);
            success &= Calc("2 + 2", 2 + 2d);
            success &= Calc("2+2", 2d + 2d);
            success &= Calc("(2+2)", 2d + 2d);
            success &= Calc("sqrt4+3*4", System.Math.Sqrt(4) + 3 * 4);
            success &= Calc("(sqrt4+3)*4", (System.Math.Sqrt(4) + 3) * 4);
            success &= Calc("5 * ±1", 5 * -1d);
            success &= Calc("abs ±1", System.Math.Abs(-1d));
            success &= Calc("sin(1+2)", System.Math.Sin(1 + 2));
            success &= Calc("sin1+2", System.Math.Sin(1) + 2);
            success &= Calc("sin1*cos2+cos1*sin2", System.Math.Sin(1) * System.Math.Cos(2) + System.Math.Cos(1) * System.Math.Sin(2));
            success &= Calc("(2 * 5 == 10) * 5", (2d * 5d == 10 ? 1d : 0d) * 5d);
            success &= Calc("min 4 6", System.Math.Min(4d, 6d));
            success &= Calc("max 4 6", System.Math.Max(4d, 6d));
            success &= Calc("(4 gte 4)", 4d >= 4d ? 1d : 0d);
            success &= Calc("(4 gte 3)", 4d >= 3d ? 1d : 0d);
            success &= Calc("(3 gte 4)", 3d >= 4d ? 1d : 0d);
            success &= Calc("$Health / $MaxHealth", player.GetHealth() / player.MaxHealth);
            success &= Calc("$bar", bar);
            success &= Calc("$Foo + $bar", foo + bar);
            success &= Calc("round (rand * 10 + 90)");
            success &= Calc("1d4+1 + 1D6+1");
            success &= Calc("true", 1d);
            success &= Calc("false", 0d);
            success &= Calc("!true", 0d);
            success &= Calc("!false", 1d);
            success &= Calc("true & true", 1d);
            success &= Calc("true & false", 0d);
            success &= Calc("false & true", 0d);
            success &= Calc("false & false", 0d);
            success &= Calc("true | true", 1d);
            success &= Calc("true | false", 1d);
            success &= Calc("false | true", 1d);
            success &= Calc("false | false", 0d);

            System.Console.WriteLine("OVERALL RESULT: " + success);
        }

        private static bool Calc(string infix)
        {
            System.Console.WriteLine("{0} = {1} (nocheck)", infix, interpreter.Calculate(infix));

            return true;
        }

        private static bool Calc(string infix, double intendedResult)
        {
            double result = interpreter.Calculate(infix);
            bool match = System.Math.Abs(intendedResult - result) < double.Epsilon;
            System.Console.WriteLine("{0} = {1} -> {2}", infix, result, match);

            return match;
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
