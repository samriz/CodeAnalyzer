/////////////////////////////////////////////////////////////////////////////
// FunctionNode.cs - class for building nodes that contain information     //
// about functions.                                                        //
// in the package.                                                         //
// ver 1.0                                                                 //
// Language:    C#, 2020, .Net Framework 4.7.2                             //
// Platform:    MSI GS65 Stealth, Win10                                    //
// Application: CSE681, Project #2, Winter 2021                            //
// Author:      Sameer Rizvi, Syracuse University                          //
//              srizvi@syr.edu                                             //
/////////////////////////////////////////////////////////////////////////////
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
using System.Threading.Tasks;

namespace CodeAnalyzer
{
    public class FunctionNode
    {
        private string functionName;
        private int numberOfScopes;
        private int numberOfLines;
        private string className;
        private string namespaceName;

        public FunctionNode() 
        {
            this.functionName = "";
            this.className = "";
            this.namespaceName = "";
            this.numberOfScopes = 0;
            this.numberOfLines = 0;
        }
        public FunctionNode(string functionName)
        {
            this.functionName = functionName;
            this.className = "";
            this.numberOfScopes = 0;
            this.numberOfLines = 0;
        }
        public string GetClassName()
        {
            return className;
        }
        public string GetFunctionName()
        {
            return functionName;
        }
        public string GetNamespaceName()
        {
            return namespaceName;
        }
        public int GetNumberOfScopes()
        {
            return numberOfScopes;
        }
        public int GetNumberOfLines()
        {
            return numberOfLines;
        }
        public void SetNamespaceName(string namespaceName) => this.namespaceName = namespaceName;
        public void SetClassName(string className) => this.className = className;
        public void SetFunctionName(string functionName) => this.functionName = functionName;
        public void SetNumberOfScopes(int numberOfScopes) => this.numberOfScopes = numberOfScopes;
        public void SetNumberOfLines(int numberOfLines) => this.numberOfLines = numberOfLines;

// ---------------- test stub --------------------
#if (test_functionnode)
        static void Main(string[] args)
        {

        }
#endif
    }
}