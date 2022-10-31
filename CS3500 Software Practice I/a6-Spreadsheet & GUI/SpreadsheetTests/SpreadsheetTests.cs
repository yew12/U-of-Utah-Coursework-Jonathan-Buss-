using Microsoft.VisualStudio.TestTools.UnitTesting;
using SS;
using SpreadsheetUtilities;
using System.Collections.Generic;

namespace SpreadsheetTests
{
    /// <summary>
    /// Test file for our Spreadsheet project.
    /// </summary>
    [TestClass]
    public class SpreadsheetTests
    {
        // *************** \\
        // EXCEPTION TESTS
        // *************** \\

        /// <summary>
        /// simple GetCellContentsTest method test
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SimpleInvalidGetCellContentsTest()
        {
            Spreadsheet s = new Spreadsheet();
            s.GetCellContents("25");
        }

        /// <summary>
        /// simple SetCellContents (string) method test
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SimpleInvalidSetCellContentsString()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("25", "11");
        }

        /// <summary>
        /// simple SetCellContents (double) method test
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SimpleInvalidSetCellContentsDouble()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("25", "Valid text");
        }

        /// <summary>
        /// simple SetCellContents (Formula) method test
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void SimpleInvalidSetCellContentsFormula()
        {
            Spreadsheet s = new Spreadsheet();
            Formula f = new Formula("2+2");
            s.SetContentsOfCell("25", "f");
        }

        // *************** \\
        // ALL OTHER TESTS
        // *************** \\

        /// <summary>
        /// Getting Cell Contents for an Empty cell
        /// </summary>
        [TestMethod]
        public void SimpleGetCellContentsStringEmptyCell()
        {
            Spreadsheet s = new Spreadsheet();
            Assert.AreEqual(string.Empty, s.GetCellContents("A1"));
        }

        /// <summary>
        /// read method name
        /// </summary>
        [TestMethod]
        public void SimpleSetCellContentsStringReplace()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "2");
            s.SetContentsOfCell("A1", "f");
            Assert.AreEqual("f", s.GetCellContents("A1"));
        }

        /// <summary>
        /// read method name
        /// </summary>
        [TestMethod]
        public void SimpleSetCellContentsDouble()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "32");
            Assert.AreEqual(32.0, s.GetCellContents("A1"));
        }

        /// <summary>
        /// read method name
        /// </summary>
        [TestMethod]
        public void SimpleSetCellContentsDoubleReplace()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "32");
            s.SetContentsOfCell("A1", "40");
            Assert.AreEqual(40.0, s.GetCellContents("A1"));
        }

        /// <summary>
        /// read method name
        /// </summary>
        [TestMethod]
        public void SimpleSetCellContentsFormula()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=2+2");
            Assert.AreEqual(new Formula("2 + 2"), s.GetCellContents("A1"));
        }

        /// <summary>
        /// read method name
        /// </summary>
        [TestMethod]
        public void SimpleSetCellContentsFormulaMultiple()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=2+2");
            s.SetContentsOfCell("A3", "=a1+2");
            Assert.AreEqual(new Formula("2 + 2"), s.GetCellContents("A1"));
            Assert.AreEqual(new Formula("a1 + 2"), s.GetCellContents("A3"));
        }

        /// <summary>
        /// read method name
        /// </summary>
        [TestMethod]
        public void SimpleSetCellContentsFormulaReplace()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Formula f1 = new Formula("2 + 2");
            Formula f2 = new Formula("a1 + 2");
            s.SetContentsOfCell("A1", "=2+2");
            s.SetContentsOfCell("A1", "=a1+2");
            Assert.AreEqual(new Formula("a1 + 2"), s.GetCellContents("A1"));
        }

        /// <summary>
        /// Gets the names of all the nonempty cells
        /// </summary>
        [TestMethod]
        public void SimpleGetNamesOfAllNonemptyCells()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "2");
            s.SetContentsOfCell("A2", "B1");
            s.SetContentsOfCell("A5", "=A3");
            s.SetContentsOfCell("A4", "55");
            List<string> keys = (List<string>)s.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(4, keys.Count);
        }

        /// <summary>
        /// Gets the names of all the nonempty cells when we 
        /// don't have any nonempty cells
        /// </summary>
        [TestMethod]
        public void GetNamesOfAllNonemptyCellsWithOnlyEmptyCells()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            IList<string> nonemptyList = (IList<string>)s.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(0, nonemptyList.Count);
        }

        /// <summary>
        /// Simple get Direct dependents tests
        /// since we change a1, we then do a proper recalculation
        /// we should return the direct dependents of A1 are B1 and C1(including a1)
        /// </summary>
        [TestMethod]
        public void GetDirectDependentsTestRecalculate()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("b1", "7");
            s.SetContentsOfCell("c1", "=a1 + b1");
            s.SetContentsOfCell("d1", "=a1 * c1");
            IList<string> dependents = s.SetContentsOfCell("a1", "f");
            // "a1" should be at the top of our list
            // we don't include "b1" because at the end of the day it contains 7
            Assert.AreEqual(dependents[0], "a1");
        }

        /// <summary>
        /// Simple get Direct dependents tests
        /// Should be returning only d1 because we are returning
        /// everything that d1 is dependent upon, which is none of these.
        /// </summary>
        [TestMethod]
        public void GetDirectDependentsTestNoRecalculation()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("b1", "7");
            s.SetContentsOfCell("c1", "=a1 + b1");
            IList<string> dependents = s.SetContentsOfCell("d1", "=a1 * c1");
            // d1 should be the only thing in our list 
            Assert.AreEqual(dependents[0], "d1");
            Assert.AreEqual(1, dependents.Count);
        }

        /// <summary>
        /// more tests on our SetCellMethods (Formula)
        /// </summary>
        [TestMethod]
        public void TestEmptyFormulas()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            Assert.AreEqual(string.Empty, s.GetCellContents("A1"));
        }

        /// <summary>
        /// Testing to make sure random variables work
        /// </summary>
        [TestMethod]
        public void TestRandomVariables2()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("x", "1");
            Assert.AreEqual(1.0, s.GetCellContents("x"));
        }

        /// <summary>
        /// Testing a large list of direct dependents 
        /// </summary>
        [TestMethod]
        public void StressTest()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "6");
            s.SetContentsOfCell("b1", "=a1 + d1");
            s.SetContentsOfCell("c1", "=a1 + e1");
            s.SetContentsOfCell("d1", "=a1 + f1");
            s.SetContentsOfCell("e1", "=a1 + g1");
            s.SetContentsOfCell("f1", "=a1 + h1");
            s.SetContentsOfCell("g1", "=a1 + i1");
            s.SetContentsOfCell("h1", "=i1 + j1");
            IList<string> dependents = s.SetContentsOfCell("a1", "g");
            Assert.AreEqual("a1", dependents[0]);
            Assert.AreEqual(7, dependents.Count);
        }

        /// <summary>
        /// Testing a large list of direct dependents 
        /// </summary>
        [TestMethod]
        public void StressTest2()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "6");
            s.SetContentsOfCell("b1", "=a1 + 1");
            s.SetContentsOfCell("c1", "=a1 + 1");
            s.SetContentsOfCell("d1", "=a1 + 1");
            s.SetContentsOfCell("e1", "=a1 + 1");
            s.SetContentsOfCell("f1", "=a1 + 1");
            s.SetContentsOfCell("g1", "=a1 + 1");
            s.SetContentsOfCell("h1", "=a1 + 1");
            s.SetContentsOfCell("i1", "=a1 + d1");
            s.SetContentsOfCell("j1", "=a1 + 1");
            s.SetContentsOfCell("k1", "=a1 + 1");
            s.SetContentsOfCell("l1", "=a1 + 1");
            s.SetContentsOfCell("m1", "=a1 + 1");
            s.SetContentsOfCell("n1", "=a1 + 1");
            s.SetContentsOfCell("o1", "=a1 + 1");
            s.SetContentsOfCell("p1", "=a1 + 1");
            s.SetContentsOfCell("q1", "=a1 + 1");
            s.SetContentsOfCell("q2", "=a1 + 1");
            s.SetContentsOfCell("q3", "=a1 + 1");
            s.SetContentsOfCell("q4", "=a1 + 1");
            s.SetContentsOfCell("q5", "=a1 + 1");
            s.SetContentsOfCell("q6", "=i1 + 1");
            IList<string> dependents = s.SetContentsOfCell("a1", "g");
            Assert.AreEqual("a1", dependents[0]);
            Assert.AreEqual(22, dependents.Count);
        }

        /// <summary>
        /// Testing an even larger list of direct dependents 
        /// </summary>
        [TestMethod]
        public void StressTest3()
        {
            AbstractSpreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("a1", "6");
            s.SetContentsOfCell("b1", "=a1 + 1");
            s.SetContentsOfCell("c1", "=a1 + 1");
            s.SetContentsOfCell("d1", "=a1 + 1");
            s.SetContentsOfCell("e1", "=a1 + 1");
            s.SetContentsOfCell("f1", "=a1 + 1");
            s.SetContentsOfCell("g1", "=a1 + 1");
            s.SetContentsOfCell("h1", "=a1 + 1");
            s.SetContentsOfCell("i1", "=a1 + d1");
            s.SetContentsOfCell("j1", "=a1 + 1");
            s.SetContentsOfCell("k1", "=a1 + 1");
            s.SetContentsOfCell("l1", "=a1 + 1");
            s.SetContentsOfCell("m1", "=a1 + 1");
            s.SetContentsOfCell("n1", "=a1 + 1");
            s.SetContentsOfCell("o1", "=a1 + 1");
            s.SetContentsOfCell("p1", "=a1 + 1");
            s.SetContentsOfCell("q1", "=a1 + 1");
            s.SetContentsOfCell("q2", "=a1 + 1");
            s.SetContentsOfCell("q3", "=a1 + 1");
            s.SetContentsOfCell("q4", "=a1 + 1");
            s.SetContentsOfCell("q5", "=a1 + 1");
            s.SetContentsOfCell("q6", "=i1 + 1");
            s.SetContentsOfCell("b2", "=a1 + 1");
            s.SetContentsOfCell("c2", "=a1 + 1");
            s.SetContentsOfCell("d2", "=a1 + 1");
            s.SetContentsOfCell("e2", "=a1 + 1");
            s.SetContentsOfCell("f2", "=a1 + 1");
            s.SetContentsOfCell("g2", "=a1 + 1");
            s.SetContentsOfCell("h2", "=a1 + 1");
            s.SetContentsOfCell("i2", "=a1 + d1");
            s.SetContentsOfCell("j2", "=a1 + 1");
            s.SetContentsOfCell("k2", "=a1 + 1");
            s.SetContentsOfCell("l2", "=a1 + 1");
            s.SetContentsOfCell("m2", "=a1 + 1");
            s.SetContentsOfCell("n2", "=a1 + 1");
            s.SetContentsOfCell("o2", "=a1 + 1");
            s.SetContentsOfCell("p2", "=a1 + 1");
            s.SetContentsOfCell("q10", "=a1 + 1");
            s.SetContentsOfCell("q11", "=a1 + 1");
            s.SetContentsOfCell("q12", "=a1 + 1");
            s.SetContentsOfCell("q13", "=a1 + 1");
            s.SetContentsOfCell("q14", "=a1 + 1");
            s.SetContentsOfCell("q15", "=i1 + 1");
            IList<string> dependents = s.SetContentsOfCell("a1", "g");
            Assert.AreEqual("a1", dependents[0]);
            Assert.AreEqual(43, dependents.Count);
        }

        /// <summary>
        /// A stress test involving all three of the previous 
        /// stress tests
        /// </summary>
        [TestMethod]
        public void StressTest4()
        {
            StressTest();
            StressTest2();
            StressTest3();
        }
       
        // ****************** \\
        //  A5 SPECIFIC TESTS
        // ****************** \\

        /// <summary>
        /// simple test to for our lookup method 
        /// </summary>
        [TestMethod]
        public void SimpleLookupTest()
        {
            Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "1.0");
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("a2", "=a1+5");
            Assert.AreEqual(10.0, s.GetCellValue("a2"));

        }

        /// <summary>
        /// simple test to for our lookup method 
        /// </summary>
        [TestMethod]
        public void SimpleLookupWithMultipleVars()
        {
            Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "1.0");
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("a2", "=a1+a1");
            Assert.AreEqual(10.0, s.GetCellValue("a2"));
        }

        /// <summary>
        /// simple test to for our lookup method a
        /// </summary>
        [TestMethod]
        public void LookupWithMultipleDifferentVars()
        {
            Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "1.0");
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("a3", "10");
            s.SetContentsOfCell("a2", "=a1+a3");
            Assert.AreEqual(15.0, s.GetCellValue("a2"));
        }

        /// <summary>
        /// simple test to for our lookup method a
        /// </summary>
        [TestMethod]
        public void LookupWithChangedVar()
        {
            Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "1.0");
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("a3", "10");
            s.SetContentsOfCell("a1", "50");
            s.SetContentsOfCell("a2", "=a1+a3");
            Assert.AreEqual(60.0, s.GetCellValue("a2"));
        }

        /// <summary>
        /// simple test to for our lookup method that should throw a formula error
        /// </summary>
        [TestMethod]
        public void LookupWithInvalidVariable()
        {
            Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "1.0");
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("a3", "F");
            s.SetContentsOfCell("a2", "=a1+a3");
            Assert.IsInstanceOfType(s.GetCellValue("a2"), typeof(FormulaError));
        }

        /// <summary>
        /// simple test to for our lookup method
        /// </summary>
        [TestMethod]
        public void LookupWithSeveralVariables()
        {
            Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "1.0");
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("a3", "10");
            s.SetContentsOfCell("z3", "10");
            s.SetContentsOfCell("f3", "10");
            s.SetContentsOfCell("a2", "=a1*a3 + z3 + f3");
            Assert.AreEqual(70.0, s.GetCellValue("a2"));
        }

        /// <summary>
        /// simple test to for our lookup method
        /// </summary>
        [TestMethod]
        public void LookupWithSeveralVariables2()
        {
            Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "1.0");
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("a3", "10");
            s.SetContentsOfCell("a4", "10");
            s.SetContentsOfCell("a5", "10");
            s.SetContentsOfCell("a6", "10");
            s.SetContentsOfCell("a2", "=a1*a3 + a5 + a4/ a6");
            Assert.AreEqual(61.0, s.GetCellValue("a2"));
        }

        /// <summary>
        /// simple test to for our save method
        /// </summary>
        [TestMethod]
        public void SimpleSaveTest()
        {
            Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "1.0");
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("a2", "11");
            s.SetContentsOfCell("a3", "12");
            s.SetContentsOfCell("a4", "13");
            s.SetContentsOfCell("a5", "14");
            s.Save("File1.txt");
        }

        /// <summary>
        /// simple test to for our save method
        /// </summary>
        [TestMethod]
        public void SimpleSaveTestWithFormulas()
        {
            Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "1.0");
            s.SetContentsOfCell("a1", "5");
            s.SetContentsOfCell("a2", "11");
            s.SetContentsOfCell("a3", "12");
            s.SetContentsOfCell("a4", "13");
            s.SetContentsOfCell("a5", "=a2+a3");
            s.SetContentsOfCell("a6", "=1+2");
            s.SetContentsOfCell("a7", "=1+a4");
            s.SetContentsOfCell("a7", "=1+a4");
            s.Save("SaveTestWithFormulas.txt");
        }

        /// <summary>
        /// Invalid file name
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void InvalidFile_SaveMethod()
        {
            Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "");
            s.Save("");
        }

        /// <summary>
        /// simple test to for our constructor
        /// </summary>
        [TestMethod]
        public void SimpleReadFileTest()
        {
            Spreadsheet s = new Spreadsheet("A5_Test_File.txt", s => true, s => s.ToUpper(), "1.0");
            Assert.AreEqual(5.0, s.GetCellContents("a1"));
            Assert.AreEqual(11.0, s.GetCellContents("a2"));
        }

        /// <summary>
        /// simple test to for our constructor 
        /// </summary>
        [TestMethod]
        public void SimpleReadFileTestWithFormulas()
        {
            Spreadsheet s = new Spreadsheet("A5_Test_File2.txt", s => true, s => s.ToUpper(), "2.0");
            Assert.AreEqual(4.0, s.GetCellContents("b1"));
            Assert.AreEqual("a3+a5", s.GetCellContents("b2"));
        }

        /// <summary>
        /// simple test to for our constructor 
        /// 
        /// Referenced PS4GradingTests.cs
        /// </summary>
        [TestMethod]
        public void SimpleReadFileTestWithEmpty()
        {
            Spreadsheet s = new Spreadsheet("A5_Test_File_Empty.txt", s => true, s => s.ToUpper(), "2.0");
            Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
        }

        /// <summary>
        /// invalid file for our constructor
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void VersionMismatch_ReadFileConstructor()
        {
            // I inputed 1.0 when our file version is 2.0
            Spreadsheet s = new Spreadsheet("A5_Test_File_VersionMismatch.txt", s => true, s => s.ToUpper(), "1.0");
        }

        /// <summary>
        /// simple test to for our get cell value
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void InvalidName_GetCellValue()
        {
            Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "");
            s.GetCellValue("1");
        }

        /// <summary>
        /// simple test to for our get cell value method
        /// </summary>
        [TestMethod]
        public void GetCellValue_EmptyReturn()
        {
            Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "");
            Assert.AreEqual(string.Empty, s.GetCellValue("a1"));
        }

        /// <summary>
        /// Simple circular exception test from PS4GradingTests.cs
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestSimpleCircular()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2");
            s.SetContentsOfCell("A2", "=A1");
        }

        /// <summary>
        /// circular exception test from PS4GradingTests.cs
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestComplexCircular()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("A1", "=A2+A3");
            s.SetContentsOfCell("A3", "=A4+A5");
            s.SetContentsOfCell("A5", "=A6+A7");
            s.SetContentsOfCell("A7", "=A1+A1");
        }

        /// <summary>
        /// empty set contents of cell from PS4GradingTests
        /// </summary>
        [TestMethod]
        public void TestExplicitEmptySet()
        {
            Spreadsheet s = new Spreadsheet();
            s.SetContentsOfCell("B1", "");
            Assert.IsFalse(s.GetNamesOfAllNonemptyCells().GetEnumerator().MoveNext());
        }

        /// <summary>
        /// Tests if we return the correct version of our input file
        /// </summary>
        [TestMethod]
        public void SimpleGetSavedVersionTest()
        {
            Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "");
            Assert.AreEqual("11.0",s.GetSavedVersion("A5_Test_File_Version11.txt"));
        }

        /// <summary>
        /// Tests if we throw an exception when we send in a file with no spreadsheet element
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void GetSavedVersion_NoSpreadsheetElement()
        {
            Spreadsheet s = new Spreadsheet(s => true, s => s.ToUpper(), "");
            s.GetSavedVersion("A5_Test_File_NoSpreadsheetElement.txt");
        }

        
    }
}