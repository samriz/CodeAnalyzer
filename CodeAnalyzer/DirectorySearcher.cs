/////////////////////////////////////////////////////////////////////
// DirectorySearcher.cs - Find all C# files within a directory     //
//                                                                 //
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
 *  This is a very simple class. All it does is 
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

namespace CodeAnalyzer
{
    public class DirectorySearcher
    {
        //accept user-defined directory and search it
        private string DirectoryPath; //the directory we want to search
        private string FilenameExtension; //.cs, .cpp, .h, .py, etc.
        private List<string> FilesWithFullPath; //files with full path
        private List<string> FileNames; //just file names

        public DirectorySearcher(string DirectoryPath, string FilenameExtension = ".cs") //filename extension defaulted to .cs
        {
            this.DirectoryPath = DirectoryPath;
            this.FilenameExtension = FilenameExtension;
            //FileNames = new List<string>();
        }
        public DirectorySearcher(string DirectoryPath, List<string> FilesWithFullPath)
        {
            this.DirectoryPath = DirectoryPath;
            this.FilesWithFullPath = FilesWithFullPath;
            //FileNames = new List<string>();
        }
        public void SetDirectoryPath(string DirectoryPath) 
        { 
            this.DirectoryPath = DirectoryPath; 
        }
        public void SetFilesInDirectory(List<string> FilesWithFullPath)
        {
            this.FilesWithFullPath = FilesWithFullPath;
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
            DirectorySearcher DS = new DirectorySearcher(@"..\..\..\CodeAnalyzer");
            foreach(var filename in DS.GetFileNames())
            {
                Console.WriteLine(filename);
            }
            Console.ReadKey();
        }
#endif
    }
}