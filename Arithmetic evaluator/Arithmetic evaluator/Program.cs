using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arithmetic_evaluator
{
    class Program
    {
        static void Main(string[] args)
        {
            // read function from command-line arguments if one was given
            if (args.Length != 0)
            {
                Console.WriteLine(Parser.Evaluate(args[0]));
                return;
            }

            // test order of operations and negative numbers
            Console.WriteLine(Parser.Evaluate("(11-5)*2-3+1")); //  10
            Console.WriteLine(Parser.Evaluate("3+4*(2)")); //       11
            Console.WriteLine(Parser.Evaluate("-1(-3*4)")); //      12
            Console.WriteLine(Parser.Evaluate("9-5/(8-3)*2+6")); // 13
            Console.WriteLine(Parser.Evaluate("-(3+4)*-2")); //     14
            Console.WriteLine(Parser.Evaluate("3+3 * 3+3")); //     15
            Console.WriteLine(Parser.Evaluate("8 / 2(2+2)")); //    16
            Console.WriteLine(Parser.Evaluate("150/(6+3*8)-5")); // 0
            Console.WriteLine(Parser.Evaluate("(2-3)(4/6)")); //    -0.6666666...

            // test power operator ("^")
            Console.WriteLine(Parser.Evaluate("5^2*2^4")); //       400
            Console.WriteLine(Parser.Evaluate("4(0.4)^2")); //      0.64 or 64, depending upon culture

            try
            {
                Console.WriteLine(Parser.Evaluate("apple"));
            }
            catch (FormatException e)
            {
                Console.WriteLine("Cannot evaluate \"apple\" (" + e.Message + ")");
            }

            // original one
            Console.WriteLine(Parser.Evaluate("5+(6/6)+(666+663)/214*4 + (666+663)")); //1359.841121...


            Console.ReadKey();
        }

    }
}

