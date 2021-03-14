using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalyzer
{
    public class TypeRelationshipNode
    {
        string className;
        string explanation;
        public TypeRelationshipNode()
        {

        }
        public TypeRelationshipNode(string className, string explanation)
        {
            this.className = className;
            this.explanation = explanation;
        }
        public void SetClassName(string className) => this.className = className;
        public void SetExplanation(string explanation) => this.explanation = explanation;
        public string GetClassName() => className;
        public string GetExplanation() => explanation;
    }
}