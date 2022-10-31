using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    /// <summary> 
    /// Author:    Jonathan Gage Buss 
    /// Partner:   none 
    /// Date:      January 13, 2022
    /// Course:    CS 3500, University of Utah, School of Computing 
    /// Copyright: CS 3500 and Jonathan Gage Buss - This work may not be copied for use in Academic Coursework. 
    /// 
    /// I, Jonathan Gage Buss, certify that I wrote this code from scratch and did not copy it in part or whole from  
    /// another source.  All references used in the completion of the assignment are cited in my README file. 
    /// 
    /// File Contents 
    /// 
    /// This library class evaluates an expression similar to how a spreadsheet would. With the use
    /// of variables and without the use of variables. 
    /// </summary>
    public static class Evaluator
    {
        /// <summary>
        /// This is a delegate function that allows us to pass it through other functions
        /// </summary>
        /// <param name="variable">our variable</param>
        /// <returns>returns the looked up variable in the form of an integer</returns>
        public delegate int Lookup(String variable);

        /// <summary>
        /// Helper method that goes through and applies neccessary operations
        /// </summary>
        /// <param name="operation">
        /// {*, /, +, -)
        /// </param>
        /// <param name="num1">
        /// If multiplication/division -> our current value number
        /// If addition/subtraction -> one of two popped numbers from stack or current value numbers
        /// </param>
        /// <param name="num2">
        /// If multiplication/division -> number popped from stack
        /// If addition/subtraction -> second popped number
        /// </param>
        /// <returns>
        /// returns our new number after using expression
        /// </returns>
        public static int ExpressionExecuter(string operation, int num1, int num2)
        {
            if(operation.Equals("*")) {
                return num2 * num1;
            }
            if (operation.Equals("/"))
            {
                // check if division by 0 occurs, if so throw exception
                if(num1 == 0)
                {
                    throw new ArgumentException("Cannot divide by 0");
                }
                return num2 / num1;
            }
            if (operation.Equals("+"))
            {
                return num2 + num1;
            }          
            if (operation.Equals("-"))
            {
                return num2 - num1;
            }
            // return our evaluated expression
            return num2;
        }

        /// <summary>
        /// This helper method is used anytime we need to pop the value stack twice 
        /// and the operators stack once
        /// </summary>
        /// <param name="operators"> operators stack</param>
        /// <param name="value">values stack</param>
        public static void popTwoValues(Stack<string> operators, Stack<int> value)
        {   
            // throw if value stack does not have two variables
            if(value.Count() < 2)
            {
                throw new ArgumentException("Invalid expression");
            }          

            // pop value stack twice
            int poppedNum1 = value.Pop();
            int poppedNum2 = value.Pop();
            // pop operator stack once
            string oper = operators.Pop();
            // send to ExpressionExecuter to apply operation and push to value stack
            value.Push(ExpressionExecuter(oper, poppedNum1, poppedNum2));
        }

        /// <summary>
        /// This is a helper method for if the given token is an integer/variable that contains an integer
        /// </summary>
        /// <param name="operators">operators stack</param>
        /// <param name="value">value stack</param>
        /// <param name="token">token needed to be pushed to value stack</param>
        public static void tokenIsInteger(Stack<string> operators, Stack<int> value, int token, string[] substrings)
        {
            // need to check if * or / is at top operators stack
            if(operators.Count != 0 && (operators.Peek() == "*" || operators.Peek() == "/"))
            {
                //pop the value stack
                int poppedNum = value.Pop();
                // pop the operators stack
                string oper = operators.Pop();
                // send to ExpressionExecuter to apply operation and push to value stack
                value.Push(ExpressionExecuter(oper, token, poppedNum));
            }
            else
            {
                // if not, push value onto stack
                value.Push(token);
            }

        }

        /// <summary>
        /// This function takes in a string representing an expression
        /// and does the computations needed for that given expression.
        /// </summary>
        /// <param name="expression">string expression input</param>
        /// <param name="variableEvaluator">cell number inputed. Ex) "A7, C1, Lookup1"</param>
        /// <returns>Returns the computed integer</returns>
        public static int Evaluate(String expression, Lookup variable)
        {
            // operator stack
            Stack<string> operators = new Stack<string>();
            // value stack
            Stack<int> value = new Stack<int>();

            // removes whitespace - https://codereview.stackexchange.com/questions/84763/evaluating-an-expression-with-integers-and-as-well-as
            expression = expression.Replace("\\s+", "");

            // check if the expression is valid
            if (expression == "" || (expression.Contains(")") && !(expression.Contains("("))))
            {
                throw new ArgumentException("Invalid expression input");
            }
           
            // splits string into token - from A1 docs
            string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            // loop through substrings and check each value
            foreach (string token in substrings)
            {

                // if randomly getting whitespace, continue on (only happens with parenthesis)
                if(token == "" || token == " ")
                {
                    continue;
                }

                // reset num each time
                int num = 0;
                // checks to see if we can parse the integer, if we can we return true and parse num. - Microsoft docs(Parsing)
                bool convert = int.TryParse(token, out num);
                // if convert is an integer, then we go into if statement. 
                if (convert == true)
                {
                    // if integer, send to helper method
                    tokenIsInteger(operators, value, num, substrings);
                }
                // check if first character is a letter- https://stackoverflow.com/questions/3560393/how-to-check-first-character-of-a-string-if-a-letter-any-letter-in-c-sharp
                else if (Char.IsLetter(token[0]) == true)
                { 
                    // checks if first character in token is a letter, 
                    // then checks if last character if number. 
                    if(char.IsLetter(token[0]) && !(char.IsDigit(token[token.Length - 1])))
                    {
                        throw new ArgumentException("Invalid Variable");
                    }

                    // use delegate as a function - returns value of variable you pass
                    int variableName = variable(token);
                    // throw if lookup has no value else, apply operation
                    if (variableName.Equals(null))
                    {
                        throw new ArgumentException("Variable has no value");
                    }
                    else
                    {
                        // if integer is found, send to helper method
                        tokenIsInteger(operators, value, variableName, substrings);
                    }
                    
                }

                else if(token == "+" || token == "-")
                {

                    if (operators.Count != 0 && (operators.Peek() == "+" || operators.Peek() == "-"))
                    {
                        //send to helper method to operate expression and send to value stack
                        popTwoValues(operators, value);
                        
                    }

                    // otherwise push to stack
                    operators.Push(token);

                }
                
                else if (token == "*" || token == "/")
                {
                    // push regardless of what is at top of stack
                    operators.Push(token);
                }
                
                else if (token == "(")
                {
                    // push regardless of what is at top of stack
                    operators.Push(token);
                }
                
                else if (token == ")")
                {
                    // check to see if +/- is at top of stack, if so apply operation
                    if(operators.Peek() == "+" || operators.Peek() == "-")
                    {
                        // send to helper method to operate expression and send to value stack
                        popTwoValues(operators, value);
                    }
                    // the top of the operator stack should be a '('.Pop it if not empty
                    if (operators.Count != 0)
                    {
                        if (operators.Peek() == "(")
                        {
                            operators.Pop();
                        }                     
                    }
                    
                    // if * or / is at the top of the operator stack
                    if (operators.Count != 0)
                    {
                        if (operators.Peek() == "*" || operators.Peek() == "/")
                        {
                            // send to helper method to operate expression and send to value stack
                            popTwoValues(operators, value);
                        }
                    }
                
                }

                // if last token just push integer to value stack
                else if(token.Equals(substrings.Length-1))
                {
                    int.TryParse(token, out num);
                    value.Push(num);
                }
                // if token is none of these, invalid expression
                else
                {
                    throw new ArgumentException("Invalid expression");
                }
                

            }

            // result variable
            int result = 0;

            // first check if we have any values with no operators
            if(value.Count > 1 && operators.Count == 0)
            {
                // if we do, then we have an invalid expression input
                throw new ArgumentException("Invalid expression input");
            }

            // if operator stack is empty
            if (operators.Count == 0 || operators.Contains("(") == true)
            {
                // pop the last value in stack and return the final answer. 
                result = value.Pop();
                return result;
            }
            // if not empty apply last operation. 
            else
            {
                // check if there is exactly one value on operators stack and two on values stack
                if(operators.Count == 1 && value.Count == 2)
                {
                    // send to helper method to operate expression and send to value stack
                    popTwoValues(operators, value);
                    // pop the last value and return result
                    result = value.Pop();
                    return result;
                }
                else
                {
                    // send to helper method to operate expression and send to value stack
                    popTwoValues(operators, value);

                    // loops over value count incase there are any leftover values
                    while(value.Count != 1)
                    {
                        // send to helper method to operate expression and send to value stack
                        popTwoValues(operators, value);
                    }

                    // Pop the last value in stack and return the final answer. 
                    result = value.Pop();
                    return result;
                }
               
            }

        }
    }
}