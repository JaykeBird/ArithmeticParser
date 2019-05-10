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
            try
            {
                // (11 - 5) x 2 - 3 + 1
                Console.WriteLine(Parser.Evaluator("(11-5)*2-3+1") + " should equal 10"); // 10
                Console.WriteLine(Parser.Evaluator("3+4*2") + " should equal 11");

                // 9 - 5 ÷ (8 - 3) x 2 + 6
                Console.WriteLine(Parser.Evaluator("9-5/(8-3)*2+6") + " should equal 13");
                // 3 + 6 x (5 + 4) ÷ 3 - 7
                Console.WriteLine(Parser.Evaluator("3+6*(5+4)/3-7") + " should equal 14");
                // 150 ÷ (6 + 3 x 8) - 5
                Console.WriteLine(Parser.Evaluator("150/(6+3*8)-5") + " should equal 0");

                Console.WriteLine(Parser.Evaluator("5^2*2^4") + " should be 400");

                // original one
                Console.WriteLine(Parser.Evaluator("5+(6/6)+(666+663)/214*4 + (666+663)"));
            }
            catch (Exception e)
            {

                throw e;
            }
            Console.ReadKey();
        }

    }
}

