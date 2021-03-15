/////////////////////////////////////////////////////////////////////
// FunctionTracker.cs - Find functions and count their scopes      //
// and number of lines.                                            //
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
 *  Use Regex and Match classes to search for patterns that match
 *  namespaces, class, functions, loops, and statements which can contain
 *  braces.
 *  
 *  Build list of function nodes. Each function node pertains to a function.
 *  
 */
/* Required Files:
 *   FunctionNode.cs
 *   
 * Maintenance History:
 * --------------------
 * ver 1.2 : 22 January 2021
 * - first release
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeAnalyzer
{
    public class FunctionTracker
    {
        private Match namespaceMatch;
        private Match classMatch;
        private Match functionMatch1;
        private Match openingBraceMatch;
        private Match startScopeMatch;
        private Match endBraceMatch;
        private Match doWhileMatch;
        private Match elseMatch;
        Stack functionStack;
        private string className;
        private string namespaceName;

        readonly List<string> ExtractedLines;
        private List<FunctionNode> functionNodes;

        //regular expression patterns:
        private static readonly string namespacePattern;
        private static readonly string classPattern;
        private static readonly string functionPattern;
        private static readonly string openingBracePattern;
        private static readonly string startScopePattern;
        private static readonly string elsePattern;
        private static readonly string doWhilePattern;
        private static readonly string endBracePattern;

        //static contructor below
        static FunctionTracker()
        {
            namespacePattern = @"(namespace)\s+\w+\s*\{";
            classPattern = @"(class)\s+(\w+)\s*\{";
            functionPattern = @"(\w+\s+)?(\w+\s+)(\w+\s+)(\w+\s*)\(";
            openingBracePattern = @"\{";
            startScopePattern = @"(for|foreach|while|if|else\s+if)\s*\(";
            elsePattern = @"else\s*{";
            doWhilePattern = @"(do)\s*\{";
            endBracePattern = @"\}";    
        }

        //default constructor
        public FunctionTracker()
        {
            ExtractedLines = new List<string>();
            functionNodes = new List<FunctionNode>();            
            functionStack = new Stack();
            className = "";
            namespaceName = "";   
        }

        //parameterized constructor
        public FunctionTracker(List<string> ExtractedLines) : this()
        {
            this.ExtractedLines = ExtractedLines;
            DetectFunctionsAndScopes();
        }

        //find namespace, class, functions, and scopes based on certain patterns using Regex and Match classes
        private void DetectFunctionsAndScopes()
        {
            int scopeCount = 0;
            int numberOfLines = 0;
            FunctionNode FN;
            string functionName;
            List<string> adjustedLines = ExtractedLines;
            adjustedLines = TrimLines(adjustedLines);
            adjustedLines = AdjustScopes(adjustedLines);

            //start at the namespace
            for (int i = FindPositionOfNamespace(adjustedLines); i < adjustedLines.Count; i++)
            {
                namespaceMatch = Regex.Match(adjustedLines[i], namespacePattern);
                if (namespaceMatch.Success)
                {
                    namespaceName = adjustedLines[i];
                }
                classMatch = Regex.Match(adjustedLines[i], classPattern);
                if (classMatch.Success)
                {
                    string adjustedLine = adjustedLines[i];
                    int linelength = adjustedLine.Length;
                    if(adjustedLine[linelength - 1] == '{')
                    {
                        className = adjustedLine;
                        className = className.Remove(linelength - 1, 1);
                    }
                    else
                    {
                        className = adjustedLine;
                    }                   
                }
                functionMatch1 = Regex.Match(adjustedLines[i], functionPattern);
                startScopeMatch = Regex.Match(adjustedLines[i], startScopePattern);
                elseMatch = Regex.Match(adjustedLines[i], elsePattern);
                doWhileMatch = Regex.Match(adjustedLines[i], doWhilePattern);
                if (functionMatch1.Success && !startScopeMatch.Success && !elseMatch.Success && !doWhileMatch.Success)
                {
                    functionStack.Push(adjustedLines[i]);
                    functionName = adjustedLines[i].Remove(adjustedLines[i].Length - 1, 1);
                    FN = new FunctionNode(functionName);

                    //run through just the function to collect function info
                    CollectFunctionData(adjustedLines, ref i, ref FN, scopeCount, numberOfLines);
                    scopeCount = 0; //reset the scope count for next function
                }
            }
        }

        //gather information about function
        private void CollectFunctionData(List<string> functionLines, ref int functionPosition, ref FunctionNode FN, int scopeCount, int numberOfLines)
        {
            //traverse the function only
            for (int j = functionPosition + 1; j < functionLines.Count; j++)
            {
                ++numberOfLines;
                startScopeMatch = Regex.Match(functionLines[j], startScopePattern);
                elseMatch = Regex.Match(functionLines[j], elsePattern);

                doWhileMatch = Regex.Match(functionLines[j], doWhilePattern);
                endBraceMatch = Regex.Match(functionLines[j], endBracePattern);
                openingBraceMatch = Regex.Match(functionLines[j], openingBracePattern);

                //push and pop off stack based on matches
                if (startScopeMatch.Success && openingBraceMatch.Success)
                {
                    functionStack.Push(functionLines[j]);
                }
                if (elseMatch.Success && openingBraceMatch.Success)
                {
                    functionStack.Push(functionLines[j]);
                }
                if (doWhileMatch.Success && openingBraceMatch.Success)
                {
                    functionStack.Push(functionLines[j]);
                }
                if (endBraceMatch.Success)
                {
                    ++scopeCount;
                    functionStack.Pop();
                }
                if (functionStack.Count < 1)
                {
                    --numberOfLines; //decrease by 1 so we don't count the function ending brace
                    --scopeCount; //decrease scopeCount by 1 so we don't take into account the curly brackets of the function itself
                    FN.SetClassName(className);
                    FN.SetNamespaceName(namespaceName);
                    FN.SetNumberOfScopes(scopeCount);
                    numberOfLines += scopeCount;
                    FN.SetNumberOfLines(numberOfLines);
                    functionNodes.Add(FN);
                    break;
                }
            }
        }

        //function to delete whitespace from a line as long as the line's length is greater than zero
        private List<string> TrimLines(List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                //skip empty lines
                if (lines[i].Length == 0)
                {
                    continue;
                }

                //trim off blank spaces
                else
                {
                    lines[i] = lines[i].Trim();
                }
            }
            return lines;
        }

        //move an opening bracket to the line it is associated with
        private List<string> AdjustScopes(List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].StartsWith("{"))
                {
                    lines[i - 1] = lines[i - 1] + "{";
                    lines.RemoveAt(i);
                }
            }
            return lines;
        }

        //find index in list where namespace is declared
        private int FindPositionOfNamespace(List<string> list)
        {
            int positionOfNamespace = 0;
            Match namespaceMatch;
            for (int i = 0; i < list.Count; i++)
            {
                namespaceMatch = Regex.Match(list[i], @"namespace\s+");

                //if namespace is a match in the list, then save the index of where it is in the list
                if (namespaceMatch.Success)
                {
                    positionOfNamespace = i;
                    return positionOfNamespace;
                }
            }
            return positionOfNamespace;
        }
        public void SetClassName(string className) 
        { 
            this.className = className; 
        }
        public void SetNamespaceName(string namespaceName) 
        { 
            this.namespaceName = namespaceName; 
        }
        public string GetClassName() 
        { 
            return className; 
        }
        public string GetNamespaceName() 
        { 
            return namespaceName; 
        }
        public List<FunctionNode> GetFunctionNodes() 
        { 
            return functionNodes; 
        }

        // ---------------- test stub --------------------
#if (test_functiontracker)
        static void Main(string[] args)
        {
            DirectorySearcher DS = new DirectorySearcher(@"..\..\..\CodeAnalyzer");
            FileExtractor FE;
            FunctionTracker FT = new FunctionTracker();
            foreach (string file in DS.GetFilesWithFullPath())
            {
                FE = new FileExtractor(file);
                FT = new FunctionTracker(FE.GetExtractedLines());
            }
            foreach(var node in FT.GetFunctionNodes())
            {
                Console.WriteLine(node.GetFunctionName());
            }
            Console.ReadKey();
        }
#endif
    }
}