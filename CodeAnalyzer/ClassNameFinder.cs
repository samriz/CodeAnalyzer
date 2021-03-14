/////////////////////////////////////////////////////////////////////////////////
// ClassNameFinder.cs - Finds all of the user-defined class names in a file    //
// in the package.                                                             //
// ver 1.0                                                                     //
// Language:    C#, 2020, .Net Framework 4.7.2                                 //
// Platform:    MSI GS65 Stealth, Win10                                        //
// Application: CSE681, Project #2, Winter 2021                                //
// Author:      Sameer Rizvi, Syracuse University                              //
//              srizvi@syr.edu                                                 //
/////////////////////////////////////////////////////////////////////////////////
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
    public class ClassNameFinder
    {
        private static readonly string inheritancePattern;
        private List<FunctionNode> functionNodes;
        static ClassNameFinder()
        {
            inheritancePattern = @"(class)\s+(\w+)\s*\:\s*(\w+)";           
        }
        public ClassNameFinder()
        {
            functionNodes = new List<FunctionNode>();
        }
        public ClassNameFinder(List<FunctionNode> functionNodes)
        {
            this.functionNodes = functionNodes;
        }
        public List<string> GetAllClassNames()
        {
            List<string> classNames = new List<string>();

            foreach (var node in functionNodes)
            {
                classNames.Add(node.GetClassName());
            }
            IEnumerable<string> distinctClassNames = classNames.Distinct();
            List<string> distinctClassNamesList = new List<string>();

            for (int i = 0; i < distinctClassNames.ToList().Count; i++)
            {
                string[] s = distinctClassNames.ElementAt(i).ToString().Split(' ');
                string justclassname = (string)s.GetValue(s.Length - 1);
                distinctClassNamesList.Add(justclassname);
            }
            return distinctClassNamesList;
        }
        public List<string> GetAllClassNames(List<FunctionNode> functionNodes)
        {
            List<string> classNames = new List<string>();

            foreach(var node in functionNodes)
            {
                classNames.Add(node.GetClassName());
            }
            IEnumerable<string> distinctClassNames = classNames.Distinct();
            List<string> distinctClassNamesList = new List<string>();

            for (int i = 0; i < distinctClassNames.ToList().Count; i++)
            {
                string[] s = distinctClassNames.ElementAt(i).ToString().Split(' ');
                string justclassname = (string)s.GetValue(s.Length - 1);
                distinctClassNamesList.Add(justclassname);
            }
            return distinctClassNamesList;
        }
        public bool InheritanceExists(List<string> ExtractedLines)
        {
            Match inheritanceMatch;
            foreach(var line in ExtractedLines)
            {
                inheritanceMatch = Regex.Match(line, inheritancePattern);
                if (inheritanceMatch.Success)
                {
                    return true;
                }
            }
            return false;
        }
// ---------------- test stub --------------------
#if (test_classnamefinder)
        static void Main(string[] args)
        {
            DirectorySearcher DS = new DirectorySearcher(@"..\..\..\CodeAnalyzer");
            FileExtractor FE;
            FunctionTracker FT;
            ClassNameFinder CNF = new ClassNameFinder();
            foreach (string file in DS.GetFilesWithFullPath())
            {
                FE = new FileExtractor(file);
                FT = new FunctionTracker(FE.GetExtractedLines());
                CNF = new ClassNameFinder(FT.GetFunctionNodes());
            }
            foreach(var className in CNF.GetAllClassNames())
            {
                Console.WriteLine(className);
            }
            Console.ReadKey();
        }
#endif
    }
}