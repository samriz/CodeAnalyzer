/////////////////////////////////////////////////////////////////////
// AnalysisDisplayer.cs - Display function/package statistics      //
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
 *  AnalysisDisplayer class contains functions to display information
 *  about classes and their functions to the console or to an xml file.
 *  The functions that deal with xml files will create an xml document
 *  and print the information to that file.
 */
/* Required Files:
 *   FunctionNode.cs, FileExtractor.cs, FunctionTracker.cs, TypeRelationshipFinder.cs
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
using System.Xml;

namespace CodeAnalyzer
{
    public class AnalysisDisplayer
    {
        private readonly string XML_Name;
        List<FunctionNode> functionNodes;
        FileExtractor FE;
        FunctionTracker FT;
        TypeRelationshipFinder TRF;
        
        //default constructor
        public AnalysisDisplayer()
        {
        }
        //parameterized constructor that specifies the name of the xml file
        public AnalysisDisplayer(string fileName)
        {
            this.XML_Name = fileName + "_analysis.xml";
        }
        public AnalysisDisplayer(string fileName, List<FunctionNode> functionNodes) : this(fileName)
        {
            this.functionNodes = functionNodes;
        }
        public AnalysisDisplayer(FileExtractor FE, FunctionTracker FT, TypeRelationshipFinder TRF) : this(FE.GetFile())
        {
            this.FE = FE;
            this.FT = FT;
            this.TRF = TRF;
            functionNodes = FT.GetFunctionNodes();
        }     
        public string GetXMLName()
        {
            return XML_Name;
        }

        //iterate through each element in functionNodes and print its contents to the console
        public void DisplayAnalysisToStandardOutput()
        {
            foreach (var node in functionNodes)
            {
                Console.WriteLine("Class: {0}", node.GetClassName());
                Console.WriteLine("Function name: {0}", node.GetFunctionName());
                Console.WriteLine("Function complexity: {0}", node.GetNumberOfScopes());
                Console.WriteLine("Number of lines: {0}\n", node.GetNumberOfLines());
            }
        }

        //call CreateXMLDocument as long as functionNodes isn't empty
        public void DisplayAnalysisToXML()
        { 
            if(functionNodes.Count < 1)
            {
                return;
            }
            else
            {
                CreateXMLDocument();     
            }
        }
        public XmlDocument GetAnalysisInXML()
        {
            if (functionNodes.Count < 1)
            {
                return null;
            }
            else
            {
                return ReturnXMLDocument();
            }
        }
        /*iterate through the type relationships compiled by the TypeRelationshipFinder
        class and print its contents to the console*/
        public void DisplayRelationshipsToConsole()
        {
            Console.WriteLine("Type relationships for {0}:", FT.GetClassName());
            foreach (var relationship in TRF.GetRelationships())
            {
                Console.WriteLine(" {0}", relationship);
            }
            Console.WriteLine("");
        }

        //overloaded method that takes in an IEnumerable collection
        public void DisplayRelationshipsToConsole(IEnumerable<string> relationships)
        {
            Console.WriteLine("Type relationships for {0}:", FT.GetClassName());
            foreach (var relationship in relationships)
            {
                Console.WriteLine(" {0}", relationship);
            }
            Console.WriteLine("");
        }

        //create xml document and write type relationships in it
        public void DisplayRelationshipsToXML()
        {
            XmlDocument analysisXML = new XmlDocument();
            XmlElement rootElement = analysisXML.CreateElement("Class");
            XmlNode rootNode = rootElement;

            XmlElement classNameElement = analysisXML.CreateElement("ClassName");
            classNameElement.InnerText = FT.GetClassName();
            XmlNode classNameNode = classNameElement;

            XmlElement typeRelationshipsElement = analysisXML.CreateElement("TypeRelationships");
            XmlNode typeRelationshipsNode = typeRelationshipsElement;

            rootNode.AppendChild(classNameNode);

            foreach (var relationship in TRF.GetRelationships())
            {
                XmlElement relationshipElement = analysisXML.CreateElement("Relationship");
                relationshipElement.InnerText = relationship;
                XmlNode relationshipNode = relationshipElement;

                typeRelationshipsNode.AppendChild(relationshipNode);
            }
            rootNode.AppendChild(typeRelationshipsNode);
            analysisXML.AppendChild(rootNode);
            analysisXML.Save(XML_Name);
        }

        //create xml document and write contents of functionNodes to it
        private void CreateXMLDocument()
        {
            XmlDocument analysisXML = new XmlDocument();
            XmlElement rootElement = analysisXML.CreateElement("Class");
            XmlNode rootNode = rootElement;

            XmlElement classNameElement = analysisXML.CreateElement("ClassName");
            classNameElement.InnerText = functionNodes[0].GetClassName();
            XmlNode classNameNode = classNameElement;
            rootNode.AppendChild(classNameNode);         

            foreach (var node in functionNodes)
            {
                XmlElement functionElement = analysisXML.CreateElement("Function");
                XmlNode functionNode = functionElement;

                XmlElement functionNameElement = analysisXML.CreateElement("FunctionName");
                functionNameElement.InnerText = node.GetFunctionName();
                XmlNode functionNameNode = functionNameElement;

                XmlElement scopeElement = analysisXML.CreateElement("NumberOfScopes");
                scopeElement.InnerText = node.GetNumberOfScopes().ToString();
                XmlNode scopeNode = scopeElement;

                XmlElement linesElement = analysisXML.CreateElement("NumberOfLines");
                linesElement.InnerText = node.GetNumberOfLines().ToString();
                XmlNode linesNode = linesElement;

                functionNode.AppendChild(functionNameNode);
                functionNode.AppendChild(scopeNode);
                functionNode.AppendChild(linesNode);
                rootNode.AppendChild(functionNode);
            }
            analysisXML.AppendChild(rootNode);
            analysisXML.Save(XML_Name);
        }
        private XmlDocument ReturnXMLDocument()
        {
            XmlDocument analysisXML = new XmlDocument();
            XmlElement rootElement = analysisXML.CreateElement("Class");
            XmlNode rootNode = rootElement;

            XmlElement classNameElement = analysisXML.CreateElement("ClassName");
            classNameElement.InnerText = functionNodes[0].GetClassName();
            XmlNode classNameNode = classNameElement;
            rootNode.AppendChild(classNameNode);

            foreach (var node in functionNodes)
            {
                XmlElement functionElement = analysisXML.CreateElement("Function");
                XmlNode functionNode = functionElement;

                XmlElement functionNameElement = analysisXML.CreateElement("FunctionName");
                functionNameElement.InnerText = node.GetFunctionName();
                XmlNode functionNameNode = functionNameElement;

                XmlElement scopeElement = analysisXML.CreateElement("NumberOfScopes");
                scopeElement.InnerText = node.GetNumberOfScopes().ToString();
                XmlNode scopeNode = scopeElement;

                XmlElement linesElement = analysisXML.CreateElement("NumberOfLines");
                linesElement.InnerText = node.GetNumberOfLines().ToString();
                XmlNode linesNode = linesElement;

                functionNode.AppendChild(functionNameNode);
                functionNode.AppendChild(scopeNode);
                functionNode.AppendChild(linesNode);
                rootNode.AppendChild(functionNode);
            }
            analysisXML.AppendChild(rootNode);
            return analysisXML;
        }

        // ---------------- test stub --------------------
#if (test_analysisdisplayer)
        static void Main(string[] args)
        {
            DirectorySearcher DS = new DirectorySearcher(@"..\..\..\CodeAnalyzer");
            FileExtractor FE;
            FunctionTracker FT;
            TypeRelationshipFinder TRF;
            ClassNameFinder CNF;
            AnalysisDisplayer AD;
            foreach (string file in DS.GetFilesWithFullPath())
            {
                FE = new FileExtractor(file);
                FT = new FunctionTracker(FE.GetExtractedLines());
                CNF = new ClassNameFinder(FT.GetFunctionNodes());
                TRF = new TypeRelationshipFinder(FT.GetClassName(), CNF.GetAllClassNames(), FE.GetExtractedLines());
                AD = new AnalysisDisplayer(FE, FT, TRF);
                AD.DisplayAnalysisToStandardOutput();
            }
            Console.ReadKey();
        }
#endif
    }
}