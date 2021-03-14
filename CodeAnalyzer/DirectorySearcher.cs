/////////////////////////////////////////////////////////////////////
// DirectorySearcher.cs - Find all files that meet a particular    //
// restraint within a directory path of the user's choice          //
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

namespace CodeAnalyzer
{
    public class DirectorySearcher
    {
        //accept user-defined directory and search it
        private string DirectoryPath; //the directory we want to search
        private string FilenameExtension; //.cs, .cpp, .h, .py, etc.
        private List<string> FilesWithFullPath; //files with full path
        private List<string> FileNames = new List<string>(); //just file names

        public DirectorySearcher(string DirectoryPath, string FilenameExtension = ".cs") //filename extension defaulted to .cs
        {
            this.DirectoryPath = DirectoryPath;
            this.FilenameExtension = FilenameExtension;
        }
        public DirectorySearcher(string DirectoryPath, List<string> FilesWithFullPath)
        {
            this.DirectoryPath = DirectoryPath;
            this.FilesWithFullPath = FilesWithFullPath;
            //AddFileNamesToList(FilesWithFullPath);
        }
        public bool IsDirectoryPathTypeAString(string path)
        {
            //is the user-defined directory path a string?
            if (DirectoryPath.GetType() != typeof(string))
            {
                return false;
            }
            else 
            { 
                return true; 
            }
        }
        public void SetDirectoryPath(string DirectoryPath) => this.DirectoryPath = DirectoryPath;
        public void SetFilesInDirectory(List<string> FilesWithFullPath)
        {
            this.FilesWithFullPath = FilesWithFullPath;
            //FilesWithFullPath = Directory.GetFiles(DirectoryPath, filetype, SearchOption.AllDirectories).ToList();
            //AddFileNamesToList(FilesWithFullPath);
        }
        public string GetDirectoryPath() 
        { 
            return DirectoryPath; 
        }
        public string GetFilenameExtension() 
        { 
            return FilenameExtension;
        }
        public List<string> GetFilesWithFullPath() 
        { 
            return FilesWithFullPath; 
        }
        public List<string> GetFileNames() 
        { 
            return FileNames; 
        }
        public void AddFileNamesToList(List<string> Files)
        {
            foreach (string File in Files)
            {
                //add just the file names into a list
                FileNames.Add(Path.GetFileName(File));
            }
        }
// ---------------- test stub --------------------
#if(test_directorysearcher)
        static void Main(string[] args)
        {
            DirectorySearcher DS = new DirectorySearcher(@"C:\Users\srizv\OneDrive - Syracuse University\Syracuse University\Courses\CSE 681 (2)\Project 2\CodeAnalyzer");
            foreach(var filename in DS.GetFileNames())
            {
                Console.WriteLine(filename);
            }
            Console.ReadKey();
        }
#endif
    }
}