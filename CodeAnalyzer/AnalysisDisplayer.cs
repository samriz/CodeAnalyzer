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
        public AnalysisDisplayer()
        {
            FE = null;
            FT = null;
        }
        public AnalysisDisplayer(FileExtractor FE, FunctionTracker FT, TypeRelationshipFinder TRF) : this(FE.GetFile())
        {
            this.FE = FE;
            this.FT = FT;
            this.TRF = TRF;
            functionNodes = FT.GetFunctionNodes();
        }
        public AnalysisDisplayer(string fileName, List<FunctionNode> functionNodes) : this(fileName)
        {
            this.functionNodes = functionNodes;
        }
        public AnalysisDisplayer(string fileName): this()
        {
            this.XML_Name = fileName + "_analysis.xml";
        }
        public string GetXMLName()
        {
            return XML_Name;
        }
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
        public void DisplayRelationshipsToConsole()
        {
            foreach (var relationship in TRF.GetRelationships())
            {
                Console.WriteLine(relationship);
            }
        }
        public void DisplayRelationshipsToConsole(IEnumerable<string> relationships)
        {
            foreach(var relationship in relationships)
            {
                Console.WriteLine(relationship);
            }
        }
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

        private void CreateXMLDocumentUsingXmlWriter()
        {
            /*XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.IndentChars = ("    ");
                settings.CloseOutput = true;
                settings.OmitXmlDeclaration = true;
                XmlDocument
                using (XmlWriter writer = XmlWriter.Create(XML_Name, settings))
                {
                    writer.WriteStartElement("CodeAnalyzer");
                    //foreach (var node in functionScopes)
                    foreach (FunctionNode node in FT.GetFunctionNodes())
                    {
                        writer.WriteStartElement("Analysis", "Class Name: " + node.GetClassName());
                        writer.WriteElementString("Analysis", "Function Name: " + node.GetFunctionName());
                        writer.WriteElementString("Analysis", "Number of Scopes: " + node.GetNumberOfScopes());
                        writer.WriteElementString("Analysis", "Number of Lines: " + node.GetNumberOfLines());
                    }
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Flush();
                }*/
        }
#if (test_analysisdisplayer)
        static void Main(string[] args)
        {

        }
#endif
    }
}