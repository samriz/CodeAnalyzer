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
        //need to figure out how to detect namespaces, class, functions, etc. and analyze them
        static List<string> keywords = new List<string> { "namespace", "class", "if", "for", "foreach", "while", "do", "public", "private", "static", "void", "{", "}" };

        readonly List<string> ExtractedLines;
        readonly private List<FunctionNode> functionNodes;

        //regular expression patterns:
        private static readonly string namespacePattern;
        private static readonly string classPattern;
        private static readonly string functionPattern;
        private static readonly string openingBracePattern;
        private static readonly string startScopePattern;
        private static readonly string elsePattern;
        private static readonly string doWhilePattern;
        //private static readonly string endScopePattern;
        private static readonly string endBracePattern;

        static FunctionTracker()
        {
            //Console.WriteLine("FunctionTracker Static Constructor is invoked.");
            namespacePattern = @"(namespace)\s+\w+\s*\{";
            classPattern = @"(class)\s+(\w+)\s*\{";
            //functionPattern = @"(\w+\s+)?(\w+\s+)?\w+\s+\w+\s*\(";
            //functionPattern = @"(public | private | static)?\s+(\w+\s+)?(\w+\s+)?\w+\s*\(";
            functionPattern = @"(\w+\s+)?(\w+\s+)(\w+\s+)(\w+\s*)\(";
            openingBracePattern = @"\{";
            startScopePattern = @"(for|foreach|while|if|else\s+if)\s*\(";
            elsePattern = @"else\s*{";
            doWhilePattern = @"(do)\s*\{";
            //endScopePattern = @"\){";
            endBracePattern = @"\}";    
        }
        public FunctionTracker()
        {
            ExtractedLines = new List<string>();
            functionNodes = new List<FunctionNode>();
        }
        public FunctionTracker(List<string> ExtractedLines) : this()
        {
            this.ExtractedLines = ExtractedLines;
        }
        public void DetectFunctionsAndScopes()
        {
            Stack functionStack = new Stack();
            int scopeCount = 0;
            int numberOfLines = 0;
            FunctionNode FN;
            Match namespaceMatch;
            Match classMatch;
            Match functionMatch1;
            Match openingBraceMatch;
            Match startScopeMatch;
            Match endBraceMatch;
            Match doWhileMatch;
            Match elseMatch;
            string functionName;
            string className = "";
            string namespaceName = "";

            //List<string> adjustedLines = FE.GetExtractedLines();
            List<string> adjustedLines = ExtractedLines;
            //List<string> adjustedLines = new List<string>(FE.GetExtractedLines());
            //adjustedLines = RemoveWhiteSpaceAndBlankNewLines(adjustedLines);
            adjustedLines = TrimLines(adjustedLines);
            adjustedLines = AdjustScopes(adjustedLines);
            //RemoveUsings(adjustedLines);

            for (int i = FindPositionOfNamespace(adjustedLines); i < adjustedLines.Count; i++)
            //for (int i = 92; i < adjustedLines.Count; i++)
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
                //openingBraceMatch = Regex.Match(adjustedLines[i], openingBracePattern);
                if (functionMatch1.Success && !startScopeMatch.Success && !elseMatch.Success && !doWhileMatch.Success)
                {
                    functionStack.Push(adjustedLines[i]);
                    functionName = adjustedLines[i].Remove(adjustedLines[i].Length - 1, 1);
                    FN = new FunctionNode(functionName);
                    for(int j = i+1; j < adjustedLines.Count; j++) //traverse the function only
                    {
                        ++numberOfLines;
                        startScopeMatch = Regex.Match(adjustedLines[j], startScopePattern);
                        elseMatch = Regex.Match(adjustedLines[j], elsePattern);

                        doWhileMatch = Regex.Match(adjustedLines[j], doWhilePattern);
                        endBraceMatch = Regex.Match(adjustedLines[j], endBracePattern);
                        openingBraceMatch = Regex.Match(adjustedLines[j], openingBracePattern);
                        if (startScopeMatch.Success && openingBraceMatch.Success)
                        //if (startScopeMatch.Success || elseMatch.Success || doWhileMatch.Success)
                        {
                            functionStack.Push(adjustedLines[j]);
                        }
                        if (elseMatch.Success && openingBraceMatch.Success)
                        {
                            functionStack.Push(adjustedLines[j]);
                        }
                        if (doWhileMatch.Success && openingBraceMatch.Success)
                        {
                            functionStack.Push(adjustedLines[j]);
                        }
                        if (endBraceMatch.Success)
                        {
                            ++scopeCount;
                            functionStack.Pop();
                        }
                        if(functionStack.Count < 1)
                        {
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
                    scopeCount = 0;
                }
            }
        }
        /*public void CountLines()
        {
            Stack functionStack = new Stack();
            int numberOfLines = 0;
            string functionName;
            Match functionMatch;
            Match startScopeMatch;
            Match endScopeMatch;
            Match doWhileMatch;
            Match elseMatch;

           for (int i = FindPositionOfNamespace(FE.GetExtractedLines()); i < FE.GetExtractedLines().Count; i++)
            //for (int i = 0; i < FE.GetExtractedLines().Count; i++)
            {
                functionMatch = Regex.Match(FE.GetExtractedLines()[i], functionPattern);

                if (functionMatch.Success)
                {
                    functionStack.Push(FE.GetExtractedLines()[i]);
                    functionName = FE.GetExtractedLines()[i];
                    functionName = functionName.Trim();
                    //FN = new FunctionNode(lines[i]);
                    for (int j = i + 1; j < FE.GetExtractedLines().Count; j++) //traverse the function only
                    {
                        ++numberOfLines;
                        
                        startScopeMatch = Regex.Match(FE.GetExtractedLines()[j], startScopePattern);
                        elseMatch = Regex.Match(FE.GetExtractedLines()[j], elsePattern);

                        doWhileMatch = Regex.Match(FE.GetExtractedLines()[j], doWhilePattern);
                        endScopeMatch = Regex.Match(FE.GetExtractedLines()[j], endBracePattern);
                        
                        if (startScopeMatch.Success)
                        {
                            functionStack.Push(FE.GetExtractedLines()[j]);
                        }
                        if (elseMatch.Success)
                        {
                            functionStack.Push(FE.GetExtractedLines()[j]);
                        }
                        if (doWhileMatch.Success)
                        {
                            functionStack.Push(FE.GetExtractedLines()[j]);
                        }
                        if (endScopeMatch.Success)
                        {
                            functionStack.Pop();
                        }
                        if (functionStack.Count < 1)
                        {
                            foreach(var node in functionNodes)
                            {
                                if(functionName == node.GetFunctionName())
                                {
                                    node.SetNumberOfLines(numberOfLines);
                                }
                            }
                            break;
                        }
                    }
                    numberOfLines = 0;
                }
            }
        }*/
        private List<string> TrimLines(List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Length == 0)
                {
                    continue;
                }
                else
                {
                    //AllExtractedLines[i] = AllExtractedLines[i].Trim();
                    lines[i] = lines[i].Trim();
                }
            }
            return lines;
        }
        private List<string> RemoveWhiteSpaceAndBlankNewLines(List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Length == 0)
                {
                    lines.RemoveAt(i);
                }
                else
                {
                    lines[i] = lines[i].Trim();
                }
            }
            return lines;
        }
        private List<string> AdjustScopes(List<string> lines)
        {
            //for(int i = 1; i < scopeList.Count; i++)
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
        private List<string> FindAndRemoveComments(List<string> lines)
        {
            Match singleLineCommentMatch;
            Match multiLineCommentMatch1;

            for (int i = 0; i < lines.Count; i++)
            {
                singleLineCommentMatch = Regex.Match(lines[i], @"\/\/");
                multiLineCommentMatch1 = Regex.Match(lines[i], @"\/\*\s*");
            }
            return lines;
        }
        private void RemoveUsings(List<string> lines)
        {
            Match usingMatch;
            for (int i = 0; i < lines.Count; i++)
            {
                usingMatch = Regex.Match(lines[i], @"^(using)\s+");
                if (usingMatch.Success)
                {
                    lines.RemoveAt(i);
                }
            }
        }
        private int FindPositionOfNamespace(List<string> list)
        {
            int positionOfNamespace = 0;
            Match namespaceMatch;
            for (int i = 0; i < list.Count; i++)
            {
                namespaceMatch = Regex.Match(list[i], @"namespace\s+");

                if (namespaceMatch.Success)
                {
                    positionOfNamespace = i;
                    return positionOfNamespace;
                }
            }
            return positionOfNamespace;
        }
        /*private int CountLines(List<string> lines)
        {
            int lineCount = 0;
            foreach (string line in lines)
            {
                ++lineCount;
            }
            return lineCount;
        }*/
        /*private List<string> AdjustScopes(List<string> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Contains("{"))
                {
                    list[i - 1] = list[i - 1] + "{";
                    list.RemoveAt(i);
                }
            }
            return list;
        }*/
        /*private void RemoveWhiteSpaceAndBlankNewLines(List<string> lines)
        {
            for (int i = 0; i < lines.Count; i++)
            {
                if (lines[i].Length == 0)
                {
                    lines.Remove(lines[i]);
                }
                else
                {
                    lines[i] = lines[i].Trim();
                }
            }
        }*/
        public List<FunctionNode> GetFunctionNodes()
        {
            return functionNodes;
        }
        public List<string> GetKeywords()
        {
            return keywords;
        }
#if (test_functiontracker)
        static void Main(string[] args)
        {

        }
#endif
    }
}