using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeAnalyzer
{
    public class TypeRelationshipFinder
    {
        List<string> classNames;
        FileExtractor FE;
        //List<string> fileLines;
        public TypeRelationshipFinder(List<string> classNames, FileExtractor FE)
        {
            this.classNames = classNames;
            this.FE = FE;
        }
        public TypeRelationshipFinder(IEnumerable<string> classNames, FileExtractor FE)
        {
            this.classNames = classNames.ToList();
            this.FE = FE;
        }
        public void FindRelationships()
        {
            foreach(var line in FE.GetExtractedLines())
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
