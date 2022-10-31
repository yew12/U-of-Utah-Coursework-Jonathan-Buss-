using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

// changed the regex formula to check for variables
// fixed our SetCellContents - formula
// added an empty string check in SetCellContents - text
namespace SS
{
    /// <summary>
    /// Abstract class that inherits the AbstractSpreadsheet.cs file
    /// 
    /// This class sets each individual cell that is nonempty. It also keeps track of
    /// all the cells that are dependent on one another. 
    /// </summary>
    /// <inheritdoc/>
    public class Spreadsheet : AbstractSpreadsheet
    {
        // dependency graph
        private DependencyGraph dependencyGraph;
        // dictionary takes for our cells, utilizes our private Cell class
        private Dictionary<string, Cell> cells;

        /// <summary>
        /// Zero argument constructor for our instance variables
        /// 
        /// Default constructor
        /// </summary>
        public Spreadsheet() : this(s => true, s => s, "default")
        { }

        // A5
        /// <summary>
        /// 3 parameter constructor 
        /// </summary>
        public Spreadsheet(Func<string, bool> isValid, Func<string, string> normalize, string version)
            : base(isValid, normalize, version)
        {
            dependencyGraph = new DependencyGraph();
            cells = new Dictionary<string, Cell>();
        }

        // A5
        /// <summary>
        /// Constructor for our instance variables
        /// 
        /// reads in our xml file 
        /// 
        /// Example:
        /// <cell>
        ///     <name>cell name goes here</name>
        ///     <contents>cell contents goes here</contents>    
        /// </cell>
        /// </summary>
        public Spreadsheet(string filePath, Func<string, bool> isValid, Func<string, string> normalize, string version)
            : this(isValid, normalize, version)
        {
            string cellName = "";
            string cellContents= "";

            try
            {
                // create our XmlReader object
                using (XmlReader reader = XmlReader.Create(filePath))
                {
                    while (reader.Read())
                    {
                        // if start element go in an start checking for cells
                        if (reader.IsStartElement())
                        {                  
                            // check if start element -> check version
                            if(version != GetSavedVersion(filePath))
                            {
                                throw new SpreadsheetReadWriteException("Version mismatch");
                            }

                            // gets each root element name and converts to strings for our switch statement
                            switch (reader.Name)
                            {
                                // name case
                                case "Name":
                                case "name":
                                    // gets the value of our cell name 
                                    cellName = reader.ReadInnerXml(); // https://stackoverflow.com/questions/25782928/alternative-to-switch-case-reading-xml
                                    break;
                                // contents case
                                case "Content":
                                case "Contents":
                                case "content":
                                case "contents":
                                    // gets the content of our cell name 
                                    cellContents = reader.ReadInnerXml(); // https://stackoverflow.com/questions/25782928/alternative-to-switch-case-reading-xml
                                    break;                                
                            }                                                                                   
                        }
                        // check if we are at the end of the cell element "</Cell>" || "</cell>"
                        else if (reader.Name == "Cell" || reader.Name == "cell") // https://stackoverflow.com/questions/38642592/how-to-check-if-an-element-tag-is-end-element
                        {
                            // if we are not at start element, we assume we are at end. 
                            SetContentsOfCell(cellName, cellContents);
                        }                            
                    }
                }
            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Invalid file path, try a new file path");
            }
        }

        /// <summary>
        /// If name is null or invalid, throws an InvalidNameException.
        /// 
        /// Otherwise, returns the contents (as opposed to the value) of the named cell.  The return
        /// value should be either a string, a double, or a Formula.
        /// </summary>
        //  A5
        /// <exception cref="InvalidNameException"> 
        ///   Thrown if the name is invalid: blank/empty/""
        /// </exception>
        public override object GetCellContents(string name)
        {
            // first, check if valid variable name 
            if (CellNameValidator(name) == false)
            {
                // if false, throw exception
                throw new InvalidNameException();
            }

            // valid variable name, now normalize
            name = Normalize(name);

            // then check if the cell does not contain the key, we assume it is empty (it's default state)
            if (!cells.ContainsKey(name))
            {
                // return an empty string
                return string.Empty;
            }

            // returning the contents of this cell at given key(cell name)
            return cells[name].content;
        }

        /// <summary>
        // A5
        ///  Returns the names of all non-empty cells.
        /// </summary>
        // A5 
        /// <returns>
        ///     Returns an Enumerable that can be used to enumerate
        ///     the names of all the non-empty cells in the spreadsheet.  If 
        ///     all cells are empty then an IEnumerable with zero values will be returned.
        /// </returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            /* since the default state for a cell is that it contains an empty string
             *  we just return the keys for cells dictionary because this will return 
             *  every key that is nonempty.
            */
            List<string> keys = cells.Keys.ToList();
            return keys; // Getting the just the keys for a dictionary https://stackoverflow.com/questions/5188158/how-to-get-all-the-keys-only-keys-from-dictionary-object-without-going-throug
        }

        // A5 
        /// <summary>
        ///  Set the contents of the named cell to the given number.  
        /// </summary>
        /// 
        // A5
        /// <requires> 
        ///   The name parameter must be valid: non-empty/not ""
        /// </requires>
        // A5
        /// <exception cref="InvalidNameException"> 
        ///   If the name is invalid, throw an InvalidNameException
        /// </exception>
        /// 
        // A5
        ///<returns>
        ///   <para>
        ///       This method returns a LIST consisting of the passed in name followed by the names of all 
        ///       other cells whose value depends, directly or indirectly, on the named cell.
        ///   </para>
        ///
        ///   <para>
        ///       The order must correspond to a valid dependency ordering for recomputing
        ///       all of the cells, i.e., if you re-evaluate each cell in the order of the list,
        ///       the overall spreadsheet will be consistently updated.
        ///   </para>
        ///
        ///   <para>
        ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///     set {A1, B1, C1} is returned, i.e., A1 was changed, so then A1 must be 
        ///     evaluated, followed by B1 re-evaluated, followed by C1 re-evaluated.
        ///   </para>
        /// </returns>
        // A5 (changed from public to protected)
        protected override IList<string> SetCellContents(string name, double number)
        {
            // declare a new list of dependents
            List<string> cellDependents = new List<string>();
            // instantiate the cell contents
            Cell content = new Cell(number);

            // then check if key doesn't exist
            if (!cells.ContainsKey(name))
            {
                // if not, add the cell and it's content
                cells.Add(name, content);
            }
            else
            {
                // if it does exist, we want to replace the content with the new contents since a cell holds one thing at a time
                cells[name] = content; // How to replace value at certain index - https://www.tutorialspoint.com/how-to-update-the-value-stored-in-a-dictionary-in-chash
            }

            // need to now update our dependency graphs with correct cell links

            // replace the cells with an empty hashset
            dependencyGraph.ReplaceDependees(name, new HashSet<string>());
            // once our dependency graph is updated, create our list of correct cell list
            cellDependents = GetCellsToRecalculate(name).ToList();

            // call helper method to re-evaluate the recalculated cells
            evaluateChangedCells(cellDependents);

            // once recalculated, we return our list of correct order of cell dependencies
            return cellDependents;
        }

        // A5
        /// <summary>
        /// The contents of the named cell becomes the text.  
        /// </summary> 
        // A5
        /// <requires> 
        ///   The name parameter must be valid/non-empty ""
        /// </requires>
        // A5
        /// <exception cref="InvalidNameException"> 
        ///   If the name is invalid, throw an InvalidNameException
        /// </exception>  
        // A5
        /// <param name="name"> The name of the cell </param>
        /// <param name="text"> The new content/value of the cell</param>
        // A5
        /// <returns>
        ///   <para>
        ///       This method returns a LIST consisting of the passed in name followed by the names of all 
        ///       other cells whose value depends, directly or indirectly, on the named cell.
        ///   </para>
        ///
        ///   <para>
        ///       The order must correspond to a valid dependency ordering for recomputing
        ///       all of the cells, i.e., if you re-evaluate each cell in the order of the list,
        ///       the overall spreadsheet will be consistently updated.
        ///   </para>
        ///
        ///   <para>
        ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///     set {A1, B1, C1} is returned, i.e., A1 was changed, so then A1 must be 
        ///     evaluated, followed by B1 re-evaluated, followed by C1 re-evaluated.
        ///   </para>
        /// </returns>
        // A5 (changed from public to protected)
        protected override IList<string> SetCellContents(string name, string text)
        {
            // declare a new list of dependents to be returned later
            List<string> cellDependents = new List<string>();
            // instantiate the cell contents
            Cell content = new Cell(text);

            // then check if key doesn't exist
            if (!cells.ContainsKey(name))
            {
                // if not, add the cell and it's content
                cells.Add(name, content);
            }
            else
            {
                // if it does exist, we want to replace the content with the new contents since a cell holds one thing at a time
                cells[name] = content; // How to replace value at certain index - https://www.tutorialspoint.com/how-to-update-the-value-stored-in-a-dictionary-in-chash
            }

            // need to now update our dependency graphs with correct cell links

            // replace the cells with an empty hashset because a string is not dependent on anything
            dependencyGraph.ReplaceDependees(name, new HashSet<string>());

            // once our dependency graph is updated, create our list of correct cell list
            cellDependents = GetCellsToRecalculate(name).ToList();

            // call helper method to re-evaluate the recalculated cells
            evaluateChangedCells(cellDependents);

            // once recalculated, we return our list of correct order of cell dependencies
            return cellDependents;
        }

        // A5
        /// <summary>
        /// Set the contents of the named cell to the formula.  
        /// </summary>
        // A5
        /// <requires> 
        ///   The name parameter must be valid/non empty
        /// </requires>
        // A5
        /// <exception cref="InvalidNameException"> 
        ///   If the name is invalid, throw an InvalidNameException
        /// </exception>
        // A5 
        /// <exception cref="CircularException"> 
        ///   If changing the contents of the named cell to be the formula would 
        ///   cause a circular dependency, throw a CircularException.  
        ///   (NOTE: No change is made to the spreadsheet.)
        /// </exception>
        // A5
        /// <returns>
        ///   <para>
        ///       This method returns a LIST consisting of the passed in name followed by the names of all 
        ///       other cells whose value depends, directly or indirectly, on the named cell.
        ///   </para>
        ///
        ///   <para>
        ///       The order must correspond to a valid dependency ordering for recomputing
        ///       all of the cells, i.e., if you re-evaluate each cell in the order of the list,
        ///       the overall spreadsheet will be consistently updated.
        ///   </para>
        ///
        ///   <para>
        ///     For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///     set {A1, B1, C1} is returned, i.e., A1 was changed, so then A1 must be 
        ///     evaluated, followed by B1 re-evaluated, followed by C1 re-evaluated.
        ///   </para>
        /// </returns>
        // A5 (changed from public to protected)
        protected override IList<string> SetCellContents(string name, Formula formula)
        {
            // we send in our formula object, and our Lookup helper method that is acting as a delegate (takes in string, out double)
            Cell content = new Cell(formula, Lookup); // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/local-functions
            // declare a new list of dependents
            List<string> cellDependents = new List<string>();
            // need to keep track of our old dependents in case we have a circularException()
            IEnumerable<string> originalDependents = dependencyGraph.GetDependents(name);

            // need a try catch in case we need to revert back to cell's original form
            try
            {
                /* we want to try and catch a circular exception early so we don't update the 
                * cell with invalid information. If we pass this "check" by recalculating cells,
                * we can safely add the formula and return a valid list. 
                */

                // need to update dependency graph
                dependencyGraph.ReplaceDependees(name, formula.GetVariables());

                // once our dependency graph is updated, create our list of correct cell list
                cellDependents = GetCellsToRecalculate(name).ToList();

                // if we can recalculate cells successfully, there is no circular exception meaning we can update state of spreadsheet
                Changed = true;

                // then check if key doesn't exist
                if (!cells.ContainsKey(name))
                {
                    // if not, add the cell and it's content
                    cells.Add(name, content);
                }
                else
                {
                    // if it does exist, we want to replace the content with the new contents since a cell holds one thing at a time
                    cells[name] = content; // How to replace value at certain index - https://www.tutorialspoint.com/how-to-update-the-value-stored-in-a-dictionary-in-chash
                }

                // call helper method to re-evaluate the recalculated cells
                evaluateChangedCells(cellDependents);

                // once recalculated, we return our list of correct order of cell dependencies
                return cellDependents;
            }
            catch (CircularException e)
            {
                // if we get this exception, we need to revert back to it's original form
                dependencyGraph.ReplaceDependents(name, originalDependents);
                // then throw exception
                throw e;
            }
        }


        // A5
        /// <summary>
        /// Returns an enumeration, without duplicates, of the names of all cells whose
        /// values depend directly on the value of the named cell.  In other words, returns
        /// an enumeration, without duplicates, of the names of all cells that contain
        /// formulas containing name.
        /// </summary>
        // A5
        /// <exception cref="InvalidNameException"> 
        ///   If the name is invalid, throw an InvalidNameException
        /// </exception>
        // A5
        /// <param name="name"></param>
        /// <returns>
        ///   Returns an enumeration, without duplicates, of the names of all cells that contain
        ///   formulas containing name.
        /// 
        ///   <para>For example, suppose that: </para>
        ///   <list type="bullet">
        ///      <item>A1 contains 3</item>
        ///      <item>B1 contains the formula A1 * A1</item>
        ///      <item>C1 contains the formula B1 + A1</item>
        ///      <item>D1 contains the formula B1 - C1</item>
        ///   </list>
        /// 
        ///   <para>The direct dependents of A1 are B1 and C1</para>
        /// 
        /// </returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            // need the direct dependents - use our dependency graph
            return dependencyGraph.GetDependents(name);
        }

        // A5
        /// <summary>
        /// this gets our cell value of either a string, double, or Formula Error
        /// </summary>
        /// 
        /// <exception cref="InvalidNameException"> 
        ///   If the name is invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <param name="name"> The name of the cell that we want the value of (will be normalized)</param>
        /// 
        /// <returns>
        ///   Returns the value (as opposed to the contents) of the named cell.  The return
        ///   value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </returns>
        public override object GetCellValue(string name)
        {
            // check if valid variable name 
            if (CellNameValidator(name) == false)
            {
                // if false, throw exception
                throw new InvalidNameException();
            }

            // normalize our cell name
            name = Normalize(name);

            // then check if the cell does not contain the key, we assume it is empty (it's default state)
            if (!cells.ContainsKey(name))
            {
                // return an empty string
                return string.Empty;
            }
            // returning the value of this cell at given key(cell name)
            return cells[name].value;
        }

        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed { get; protected set; }

        /// <summary>
        ///   <para>Sets the contents of the named cell to the appropriate value. </para>
        ///   <para>
        ///       First, if the content parses as a double, the contents of the named
        ///       cell becomes that double.
        ///   </para>
        ///
        ///   <para>
        ///       Otherwise, if content begins with the character '=', an attempt is made
        ///       to parse the remainder of content into a Formula.  
        ///       There are then three possible outcomes:
        ///   </para>
        ///
        ///   <list type="number">
        ///       <item>
        ///           If the remainder of content cannot be parsed into a Formula, a 
        ///           SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       </item>
        /// 
        ///       <item>
        ///           If changing the contents of the named cell to be f
        ///           would cause a circular dependency, a CircularException is thrown,
        ///           and no change is made to the spreadsheet.
        ///       </item>
        ///
        ///       <item>
        ///           Otherwise, the contents of the named cell becomes f.
        ///       </item>
        ///   </list>
        ///
        ///   <para>
        ///       Finally, if the content is a string that is not a double and does not
        ///       begin with an "=" (equal sign), save the content as a string.
        ///   </para>
        /// </summary>
        ///
        /// <exception cref="InvalidNameException"> 
        ///   If the name parameter is null or invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <exception cref="SpreadsheetUtilities.FormulaFormatException"> 
        ///   If the content is "=XYZ" where XYZ is an invalid formula, throw a FormulaFormatException.
        /// </exception>
        /// 
        /// <exception cref="CircularException"> 
        ///   If changing the contents of the named cell to be the formula would 
        ///   cause a circular dependency, throw a CircularException.  
        ///   (NOTE: No change is made to the spreadsheet.)
        /// </exception>
        /// 
        /// <param name="name"> The cell name that is being changed</param>
        /// <param name="content"> The new content of the cell</param>
        /// 
        /// <returns>
        ///       <para>
        ///           This method returns a list consisting of the passed in cell name,
        ///           followed by the names of all other cells whose value depends, directly
        ///           or indirectly, on the named cell. The order of the list MUST BE any
        ///           order such that if cells are re-evaluated in that order, their dependencies 
        ///           are satisfied by the time they are evaluated.
        ///       </para>
        ///
        ///       <para>
        ///           For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///           list {A1, B1, C1} is returned.  If the cells are then evaluate din the order:
        ///           A1, then B1, then C1, the integrity of the Spreadsheet is maintained.
        ///       </para>
        /// </returns>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            // check if valid variable name 
            if (CellNameValidator(name) == false)
            {
                // if false, throw exception
                throw new InvalidNameException();
            }

            // valid variable name, now we need to normalize variable
            name = Normalize(name);

            // reset num each time
            double num = 0;
            // checks to see if we can parse the integer, if we can we return true and parse num. - Microsoft docs(Parsing)
            bool isDouble = double.TryParse(content, out num);

            // check if text is an empty string, if so just return
            if (content.Equals(String.Empty))
            {
                return new List<string>();
            }
            // checks to see if content is a double
            else if (isDouble == true)
            {
                // update our changed method
                Changed = true;
                // set our contents of given cell - double

                // need to evaluate through the list. 
                return SetCellContents(name, num);
            }
            // if content is a formula
            else if (content.First().Equals('='))
            {
                // removes the equal sign in our formula so we can pass the formula
                string removedEqualFormula = content.Remove(0, 1); // Removing first char of string -> https://reactgo.com/c-sharp-remove-first-character/

                // converting our string to type Formula to be passed into correct method - validates and normalizes variable                
                Formula formula = new Formula(removedEqualFormula, Normalize, IsValid);

                // sets/evaluates our formula 
                return SetCellContents(name, formula);
            }

            // update our changed method
            Changed = true;
            // if neither, we assume content is a string
            return SetCellContents(name, content);
        }

        /// <summary>
        ///   Look up the version information in the given file. If there are any problems opening, reading, 
        ///   or closing the file, the method should throw a SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        /// 
        /// <remarks>
        ///   In an ideal world, this method would be marked static as it does not rely on an existing SpreadSheet
        ///   object to work; indeed it should simply open a file, lookup the version, and return it.  Because
        ///   C# does not support this syntax, we abused the system and simply create a "regular" method to
        ///   be implemented by the base class.
        /// </remarks>
        /// 
        /// <exception cref="SpreadsheetReadWriteException"> 
        ///   Thrown if any problem occurs while reading the file or looking up the version information.
        /// </exception>
        /// 
        /// <param name="filename"> The name of the file (including path, if necessary)</param>
        /// <returns>Returns the version information of the spreadsheet saved in the named file.</returns>
        public override string GetSavedVersion(string filename)
        {
            try
            {
                // read through it again to see if it can find "Spreadsheet" element
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    // maybe reset position to top of file 

                    // reads until we reach spreadsheet element
                    if (reader.ReadToFollowing("Spreadsheet")) // https://csharp.hotexamples.com/examples/System.Xml/XmlReader/ReadToFollowing/php-xmlreader-readtofollowing-method-examples.html
                    {
                        // returns the version of the spreadsheet
                        return reader.GetAttribute(0);
                    }
                }

                // read through it again to see if it can find "spreadsheet" element
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    // maybe reset position to top of file 
                    // reads until we reach spreadsheet element
                    if (reader.ReadToFollowing("spreadsheet")) // https://csharp.hotexamples.com/examples/System.Xml/XmlReader/ReadToFollowing/php-xmlreader-readtofollowing-method-examples.html
                    {
                        // returns the version of the spreadsheet
                        return reader.GetAttribute(0);
                    }
                }

                // if spreadsheet element is not in file, invalid file 
                throw new Exception();
            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Invalid File, try using a different file name.");
            }
        }

        /// <summary>
        /// Writes the contents of this spreadsheet to the named file using an XML format.
        /// The XML elements should be structured as follows:
        /// 
        /// <spreadsheet version="version information goes here">
        /// 
        /// <cell>
        /// <name>cell name goes here</name>
        /// <contents>cell contents goes here</contents>    
        /// </cell>
        /// 
        /// </spreadsheet>
        /// 
        /// There should be one cell element for each non-empty cell in the spreadsheet.  
        /// If the cell contains a string, it should be written as the contents.  
        /// If the cell contains a double d, d.ToString() should be written as the contents.  
        /// If the cell contains a Formula f, f.ToString() with "=" prepended should be written as the contents.
        /// 
        /// If there are any problems opening, writing, or closing the file, the method should throw a
        /// SpreadsheetReadWriteException with an explanatory message.
        /// </summary>
        public override void Save(string filename)
        {
            // BEGINNING IS USED FROM EXAMPLE IN FORSTUDENTS            
            // We want some non-default settings for our XML writer.
            // Specifically, use indentation to make it more (human) readable.
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";

            try
            {
                using (XmlWriter writer = XmlWriter.Create(filename, settings))  // using create replaces the filename if it already exists
                {
                    // write the spreadsheet start element with version - ex XML version
                    writer.WriteStartDocument();

                    // spreadsheet - version ""
                    writer.WriteStartElement("spreadsheet");
                    writer.WriteAttributeString("version", Version);

                    // write all the nonempty cell objects
                    // for each loop for all GetNamesOfNonEmpty
                    foreach (string cellName in GetNamesOfAllNonemptyCells())
                    {
                        // start cell element
                        writer.WriteStartElement("cell");
                        // name 
                        writer.WriteElementString("name", cellName);

                        // check if it is a formula so we can add an "=" before
                        if(GetCellContents(cellName) is Formula formula)
                        {
                            // add the "=" to front of formula
                            writer.WriteElementString("contents", $"={formula.ToString()}");
                        }
                        else
                        {
                            // content
                            writer.WriteElementString("contents", GetCellContents(cellName).ToString());
                        }
                        // end cell element
                        writer.WriteEndElement();
                    }

                    // Spreadsheet end element
                    writer.WriteEndElement();
                    // end document 
                    writer.WriteEndDocument();
                }
                // if we get through saving successfully, update our changed 
                Changed = false;
            }
            catch (Exception)
            {
                throw new SpreadsheetReadWriteException("Invalid File, try using a different file name.");
            }
        }

        // HELPER METHODS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cellDependents"></param>
        private void evaluateChangedCells(IEnumerable<string> cellDependents)
        {
            // loop through all the changed cells
            foreach(string cell in cellDependents)
            {
                object recalculateCells = cells[cell].content;
                // check if string
                if(recalculateCells is string)
                {
                    cells[cell].value = recalculateCells;
                }
                // check if double
                if (recalculateCells is double)
                {
                    cells[cell].value = recalculateCells;
                }
                // check if formula
                if (recalculateCells is Formula formula)
                {
                    // evaluate the formula
                    recalculateCells = formula.Evaluate(Lookup);
                    // then replace the value
                    cells[cell].value = recalculateCells;
                }
            }
        }

        /// <summary>
        /// This helper method just checks if the cell variable name sent in
        /// is of a valid type
        /// </summary>
        /// <param name="cellName">the cell name sent in</param>
        /// <returns>returns true if valid, otherwise false</returns>
        private bool CellNameValidator(string cellName)
        {
            // Regex pattern for our variable pattern - one (or more) letters followed by one (or more) digits
            string variablePattern = @"^[a-zA-Z]+[0-9]+$";

            // Specific Token Rule - has to be either an operator, digit(including sci notation), or parenthesis
            if (Regex.IsMatch(cellName, variablePattern) && IsValid(cellName))
            {
                // if it is a valid cell name, return true
                return true;
            }
            else
            {
                // if it's invalid, return false
                return false;
            }
        }

        /// <summary>
        /// This is a local  that looks up the value of the cell sent in
        /// 
        /// It essentially acts as the Lookup delegate where it takes in a string and 
        /// outputs a double (Func<string, double>)
        /// </summary>
        /// 
        /// "Local functions (C# Programming Guide)"
        /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/local-functions#implementation-as-a-delegate
        /// 
        /// <param name="cellName">the name of our cell</param>
        /// <exception cref="ArgumentException"> 
        ///   Thrown if the cell doesn't exist or that given cell value is a string
        /// </exception>
        /// <returns></returns>
        double Lookup(string cellName)
        {
            object cellValue = GetCellValue(cellName);
            if (cellValue is string)
            {
                // if it is a string throw
                throw new ArgumentException("Invalid element");
            }
            
            try
            {
                // if neither throw, we assume it is a double 
                return (double)cellValue;
            }
            // if we can't cast it, then we assume variable doesn't exist
            catch (Exception)
            {
                throw new ArgumentException();
            }
            
        }

        /// <summary>
        /// Private class that represents a singular cell. 
        /// Takes in either a string, double, or formula of type Formula
        /// and instantiates them.
        /// Get/Set allows us to get the contents of given cell.
        /// 
        /// constructors double string formula
        /// set contents(objects)
        /// </summary>
        private class Cell
        {
            // using object so we can assign value and contents to either a string, double or Formula
            public object value { get; set; }

            // instance variable, getters/setters for to keep track of the contents of given cell
            public object content { get; set; }

            /// <summary>
            /// Constructor for our object instance variables 
            /// 
            /// If a cell's contents is a string, its value is that string.
            /// </summary>
            /// <param name="strContent"></param>
            public Cell(string strContent)
            {
                // value is the same as our content
                value = strContent;
                content = strContent;
            }

            /// <summary>
            /// Constructor for our object instance variables 
            /// 
            /// If a cell's contents is a double, its value is that double.
            /// </summary>
            /// <param name="doubleContent"></param>
            public Cell(double doubleContent)
            {
                // value is the same as our content
                value = doubleContent;
                content = doubleContent;
            }

            /// <summary>
            /// Constructor for our object instance variables 
            /// 
            /// Our contents is our string formula that we send in
            /// Our value will either be a double (our evaluated formula) or a Formula Error
            /// So anytime we get try to get our value, and that value is of type FormulaError
            /// then we should "throw" FormulaError in our test
            /// </summary>
            /// <param name="formula">our valid formula</param>
            /// <param name="lookup">our delegate that is previously in the form of a helper method Lookup</param>
            public Cell(Formula formula, Func<string, double> lookup)
            {
                content = formula;
                // need to evaluate value with our looked up variables
                value = formula.Evaluate(lookup); //  this may return either a a double or FormulaError   
            }
        }
    }
}
