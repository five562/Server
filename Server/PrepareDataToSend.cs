using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;

namespace Server
{
    class PrepareDataToSend
    {
        /*
        public void GetFolderTree()
        {
            XmlDocument doc = new XmlDocument();
            List<string> projects = new List<string>(0);
            string directory = @"C:\TestCaseEditor\";
            if (Directory.Exists(directory))
            {
                XmlElement root = doc.CreateElement("Tree");
                doc.AppendChild(root);
                //Get all projects name
                XmlElement projectElement = doc.CreateElement("Projects");
                string[] projectDirectories = Directory.GetDirectories(directory);
                foreach(string direc in projectDirectories)
                {
                    projectElement.InnerText += new DirectoryInfo(direc).Name + "; ";
                    projects.Add(new DirectoryInfo(direc).Name);
                }
                root.AppendChild(projectElement);

                
                foreach(string project in projects)
                {
                    //Get all verions name of each project
                    XmlElement versionElement = doc.CreateElement(project);
                    string[] versionDirectories = Directory.GetDirectories(directory + project + @"\");
                    List<string> versions = new List<string>();
                    foreach(string version in versionDirectories)
                    {
                        versionElement.InnerText += new DirectoryInfo(version).Name + "; ";
                        versions.Add(new DirectoryInfo(version).Name);
                    }
                    root.AppendChild(versionElement);
                }




                doc.Save(@"C:\Users\liyi5\Desktop\folderTree.xml");
            }
        }
        */
    }
}
