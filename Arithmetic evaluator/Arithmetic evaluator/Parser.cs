using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arithmetic_evaluator
{
    public class Parser
    {
        public static double Evaluate(string input)
        {
            // first, remove whitespace/newlines
            input = input.Replace(" ", "");
            input = input.Replace("\t", "");
            input = input.Replace("\n", "");
            input = input.Replace("\r", "");

            // if somehow anyone put in the proper symbols, let's replace it
            input = input.Replace("×", "*");
            input = input.Replace("⋅", "*");
            input = input.Replace("÷", "/");
            // also support someone putting in a backslash
            input = input.Replace("\\", "/");

            // secondly, do a quick check for invalid characters
            if (!PreCheckChars(input))
            {
                // return null;
                throw new FormatException("This expression contains some unrecognized characters. Cannot be evaluated.");
            }

            // move actual evaluation to its own function
            // as the above char-replacements only need to be done once at the beginning
            return PerformEvaluation(input);
        }

        static double PerformEvaluation(string input)
        {
            if (input.Contains("(") || input.Contains(")"))
            {
                if (!CheckValidParenthesys(input))
                {
                    //return null;
                    throw new FormatException("This expression has an unmatching number of opening and closing parantheses. Cannot be evaluated.");
                }

                // a listing of substitutions to make
                // as expressions within parantheses are evaluated and resolved
                Dictionary<string, string> Changes = new Dictionary<string, string>();

                bool openPar = false;
                int openIndex = 0;
                char prev = ' ';
                for (int i = 0; i < input.Length; i++)
                {
                    if (!openPar)
                    {
                        if (input[i] == '(')
                        {
                            // check for instances of an implied multiplication
                            // i.e. 3(4+2) or (2-3)(4/6)
                            if ("0123456789)".Contains(prev))
                            {
                                input = input.Insert(i, "*");
                                i++;
                            }
                            openPar = true;
                            openIndex = i;
                        }
                    }
                    else
                    {
                        if (input[i] == ')')
                        {
                            string subInput = input.Substring(openIndex + 1, i - openIndex - 1);

                            if (!Changes.ContainsKey(subInput))
                            {
                                string evaluatedSubInput = PerformEvaluation(subInput).ToString();
                                Changes.Add(subInput, evaluatedSubInput);
                            }
                            openPar = false;
                            openIndex = 0;
                        }
                    }
                    prev = input[i];
                }

                foreach (var item in Changes.Keys)
                {
                    input = input.Replace(item, Changes[item]);
                }
                input = input.Replace("(", "");
                input = input.Replace(")", "");
            }

            List<string> ParsedOperations = ParseOperations(input);
            if (ParsedOperations != null)
            {
                return PerformOperations(ref ParsedOperations);
            }
            else
            {
                // throwing a FormatException would be better than a NullReferenceException
                // since a NullReference just means something somewhere was null lol
                throw new FormatException("This expression contains some unrecognized characters. Cannot be evaluated.");
            }
        }

        // checks to make sure opening parantheses == closing parantheses (i.e. "(())" is good, "(()" is not)
        static bool CheckValidParenthesys(string input)
        {
            int openPar = 0;
            int closePar = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(')
                    openPar++;
                else if (input[i] == ')')
                    closePar++;
            }
            return openPar == closePar;
        }
        static double PerformOperations(ref List<string> OperationList)
        {
            while (OperationList.Count > 1)
            {
                int index = 0;
                while (index < OperationList.Count)
                {
                    if (OperationList[index] == "^")
                    {
                        double div1 = double.Parse(OperationList[index - 1]);
                        double div2 = double.Parse(OperationList[index + 1]);
                        OperationList[index - 1] = Math.Pow(div1, div2).ToString();
                        OperationList.RemoveAt(index);
                        OperationList.RemoveAt(index);
                    }
                    index++;
                }
                index = 0;
                while (index < OperationList.Count)
                {
                    if (OperationList[index] == "/")
                    {
                        double div1 = double.Parse(OperationList[index - 1]);
                        double div2 = double.Parse(OperationList[index + 1]);
                        OperationList[index - 1] = (div1 / div2).ToString();
                        OperationList.RemoveAt(index);
                        OperationList.RemoveAt(index);
                    }
                    index++;
                }
                index = 0;
                while (index < OperationList.Count)
                {
                    if (OperationList[index] == "*")
                    {
                        double mult1 = double.Parse(OperationList[index - 1]);
                        double mult2 = double.Parse(OperationList[index + 1]);
                        OperationList[index - 1] = (mult1 * mult2).ToString();
                        OperationList.RemoveAt(index);
                        OperationList.RemoveAt(index);
                    }
                    index++;
                }
                index = 0;
                while (index < OperationList.Count)
                {
                    if (OperationList[index] == "-")
                    {
                        double res1 = double.Parse(OperationList[index - 1]);
                        double res2 = double.Parse(OperationList[index + 1]);
                        OperationList[index - 1] = (res1 - res2).ToString();
                        OperationList.RemoveAt(index);
                        OperationList.RemoveAt(index);
                    }
                    index++;
                }
                index = 0;
                while (index < OperationList.Count)
                {
                    if (OperationList[index] == "+")
                    {
                        double sum1 = double.Parse(OperationList[index - 1]);
                        double sum2 = double.Parse(OperationList[index + 1]);
                        OperationList[index - 1] = (sum1 + sum2).ToString();
                        OperationList.RemoveAt(index);
                        OperationList.RemoveAt(index);
                    }
                    index++;
                }
            }
            return double.Parse(OperationList[0]);
        }
        static List<string> ParseOperations(string input)
        {
            List<string> Operations = new List<string>(); // organized list of operations to do
            int index = 0; // current char looked at
            int prevRes = -1; // store CheckChars result of previous number
            string SubEvaluator = ""; // buffer for numbers
            while (index < input.Length)
            {
                if (CheckChars(input[index]) == 0) // number, digit separator
                {
                    // quick loop to get through all the numbers
                    // return to main while loop once we encounter something that isn't a number
                    for (int i = index; i < input.Length; i++)
                    {
                        if (CheckChars(input[i]) == 0)
                        {
                            SubEvaluator += input[i];
                            if (i == input.Length - 1)
                            {
                                Operations.Add(SubEvaluator);
                                index = input.Length;
                            }
                        }
                        else
                        {
                            Operations.Add(SubEvaluator);
                            SubEvaluator = "";
                            index = i;
                            break;
                        }
                    }
                    prevRes = 0;
                }
                else if (CheckChars(input[index]) == 1) // operation
                {
                    // special handling for negative numbers
                    // check if the previous character was an operation (or start of string)
                    // if so, probably indicating a negative number
                    if ((prevRes == 1 || prevRes == -1) && input[index] == '-')
                    {
                        // put into SubEvaluator and move on
                        SubEvaluator += input[index];
                    }
                    else
                    {
                        Operations.Add(input[index].ToString());
                    }
                    index++;
                    prevRes = 1;
                }
                else // paranthesis, or invalid character (parantheses should not be showing up at this point)
                {
                    return null; // ends up triggering FormatException
                }
            }
            return Operations;
        }
        static int CheckChars(char input)
        {
            return CheckChars(input.ToString());
        }
        static int CheckChars(string input)
        {
            if ("0987654321.,".Contains(input))
                return 0;
            else if ("/*-+^".Contains(input))
                return 1;
            else if ("()".Contains(input))
                return 2;
            else
                return -1;
        }

        static bool PreCheckChars(string input)
        {
            // iterate through each character to make sure they're all valid
            // if it's invalid, then we can fail fast
            foreach (char c in input)
            {
                if (!"0987654321.,/*-+^()".Contains(c))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
