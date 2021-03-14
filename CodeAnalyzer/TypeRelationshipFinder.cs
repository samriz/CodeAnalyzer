using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalyzer
{
    public class TypeRelationshipFinder
    {
        private readonly List<string> classNames;
        private readonly List<string> fileLines;
        //private IEnumerable<string> relationships;

        public TypeRelationshipFinder()
        {

        }
        public TypeRelationshipFinder(List<string> classNames, List<string> fileLines)
        {
            this.classNames = classNames;
            this.fileLines = fileLines;
            //relationships = new List<string>();
        }
        public TypeRelationshipFinder(IEnumerable<string> classNames, List<string> fileLines)
        {
            this.classNames = classNames.ToList();
            this.fileLines = fileLines;
            //relationships = new List<string>();
        }
        public IEnumerable<string> FindRelationships()
        {
            IEnumerable<string> relationships = new List<string>();
            foreach (var line in fileLines)
            {
                foreach(var className in classNames)
                {
                    if (line.Contains(className))
                    {
                        string relationshipString = "This file and/or class uses " + className + ".";
                        relationships = relationships.Append(relationshipString);
                        Console.WriteLine("This file and/or class uses {0}.", className);
                    }
                }
            }
            return relationships;
        }
        public List<string> GetRelationships()
        {            
            List<string> relationshipList = FindRelationships().Distinct().ToList();
            return relationshipList;
        }
    }
}