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
        private readonly string className;
        //private IEnumerable<string> relationships;

        public TypeRelationshipFinder()
        {

        }
        public TypeRelationshipFinder(string className, List<string> classNames, List<string> fileLines)
        {
            this.className = className;
            this.classNames = classNames;
            this.fileLines = fileLines;

            //relationships = new List<string>();
        }
        public TypeRelationshipFinder(string className, IEnumerable<string> classNames, List<string> fileLines)
        {
            this.className = className;
            this.classNames = classNames.ToList();
            this.fileLines = fileLines;
            //relationships = new List<string>();
        }
        public IEnumerable<string> FindRelationships()
        {
            IEnumerable<string> relationships = new List<string>();
            foreach (var line in fileLines)
            {
                foreach(var name in classNames)
                {
                    if (line.Contains(name))
                    {
                        string relationshipString = className + " uses " + name + ".";
                        relationships = relationships.Append(relationshipString);
                        Console.WriteLine(relationshipString);
                    }
                }
            }
            return relationships;
        }
        public IEnumerable<string> GetRelationships()
        {
            //List<string> relationshipList = FindRelationships().Distinct().ToList();
            //return relationshipList;
            return FindRelationships().Distinct();
        }
    }
}