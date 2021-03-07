﻿/////////////////////////////////////////////////////////////////////
// CodeAnalyzerConsoleApp.cs - Package for using Code Analyzer     //
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
using CodeAnalyzer;
/*
 1) the Code Analyzer needs to accept a directory path in the command-line arguments 
    -this is taken care of by DirectorySearcher
2) we need to read in all of the C# files in this path
    -this is taken care of by FileExtractor
 */
namespace CodeAnalyzerDLLClient
{
    class CodeAnalyzerConsoleApp
    {
        static FileExtractor FE;
        static FunctionTracker FT;
        static ClassNameFinder CNF;
        static AnalysisDisplayer AD;
        static TypeRelationshipFinder TRF;
        static IEnumerable<string> classNames;

        static CodeAnalyzerConsoleApp()
        {
            classNames = new List<string>();
        }

#if(test_codeanalyzerconsoleapp)
        static void Main(string[] args)
        {
            //verify that at least a directory path is entered
            if (!VerifyCommandLineArguments(args))
            {
                Console.WriteLine("Something went wrong. Application will end. Press a key to exit.");
                Console.ReadKey();
                return;
            }
            else
            {
                string path = GetPathFromCommandLine(args);
<<<<<<< HEAD
                DS = new DirectorySearcher(path);
=======
                //path = path + Environment.NewLine;
                DirectorySearcher DS = new DirectorySearcher(path);

>>>>>>> parent of e467eb1 (Made DS in CodeAnalyzerConsoleApp.cs static. Refactored code in Main() by placing it in a separate function.)
                SetFilesBasedOnCommandLineArguments(args, DS);
                Console.WriteLine("Path: {0}\n", path);
<<<<<<< HEAD
                SearchAndAnalyze(args);
                classNames = classNames.Distinct();
                classNames = classNames.ToList();
                
                foreach (string file in DS.GetFilesWithFullPath())
                {
                    FE = new FileExtractor(file);
                    FT = new FunctionTracker(FE);
                    FT.DetectFunctionsAndScopes();
                    TRF = new TypeRelationshipFinder(classNames, FE);
                    AD = new AnalysisDisplayer(file, FT, TRF);
                    DisplayBasedOnCommandLineArguments(args, AD, FE);
                }
            }
            Console.ReadKey();
        }
#endif
        private static void SearchAndAnalyze(string[] args)
        {
            foreach (string file in DS.GetFilesWithFullPath())
            {
                FE = new FileExtractor(file);
                FT = new FunctionTracker(FE);
                //FT.DetectFunctionsAndScopes();
                CNF = new ClassNameFinder(FE, FT);

                foreach (var className in CNF.GetAllClassNames())
=======
                //DisplayListOfFoundFiles(DS);
                foreach (string file in DS.GetFilesWithFullPath())
                {
                    FE = new FileExtractor(file);
                    FT = new FunctionTracker(FE);
                    FT.DetectFunctionsAndScopes();
                    //FT.CountLines();
                    CNF = new ClassNameFinder(FE, FT);
                    
                    foreach(var className in CNF.GetAllClassNames())
                    {
                        classNames = classNames.Append(className);
                    }
                    AD = new AnalysisDisplayer(file, FT, CNF);
                    //AD.DisplayAnalysisToStandardOutput();


                    //DisplayBasedOnCommandLineArguments(args, AD, FE);


                    /*foreach (string line in FE.GetExtractedLines())
                    {
                        Console.WriteLine(line);
                    }*/
                }
                classNames = classNames.Distinct();
                classNames = classNames.ToList();
                foreach (var classname in classNames)
>>>>>>> parent of e467eb1 (Made DS in CodeAnalyzerConsoleApp.cs static. Refactored code in Main() by placing it in a separate function.)
                {
                    Console.WriteLine(classname);
                }
<<<<<<< HEAD
                //AD = new AnalysisDisplayer(file, FT, CNF);
                //DisplayBasedOnCommandLineArguments(args, AD, FE);
=======
                //TRF = new TypeRelationshipFinder();
>>>>>>> parent of e467eb1 (Made DS in CodeAnalyzerConsoleApp.cs static. Refactored code in Main() by placing it in a separate function.)
            }
            Console.ReadKey();
        }
#endif
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
                for (int i = 1; i < args.Length; i++)
                {
                    if (!args[i].Equals("/S") && !args[i].Equals("/X") && !args[i].Equals("/R"))
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
            return false;
        }
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
                    //display relationships between all types defined in file set, e.g., inheritance, composition, aggregation, and using relationships instead of the function sizes and complexities
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
                    //display relationships between all types defined in file set, e.g., inheritance, composition, aggregation, and using relationships instead of the function sizes and complexities
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
        private static void DisplayBasedOnCommandLineArguments(string[] args, AnalysisDisplayer AD, FileExtractor FE)
        {
            if(args.Length == 1)
            {
                AD.DisplayAnalysisToStandardOutput();
            }
            else if (args.Length == 2)
            {
                if (args.Contains("/X"))                    
                {
                        //write output to XML file
                        AD.DisplayAnalysisToXML();
                }
                else if (args.Contains("/R"))
                {
                    //display relationships between all types defined in file set, e.g., inheritance, composition, aggregation, and using relationships instead of the function sizes and complexities
                    AD.DisplayRelationshipsToConsole();
                }
                else
                {
                    AD.DisplayAnalysisToStandardOutput();
                }
            }
            else if(args.Length == 3 || args.Length == 4)
            {
                if(!args.Contains("/X"))
                    //if (!args.Contains("/X") && args.Contains("/S") || !args.Contains("/X") && args.Contains("/S") && args.Contains("/R"))
                    //if (args[i] != "/X" && args[i] == "/S" || args[i] != "/X" && args[i] == "/S" && args[i] == "/R")
                {
                    AD.DisplayAnalysisToStandardOutput();
                }
                else
                {

                    AD.DisplayAnalysisToXML();
                }
            }
        }
    } 
}