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
    public class FileExtractor
    {
        private List<string> AllExtractedLines;
        private string file;

        //default constructor
        public FileExtractor()
        {
        }
        //parameterized constructor
        //using System.IO's "File" class to read all lines from the file and save it into a list
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
        //using System.IO's "File" class to read all lines from the file and save it into a list
        public void SetExtractedLines(string file)
        {
            AllExtractedLines = File.ReadAllLines(file).ToList<string>();
        }
        public void SetExtractedLines(List<string> fileLines) 
        {
            AllExtractedLines = fileLines;
        }
        public void SetFile(string file) 
        { 
            this.file = file; 
        }
        public void ExtractFileLines(string file) 
        { 
            AllExtractedLines = File.ReadAllLines(file).ToList<string>(); 
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