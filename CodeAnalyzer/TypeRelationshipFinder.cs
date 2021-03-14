/////////////////////////////////////////////////////////////////////////////
// TypeRelationshipFinder.cs - Finds all of the type relationships         //
// in the package.                                                         //
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
 *  
 */
/* Required Files:
 *   
 *   
 * Build command:
 *   csc 
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
        //private IEnumerable<string> relationships;

        public TypeRelationshipFinder()
        {

        }
        public TypeRelationshipFinder(string className, List<string> classNames, List<string> fileLines)
        {
            this.className = className;
            this.classNames = classNames;
            this.fileLines = fileLines;

            //relationships = new List<string>();
        }
        public TypeRelationshipFinder(string className, IEnumerable<string> classNames, List<string> fileLines)
        {
            this.className = className;
            this.classNames = classNames.ToList();
            this.fileLines = fileLines;
            //relationships = new List<string>();
        }
        public IEnumerable<string> FindRelationships()
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
                        Console.WriteLine(relationshipString);
                    }
                }
            }
            return relationships;
        }
        public IEnumerable<string> GetRelationships()
        {
            //List<string> relationshipList = FindRelationships().Distinct().ToList();
            //return relationshipList;
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