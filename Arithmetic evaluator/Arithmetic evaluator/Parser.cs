using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Arithmetic_evaluator
{
    public class Parser
    {
        public static double Evaluator(string input)
        {
            // first, remove whitespace/newlines
            input = input.Replace(" ", "");
            input = input.Replace("\t", "");
            input = input.Replace("\n", "");
            input = input.Replace("\r", "");

            // secondly, do a quick check for invalid characters
            if (!CheckInvalidChars(input))
            {
                // return null;
                throw new FormatException("This expression contains some unrecognized characters. Cannot be evaluated.");
            }

            if (input.Contains("(") || input.Contains(")"))
            {
                if (!ValidParenthesys(input))
                {
                    //return null;
                    throw new FormatException("This expression has an unmatching number of opening and closing parantheses. Cannot be evaluated.");
                }

                // a listing of substitutions to make
                // as expressions within parantheses are evaluated and resolved
                Dictionary<string, string> Changes = new Dictionary<string, string>();

                bool openPar = false;
                int openIndex = 0;
                for (int i = 0; i < input.Length; i++)
                {
                    if (!openPar)
                    {
                        if (input[i] == '(')
                        {
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
                                string evaluatedSubInput = Evaluator(subInput).ToString();
                                Changes.Add(subInput, evaluatedSubInput);
                            }
                            openPar = false;
                            openIndex = 0;
                        }
                    }
                }

                foreach (var item in Changes.Keys)
                {
                    input = input.Replace(item, Changes[item]);
                }
                input = input.Replace("(", "");
                input = input.Replace(")", "");
            }

            List<string> ParsedOperations = OperationParser(input);
            if (ParsedOperations != null)
            {
                return OperationEvaluator(ref ParsedOperations);
            }
            else
            {
                // throwing a FormatException would be better than a NullReferenceException
                // since a NullReference just means something somewhere was null lol
                throw new FormatException("This expression contains some unrecognized characters. Cannot be evaluated.");
            }
        }

        // checks to make sure opening parantheses == closing parantheses (i.e. "(())" is good, "(()" is not)
        static bool ValidParenthesys(string input)
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
        static double OperationEvaluator(ref List<string> OperationList)
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
        static List<string> OperationParser(string input)
        {
            List<string> Operations = new List<string>();
            int index = 0;
            while (index < input.Length)
            {
                if (CharEvaluator(input[index]) == 0) // number, digit separator
                {
                    string SubEvaluator = "";

                    for (int i = index; i < input.Length; i++)
                    {
                        if (CharEvaluator(input[i]) == 0)
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
                            index = i;
                            break;
                        }
                    }
                }
                else if (CharEvaluator(input[index]) == 1) // operation
                {
                    Operations.Add(input[index].ToString());
                    index++;
                }
                else // paranthesis, or invalid character (parantheses should not be showing up at this point)
                {
                    return null; // ends up triggering FormatException
                }
            }
            return Operations;
        }
        static int CharEvaluator(char input)
        {
            return CharEvaluator(input.ToString());
        }
        static int CharEvaluator(string input)
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

        static bool CheckInvalidChars(string input)
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
