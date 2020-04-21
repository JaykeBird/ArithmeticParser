using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arithmetic_evaluator
{
    class Program
    {
        static void Main(string[] args)
        {
            // read function from command-line arguments if one was given
            if (args.Length != 0)
            {
                RunEvaluator(args[0]);
                return;
            }

            // test order of operations and negative numbers
            RunEvaluator("(11-5)*2-3+1"); //  10
            RunEvaluator("3+4*(2)"); //       11
            RunEvaluator("-1(-3*4)"); //      12
            RunEvaluator("9-5/(8-3)*2+6"); // 13
            RunEvaluator("-(3+4)*-2"); //     14
            RunEvaluator("3+3 * 3+3"); //     15
            RunEvaluator("8 / 2(2+2)"); //    16
            RunEvaluator("5+(6/3+3)2+2"); //  17
            RunEvaluator("150/(6+3*8)-5"); // 0
            RunEvaluator("(2-3)(4/6)"); //    -0.6666666...

            // test power operator ("^")
            RunEvaluator("5^2*2^4"); //       400
            RunEvaluator("4(0.4)^2"); //      0.64 or 64, depending upon culture

            // now some invalid expressions
            RunEvaluator("apple");
            RunEvaluator("3*/3");
            RunEvaluator("3+-*4");
            RunEvaluator("(5+2");
            RunEvaluator("*");

            // original one
            RunEvaluator("5+(6/6)+(666+663)/214*4 + (666+663)"); //1359.841121...


            Console.ReadKey();
        }

        static void RunEvaluator(string input)
        {
            try
            {
                Console.WriteLine(Parser.Evaluate(input));
            }
            catch (FormatException e)
            {
                Console.WriteLine("Cannot evaluate \"" + input + "\" (" + e.Message + ")");
            }
        }
    }
}

