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
        //private static readonly string XML_Name;
        private string XML_Name;
        private FunctionTracker FT;
        //private ClassNameFinder CNF;
        private TypeRelationshipFinder TRF;
        //private List<FunctionNode> functionScopes;

        static AnalysisDisplayer()
        {
            //XML_Name = Directory.GetCurrentDirectory() + @"\Analysis.xml";
        }
        public AnalysisDisplayer(string file, FunctionTracker FT)
        {
            this.XML_Name = file + "_analysis.xml";
            this.FT = FT;
            this.TRF = TRF;
        }
        public AnalysisDisplayer(string file, FunctionTracker FT, TypeRelationshipFinder TRF)
        {
            this.XML_Name = file + "_analysis.xml";
            this.FT = FT;
            this.TRF = TRF;
        }
        /*public AnalysisDisplayer(string file, List<FunctionNode> functionScopes) 
        {
            this.XML_Name = file + "_analysis.xml";
            this.functionScopes = functionScopes;
        }*/
        public string GetXMLName()
        {
            return XML_Name;
        }
        public void DisplayAnalysisToStandardOutput()
        {
            //foreach (FunctionNode functionScope in functionScopes)
            foreach (var node in FT.GetFunctionNodes())
            {
                Console.WriteLine("Class: {0}", node.GetClassName());
                Console.WriteLine("Function name: {0}", node.GetFunctionName());
                Console.WriteLine("Function complexity: {0}", node.GetNumberOfScopes());
                Console.WriteLine("Number of lines: {0}\n", node.GetNumberOfLines());
            }
            //TRF.FindRelationships();
        }
        public void DisplayAnalysisToXML()
        { 
            if(FT.GetFunctionNodes().Count < 1)
            //if(functionScopes.Count < 1)
            {
                return;
            }
            else
            {
                CreateXMLDocument();     
            }
        }
        public void CreateXMLDocument()
        {
            XmlDocument analysisXML = new XmlDocument();
            XmlElement rootElement = analysisXML.CreateElement("Class");
            XmlNode rootNode = rootElement;

            XmlElement classNameElement = analysisXML.CreateElement("ClassName");
            classNameElement.InnerText = FT.GetFunctionNodes()[0].GetClassName();
            XmlNode classNameNode = classNameElement;

            XmlElement functionElement;
            XmlNode functionNode;
            XmlElement functionNameElement;
            XmlNode functionNameNode;
            XmlElement scopeElement;
            XmlNode scopeNode;
            XmlElement linesElement;
            XmlNode linesNode;

            rootNode.AppendChild(classNameNode);

            foreach (FunctionNode node in FT.GetFunctionNodes())
            {
                functionElement = analysisXML.CreateElement("Function");
                functionNode = functionElement;

                functionNameElement = analysisXML.CreateElement("FunctionName");
                functionNameElement.InnerText = node.GetFunctionName();
                functionNameNode = functionNameElement;

                scopeElement = analysisXML.CreateElement("NumberOfScopes");
                scopeElement.InnerText = node.GetNumberOfScopes().ToString();
                scopeNode = scopeElement;

                linesElement = analysisXML.CreateElement("NumberOfLines");
                linesElement.InnerText = node.GetNumberOfLines().ToString();
                linesNode = linesElement;

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

        public void DisplayRelationshipsToConsole()
        {
            

        }
        public void DisplayRelationshipsToXML()
        {


        }
        /*private int CountStatements(List<string> lines)
        {
            int statementCount = 0;
            foreach (string line in lines)
            {
                if (line.EndsWith(";")) ++statementCount;
            }
            return statementCount;
        }*/
#if (test_analysisdisplayer)
        static void Main(string[] args)
        {

        }
#endif
    }
}