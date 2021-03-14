/////////////////////////////////////////////////////////////////////
// FileExtractor.cs - Read and save all data from C# file          //
// ver 1.0                                                         //
// Language:    C#, 2020, .Net Framework 4.7.2                     //
// Platform:    MSI GS65 Stealth, Win10                            //
// Application: CSE681, Project #2, Winter 2021                    //
// Author:      Sameer Rizvi, Syracuse University                  //
//              srizvi@syr.edu                                     //
/////////////////////////////////////////////////////////////////////
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace CodeAnalyzer
{
    //this is a test comment
    public class FileExtractor
    {
        //this is another test comment
        private List<string> AllExtractedLines;
        string file;

        public FileExtractor()
        {

        }
        public FileExtractor(string file)
        {
            this.file = file;
            AllExtractedLines = File.ReadAllLines(file).ToList<string>();
            //RemoveWhiteSpaceAndBlankNewLines();
            //TrimLines();
            //AdjustScopes();
        }
        public string GetFile() => file;
        public List<string> GetExtractedLines()
        {
            return AllExtractedLines;
        }
        public void SetExtractedLines(string file) => AllExtractedLines = File.ReadAllLines(file).ToList<string>();
        public void SetFile(string file) => this.file = file;
        private int FindPositionOfNamespace(List<string> list)
        {
            int positionOfNamespace = 0;
            Match namespaceMatch;
            for (int i = 0; i < list.Count; i++)
            {
                namespaceMatch = Regex.Match(list[i], @"^namespace\s+");

                if(namespaceMatch.Success)
                //if (list[i].Contains("namespace"))
                {
                    positionOfNamespace = i;
                    return positionOfNamespace;
                }
            }
            return positionOfNamespace;
        }
        public List<string> ExtractFileLines(string file)
        {
            //extract file lines and exclude comments

            //List<string> AllExtractedLines = File.ReadAllLines(file).ToList<string>();
            /*List<string> ExtractedLinesStartingAtNamespace = new List<string>();
            for (int i = FindPositionOfNamespace(AllExtractedLines); i < AllExtractedLines.Count; i++)
            {
                ExtractedLinesStartingAtNamespace.Add(AllExtractedLines[i]);
            }
            return FindAndRemoveComments(ExtractedLinesStartingAtNamespace);*/
            //return FindAndRemoveComments(File.ReadAllLines(file).ToList<string>());
            return File.ReadAllLines(file).ToList<string>();
        }
#if (test_fileextractor)
        static void Main(string[] args)
        {

        }
#endif
    }
}