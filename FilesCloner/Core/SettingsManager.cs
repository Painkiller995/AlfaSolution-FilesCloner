using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace FilesCloner.Core
{
    public class SettingsManager
    {
        private static string filename = "Settings.xml";
        private static string currentDirectory = Directory.GetCurrentDirectory();
        private static string purchaseOrderFilepath = Path.Combine(currentDirectory, filename);
        public SettingsManager() 
        {
        
        }
        public Boolean SettingsInitializer()
        {
            try 
            {
                if (!File.Exists(purchaseOrderFilepath)) {
                XElement Main = new XElement("SettingsManager");
                Main.Save(purchaseOrderFilepath);
                return true;
                }
                else
                {
                    return false;
                }
            } 
            catch
            {
                MessageBox.Show("Error while Creating Settings.xml.");
                return false;
            }
        }

        public Boolean SettingsChecker()
        {
            try
            {
                if (File.Exists(purchaseOrderFilepath))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                MessageBox.Show("Error while checking Settings.xml.");
                return false;
            }
        }

        public void WriteVal(string MainKey, string SecKey = null , string Val = null , bool IsItSec = false, bool EnableParentDuplicate = false , bool EnableChildDuplicate = false)
        {
            try
            {
            XElement doc = XElement.Load(purchaseOrderFilepath);
            var elementsToUpdate = doc.Descendants()
                                   .Where(o => o.Name == MainKey);
            if (elementsToUpdate.Count() > 0 && EnableParentDuplicate == false )
            {
                foreach (XElement element in elementsToUpdate)
                {
                    if (IsItSec)
                    {
                        if (element.Element(SecKey) != null && EnableChildDuplicate == false)
                        {
                            element.Element(SecKey).Value = Val;
                        }
                        else
                        {
                            XElement CreateChild = new XElement(SecKey, Val);
                            element.Add(CreateChild);
                        }
                    }
                    else
                    {
                        element.Value = Val;
                    }
                }
            }
            else
            {
                XElement CreateParent = new XElement(MainKey);
                if (IsItSec)
                {
                    XElement CreateChild = new XElement(SecKey, Val);
                    CreateParent.Add(CreateChild);
                    doc.Add(CreateParent);
                }
                else
                {
                    CreateParent.Value = Val;
                    doc.Add(CreateParent);
                }
            }
            doc.Save(purchaseOrderFilepath);
            }
            catch
            {
                MessageBox.Show("Error while writing data.");
            }
        }
        public string ReadVal(string MainKey, string SecKey = null, bool IsItSec = false)
        {
          
            try
            {
         
                XElement doc = XElement.Load(purchaseOrderFilepath);
                var elementsToUpdate = doc.Descendants()
                                       .Where(o => o.Name == MainKey);
                if (elementsToUpdate.Count() > 0)
                {
                 
                    if (IsItSec)
                    {
                        return elementsToUpdate.First().Element(SecKey).Value;
                    }
                    else 
                    { 
                        return elementsToUpdate.First().Value;
                    }
             
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                MessageBox.Show("Error while reading data.");
                return string.Empty;
            }
        }
        public List<string> ReadListOfValues(string MainKey, string SecKey = null, bool IsItSec = false)
        {
            List<string> x = new List<string>();
            try
            {
                XElement doc = XElement.Load(purchaseOrderFilepath);
                var elementsToUpdate = doc.Descendants()
                                       .Where(o => o.Name == MainKey);
                if (elementsToUpdate.Count() > 0)
                {
                    foreach(XElement element in elementsToUpdate)
                    {
                        if (IsItSec)
                        {
                            foreach(XElement el in element.Elements())
                            {
                             x.Add(el.Value);
                            }

                        }
                        else 
                        {
                        x.Add(element.Value);
                        }
                    }

                }
                return x;
            }
            catch
            {
                MessageBox.Show("Error while reading list of data.");
                return x;
            }
        }
   

        public void DelbyKeyandVal(string MainKey, string SecKey, string Val)
        {
            try
            {
                XElement doc = XElement.Load(purchaseOrderFilepath);
                doc.Descendants(MainKey).Elements(SecKey).Where(o => o.Value == Val).Remove();
                doc.Save(purchaseOrderFilepath);
            }
            catch
            {
                MessageBox.Show("Error while tring to delete val based on key");
            }
        }

    }
}
