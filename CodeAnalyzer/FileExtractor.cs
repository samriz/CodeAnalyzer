/////////////////////////////////////////////////////////////////////
// FileExtractor.cs - Read and save all data from C# file.         //
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
 *  Using System.IO's "File" class, we read all lines in a file and
 *  save them into a list.
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
        }
        public string GetFile() 
        { 
            return file; 
        }
        public List<string> GetExtractedLines() 
        { 
            return AllExtractedLines; 
        }
        public void SetExtractedLines(string file) 
        { 
            AllExtractedLines = File.ReadAllLines(file).ToList<string>(); 
        }
        public void SetFile(string file) 
        { 
            this.file = file; 
        }
        /*private int FindPositionOfNamespace(List<string> list)
        {
            int positionOfNamespace = 0;
            Match namespaceMatch;
            for (int i = 0; i < list.Count; i++)
            {
                namespaceMatch = Regex.Match(list[i], @"^namespace\s+");

                if(namespaceMatch.Success)
                {
                    positionOfNamespace = i;
                    return positionOfNamespace;
                }
            }
            return positionOfNamespace;
        }*/
        public List<string> ExtractFileLines(string file) 
        { 
            return File.ReadAllLines(file).ToList<string>(); 
        }

#if (test_fileextractor)
        static void Main(string[] args)
        {
            DirectorySearcher DS = new DirectorySearcher(@"..\..\..\CodeAnalyzer");
            FileExtractor FE = new FileExtractor();
            foreach (var file in DS.GetFilesWithFullPath())
            {
                FE = new FileExtractor(file);
                foreach(var line in FE.GetExtractedLines())
                {
                    Console.WriteLine(line);
                }
            }
            Console.ReadKey();
        }
#endif
    }
}