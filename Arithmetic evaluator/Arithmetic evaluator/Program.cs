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
            // test order of operations
            Console.WriteLine(Parser.Evaluate("(11-5)*2-3+1")); //  10
            Console.WriteLine(Parser.Evaluate("3+4*(2)")); //       11
            Console.WriteLine(Parser.Evaluate("-1(-3*4)")); //      12
            Console.WriteLine(Parser.Evaluate("9-5/(8-3)*2+6")); // 13
            Console.WriteLine(Parser.Evaluate("3+6*(5+4)/3-7")); // 14
            Console.WriteLine(Parser.Evaluate("3+3 * 3+3")); //     15
            Console.WriteLine(Parser.Evaluate("150/(6+3*8)-5")); // 0
            Console.WriteLine(Parser.Evaluate("(2-3)(4/6)")); //    -0.6666666...

            // test power operator ("^")
            Console.WriteLine(Parser.Evaluate("5^2*2^4")); // 400

            // original one
            Console.WriteLine(Parser.Evaluate("5+(6/6)+(666+663)/214*4 + (666+663)")); //1359.841121...


            Console.ReadKey();
        }

    }
}

