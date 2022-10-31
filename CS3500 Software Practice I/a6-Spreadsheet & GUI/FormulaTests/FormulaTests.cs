using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;
using System.Collections.Generic;

namespace FormulaTests
{
    /// <summary>
    /// Testing class for our Formula class. 
    /// </summary>
    [TestClass]
    public class FormulaTests
    {

        /// <summary>
        /// Utilizes our Formula constructor
        /// </summary>
        [TestMethod]
        [TestCategory("1")]
        public void SimpleConstructorTest()
        {
            Formula t = new Formula("x2+y3");

        }

        /// <summary>
        /// The expression is empty,
        /// throws a FormulaFormatException with an explanatory Message.
        /// </summary>
        [TestMethod()]
        [TestCategory("2")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void SimpleEmptyFormula()
        {
            Formula t = new Formula("");
        }

        /// <summary>
        /// The expression is empty with one space
        /// throws a FormulaFormatException with an explanatory Message.
        /// </summary>
        [TestMethod()]
        [TestCategory("3")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void SimpleEmptyFormulaWithOneSpace()
        {
            Formula t = new Formula(" ");
        }

        /// <summary>
        /// The expression is empty with several spaces
        /// throws a FormulaFormatException with an explanatory Message.
        /// </summary>
        [TestMethod()]
        [TestCategory("4")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void SimpleEmptyFormulaWithSeveralSpaces()
        {
            Formula t = new Formula("                 ");
        }

        /// <summary>
        /// The expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// </summary>
        [TestMethod()]
        [TestCategory("5")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void SimpleIncorrectVariable()
        {
            Formula t = new Formula("3d");
        }

        /// <summary>
        /// The expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// </summary>
        [TestMethod()]
        [TestCategory("6")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void SimpleIncorrectVariableWithSpace()
        {
            Formula t = new Formula("A 1");
        }

        /// <summary>
        /// The expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// </summary>
        [TestMethod()]
        [TestCategory("7")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void SimpleIncorrectExpression()
        {
            Formula t = new Formula("2 x+y 3");
        }

        /// <summary>
        /// These next two tests (TestCategory "8", "9", "10"), check that
        /// our validStartToken and validEndToken helper methods is throwing correctly when an
        /// incorrect starting/end token is inputted. 
        /// </summary>
        [TestMethod()]
        [TestCategory("8")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ValidStartTokenTest_RightParenthesis()
        {
            Formula t = new Formula(")");
        }

        /// <summary>
        /// See TestCategory 8
        /// </summary>
        [TestMethod()]
        [TestCategory("9")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ValidStartTokenTest_Operator()
        {
            Formula t = new Formula("+");
        }

        /// <summary>
        /// See TestCategory 8
        /// </summary>
        [TestMethod()]
        [TestCategory("10")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ValidEndTokenTest_Operator()
        {
            Formula t = new Formula("(");
        }

        /// <summary>
        /// See TestCategory 8
        /// </summary>
        [TestMethod()]
        [TestCategory("11")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ValidEndTokenTest_InvalidVariable()
        {
            Formula t = new Formula("a");
        }

        /// <summary>
        /// Tests our parenOperFollowingRule helper method to make sure
        /// that is throws when supposed to
        /// </summary>
        [TestMethod()]
        [TestCategory("12")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ParenOperFollowingRuleTest()
        {
            Formula t = new Formula("(+");
        }

        /// <summary>
        /// Tests our parenOperFollowingRule helper method to make sure
        /// that is throws when supposed to
        /// </summary>
        [TestMethod()]
        [TestCategory("13")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void ExtraFollowingRuleTest()
        {
            Formula t = new Formula("(1+1)1");
        }

        /// <summary>
        /// This method tests for an exception on unequal parenthesis
        /// </summary>
        [TestMethod]
        [TestCategory("14")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void SimpleUnequalParenthesis()
        {
            Formula t = new Formula("((1+1)1");
        }

        /// <summary>
        /// A simple toString() test
        /// </summary>
        [TestMethod()]
        [TestCategory("15")]
        public void SimpleToStringTest()
        {
            Formula t = new Formula("x1 + y1");
            Assert.AreEqual(t.ToString(), "x1+y1");
        }

        /// <summary>
        /// A simple toString() test 
        /// </summary>
        [TestMethod()]
        [TestCategory("16")]
        public void SimpleToStringTest2()
        {
            Formula t = new Formula("x + Y");
            Assert.AreEqual(t.ToString(), "x+Y");
        }

        /// <summary>
        /// A simple normalize test
        /// </summary>
        [TestMethod()]
        [TestCategory("17")]
        public void SimpleNormalize()
        {
            Formula t = new Formula("x1+x2", s => s.ToUpper(), s => true);
            Formula normalized = new Formula("X1 + X2");
            Assert.AreEqual(normalized, t);

        }

        /// <summary>
        /// Utilizes our Formula constructor with scientific notation
        /// </summary>
        [TestMethod]
        [TestCategory("18")]
        public void SimpleConstructorTestWithScientificNotation()
        {
            Formula t = new Formula("5e4");
        }

        /// <summary>
        /// Simple addition test
        /// </summary>
        [TestMethod]
        [TestCategory("19")]
        public void SimpleAdditionTest()
        {
            Formula t = new Formula("1+1");
            object eval = t.Evaluate(s => 0);
            Assert.AreEqual(2.0, eval);
        }

        /// <summary>
        /// Simple subtraction test
        /// </summary>
        [TestMethod]
        [TestCategory("20")]
        public void SimpleSubtractionTest()
        {
            Formula t = new Formula("1-1");
            object eval = t.Evaluate(s => 0);
            Assert.AreEqual(0.0, eval);
        }

        /// <summary>
        /// Simple multiplication test
        /// </summary>
        [TestMethod]
        [TestCategory("21")]
        public void SimpleMultiplicationTest()
        {
            Formula t = new Formula("100*2");
            object eval = t.Evaluate(s => 0);
            Assert.AreEqual(200.0, eval);
        }

        /// <summary>
        /// Simple division test
        /// </summary>
        [TestMethod]
        [TestCategory("22")]
        public void SimpleDivisionTest()
        {
            Formula t = new Formula("100/2");
            object eval = t.Evaluate(s => 0);
            Assert.AreEqual(50.0, eval);
        }

        /// <summary>
        /// Simple doubles test
        /// </summary>
        [TestMethod]
        [TestCategory("23")]
        public void SimpleDoublesTest()
        {
            Formula t = new Formula("10.4203491 + 10.5843312");
            object eval = t.Evaluate(s => 0);
            Assert.AreEqual(21.004680299999997, eval);
        }

        /// <summary>
        /// Simple doubles test
        /// </summary>
        [TestMethod]
        [TestCategory("24")]
        public void SimpleAdditionWithVariableTest()
        {
            Formula t = new Formula("10 + A1");
            object eval = t.Evaluate(s => 2);
            Assert.AreEqual(12.0, eval);
        }

        /// <summary>
        /// Simple doubles test
        /// </summary>
        [TestMethod]
        [TestCategory("25")]
        public void SimpleAdditionWithSeveralVariablesTest()
        {
            Formula t = new Formula("10 + A1/ A2");
            object eval = t.Evaluate(s => 2);
            Assert.AreEqual(11.0, eval);
        }

        /// <summary>
        /// Doubles test with variables
        /// </summary>
        [TestMethod]
        [TestCategory("26")]
        public void DoublesAndVariables()
        {
            Formula t = new Formula("15.5 + A1/ A2");
            object eval = t.Evaluate(s => 2);
            Assert.AreEqual(16.5, eval);
        }

        /// <summary>
        /// Addition with doubles and scientific notation
        /// </summary>
        [TestMethod]
        [TestCategory("27")]
        public void AdditionDoublesAndScientificNotation()
        {
            Formula t = new Formula("15.5 + 10e5");
            object eval = t.Evaluate(s => 0);
            Assert.AreEqual(1000015.5, eval);
        }

        /// <summary>
        /// Addition with doubles, scientific notation and variables
        /// </summary>
        [TestMethod]
        [TestCategory("28")]
        public void AdditionDoublesScientificNotationVariables()
        {
            Formula t = new Formula("15.5 + 10e5/a2");
            object eval = t.Evaluate(s => 2);
            Assert.AreEqual(500015.5, eval);
        }

        /// <summary>
        /// MultivariableTest
        /// </summary>
        [TestMethod]
        [TestCategory("29")]
        public void MultivariableTest()
        {
            Formula t = new Formula("y1*3-8/2+4*(8-9*2)/14*x7");
            object eval = t.Evaluate(s => (s == "x7") ? 1 : 4);
            Assert.AreEqual(5.142857142857142, eval);
        }

        /// <summary>
        /// Stress test
        /// </summary>
        [TestMethod]
        [TestCategory("30")]
        public void StressTest()
        {
            Formula t = new Formula("(((((((((9+3/2*6+3+2+1+1)+5))))))))");
        }

        /// <summary>
        /// Stress test 2 - a different formula variation 
        /// </summary>
        [TestMethod]
        [TestCategory("31")]
        public void StressTest2()
        {
            Formula t = new Formula("(2+3)*6/(3+2+6*5)+((((1+1))))");
        }

        /// <summary>
        /// Stress test 3 - a different formula variation 
        /// </summary>
        [TestMethod]
        [TestCategory("32")]
        public void StressTest3()
        {
            Formula t = new Formula("(1)+1.000+(6.248583*7000000000.0)/(100000000.49294-2)");
        }

        /// <summary>
        /// Stress test 3 - a different formula variation 
        /// </summary>
        [TestMethod]
        [TestCategory("33")]
        public void StressTest4()
        {
            Formula t = new Formula("(((z9)+(1.4520)+((6.248583*g5))/(100000400.49294-a2)))");
        }

        /// <summary>
        /// Simple hashset test to make sure we are adding all of our variables correctly
        /// </summary>
        [TestMethod]
        [TestCategory("34")]
        public void SimpleHashsetTest()
        {
            Formula t = new Formula("z9*c5/b3 + 1");
            List<string> variables = new List<string>(t.GetVariables());
            Assert.AreEqual(3, variables.Count);
        }

        /// <summary>
        /// Simple hashset test with duplicate variables
        /// </summary>
        [TestMethod]
        [TestCategory("35")]
        public void HashsetTestWithDuplicates()
        {
            Formula t = new Formula("z9*c5/b3 + 1 *(b3+z4)");
            List<string> variables = new List<string>(t.GetVariables());
            Assert.AreEqual(4, variables.Count);
        }

        /// <summary>
        /// Divide by 0 FormulaError
        /// </summary>
        [TestMethod]
        [TestCategory("36")]
        public void DivisionByZero()
        {
            Formula f = new Formula("1000000/0");
            Assert.IsInstanceOfType(f.Evaluate(s => 0), typeof(FormulaError));
        }

        /// <summary>
        /// Divide by 0 FormulaError
        /// </summary>
        [TestMethod]
        [TestCategory("37")]
        public void DivisionByZeroWithVariables()
        {
            Formula f = new Formula("A1/0");
            Assert.IsInstanceOfType(f.Evaluate(s => 16), typeof(FormulaError));
        }

        /// <summary>
        /// Divide by 0 FormulaError 
        /// </summary>
        [TestMethod]
        [TestCategory("38")]
        public void DivisionByZeroWithMultivariables()
        {
            Formula f = new Formula("(A1+ A3) - (A6+Z9) / 0");
            Assert.IsInstanceOfType(f.Evaluate(s => 5), typeof(FormulaError));
        }

        /// <summary>
        /// Right parenthesis rule
        /// </summary>
        [TestMethod]
        [TestCategory("39")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void RightParenthesisRule()
        {
            Formula t = new Formula("(2+3)) + 3");
        }

        /// <summary>
        /// Balanced parenthesis rule - openParen == closedParen
        /// </summary>
        [TestMethod]
        [TestCategory("40")]
        [ExpectedException(typeof(FormulaFormatException))]
        public void BalancedParenthesisRule()
        {
            Formula t = new Formula("(((2+3))");
        }

        /// <summary>
        /// Simple hashcode test
        /// </summary>
        [TestMethod]
        [TestCategory("41")]
        public void SimpleHashCodeTest()
        {
            Formula f1 = new Formula("2+a1");
            Formula f2 = new Formula("2+a1");
            int hash1 = f1.GetHashCode();
            int hash2 = f2.GetHashCode();
            Assert.AreEqual(hash1, hash2);
        }

        /// <summary>
        /// Repeated Variable test from PS1 tests
        /// </summary>
        [TestMethod]
        [TestCategory("42")]
        public void TestRepeatedVar()
        {
            Formula t = new Formula("a4-a4*a4/a4");
            object eval = t.Evaluate(s => 3);
            Assert.AreEqual(0.0, eval);
        }
       
        /// <summary>
        /// Repeated Variable test from PS1 tests
        /// </summary>
        [TestMethod]
        [TestCategory("43")]
        public void SimpleEqualsTest()
        {
            Formula f1 = new Formula("1+1");
            Formula f2 = new Formula("1 + 1");
            bool conditional = f1.Equals(f2);
            Assert.IsTrue(conditional);
        }

        /// <summary>
        /// equals test with unequal formulas
        /// </summary>
        [TestMethod]
        [TestCategory("44")]
        public void EqualsTestWithDifferentFormulas()
        {
            Formula f1 = new Formula("1+1");
            Formula f2 = new Formula("1 + 2");
            bool conditional = f1.Equals(f2);
            Assert.IsFalse(conditional);
        }

        /// <summary>
        /// equals test with variables
        /// </summary>
        [TestMethod]
        [TestCategory("45")]
        public void EqualsTestWithVariables()
        {
            Formula f1 = new Formula("A1+1");
            Formula f2 = new Formula("A1 +     1");
            bool conditional = f1.Equals(f2);
            Assert.IsTrue(conditional);
        }

        /// <summary>
        /// equals test with doubles
        /// </summary>
        [TestMethod]
        [TestCategory("46")]
        public void EqualsTestWithDoubles()
        {
            Formula f1 = new Formula("2.000 + x7");
            Formula f2 = new Formula("2.0 + x7");
            bool conditional = f1.Equals(f2);
            Assert.IsTrue(conditional);
        }

        /// <summary>
        /// equals test with variables
        /// </summary>
        [TestMethod]
        [TestCategory("47")]
        public void EqualsTestWithVariationOfVariables()
        {
            Formula f1 = new Formula("X1 + X2");
            Formula f2 = new Formula("x1 + x2");
            bool conditional = f1.Equals(f2);
            Assert.IsFalse(conditional);
        }

        /// <summary>
        /// equals test with variables and doubles
        /// </summary>
        [TestMethod]
        [TestCategory("48")]
        public void EqualsTestWithVariablesAndDoubles()
        {
            Formula f1 = new Formula("X1 + X2 + 3.000000000001");
            Formula f2 = new Formula("X1 + X2 + 3.00");
            bool conditional = f1.Equals(f2);
            Assert.IsTrue(conditional);
        }

        /// <summary>
        /// From PS1 grading test
        /// </summary>
        [TestMethod]
        [TestCategory("49")]
        public void TestComplexAndParentheses()
        {
            Formula t = new Formula("2+3*5+(3+4*8)*5+2");
            object eval = t.Evaluate(s => 0);
            Assert.AreEqual(194.0, eval);
        }

        /// <summary>
        /// Testing our == method
        /// </summary>
        [TestMethod]
        [TestCategory("50")]
        public void EqualsOperatorMethodTest()
        {
            Formula f1 = new Formula("2+3*5+(3+4*8)*5+2");
            Formula f2 = new Formula("2+3*5+(3+4*8)*5+2");
            Assert.IsTrue(f1 == f2);
        }

        /// <summary>
        /// Testing our != method
        /// </summary>
        [TestMethod]
        [TestCategory("51")]
        public void NotEqualsOperatorMethodTest()
        {
            Formula f1 = new Formula("1+3*5+(3+4*8)*5*2");
            Formula f2 = new Formula("2+3*5+(3+4*8)*5+2");
            Assert.IsTrue(f1 != f2);
        }

    }
}