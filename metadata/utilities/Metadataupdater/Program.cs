using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace Metadataupdater
{
    class Program
    {
        static void Main(string[] args)
        {

            SortAllDocs( args );  
        }

        private static void SortAllDocs(string[] args)
        {
              if (args.Length == 0)
            {
                throw new ArgumentException("Please pass a filename as an argument.");

            }
            DirectoryInfo dir = new DirectoryInfo(args[0]);
            XmlDocument commonDoc = null;
            foreach (FileInfo file in dir.GetFiles("*.xml"))
            {
                SortDoc(file);
            }
        }

        private static void SortDoc(FileInfo arg)
        {
          

            XmlDocument doc = new XmlDocument();
            String fileName = arg.FullName;
            doc.Load( fileName );
            arg.MoveTo( fileName + ".bak");

            SortedList<String, XmlElement> elements = new SortedList<String, XmlElement>();
            foreach (XmlNode child in doc.DocumentElement.ChildNodes)
            {
                XmlElement namedItem = child as XmlElement;
                if (namedItem != null)
                {
                    string name = namedItem.GetAttribute("name");
                    if (elements.ContainsKey(name))
                    {
                        name = name + "_1";
                    }
                    elements.Add(name, namedItem);
                }
            }

            XmlDocument sortedCopy = new XmlDocument();
            string version = doc.DocumentElement.GetAttribute("version");
            string package = doc.DocumentElement.GetAttribute("package");
            sortedCopy.AppendChild(sortedCopy.CreateComment(" ======================================== "));
            sortedCopy.AppendChild(sortedCopy.CreateComment("    SIFWorks ADK Object Definition File   "));
            sortedCopy.AppendChild(sortedCopy.CreateComment("     Copyright ©2011 Pearson Education, Inc., or associates.     "));
            sortedCopy.AppendChild(sortedCopy.CreateComment("     All Rights Reserved                  "));
            sortedCopy.AppendChild(sortedCopy.CreateComment(" ======================================== "));

            String tmp = "    SIF " + version + " " + package + " objects";
            sortedCopy.AppendChild(sortedCopy.CreateComment(tmp + new string( ' ', 42-tmp.Length) ));
            sortedCopy.AppendChild(sortedCopy.CreateComment(" ======================================== "));
            sortedCopy.AppendChild( sortedCopy.CreateElement( "adk" ) );            
            sortedCopy.DocumentElement.SetAttribute("package", package);
            sortedCopy.DocumentElement.SetAttribute("version", version);
            sortedCopy.DocumentElement.SetAttribute("namespace", doc.DocumentElement.GetAttribute("namespace"));

            foreach (KeyValuePair<string, XmlElement> element in elements)
            {
                XmlNode child = sortedCopy.ImportNode(element.Value, true);
                sortedCopy.DocumentElement.AppendChild( sortedCopy.CreateComment( element.Key + " " + element.Value.Name ));
                sortedCopy.DocumentElement.AppendChild( child );
            }

            sortedCopy.Save( fileName );


        }

        


        private static void addRepeatableContainers(String[] args)
        {
            if (args.Length == 0)
            {
                throw new ArgumentException("Please pass the datadef directory as an argument.");

            }

            DirectoryInfo dir = new DirectoryInfo(args[0]);
            XmlDocument commonDoc = null;
            foreach (FileInfo file in dir.GetFiles("*.xml"))
            {
                if (file.Name == "common.xml")
                {
                    commonDoc = new XmlDocument();
                    commonDoc.Load(file.FullName);
                    break;
                }
            }


            foreach (FileInfo file in dir.GetFiles("*.xml"))
            {
                if (file.Name != "common.xml")
                {
                    UpdateLegacyMetadata(file, commonDoc["adk"]);
                }
            }

            File.Delete(dir.FullName + "\\common.xml");

            commonDoc.Save(dir.FullName + "\\common.xml");

        }

       

        private static void UpdateLegacyMetadata(FileInfo legacyFile, XmlElement commonDocRoot )
        {
            XmlDocument metadata = new XmlDocument();
            metadata.Load(legacyFile.FullName);
            XmlElement root = metadata["adk"];

            List<XmlElement> objectElements = new List<XmlElement>();

            foreach (XmlNode node in root.ChildNodes)
            {
                if(node.Name == "object")
                {
                    objectElements.Add( (XmlElement)node );
                }
            }
            foreach( XmlElement objectNode in objectElements )
            {
                List<XmlElement> elements = new List<XmlElement>();
                foreach(XmlElement element in objectNode.GetElementsByTagName( "element" ))
                {
                    elements.Add(element);
                }
                foreach (XmlElement element in elements)
                {
                    string objectName = element.GetAttribute("name");
                    string flags = element.GetAttribute("flags");
                    if (flags.ToUpper().Equals("OR"))
                    {
                        XmlElement desc = element["desc"];
                        string listName = objectName + "List";
                        if (containsObject(commonDocRoot, objectName))
                        {
                            if (!containsObject(commonDocRoot, listName))
                            {
                                AddListElement(listName, objectName, desc == null ? null : desc.InnerText, commonDocRoot);
                            }
                        }
                        else
                        {
                            if (!containsObject(root, listName))
                            {
                                AddListElement(listName, objectName, desc == null ? null : desc.InnerText, root);
                            }
                        }
                        element.SetAttribute("flags", "O");
                        element.SetAttribute("name", listName);
                        element.SetAttribute("collapsed", "true");

                    }
                }
                
            }

            legacyFile.Delete();
            metadata.Save(legacyFile.FullName );


        }

        private static bool containsObject(XmlElement root, string objectName)
        {
            foreach( XmlElement element in root.GetElementsByTagName( "object" ) )
            {
                if( element.GetAttribute("name") == objectName )
                {
                    return true;
                }
            }
            return false;
        }

        private static void AddListElement(String listName, String subObjectName, string description, XmlElement root)
        {

            XmlComment comment = root.OwnerDocument.CreateComment(listName);
            root.AppendChild(comment);

            XmlElement newObject = root.OwnerDocument.CreateElement("object");
            newObject.SetAttribute("name", listName);

            XmlElement element = root.OwnerDocument.CreateElement("element");
            element.SetAttribute("name", subObjectName);
            element.SetAttribute("flags", "O");
            if (description != null && description.Length > 0)
            {
                XmlElement desc = root.OwnerDocument.CreateElement("desc");
                desc.InnerText = description;
                element.AppendChild(desc);
            }

            newObject.AppendChild(element);
            root.AppendChild(newObject);

        }
    }
}
