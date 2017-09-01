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
        private const double FALSE = 0d;
        private const double TRUE = 1d;

        private static Player player;
        private static Hef.Math.Interpreter interpreter;

        static void Main(string[] args)
        {
            double foo = 40d;
            double bar = 2d;
            double hundred = 100d;

            player = new Player();
            interpreter = new Interpreter();
            interpreter.SetContext("player", new Player());
            interpreter.SetContext("World", new World());
            interpreter.SetVar("Foo", foo);
            interpreter.SetVar("bar", bar);
            interpreter.SetVar("hundred", hundred);
            
            bool success = true;

            // Old tests.
            success &= Test("±1", -1d);
            success &= Test("1-1", 1d - 1d);
            success &= Test("1-±1", 1d - -1d);
            success &= Test("2 + 2", 2 + 2d);
            success &= Test("2+2", 2d + 2d);
            success &= Test("(2+2)", 2d + 2d);
            success &= Test("sqrt 4 + 3 * 4", System.Math.Sqrt(4) + 3 * 4);
            success &= Test("sqrt 4+3*4", System.Math.Sqrt(4) + 3 * 4);
            success &= Test("(sqrt 4+3)*4", (System.Math.Sqrt(4) + 3) * 4);
            success &= Test("5 * ±1", 5 * -1d);
            success &= Test("abs ±1", System.Math.Abs(-1d));
            success &= Test("sin(1+2)", System.Math.Sin(1 + 2));
            success &= Test("sin 1+2", System.Math.Sin(1) + 2);
            success &= Test("sin 1*cos 2+cos 1*sin 2", System.Math.Sin(1) * System.Math.Cos(2) + System.Math.Cos(1) * System.Math.Sin(2));
            success &= Test("(2 * 5 == 10) * 5", (2d * 5d == 10 ? 1d : 0d) * 5d);
            success &= Test("min 4 6", System.Math.Min(4d, 6d));
            success &= Test("max 4 6", System.Math.Max(4d, 6d));
            success &= Test("(4 gte 4)", 4d >= 4d ? 1d : 0d);
            success &= Test("(4 gte 3)", 4d >= 3d ? 1d : 0d);
            success &= Test("(3 gte 4)", 3d >= 4d ? 1d : 0d);
            success &= Test("$player.Health / $player.MaxHealth", player.GetHealth() / player.MaxHealth);
            success &= Test("$bar", bar);
            success &= Test("sqrt($hundred^2)", System.Math.Sqrt(hundred * hundred));
            success &= Test("$Foo + $bar", foo + bar);
            success &= Test("round (rand * 10 + 90)");
            success &= Test("1 d 4+1 + 1 D 6+1");

            // Comparison.
            success &= Test("1 == 0", BoolToDouble(1d == 0d));
            success &= Test("1 == 1", BoolToDouble(1d == 1d));
            success &= Test("1 eq 0", BoolToDouble(1d == 0d));
            success &= Test("1 eq 1", BoolToDouble(1d == 1d));
            success &= Test("1 gt 0", BoolToDouble(1d > 0d));
            success &= Test("1 gt 1", BoolToDouble(1d > 1d));
            success &= Test("1 gt 2", BoolToDouble(1d > 2d));
            success &= Test("1 gte 0", BoolToDouble(1d >= 0d));
            success &= Test("1 gte 1", BoolToDouble(1d >= 1d));
            success &= Test("1 gte 2", BoolToDouble(1d >= 2d));
            success &= Test("1 lt 0", BoolToDouble(1d < 0d));
            success &= Test("1 lt 1", BoolToDouble(1d < 1d));
            success &= Test("1 lt 2", BoolToDouble(1d < 2d));
            success &= Test("1 lte 0", BoolToDouble(1d <= 0d));
            success &= Test("1 lte 1", BoolToDouble(1d <= 1d));
            success &= Test("1 lte 2", BoolToDouble(1d <= 2d));

            // Boolean.
            success &= Test("!1", FALSE);
            success &= Test("!0", TRUE);
            success &= Test("!2", FALSE);
            success &= Test("!0.5", FALSE);
            success &= Test("true", BoolToDouble(true));
            success &= Test("false", BoolToDouble(false));
            success &= Test("!true", BoolToDouble(!true));
            success &= Test("!false", BoolToDouble(!false));
            success &= Test("true & true", BoolToDouble(true && true));
            success &= Test("true & false", BoolToDouble(true && false));
            success &= Test("false & true", BoolToDouble(false && true));
            success &= Test("false & false", BoolToDouble(false && false));
            success &= Test("true and true", BoolToDouble(true && true));
            success &= Test("true and false", BoolToDouble(true && false));
            success &= Test("false and true", BoolToDouble(false && true));
            success &= Test("false and false", BoolToDouble(false && false));
            success &= Test("true | true", BoolToDouble(true || true));
            success &= Test("true | false", BoolToDouble(true || false));
            success &= Test("false | true", BoolToDouble(false || true));
            success &= Test("false | false", BoolToDouble(false || false));
            success &= Test("true or true", BoolToDouble(true || true));
            success &= Test("true or false", BoolToDouble(true || false));
            success &= Test("false or true", BoolToDouble(false || true));
            success &= Test("false or false", BoolToDouble(false || false));

            // Trigonometry.
            success &= Test("cos 0", System.Math.Cos(0d));
            success &= Test("cos (pi / 2)", System.Math.Cos(System.Math.PI / 2d));
            success &= Test("cos pi", System.Math.Cos(System.Math.PI));
            success &= Test("sin 0", System.Math.Sin(0d));
            success &= Test("sin (pi / 2)", System.Math.Sin(System.Math.PI / 2d));
            success &= Test("sin pi", System.Math.Sin(System.Math.PI));
            success &= Test("acos 0", System.Math.Acos(0d));
            success &= Test("acos 1", System.Math.Acos(1d));
            success &= Test("acos ±1", System.Math.Acos(-1d));
            success &= Test("asin 0", System.Math.Asin(0d));
            success &= Test("asin 1", System.Math.Asin(1d));
            success &= Test("asin ±1", System.Math.Asin(-1d));
            success &= Test("tan 0", System.Math.Tan(0));
            success &= Test("tan pi", System.Math.Tan(System.Math.PI));
            success &= Test("tan (pi / 4)", System.Math.Tan(System.Math.PI / 4d));
            success &= Test("tan (3 * pi / 4)", System.Math.Tan(3 * System.Math.PI / 4d));
            success &= Test("degrad 0", 0d);
            success &= Test("degrad 90", System.Math.PI * .5d);
            success &= Test("degrad 180", System.Math.PI);
            success &= Test("degrad 270", System.Math.PI * 1.5d);
            success &= Test("degrad 360", System.Math.PI * 2d);
            success &= Test("raddeg (0)", 0d);
            success &= Test("raddeg (pi * 0.5)", 90d);
            success &= Test("raddeg (pi)", 180d);
            success &= Test("raddeg (pi * 1.5)", 270d);
            success &= Test("raddeg (pi * 2)", 360d);
            success &= Test("deg2rad 0", 0d);
            success &= Test("deg2rad 90", System.Math.PI * .5d);
            success &= Test("deg2rad 180", System.Math.PI);
            success &= Test("deg2rad 270", System.Math.PI * 1.5d);
            success &= Test("deg2rad 360", System.Math.PI * 2d);
            success &= Test("rad2deg (0)", 0d);
            success &= Test("rad2deg (pi * 0.5)", 90d);
            success &= Test("rad2deg (pi)", 180d);
            success &= Test("rad2deg (pi * 1.5)", 270d);
            success &= Test("rad2deg (pi * 2)", 360d);

            // Writing style and comma separator.
            success &= Test("min(1,2)", System.Math.Min(1, 2));
            success &= Test("min(1, 2)", System.Math.Min(1, 2));
            success &= Test("min(1 2)", System.Math.Min(1, 2));
            success &= Test("min 1 2", System.Math.Min(1, 2));
            success &= Test("min 1,2", System.Math.Min(1, 2));
            success &= Test("min 1, 2", System.Math.Min(1, 2));
            success &= Test("min(2,1)", System.Math.Min(1, 2));
            success &= Test("min(2, 1)", System.Math.Min(1, 2));
            success &= Test("min(2 1)", System.Math.Min(1, 2));
            success &= Test("min 2 1", System.Math.Min(1, 2));
            success &= Test("min 2,1", System.Math.Min(1, 2));
            success &= Test("min 2, 1", System.Math.Min(1, 2));
            
            System.Console.WriteLine("--------------------\nOVERALL RESULT: " + success);
        }

        private static bool Test(string infix)
        {
            System.Console.WriteLine("{0} = {1} (nocheck)", infix, interpreter.Calculate(infix));

            return true;
        }

        private static bool Test(string infix, double intendedResult)
        {
            double result = interpreter.Calculate(infix);
            bool match = System.Math.Abs(intendedResult - result) < double.Epsilon;

            if (match)
            {
                System.Console.ForegroundColor = System.ConsoleColor.Green;
                System.Console.WriteLine("{0} = {1} -> {2}", infix, result, match);
            }
            else
            {
                System.Console.ForegroundColor = System.ConsoleColor.Red;
                System.Console.WriteLine("{0} = {1} (instead of {3}) -> {2}", infix, result, match, intendedResult);
            }

            System.Console.ForegroundColor = System.ConsoleColor.Gray;

            return match;
        }

        private static bool DoubleToBool(double value)
        {
            return System.Math.Abs(value - 1d) < double.Epsilon;
        }

        private static double BoolToDouble(bool value)
        {
            return value ? 1d : 0d;
        }
    }

    public class Player : Hef.Math.IInterpreterContext
    {
        public double MaxHealth
        {
            get
            {
                return 100d;
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

    public class World : Hef.Math.IInterpreterContext
    {
        public bool TryGetVariable(string name, out double value)
        {
            value = 0d;
            if (name == "width" || name == "height")
            {
                value = 8d;
                return true;
            }

            return false;
        }
    }
}
