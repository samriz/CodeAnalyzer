/////////////////////////////////////////////////////////////////////
// CodeAnalyzerConsoleApp.cs - Package for using Code Analyzer.    //
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
 *  Creates instances of all of the classes I have defined in the 
 *  "CodeAnalyzer" project.
 */
/* Required Files:
 *   AnalysisDisplayer.cs, ClassNameFinder.cs, DirectorySearcher.cs,
 *   FileExtractor.cs, FunctionNode.cs, FunctionTracker.cs, 
 *   TypeRelationshipFinder.cs
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
using CodeAnalyzer;

namespace CodeAnalyzerDLLClient
{
    class CodeAnalyzerConsoleApp
    {

        //these need to be static so that all functionNodes and all classNames over all files can be gathered
        static IEnumerable<string> classNames; //declared as an IEnumerable collection so that Distinct() can be used
        static List<FunctionNode> functionNodes;

        static CodeAnalyzerConsoleApp()
        {
            classNames = new List<string>();
            functionNodes = new List<FunctionNode>();
        }
#if(test_codeanalyzerconsoleapp)
        static void Main(string[] args)
        {
            //verify that at least a directory path is entered
            //in the below function, we determine if the directory path that was entered is a valid one i.e. that it exists
            if (!VerifyCommandLineArguments(args))
            {
                Console.WriteLine("Something went wrong. Application will end. Press a key to exit.");
                Console.ReadKey();
                return;
            }
            else
            {
                string path = GetPathFromCommandLine(args);
                DirectorySearcher DS = new DirectorySearcher(path);

                //search directory or directories and fill a list with the C# file names and paths
                SetFilesBasedOnCommandLineArguments(args, DS);
                Console.WriteLine("Path: {0}\n", path);

                FileExtractor FE = new FileExtractor();
                FunctionTracker FT = new FunctionTracker();
                ClassNameFinder CNF;
                AnalysisDisplayer AD = new AnalysisDisplayer();
                TypeRelationshipFinder TRF = new TypeRelationshipFinder();

                CollectFunctionNodes(ref DS, ref FE, ref FT);
                CNF = new ClassNameFinder();
                CollectClassNames(ref CNF, ref DS);
               
                SearchAndAnalyze(args, ref DS, ref FE, ref FT, ref AD, ref TRF);
                classNames = classNames.Distinct().ToList();
            }
            Console.ReadKey();
        }
#endif
        private static void SearchAndAnalyze(string[] args, ref DirectorySearcher DS, ref FileExtractor FE, ref FunctionTracker FT, ref AnalysisDisplayer AD, ref TypeRelationshipFinder TRF)
        {
            foreach (string file in DS.GetFilesWithFullPath())
            {
                FE = new FileExtractor(file);   
                FT = new FunctionTracker(FE.GetExtractedLines());             
                TRF = new TypeRelationshipFinder(FT.GetClassName(), classNames, FE.GetExtractedLines());
                AD = new AnalysisDisplayer(FE, FT, TRF);
                DisplayBasedOnCommandLineArguments(args, AD);
            }
        }
        private static void CollectFunctionNodes(ref DirectorySearcher DS, ref FileExtractor FE, ref FunctionTracker FT)
        {
            foreach (string file in DS.GetFilesWithFullPath())
            {
                FE = new FileExtractor(file);
                FT = new FunctionTracker(FE.GetExtractedLines());
                functionNodes.AddRange(FT.GetFunctionNodes());
            }
        }
        private static void CollectClassNames(ref ClassNameFinder CNF, ref DirectorySearcher DS)
        {          
            foreach(string file in DS.GetFilesWithFullPath())
            {
                foreach (var className in CNF.GetAllClassNames(functionNodes))
                {
                    classNames = classNames.Append(className);
                }
            } 
        }
        private static string GetPathFromCommandLine(string[] args)
        {
            string path = Path.GetFullPath(args[0]);
            return path;
        }
        private static void DisplayListOfFoundFiles(DirectorySearcher DS)
        {
            List<string> DiscoveredFileNames = DS.GetFileNames();
            Console.WriteLine("\nNumber of {0} files found in this directory: {1}", DS.GetFilenameExtension(), DiscoveredFileNames.Count);
            //Console.WriteLine("Below is a numbered list of these files:", DS.GetFilenameExtension());
            Console.WriteLine("Below is a numbered list of these files:");
            int filecount = 0;
            foreach (string file in DiscoveredFileNames)
            {
                Console.WriteLine("\n{0}{1} {2}", ++filecount, ")", file);
            }
            Console.WriteLine("\n");
        }
        public static bool VerifyCommandLineArguments(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Not enough command-line arguments.");
                return false;
            }
            else if (args.Length > 4)
            {
                Console.WriteLine("Too many command-line arguments. The maximum number allowed is 4.");
                return false;
            }
            else
            {
                //if the first argument isn't a path then return false
                if (!Directory.Exists(args[0]))
                {
                    Console.WriteLine("Invalid path and/or directory doesn't exist.");
                    return false;
                }
                //if the first argument is a valid path then make sure the other command-line arguments are the allowed ones: /S, /X, /R
                {
                    if (args.Length > 1 && !args.Contains("/S") && !args.Contains("/X") && !args.Contains("/R"))
                    {
                        Console.WriteLine("Invalid command(s) detected.");
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
        }

        //determine if just the top directory should be searched or its subdirectories as well
        //Search the directory using the System.IO "Directory" class
        //save the files that are found into an instance of DirectorySearcher's "FilesWithFullPath" list
        private static void SetFilesBasedOnCommandLineArguments(string[] args, DirectorySearcher DS)
        {
            if (args.Length == 1)
            {
                DS.SetFilesInDirectory(Directory.GetFiles(DS.GetDirectoryPath(), "*" + DS.GetFilenameExtension(), SearchOption.TopDirectoryOnly).ToList());
            }
            else if (args.Length == 2)
            {
                if(args.Contains("/S"))
                {
                    DS.SetFilesInDirectory(Directory.GetFiles(DS.GetDirectoryPath(), "*" + DS.GetFilenameExtension(), SearchOption.AllDirectories).ToList());
                }
                else if (args.Contains("/X"))
                {
                    DS.SetFilesInDirectory(Directory.GetFiles(DS.GetDirectoryPath(), "*" + DS.GetFilenameExtension(), SearchOption.TopDirectoryOnly).ToList());
                }
                else if (args.Contains("/R"))
                {
                    DS.SetFilesInDirectory(Directory.GetFiles(DS.GetDirectoryPath(), "*" + DS.GetFilenameExtension(), SearchOption.TopDirectoryOnly).ToList());
                }
            }
            else if (args.Length == 3 )
            {
                if(args.Contains("/S") && args.Contains("/X"))                    
                {
                    DS.SetFilesInDirectory(Directory.GetFiles(DS.GetDirectoryPath(), "*" + DS.GetFilenameExtension(), SearchOption.AllDirectories).ToList());
                }
                else if(args.Contains("/S") && args.Contains("/R"))
                {
                    DS.SetFilesInDirectory(Directory.GetFiles(DS.GetDirectoryPath(), "*" + DS.GetFilenameExtension(), SearchOption.AllDirectories).ToList());
                }
                else if(args.Contains("/X") && args.Contains("/R"))
                {
                    DS.SetFilesInDirectory(Directory.GetFiles(DS.GetDirectoryPath(), "*" + DS.GetFilenameExtension(), SearchOption.TopDirectoryOnly).ToList());
                }
            }
            else if(args.Length == 4)
            {
                if(args.Contains("/X") && args.Contains("/R") && args.Contains("/S"))
                {
                    DS.SetFilesInDirectory(Directory.GetFiles(DS.GetDirectoryPath(), "*" + DS.GetFilenameExtension(), SearchOption.AllDirectories).ToList());
                }
            }
        }
        private static void DisplayBasedOnCommandLineArguments(string[] args, AnalysisDisplayer AD)
        {
            if(args.Length == 1)
            {
                AD.DisplayAnalysisToStandardOutput();
            }
            else if (args.Length == 2)
            {
                if (args.Contains("/X"))                    
                {
                        AD.DisplayAnalysisToXML();
                }
                else if (args.Contains("/R"))
                {
                    AD.DisplayRelationshipsToConsole();
                }
                else
                {
                    AD.DisplayAnalysisToStandardOutput();
                }
            }
            else if(args.Length == 3)
            {
                if(!args.Contains("/X"))
                {
                    
                    AD.DisplayRelationshipsToConsole();
                }
                else
                {
                    if (args.Contains("/R"))
                    {
                        AD.DisplayRelationshipsToXML();
                    }
                    else 
                    { 
                        AD.DisplayAnalysisToXML();
                    }
                }
            }
            else if(args.Length == 4)
            {
                AD.DisplayRelationshipsToXML();
            }
        }
    } 
}