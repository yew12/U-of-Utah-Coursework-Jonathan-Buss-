// See https://aka.ms/new-console-template for more information

using FormulaEvaluator;

namespace Test_The_Evaluator_App
{
    class Test_the_library
    {
        /// <summary>
        /// Our main class to utilize our library. Consists of various tests broken up into 
        /// their own methods.
        /// Can comment out each method if need be. 
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        { 
            // Some simple multiplication tests
            void multiplicationTests()
            {
                // Basic test
                Console.WriteLine($"1 * 2 = {Evaluator.Evaluate("1 * 2", null)}");
                // A: 10
                Console.WriteLine($"1 * 2 * 5 = {Evaluator.Evaluate("1 * 2 *5", null)}");
                // A: 12
                Console.WriteLine($"24 * 2 = {Evaluator.Evaluate("24 * 2", null)}");
            }
            multiplicationTests();

            // Some simple division tests
            void divisionTests()
            {
                // A: 5
                Console.WriteLine($"10 / 2 = {Evaluator.Evaluate("10 / 2", null)}");
                // A: ArgumentException error
                Console.WriteLine($"24 * 0 = {Evaluator.Evaluate("24 / 0", null)}");
            }
            divisionTests();

            // Some simple addition tests
            void additionTests()
            {
                // A: 5
                Console.WriteLine($"10 + 2 = {Evaluator.Evaluate("10 + 2", null)}");
                // A: 24
                Console.WriteLine($"24 + 0 = {Evaluator.Evaluate("24 + 0", null)}");
            }
            additionTests();

            // Some simple subtraction tests
            void subtractionTests()
            {
                // A: 8
                Console.WriteLine($"10 - 2 = {Evaluator.Evaluate("10 - 2", null)}");
                // A: 12
                Console.WriteLine($"24 - 12 = {Evaluator.Evaluate("24 - 12", null)}");
            }
            //subtractionTests();

            // Some simple subtraction tests
            void multiplicationAndDivisionTests()
            {
                // A: 5
                Console.WriteLine($"5 * 2 / 2 = {Evaluator.Evaluate("5 * 2 / 2 ", null)}");
                // A: 12.5 or 12 by rounding floor from c#
                Console.WriteLine($"5 * 2 / 2 * 5 / 2 = {Evaluator.Evaluate("5 * 2 / 2 * 5 / 2", null)}");
            }
            multiplicationAndDivisionTests();

            // Some simple parenthesis tests
            void parenthesisTests()
            {
                // with addition
                Console.WriteLine($"( 5 + 2 ) = {Evaluator.Evaluate("( 5 + 2 ) ", null)}");
                // with multiplication
                Console.WriteLine($"( 5 * 2 ) = {Evaluator.Evaluate("( 5 * 2 ) ", null)}");
                // with division
                Console.WriteLine($"( 10 / 2 ) = {Evaluator.Evaluate("( 10 / 2 ) ", null)}");
                // with division - moved parenthesis
                Console.WriteLine($"( 10 ) / 2 = {Evaluator.Evaluate("( 10 ) / 2 ", null)}");
                // with subtraction
                Console.WriteLine($"( 10 - 11 ) = {Evaluator.Evaluate("( 10 - 11 ) ", null)}");
                // with subtraction - moved parenthesis
                Console.WriteLine($"( 10 ) - 11 = {Evaluator.Evaluate("( 10 ) - 11 ", null)}");
            }
            parenthesisTests();

            // Some more complex parenthesis tests
            void parenthesisWithNonParenthesisTests()
            {
                // A: 27
                Console.WriteLine($"(2+3) * 5 + 2 = {Evaluator.Evaluate("(2+3) * 5 + 2", null)}");
                // A: 19
                Console.WriteLine($" 2+ (3 * 5) + 2 = {Evaluator.Evaluate("2+ (3 * 5) + 2 ", null)}");
                // A: -15
                Console.WriteLine($"2 - (3 * 5 + 2) = {Evaluator.Evaluate("2 - (3 * 5 + 2) ", null)}");
                // A: 19
                Console.WriteLine($"(2+ 3 * 5 + 2) = {Evaluator.Evaluate("(2+ 3 * 5 + 2)", null)}");
                // A: 17
                Console.WriteLine($"2+ 3 * 5 = {Evaluator.Evaluate("2+ 3 * 5 ", null)}");
            }
            parenthesisWithNonParenthesisTests();

            // Some more complex parenthesis tests
            void embeddedParenthesis()
            {
                // A: 2
                Console.WriteLine($"((10-5)*2)/5 = {Evaluator.Evaluate("((10-5)*2)/5", null)}");
                // A: 2 - added multiple parenthesis
                Console.WriteLine($"(((10-5)*2)/5) = {Evaluator.Evaluate("(((10-5)*2)/5)", null)}");
                // A: 2 - even with a missing parenthesis at the end
                Console.WriteLine($"(((10-5)*2)/5 = {Evaluator.Evaluate("(((10-5)*2)/5", null)}");

            }
            embeddedParenthesis();

            // Some more complex parenthesis tests
            void throwTests()
            {
                // throw with no parenthesis
                Console.WriteLine($"((10-5)*2)/0 = {Evaluator.Evaluate("((10-5)*2)/0", null)}");

                // throw with parenthesis at end
                Console.WriteLine($"(((10-5)*2)/0) = {Evaluator.Evaluate("(((10-5)*2)/0)", null)}");

                // throw if value stack is empty w/ parenthesis
                Console.WriteLine($"( + ) = {Evaluator.Evaluate("( + )", null)}");

                // throw if value stack is empty with out parenthesis
                Console.WriteLine($"+ = {Evaluator.Evaluate("+", null)}");

                // throw if we have an invalid input with "p"
                Console.WriteLine($"p = {Evaluator.Evaluate("p", null)}");
            }
            throwTests();

            // simple throw tests for invalid expression (unrecognized expression)
            void invalidExpressionTests()
            {
                Console.WriteLine($"] = {Evaluator.Evaluate("]", null)}");

                Console.WriteLine($",. = {Evaluator.Evaluate(",.", null)}");
            }
            invalidExpressionTests();

            //** TESTS BELOW ARE USED FROM PROFESSOR'S PIAZZA POST **\\
            void makeSureThatTheseAreEqual(int expected, int actual)
            {
                if (expected != actual)
                {
                    Console.WriteLine("Expected {0}, but got {1}!", expected, actual);
                }
            }

            int returnFiveIfVariableIsABC123(String variable_name)
            {
                if (variable_name == "ABC123")
                {
                    return 5;
                }
                else
                {
                    return 0;
                }
            }

            int returnOne(String variable_name)
            {
                return 1;
            }

            makeSureThatTheseAreEqual(0, Evaluator.Evaluate("Z7", returnFiveIfVariableIsABC123));
            makeSureThatTheseAreEqual(5, Evaluator.Evaluate("ABC123", returnFiveIfVariableIsABC123));

            makeSureThatTheseAreEqual(2, Evaluator.Evaluate("1+A1", returnOne));
            makeSureThatTheseAreEqual(3, Evaluator.Evaluate("1+2*A1", returnOne));

            makeSureThatTheseAreEqual(0, Evaluator.Evaluate("17*Z9", returnFiveIfVariableIsABC123));
            makeSureThatTheseAreEqual(85, Evaluator.Evaluate("17*ABC123", returnFiveIfVariableIsABC123));

            // cannot divide by 0 case. 
            makeSureThatTheseAreEqual(0, Evaluator.Evaluate("17/ABC12", returnFiveIfVariableIsABC123));

        }
    }
}