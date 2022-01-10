using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FilesCloner.Core
{
    public class ExtensionsManager
    {

        /// User configuration manager
        SettingsManager Loader = new SettingsManager();
        /// User configuration manager
       
        public ExtensionsManager()
        {
            Loader.SettingsInitializer();
 
        }

        public void addExt(string EXT)
        {
            try
            {
                if (ListOfEXT().Contains(EXT.ToUpper()) == false)
                {
                    Loader.WriteVal(MainKey: "AllExt", SecKey: "EXT", Val: EXT.ToUpper(), IsItSec: true, EnableChildDuplicate: true);
                }
                else
                {
                    MessageBox.Show("This Ext is in the list already");
                }
            }
            catch
            {
                MessageBox.Show("Error while tring to add ext");
            }
        }

        public void DelExt(string EXT)
        {
            try
            {
 
                if (ListOfEXT().Contains(EXT.ToUpper()))
                {
                    //Loader.DelByVal(Val: EXT.ToUpper());
                    Loader.DelbyKeyandVal(MainKey: "AllExt", SecKey: "EXT", Val: EXT.ToUpper());
                }
                else
                {
                    MessageBox.Show("This Ext is not in the list");
                }
            }
            catch
            {
                MessageBox.Show("Error while tring to delete ext");
            }
        }

        public List<string> ListOfEXT()
        {
          return  Loader.ReadListOfValues(MainKey: "AllExt", SecKey: "EXT", true);
        }

    }
}
