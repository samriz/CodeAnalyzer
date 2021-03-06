/////////////////////////////////////////////////////////////////////////////
// ClassRelationshipFinder.cs - Determine all of the type relationships    //
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
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/*1.You need to do two passes. In the first pass, you need to find all type names including classes, structs and interfaces. In the second pass, you will search these names in every line. Depending on the scope of the line where you found the name, you will establish a relationship. For example, say you have a type name A in your list that you found after the first pass, and during the second pass you encountered A in a function in class B. Then you will say B uses A.*/

namespace CodeAnalyzer
{
    public class ClassRelationshipFinder
    {
        private FileExtractor FE;
        private FunctionTracker FT;
        private static readonly string inheritancePattern;
        static List<string> distinctClassNamesList;
        static ClassRelationshipFinder()
        {
            inheritancePattern = @"(class)\s+(\w+)\s*\:\s*(\w+)";
            distinctClassNamesList = new List<string>();
        }
        public ClassRelationshipFinder(FileExtractor FE, FunctionTracker FT)
        {
            this.FE = FE;
            this.FT = FT;
        }
        public void TypeRelationships()
        {
            List<string> classNames = new List<string>();

            foreach(var node in this.FT.GetFunctionNodes())
            {
                classNames.Add(node.GetClassName());
            }

            IEnumerable<string> distinctClassNames = classNames.Distinct();
            distinctClassNamesList = distinctClassNames.ToList();
            
            for (int i = 0; i < distinctClassNamesList.Count(); i++)
            {
                string[] s = distinctClassNamesList[i].Split(' ');
                string justclassname = (string)s.GetValue(s.Length - 1);
                distinctClassNamesList.Add(justclassname);
            }
            foreach(var classname in distinctClassNamesList)
            {
                Console.WriteLine(classname);
            }
            /*foreach(var line in this.FE.GetExtractedLines())
            {
                foreach(var distinct in distinctClassNamesList)
                {
                    
                }
            }*/
        }
        public bool InheritanceExists()
        {
            Match inheritanceMatch;
            foreach(var line in FE.GetExtractedLines())
            {
                inheritanceMatch = Regex.Match(line, inheritancePattern);
                if (inheritanceMatch.Success)
                {
                    return true;
                }
            }
            return false;
        }
        public bool AggregationExists()
        {
            return false;
        }
        public bool CompositionExists()
        {
            return false;
        }
        public bool UsingExists()
        {
            return false;
        }
#if (test_classrelationshipfinder)
        static void Main(string[] args)
        {

        }
#endif
    }
}
