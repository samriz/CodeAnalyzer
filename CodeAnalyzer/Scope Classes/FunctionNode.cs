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
        /*private string scopeHead;
        private List<string> functionContents;
        private List<ScopeNode> children;
        private ScopeNode parent;
        public ScopeNode()
        {
            parent = null;
            this.scopeHead = "";
            functionContents = null;
            children = new List<ScopeNode>();
        }
        public ScopeNode(string scopeHead) : this()
        {
            this.scopeHead = scopeHead;
        }
        public ScopeNode(string scopeHead, List<string> functionContents)
        {
            this.scopeHead = scopeHead;
            this.functionContents = functionContents;
            children = new List<ScopeNode>();
        }
        public string GetScopeHead()
        {
            return scopeHead;
        }
        public void SetScopeHead(string scopeHead) => this.scopeHead = scopeHead;
        public List<ScopeNode> GetChildren()
        {
            return children;
        }
        public void AddChild(ScopeNode sn) => children.Add(sn);
        public ScopeNode GetParent()
        {
            return parent;
        }
        public void SetParent(ScopeNode parent) => this.parent = parent;
        public void PrintChildren()
        {
            foreach(ScopeNode child in children)
            {
                Console.WriteLine(child.GetScopeHead());
            }
        }*/
    }
}