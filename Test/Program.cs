using System;
using System.Collections.Generic;

namespace Hef.Math.Test
{
    class Program
    {
        private static readonly Player player = new Player();
        private static string Format = "{0} -> {1} = {2}";

        static void Main(string[] args)
        {
            Calc("10^2");
            Calc("sqrt4+3*4");
            Calc("(sqrt4+3)*4");
            Calc("5 * !1");
            Calc("abs !1");
            Calc("sin(1+2)");
            Calc("sin1+2");
            Calc("sin1*cos2+cos1*sin2");
            Calc("(2 * 5 == 10) * 5");
            Calc("$MaxHealth + $MaxMana * 2 + $XP");
            Calc("min 4 6");
            Calc("(4 gte 4)");
            Calc("round (rand * 10 + 90)");
            Calc("1d4+1 + 1D6+1");
        }

        private static void Calc(string infix)
        {
            Console.WriteLine(Program.Format, infix, Hef.Math.Interpreter.InfixToRpn(infix), player.Interpreter.Calculate(infix));
        }
    }

    public class Player : Hef.Math.IInterpreterContext
    {
        public Hef.Math.Interpreter Interpreter
        {
            get;
            private set;
        }

        public Player()
        {
            this.Interpreter = new Hef.Math.Interpreter(this);
        }

        public Dictionary<string, double> GetVariables()
        {
            return new Dictionary<string, double>()
                {
                    {"MaxHealth", 100},
                    {"MaxMana", 50}
                };
        }

        public bool TryGetVariable(string name, out double value)
        {
            value = 0d;
            if (name == "XP")
            {
                value = 24d;
                return true;
            }
            else if (name == "MaxHealth")
            {
                value = 100d;
                return true;
            }
            else if (name == "MaxMana")
            {
                value = 50d;
                return true;
            }

            return false;
        }
    }
}
