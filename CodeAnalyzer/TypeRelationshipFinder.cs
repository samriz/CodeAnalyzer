/////////////////////////////////////////////////////////////////////////////
// TypeRelationshipFinder.cs - Finds all of the classes a particular class //
// uses and/or depends on.                                                 //
//                                                                         //
// ver 1.0                                                                 //
// Language:    C#, 2020, .Net Framework 4.7.2                             //
// Platform:    MSI GS65 Stealth, Win10                                    //
// Application: CSE681, Project #2, Winter 2021                            //
// Author:      Sameer Rizvi, Syracuse University                          //
//              srizvi@syr.edu                                             //
/////////////////////////////////////////////////////////////////////////////
/*
 * Package Operations:
 * -------------------
 *  The TypeRelationshipFinder class has 2 parameterized constructors that
 *  take in the class name that you want to find type relationships in, a
 *  list/collection of class names you may have compiled together using the
 *  ClassNameFinder class, and a list of file lines you may have extracted using
 *  the FileExtractor class.
 */
/* Required Files:
 *   
 *   
 * Maintenance History:
 * --------------------
 * ver 1.2 : 22 January 2021
 * - first release
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalyzer
{
    public class TypeRelationshipFinder
    {
        private readonly List<string> classNames;
        private readonly List<string> fileLines;
        private readonly string className;

        //default constructor
        public TypeRelationshipFinder()
        {
        }
        //parameterized constructor
        public TypeRelationshipFinder(string className, List<string> classNames, List<string> fileLines)
        {
            this.className = className;
            this.classNames = classNames;
            this.fileLines = fileLines;
        }
        //parameterized constructor.
        //this one accepts an IEnumerable collection so that the Distinct function can be called to eliminate unnecessary duplicates
        public TypeRelationshipFinder(string className, IEnumerable<string> classNames, List<string> fileLines)
        {
            this.className = className;
            this.classNames = classNames.ToList();
            this.fileLines = fileLines;
        }        
        //compare each line of the file with a list of all user-defined class names in a particular directory
        //returns a list of strings that contain the class name and which types it uses
        private IEnumerable<string> FindRelationships()
        {
            IEnumerable<string> relationships = new List<string>();
            foreach (var line in fileLines)
            {
                foreach(var name in classNames)
                {
                    if (line.Contains(name))
                    {
                        string relationshipString = className + " uses " + name + ".";
                        relationships = relationships.Append(relationshipString);
                    }
                }
            }
            return relationships;
        }
        //return all of the relationships a certain class has
        public IEnumerable<string> GetRelationships() 
        { 
            return FindRelationships().Distinct(); 
        }     
// ---------------- test stub --------------------
#if (test_typerelationshipfinder)
        static void Main(string[] args)
        {
            DirectorySearcher DS = new DirectorySearcher(@"..\..\..\CodeAnalyzer");
            FileExtractor FE;
            FunctionTracker FT;
            TypeRelationshipFinder TRF = new TypeRelationshipFinder();
            ClassNameFinder CNF;
            //IEnumerable<string> classNames = new List<string>();
            /*foreach (string file in DS.GetFilesWithFullPath())
            {
                foreach (var className in CNF.GetAllClassNames(functionNodes))
                {
                    classNames = classNames.Append(className);
                }
            }*/
            foreach (string file in DS.GetFilesWithFullPath())
            {
                FE = new FileExtractor(file);
                FT = new FunctionTracker(FE.GetExtractedLines());
                CNF = new ClassNameFinder(FT.GetFunctionNodes());
                /*foreach (var className in CNF.GetAllClassNames())
                {
                    classNames = classNames.Append(className);
                }
                TRF = new TypeRelationshipFinder(FT.GetClassName(), classNames, FE.GetExtractedLines());*/
                TRF = new TypeRelationshipFinder(FT.GetClassName(), CNF.GetAllClassNames(), FE.GetExtractedLines());
            }
            Console.ReadKey();
        }
#endif
    }
}