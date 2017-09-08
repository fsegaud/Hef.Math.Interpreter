namespace Hef.Math.Example
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Calculator \n" +
                              "'setv key value' to set variable\n" +
                              "'q' to quit\n----------");

            Interpreter interpreter = new Interpreter();

            int lastResultIdx = 0;
            bool stop = false;
            while (!stop)
            {
                try
                {
                    System.Console.Write("  : ");
                    string input = System.Console.ReadLine();
                    if (string.IsNullOrEmpty(input))
                    {
                    }
                    else if (input == "q" || input == "Q")
                    {
                        System.Console.WriteLine("Bood Bye :)");
                        stop = true;
                    }
                    else if (input.StartsWith("setv"))
                    {
                        string[] tokens = input.Split(new[] { ' ' }, System.StringSplitOptions.RemoveEmptyEntries);

                        double value;
                        if (tokens.Length == 3 && double.TryParse(tokens[2], out value))
                        {
                            interpreter.SetVar(tokens[1], value);
                        }
                        else
                        {
                            System.Console.WriteLine("Syntax: setv key value");
                        }
                    }
                    else
                    {
                        double result = interpreter.Calculate(input);
                        interpreter.SetVar(lastResultIdx.ToString(), result);
                        System.Console.WriteLine("${1}> {0}", result, lastResultIdx);
                        lastResultIdx++;
                    }
                }
                catch (System.Exception e)
                {
                    System.Console.Error.WriteLine(e);
                }
            }
        }
    }
}
