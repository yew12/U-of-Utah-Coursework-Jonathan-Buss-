using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{
    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph DG:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in DG is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in DG is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose DG = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        /** We are using a dictionary over a hashtable due to it's ability to dynamically grow
         * when needed. It is also faster to retrieve data. We use hashset because
         * we may have multiple dependees/dependents and each value in the hashset is unique so it 
         * prevents duplicate values. Also finding/adding/removing a value in a hashset 
         * is O(1) which meet our requirements. 
         */
        private Dictionary<string, HashSet<string>> dependents;
        private Dictionary<string, HashSet<string>> dependees;
        // number of key-value pairs
        private int size;


        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            // Empty dictionary 
            dependents = new Dictionary<string, HashSet<string>>();
            dependees = new Dictionary<string, HashSet<string>>();
        }

        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            // returns our size instance variable
            get { return size; }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If dg is a DependencyGraph, you would
        /// invoke it like this:
        /// dg["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get
            {
                if (dependees.Count == 0)
                {
                    return 0;
                }
                return dependees[s].Count;
            }
        }

        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            // check to see if dependents dict contains a key, if so then we assume it is non-empty
            if (dependents.ContainsKey(s))
            {
                // return true to signify it is non-empty
                return true;
            }
            else
            {
                // else, we assume if there is no key it is empty
                return false;
            }
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// s is our dependees key.
        /// </summary>
        public bool HasDependees(string s)
        {
            // check to see if dependents dict contains a key, if so then we assume it is non-empty
            if (dependees.ContainsKey(s))
            {
                // return true to signify it is non-empty
                return true;
            }
            else
            {
                // else, we assume if there is no key it is empty
                return false;
            }
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            // if the dependees dict is empty just return an empty dictionary
            if (HasDependents(s) == false)
            {
                // just return the hashset, not an entirely new dictionary
                // this is because the IEnumerable only enumerates over our hashset
                return new HashSet<string>();
            }
            else
            {
                // we are returning an enumerable hashset linked to key s
                return dependents[s];
            }
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            // if the dependees dict is empty just return an empty dictionary
            if (HasDependees(s) == false)
            {
                // just return the hashset, not an entirely new dictionary
                // this is because the IEnumerable only enumerates over our hashset
                return new HashSet<string>();
            }
            else
            {
                // we are returning an enumerable hashset linked to key s
                return dependees[s];
            }

        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> (dependents)s must be evaluated first. T depends on S</param>
        /// <param name="t"> (dependees)t cannot be evaluated until s is</param> 
        public void AddDependency(string s, string t)
        {
            // first check is if key exists because it is depended on
            if (dependents.ContainsKey(s))
            {
                // since we are using a hashset, we don't need to check if there are duplicate
                // key value pairs, so just add value
                dependents[s].Add(t);

                // check to see if dependees contains key too
                if (dependees.ContainsKey(t))
                {
                    // if it does, add the value to key
                    dependees[t].Add(s);
                }
                else
                {
                    // if it does not contain key, then create key/value pair
                    dependees.Add(t, new HashSet<string>());
                    dependees[t].Add(s);
                    // increase key-value pair for dependent size
                    size++;
                }


            }
            // does not contain key
            else
            {
                // first create our key w/ empty hashset placeholder
                dependents.Add(s, new HashSet<string>());
                // adds the key value pair
                dependents[s].Add(t);

                // check dependees dict to see if key is present
                if (dependees.ContainsKey(t))
                {
                    // if key is present, add value to given key
                    dependees[t].Add(s);

                }
                else
                {
                    // if it does not contain key, then create key/value pair
                    dependees.Add(t, new HashSet<string>());
                    dependees[t].Add(s);

                }
                // update our key value pairs size
                size++;

            }
        }

        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        ///             
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            // check to see if the value exists
            if (dependents.ContainsKey(s))
            {
                dependents[s].Remove(t);
                // check to see a key has no values associated, if so delete key
                if (dependents[s].Count.Equals(0))
                {
                    dependents.Remove(s);
                }

                // now remove for dependees dict
                dependees[t].Remove(s);

                // check to see a key has no values associated, if so delete key
                if (dependees[t].Count.Equals(0))
                {
                    dependees.Remove(t);
                }
                // decrement size
                size--;
            }
            // check if dependees dict contains key - (key is now s for dependees)
            else if (dependees.ContainsKey(s))
            {
                // if so, remove key/value pair
                dependees[s].Remove(t);

                // check to see a key has no values associated, if so delete key
                if (dependees[s].Count.Equals(0))
                {
                    dependees.Remove(s);
                }

                // now update our dependents dictionary - (t is now the key for dependents)
                dependents[t].Remove(s);

                // check to see a key has no values associated, if so delete key
                if (dependents[t].Count.Equals(0))
                {
                    dependees.Remove(t);
                }
                // update size
                size--;
            }
            // if neither dictionaries contain key, return
            else
            {
                return;
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// make sure to update dependees
        /// 
        /// UTILIZE YOUR HELPER METHODS
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            // if dependents empty then add that key
            if (!dependents.ContainsKey(s))
            {
                // add the new key with the new values
                dependents.Add(s, new HashSet<string>(newDependents));

                // now update dependees 
                foreach (string t in newDependents)
                {
                    AddDependency(s, t);
                }
            }
            // if key is present go through remove and add newDependents
            else if (dependents.ContainsKey(s))
            {
                // first need to remove any keys in dependee that need to be deleted
                foreach (string t in dependents[s])
                {
                    RemoveDependency(s, t);
                }

                // now update dependees 
                foreach (string t in newDependents)
                {
                    AddDependency(s, t);
                }

            }
        }

            /// <summary>
            /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
            /// t in newDependees, adds the ordered pair (t,s).
            /// Update Dependents
            /// 
            /// UTILIZE YOUR HELPER METHODS
            /// </summary>
            public void ReplaceDependees(string s, IEnumerable<string> newDependees)
            {

                // if dependees empty then add that key
                if (!dependees.ContainsKey(s))
                {
                    // add the new key with the new values
                    dependees.Add(s, new HashSet<string>(newDependees));

                    // now update dependents 
                    foreach (string t in newDependees)
                    {
                        AddDependency(t, s);
                    }
                }
                // if key is present go through remove and add newDependents
                else if (dependees.ContainsKey(s))
                {
                    // first need to remove any keys in dependents that need to be deleted
                    foreach (string t in dependees[s])
                    {
                        RemoveDependency(t, s);
                    }

                    // now update dependents 
                    foreach (string t in newDependees)
                    {
                        AddDependency(t, s);
                    }

                }
            }
        }
    }
