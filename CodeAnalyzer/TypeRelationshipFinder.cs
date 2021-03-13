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

        public TypeRelationshipFinder()
        {

        }
        public TypeRelationshipFinder(List<string> classNames, List<string> fileLines)
        {
            this.classNames = classNames;
            this.fileLines = fileLines;
        }
        public TypeRelationshipFinder(IEnumerable<string> classNames, List<string> fileLines)
        {
            this.classNames = classNames.ToList();
            this.fileLines = fileLines;
        }
        public void FindRelationships()
        {
            foreach(var line in fileLines)
            {
                foreach(var className in classNames)
                {
                    if (line.Contains(className))
                    {
                        Console.WriteLine("This file and/or class uses {0}.", className);
                    }
                }
            }
        }
    }
}