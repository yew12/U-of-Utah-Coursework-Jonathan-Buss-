using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadsheetUtilities;


namespace DevelopmentTests
{
    /// <summary>
    ///This is a test class for DependencyGraphTest and is intended
    ///to contain all DependencyGraphTest Unit Tests
    ///</summary>
    [TestClass()]
    public class DependencyGraphTest
    {

        /// <summary>
        /// Just adding a new key value pair
        ///</summary>
        [TestMethod()]
        public void SimpleAddNewPair()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
        }



        /// <summary>
        /// adding a new key value pair, then another value with same key
        ///</summary>
        [TestMethod()]
        public void AddTwoValuesSameKey()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            t.AddDependency("x", "z");
        }

        /// <summary>
        /// A simple remove test that adds two pairs with same key 
        /// then removes those pairs, including key
        /// </summary>
        [TestMethod()]
        public void SimpleRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "c");
            t.AddDependency("a", "d");
            t.RemoveDependency("a", "d");
            t.RemoveDependency("a", "c");
        }

        /// <summary>
        /// This test removes a key/value pair that is in the dependees dictionary but 
        /// not in the dependents dictionary
        /// </summary>
        [TestMethod()]
        public void RemovingIfKeyDoesNotExistForDependent()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "c");
            t.AddDependency("a", "d");
            t.RemoveDependency("c", "a");
        }

        /// <summary>
        /// This test should just return if we send in a key that 
        /// does not exist
        /// </summary>
        [TestMethod()]
        public void RemovingIfKeyDoesNotExistForEither()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "c");
            t.AddDependency("a", "d");
            t.RemoveDependency("z", "a");
        }

        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyTest()
        {
            DependencyGraph t = new DependencyGraph();
            Assert.AreEqual(0, t.Size);
        }
        
        /// <summary>
        /// Checks HasDependees test
        ///</summary>
        [TestMethod()]
        public void SimpleHasDependeesTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(true, t.HasDependees("y"));
        }

        /// <summary>
        /// Checks HasDependents test
        ///</summary>
        [TestMethod()]
        public void SimpleHasDependentsTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(true, t.HasDependents("x"));
        }


        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void SimpleEmptyRemoveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(1, t.Size);
            t.RemoveDependency("x", "y");
            Assert.AreEqual(0, t.Size);
        }

        /// <summary>
        /// Currently a test to cover more branch coverage in remove method
        /// Deletes the key for the dependee and the dependent in the 
        /// first else if statement for remove method
        ///</summary>
        [TestMethod()]
        public void RemoveTest_DeletingKeyForDependent()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "c");
            Assert.AreEqual(t["c"], t.Size);
            t.RemoveDependency("c", "a");
            Assert.AreEqual(0, t.Size);
           
        }


        /// <summary>
        ///Empty graph should contain nothing
        ///</summary>
        [TestMethod()]
        public void EmptyEnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            IEnumerator<string> e1 = t.GetDependees("y").GetEnumerator();
            Assert.IsTrue(e1.MoveNext());
            Assert.AreEqual("x", e1.Current);
            IEnumerator<string> e2 = t.GetDependents("x").GetEnumerator();
            Assert.IsTrue(e2.MoveNext());
            Assert.AreEqual("y", e2.Current);
            t.RemoveDependency("x", "y");
            Assert.IsFalse(t.GetDependees("y").GetEnumerator().MoveNext());
            Assert.IsFalse(t.GetDependents("x").GetEnumerator().MoveNext());
        }


        /// <summary>
        /// Replace on an empty with several values, DG shouldn't fail.
        /// For dependents methods
        ///</summary>
        [TestMethod()]
        public void ReplaceTest_AddValuesToOneKey()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(t.Size, 1);
            t.RemoveDependency("x", "y");
            t.ReplaceDependents("x", new HashSet<string>() { "a", "b", "s" });
            Assert.AreEqual(t.Size, 3);
        }


        /// <summary>
        /// Replace on an empty DG shouldn't fail
        ///</summary>
        [TestMethod()]
        public void SimpleReplaceTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("x", "y");
            Assert.AreEqual(t.Size, 1);
            t.RemoveDependency("x", "y");
            t.ReplaceDependents("x", new HashSet<string>());
            t.ReplaceDependees("y", new HashSet<string>());
            Assert.AreEqual(t.Size, 0);
        }



        ///<summary>
        ///It should be possibe to have more than one DG at a time.
        ///</summary>
        [TestMethod()]
        public void StaticTest()
        {
            DependencyGraph t1 = new DependencyGraph();
            DependencyGraph t2 = new DependencyGraph();
            t1.AddDependency("x", "y");
            Assert.AreEqual(1, t1.Size);
            Assert.AreEqual(0, t2.Size);
        }




        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void SizeTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");
            Assert.AreEqual(4, t.Size);
        }


        /// <summary>
        ///Non-empty graph contains something
        ///</summary>
        [TestMethod()]
        public void EnumeratorTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("a", "b");
            t.AddDependency("a", "c");
            t.AddDependency("c", "b");
            t.AddDependency("b", "d");

            IEnumerator<string> e = t.GetDependees("a").GetEnumerator();
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("b").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            String s1 = e.Current;
            Assert.IsTrue(e.MoveNext());
            String s2 = e.Current;
            Assert.IsFalse(e.MoveNext());
            Assert.IsTrue(((s1 == "a") && (s2 == "c")) || ((s1 == "c") && (s2 == "a")));

            e = t.GetDependees("c").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("a", e.Current);
            Assert.IsFalse(e.MoveNext());

            e = t.GetDependees("d").GetEnumerator();
            Assert.IsTrue(e.MoveNext());
            Assert.AreEqual("b", e.Current);
            Assert.IsFalse(e.MoveNext());
        }




        

        /// <summary>
        /// Test method inspired by a piazza post @240. 
        /// This just checks to see if ReplaceDependents work
        /// with an array that is not one to one. 
        ///</summary>
        [TestMethod()]
        public void ReplaceDependentNonOneToOne()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("A1", "A2");
            t.AddDependency("A1", "A3");
            t.ReplaceDependents("A1", new HashSet<string>() {"A4","A5", "A6"});
        }


        /// <summary>
        /// Extensive test of our ReplaceDependents method.
        /// When replacing our dependents dictionary with "x" and "0"
        /// we notice that the dependees dictionary contains the key "x",
        /// so we make sure that instead of removing the key, we add w to that key.
        ///</summary>
        [TestMethod()]
        public void ReplaceDependentsExtensiveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("b", "x");
            t.AddDependency("a", "x");
            t.AddDependency("w", "q");
            t.ReplaceDependents("w", new HashSet<string>() { "x", "o" });
        }

        /// <summary>
        /// When replacing our dependees dictionary, we are replacing
        /// a key that doesn't exist, so we add that key with the new values.
        /// We then need to update the dependents dictionary so I was testing to 
        /// see if we correctly add a value to a key that already exists. Which
        /// it does correctly.
        ///</summary>
        [TestMethod()]
        public void ReplaceDependeesExtensiveTest()
        {
            DependencyGraph t = new DependencyGraph();
            t.AddDependency("b", "x");
            t.AddDependency("x", "x");
            t.AddDependency("w", "q");
            t.ReplaceDependees("w", new HashSet<string>() { "x", "o" });
        }


        /// <summary>
        ///Using lots of data
        ///</summary>
        [TestMethod()]
        public void StressTest()
        {
            // Dependency graph
            DependencyGraph t = new DependencyGraph();

            // A bunch of strings to use
            const int SIZE = 200;
            string[] letters = new string[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                letters[i] = ("" + (char)('a' + i));
            }

            // The correct answers
            HashSet<string>[] dents = new HashSet<string>[SIZE];
            HashSet<string>[] dees = new HashSet<string>[SIZE];
            for (int i = 0; i < SIZE; i++)
            {
                dents[i] = new HashSet<string>();
                dees[i] = new HashSet<string>();
            }

            // Add a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j++)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove a bunch of dependencies
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 4; j < SIZE; j += 4)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Add some back
            for (int i = 0; i < SIZE; i++)
            {
                for (int j = i + 1; j < SIZE; j += 2)
                {
                    t.AddDependency(letters[i], letters[j]);
                    dents[i].Add(letters[j]);
                    dees[j].Add(letters[i]);
                }
            }

            // Remove some more
            for (int i = 0; i < SIZE; i += 2)
            {
                for (int j = i + 3; j < SIZE; j += 3)
                {
                    t.RemoveDependency(letters[i], letters[j]);
                    dents[i].Remove(letters[j]);
                    dees[j].Remove(letters[i]);
                }
            }

            // Make sure everything is right
            for (int i = 0; i < SIZE; i++)
            {
                Assert.IsTrue(dents[i].SetEquals(new HashSet<string>(t.GetDependents(letters[i]))));
                Assert.IsTrue(dees[i].SetEquals(new HashSet<string>(t.GetDependees(letters[i]))));
            }
        }

    }
}
