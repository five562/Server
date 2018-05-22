using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
//using System.Text;


namespace Server
{
    class XmlParser
    {
        //This method will be called only once at the beginning of the system installation.
        public void CreateFolderTreeXml()
        {
            try
            {
                if (!File.Exists(@"C:\Users\liyi5\Desktop\folderTree.xml"))
                {
                    XmlDocument doc = new XmlDocument();
                    XmlElement rootElement = doc.CreateElement("FolderTree");
                    doc.AppendChild(rootElement);

                    XmlElement testCaseElement = doc.CreateElement("TestCase");
                    rootElement.AppendChild(testCaseElement);

                    XmlElement testResultElement = doc.CreateElement("TestResult");
                    rootElement.AppendChild(testResultElement);

                    doc.Save(@"C:\Users\liyi5\Desktop\folderTree.xml");
                }
                
            }catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            
        }

        public void EditFolderTreeXml(Dictionary<string,string> testCaseDictionaly)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(@"C:\Users\liyi5\Desktop\folderTree.xml");
            
            if(testCaseDictionaly["executedBy"] == "")
            {
                //This will add the new line for the new test case xml file.
                SaveFolderTreeXmlChange("TestCase", doc, testCaseDictionaly);
            }
            else
            {
                //This will add the new line for the new test result xml file.
                SaveFolderTreeXmlChange("TestResult", doc, testCaseDictionaly);
            }
        }

        public void SaveFolderTreeXmlChange(string str, XmlDocument doc, Dictionary<string,string> testCaseDictionaly)
        {
            XmlNode firstNode = doc.SelectSingleNode("/FolderTree/" + str);
            
            if (doc.SelectSingleNode("/FolderTree/" + str + "/" + testCaseDictionaly["project"]) != null)
            {
                if (doc.SelectSingleNode("/FolderTree/" + str + "/" + testCaseDictionaly["project"] + "/" + testCaseDictionaly["version"]) != null)
                {
                    if (doc.SelectSingleNode("/FolderTree/" + str + "/" + testCaseDictionaly["project"] + "/" + testCaseDictionaly["version"] + "/" + testCaseDictionaly["module"]) != null)
                    {
                        XmlNode moduleNode = doc.SelectSingleNode("/FolderTree/" + str + "/" + testCaseDictionaly["project"] + "/" + testCaseDictionaly["version"] + "/" + testCaseDictionaly["module"]);
                        string text = moduleNode.InnerText + testCaseDictionaly["testCaseId"] + ".xml ;";
                        moduleNode.InnerText = text;
                        doc.Save(@"C:\Users\liyi5\Desktop\folderTree.xml");
                    }
                    else
                    {
                        XmlNode versionNode = doc.SelectSingleNode("/FolderTree/" + str + "/" + testCaseDictionaly["project"] + "/" + testCaseDictionaly["version"]);
                        XmlElement moduleElement = doc.CreateElement(testCaseDictionaly["module"]);
                        moduleElement.InnerText = testCaseDictionaly["testCaseId"] + ".xml ;";
                        versionNode.AppendChild(moduleElement);
                        doc.Save(@"C:\Users\liyi5\Desktop\folderTree.xml");
                    }
                }
                else
                {
                    XmlNode projectNode = doc.SelectSingleNode("/FolderTree/" + str + "/" + testCaseDictionaly["project"]);
                    XmlElement versionElement = doc.CreateElement(testCaseDictionaly["version"]);
                    projectNode.AppendChild(versionElement);
                    XmlElement moduleElement = doc.CreateElement(testCaseDictionaly["module"]);
                    moduleElement.InnerText = testCaseDictionaly["testCaseId"] + ".xml ;";
                    versionElement.AppendChild(moduleElement);
                    doc.Save(@"C:\Users\liyi5\Desktop\folderTree.xml");
                }

            }
            else
            {
                XmlElement projectElement = doc.CreateElement(testCaseDictionaly["project"]);
                firstNode.AppendChild(projectElement);
                XmlElement versionElement = doc.CreateElement(testCaseDictionaly["version"]);
                projectElement.AppendChild(versionElement);
                XmlElement moduleElement = doc.CreateElement(testCaseDictionaly["module"]);
                moduleElement.InnerText = testCaseDictionaly["testCaseId"] + ".xml ;";
                versionElement.AppendChild(moduleElement);
                doc.Save(@"C:\Users\liyi5\Desktop\folderTree.xml");
            }
        }

        public XmlDocument GenerateXmlFile(string str)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(str);
            return doc;
        } 

        public Dictionary<string,string> LoadXmlFile(XmlDocument xmlDoc, string xmlNode)
        {
            Dictionary<string, string> file = new Dictionary<string, string>();
            try
            {
                XmlNode root;
                root = xmlDoc.SelectSingleNode(xmlNode);
                if (root.HasChildNodes)
                {
                    foreach (XmlNode n in root.ChildNodes)
                    {
                        file.Add(n.Name, n.InnerText);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            return file;
        }
 
        public void SaveTestCase(XmlDocument xmlDoc)
        {
            Dictionary<string, string> xmlData = LoadXmlFile(xmlDoc, "/TestCase");
            string path = @"C:\TestCaseEditor\" + xmlData["project"] + @"\" + xmlData["version"] + @"\" + xmlData["module"];
            if (!Directory.Exists(path))
            {
                System.IO.Directory.CreateDirectory(path + @"\TestCases");
                System.IO.Directory.CreateDirectory(path + @"\TestResults");
                if (xmlData["executedBy"] == "")
                {
                    xmlDoc.Save(path + @"\TestCases\" + xmlData["testCaseId"] + ".xml");
                }
                else
                {
                    xmlDoc.Save(path + @"\TestResults\" + xmlData["testCaseId"] + ".xml");
                }
            }
            else
            {
                if (xmlData["executedBy"] == "")
                {
                    xmlDoc.Save(path + @"\TestCases\" + xmlData["testCaseId"] + ".xml");
                }
                else
                {
                    xmlDoc.Save(path + @"\TestResults\" + xmlData["testCaseId"] + ".xml");
                }
            }
            
        }
        
    }
}
