// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// Represents formulas written in standard infix notation using standard precedence
    /// rules.  The allowed symbols are non-negative numbers written using double-precision 
    /// floating-point syntax (without unary preceeding '-' or '+'); 
    /// variables that consist of a letter or underscore followed by 
    /// zero or more letters, underscores, or digits; parentheses; and the four operator 
    /// symbols +, -, *, and /.  
    /// 
    /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
    /// a single var, "x y" consists of two variables "x" and y; "x23" is a single var; 
    /// and "x 23" consists of a var "x" and a number "23".
    /// 
    /// Associated with every formula are two delegates:  a normalizer and a validator.  The
    /// normalizer is used to convert variables into a canonical form, and the validator is used
    /// to add extra restrictions on the validity of a var (beyond the standard requirement 
    /// that it consist of a letter or underscore followed by zero or more letters, underscores,
    /// or digits.)  Their use is described in detail in the constructor and method comments.
    /// </summary>
    public class Formula
    {

        // Hashset of variables that will be all in normalized form for computations 
        private HashSet<string> variables;
        // list to keep track of all tokens in the formula
        private List<string> tokens;


        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) : this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a var v such that normalize(v) is not a legal var, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a var v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            // check to see if there is atleast one token in tokens - "Check if string is empty or all spaces in C#" (StackOverflow)
            if (String.IsNullOrWhiteSpace(formula) || formula.Equals(""))
            {
                throw new FormulaFormatException("Your formula has nothing in it, " +
                    "try inputting a new expression");
            }

            // create our list of tokens in our formula so we can enumerate through it
            List<string> tokenEnum = new List<string>(GetTokens(formula));
            // initialize our list
            tokens = new List<string>();
            // initialize our hashset 
            variables = new HashSet<string>();

            // start/end tokens to be intitiated and validated
            string startToken = tokenEnum[0];
            string endToken = tokenEnum[tokenEnum.Count - 1];
            // check if first token or last token is invalid
            if (ValidStartingToken(startToken, isValid) == false)
            {
                throw new FormulaFormatException("Invalid expression, try starting your expression with" +
                    "either a valid variable, digit, or open parenthesis. ");
            }
            else if (ValidEndToken(endToken, isValid) == false)
            {
                throw new FormulaFormatException("Invalid expression, try ending your expression with" +
                    "either a valid variable, digit, or closed parenthesis. ");
            }

            // counters for our open/closed parenthesis
            int openParen = 0;
            int closedParen = 0;
            // initializing our last token so we can keep track of 
            string lastToken = "";
            // loop through each token and go through to check if our formula is correct
            foreach (string t in tokenEnum)
            {
                // parenthesis/operator following rule 
                ParenOperFollowingRule(lastToken, t);

                // extra following rule
                ExtraFollowingRule(lastToken, t);

                // validates our tokens, throws if invalid
                TokenValidator(t);

                // normalizes variables and adds them to hashset
                NormalizeAndAdd(normalize, isValid, t);

                // check left/right parenthesis
                if (t == "(")
                {
                    // update openParentheis count
                    openParen++;
                }
                else if (t == ")")
                {
                    // update openParentheis count
                    closedParen++;
                }
                // Right parenthesis rule - if at any point we have too many closedParenth, throw exception
                else if (closedParen > openParen)
                {
                    throw new FormulaFormatException("Invalid expression input, you have " +
                        "one to many right parenthesis");
                }

                // Extra Following Rule & Parenthesis/Operator Following Rule 
                lastToken = t;
            }

            // Balanced parenthesis rule - openParen == closedParen
            if (openParen != closedParen)
            {
                throw new FormulaFormatException("Invalid expression input, " +
                    "you have an unbalanced number of parenthesis");
            }

        }


        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a var symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a var symbol as its parameter, lookup returns the var's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            // operator stack
            Stack<string> operators = new Stack<string>();
            // value stack
            Stack<double> value = new Stack<double>();

            // using try catch because we have to return a FormulaError object and not an exception
            try
            {
                // loop through substrings and check each value
                foreach (string token in tokens)
                {
                    // reset num each time
                    double num = 0;
                    // checks to see if we can parse the integer, if we can we return true and parse num. - Microsoft docs(Parsing)
                    bool convert = double.TryParse(token, out num);
                    // if convert is an integer, then we go into if statement. 
                    if (convert == true)
                    {
                        // if integer, send to helper method
                        TokenIsInteger(operators, value, num, tokens);
                    }
                    // check if first character is a letter- https://stackoverflow.com/questions/3560393/how-to-check-first-character-of-a-string-if-a-letter-any-letter-in-c-sharp
                    else if (Char.IsLetter(token[0]) == true)
                    {
                        // use delegate as a function - returns value of variable you pass
                        // if variable has no value associated, lookup will throw exception

                        double variableName = lookup(token);

                        // if integer is found, send to helper method
                        TokenIsInteger(operators, value, variableName, tokens);
                    }

                    else if (token == "+" || token == "-")
                    {

                        if (operators.Count != 0 && (operators.Peek() == "+" || operators.Peek() == "-"))
                        {
                            //send to helper method to operate expression and send to value stack
                            PopTwoValues(operators, value);

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
                        if (operators.Peek() == "+" || operators.Peek() == "-")
                        {
                            // send to helper method to operate expression and send to value stack
                            PopTwoValues(operators, value);
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
                                PopTwoValues(operators, value);
                            }
                        }
                    }

                    // if last token just push integer to value stack
                    else if (token.Equals(tokens.Count - 1))
                    {
                        double.TryParse(token, out num);
                        value.Push(num);
                    }
                }

                // result variable
                double result = 0;

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
                    if (operators.Count == 1 && value.Count == 2)
                    {
                        // send to helper method to operate expression and send to value stack
                        PopTwoValues(operators, value);
                        // pop the last value and return result
                        result = value.Pop();
                        return result;
                    }
                    else
                    {
                        // send to helper method to operate expression and send to value stack
                        PopTwoValues(operators, value);

                        // loops over value count incase there are any leftover values
                        while (value.Count != 1)
                        {
                            // send to helper method to operate expression and send to value stack
                            PopTwoValues(operators, value);
                        }

                        // Pop the last value in stack and return the final answer. 
                        result = value.Pop();
                        return result;
                    }
                }
            }
            // if we divide by 0 we catch this error and return our FormulaError
            catch (ArithmeticException)
            {
                return new FormulaError("Division by Zero occured.");
            }
            // if we have a variable with an unknown value
            catch (ArgumentException)
            {
                return new FormulaError("Unknown variable with no associated value");
            }
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            // return our hashset of normalized variables
            return variables;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            // initialize our empty formula string
            string expression = "";
            // loop through our formula and add to string
            for (int i = 0; i < tokens.Count; i++)
            {
                // if we hit an digit, parse it, 
                double digit = 0;
                if (Double.TryParse(tokens[i], out digit))
                {
                    // Rounds the double to 3 decimal places incase we get digits like 2.000001 - http://net-informations.com/q/faq/round.html
                    expression += Math.Round(Double.Parse(tokens[i]), 3).ToString();
                }
                else
                {
                    // else, add the elements to the expression
                    expression += tokens[i];
                }
            }
            // returns our expression as a common formatted string
            return expression;
        }

        /// <summary>
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and var tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object obj)
        {
            // check if object is a formula
            if (obj is Formula)
            {
                // convert to a common format
                string f1 = obj.ToString();
                string f2 = this.ToString();
                // use gethashcodes
                int hash1 = f1.GetHashCode();
                int hash2 = f2.GetHashCode();

                // check if the formulas are equal by hashcodes
                if (hash1 == hash2)
                {
                    return true;
                }
            }

            // if not, return false
            return false;
        }

        /// <summary>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return true.  If one is
        /// null and one is not, this method should return false.
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            return f1.Equals(f2);
        }

        /// <summary>
        /// Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// Note that if both f1 and f2 are null, this method should return false.  If one is
        /// null and one is not, this method should return true.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !(f1.Equals(f2));
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            // returns hashcode for object sent in (this)
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
            // Patterns for individual tokens
            String lpPattern = @"\(";
            String rpPattern = @"\)";
            String opPattern = @"[\+\-*/]";
            String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
            String spacePattern = @"\s+";

            // Overall pattern
            String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                            lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

            // Enumerate matching tokens that don't consist solely of white space.
            foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
            {
                if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
                {
                    yield return s;
                }
            }

        }

        // HELPER METHODS FOR EVALUATOR

        /// <summary>
        /// This is a helper method for if the given token is an integer/variable that contains an integer
        /// </summary>
        /// <param name="operators">operators stack</param>
        /// <param name="value">value stack</param>
        /// <param name="token">token needed to be pushed to value stack</param>
        public static void TokenIsInteger(Stack<string> operators, Stack<double> value, double token, List<string> tokens)
        {
            // need to check if * or / is at top operators stack
            if (operators.Count != 0 && (operators.Peek() == "*" || operators.Peek() == "/"))
            {
                //pop the value stack
                double poppedNum = value.Pop();
                // pop the operators stack
                string oper = operators.Pop();
                // send to ExpressionExecuter to apply operation and push to value stack
                value.Push((double)ExpressionExecuter(oper, token, poppedNum));
            }
            else
            {
                // if not, push value onto stack
                value.Push(token);
            }
        }

        /// <summary>
        /// This helper method is used anytime we need to pop the value stack twice 
        /// and the operators stack once
        /// </summary>
        /// <param name="operators"> operators stack</param>
        /// <param name="value">values stack</param>
        public static void PopTwoValues(Stack<string> operators, Stack<double> value)
        {
            // pop value stack twice
            double poppedNum1 = value.Pop();
            double poppedNum2 = value.Pop();
            // pop operator stack once
            string oper = operators.Pop();
            // send to ExpressionExecuter to apply operation and push to value stack
            value.Push((double)ExpressionExecuter(oper, poppedNum1, poppedNum2));
        }

        /// <summary>
        /// Helper method that goes through and applies neccessary operations then pushes to values stack
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
        private static object ExpressionExecuter(string operation, double num1, double num2)
        {
            if (operation.Equals("*"))
            {
                return num2 * num1;
            }
            if (operation.Equals("/"))
            {
                // check if division by 0 occurs, if so throw exception
                if (num1 == 0)
                {
                    throw new ArithmeticException();
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

        // HELPER METHODS FOR CONSTRUCTOR

        /// <summary>
        /// This is a helper method that checks if the first token 
        /// is either a digit, variable or open parenthesis
        /// </summary>
        /// <param name="startToken"></param>
        /// <param name="isValid"></param>
        /// <returns>Returns true if token is valid and false if not</returns>
        private static bool ValidStartingToken(string startToken, Func<string, bool> isValid)
        {
            // variable pattern, letter followed by a number
            string variablePattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            // from Evaluator.cs 
            double digit = 0;
            bool isDigit = Double.TryParse(startToken, out digit);

            // check to see if token is a valid start token, if not return false -> "How to check first character of a string if a letter, any letter in C#"
            if (isDigit == false && startToken != "(" && !Regex.IsMatch(startToken, variablePattern))
            {
                return false;
            }
            else
            {
                // start token is valid
                return true;
            }
        }

        /// <summary>
        /// This is a helper method that checks if the end token 
        /// is either a digit, variable or closed parenthesis
        /// </summary>
        /// <param name="endToken"></param>
        /// <param name="isValid"></param>
        /// <returns>Returns true if token is valid and false if not</returns>
        private static bool ValidEndToken(string endToken, Func<string, bool> isValid)
        {
            // variable pattern, letter followed by a number 
            string variablePattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
            // from Evaluator.cs 
            double digit = 0;
            bool isDigit = Double.TryParse(endToken, out digit);

            // check to see if token is a valid end token, if not return false -> "How to check first character of a string if a letter, any letter in C#"
            if (isDigit == false && endToken != ")" && !Regex.IsMatch(endToken, variablePattern))
            {
                return false;
            }
            else
            {
                // end token is valid
                return true;
            }
        }

        /// <summary>
        /// Helper method that checks if we can pass this rule 
        /// 
        /// Parenthesis/Operator Following Rule - Any token that immediately 
        /// follows an opening parenthesis or an operator must be either a number, a variable, or an opening parenthesis.
        /// </summary>
        /// <param name="token">our current token</param>
        /// <param name="lastToken">the previous token </param>
        private static void ParenOperFollowingRule(string lastToken, string token)
        {
            // first check if last token is nothing, if so just return
            if (lastToken == "")
            {
                return;
            }
            else
            {
                // then check if last token is either an open parenthesis or an operator
                if (lastToken == "(" || ContainsOperator(lastToken))
                {
                    // variable pattern, letter followed by a number 
                    string variablePattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
                    // from Evaluator.cs 
                    double digit = 0;
                    bool isDigit = Double.TryParse(token, out digit);
                    // if true, if our current token is not a digit, variable or
                    if (isDigit == false && token != "(" && !Regex.IsMatch(token, variablePattern))
                    {
                        // if not, throw
                        throw new FormulaFormatException("Invalid expression input, " +
                            "try entering a variable, digit, or open parenthesis.");
                    }

                }
                // else return
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// Helper method that checks if we can pass this rule 
        /// 
        /// Extra Following Rule - Any token that immediately follows a number, a variable, 
        /// or a closing parenthesis must be either an operator or a closing parenthesis.
        /// </summary>
        /// <param name="token">our current token</param>
        /// <param name="lastToken">the previous token </param>
        private static void ExtraFollowingRule(string lastToken, string token)
        {
            // first check if last token is nothing, if so just return
            if (lastToken == "")
            {
                return;
            }
            else
            {
                // variable pattern, letter followed by a number 
                string variablePattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
                // from Evaluator.cs 
                double digit = 0;
                bool isDigit = Double.TryParse(lastToken, out digit);
                // then check if last token is either a digit, closed parenthesis or a variable
                if (isDigit == true || lastToken == ")" || Regex.IsMatch(lastToken, variablePattern))
                {
                    // check if our current token is an operator or a closed parentheis
                    if (token != ")" && !ContainsOperator(token))
                    {
                        // if not, throw
                        throw new FormulaFormatException("Invalid expression input, " +
                            "try entering an operator or a closing parenthesis.");
                    }

                }
                // else return
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// This method goes through and validates if token is a correct input
        /// </summary>
        /// <param name="token">our current token</param>
        /// <exception cref="FormulaFormatException"></exception>
        private void TokenValidator(string token)
        {
            // regex pattern for our variable pattern
            string variablePattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";

            // from Evaluator.cs 
            double digit = 0;
            bool isDigit = Double.TryParse(token, out digit);

            // Specific Token Rule - has to be either an operator, digit(including sci notation), or parenthesis
            if (!(isDigit == true || token == "(" || token == ")" || ContainsOperator(token)
                 || !Regex.IsMatch(variablePattern, token)))
            {
                // if fails all of this, throw exception
                throw new FormulaFormatException("Invalid token input, try inputting a new token in expression");
            }
            else
            {
                // if it is a valid token, return
                return;
            }
        }

        /// <summary>
        /// Small helper method that tells use if a token is an operator
        /// </summary>
        /// <param name="token">current token</param>
        /// <returns>true if token is an operator and false if not</returns>
        private static bool ContainsOperator(string token)
        {
            // if token is an operator, return true
            if (token == "+" || token == "-" || token == "*" || token == "/")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// This method normalizes our variable then adds to hashset if valid
        /// </summary>
        /// <param name="normalize">our normalizer func</param>
        /// <param name="isValid">our normalizer func</param>
        /// <param name="token">our current token</param>
        /// <exception cref="FormulaFormatException">Throws exception if we have an invalid variable</exception>
        private void NormalizeAndAdd(Func<string, string> normalize, Func<string, bool> isValid, string token)
        {
            // regex patter for our variable pattern
            string variablePattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";

            // first check if it is a variable 
            if (Regex.IsMatch(token, variablePattern))
            {
                // normalize tokens
                string normTokens = normalize(token);
                // if they are valid, add to hashmap, if not throw
                if (isValid(normTokens))
                {
                    variables.Add(normTokens);
                    tokens.Add(normTokens);
                }
                else
                {
                    throw new FormulaFormatException("Invalid variable input, " +
                        "try inputting a valid variable");
                }
            }
            // it is not a variable add
            else
            {
                tokens.Add(token);
            }
        }


    }



    /// <summary>
    /// Used to report syntactic errors in the argument to the Formula constructor.
    /// </summary>
    public class FormulaFormatException : Exception
    {
        /// <summary>
        /// Constructs a FormulaFormatException containing the explanatory message.
        /// </summary>
        public FormulaFormatException(String message) : base(message)
        {
        }
    }

    /// <summary>
    /// Used as a possible return value of the Formula.Evaluate method.
    /// </summary>
    public struct FormulaError
    {
        /// <summary>
        /// Constructs a FormulaError containing the explanatory reason.
        /// </summary>
        /// <param name="reason"></param>
        public FormulaError(String reason)
            : this()
        {
            Reason = reason;
        }

        /// <summary>
        ///  The reason why this FormulaError was created.
        /// </summary>
        public string Reason { get; private set; }
    }
}

