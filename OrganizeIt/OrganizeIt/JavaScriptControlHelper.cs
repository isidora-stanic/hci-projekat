using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Runtime.InteropServices;
using OrganizeIt;
using System.Windows;

namespace HelpSistem
{
    [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
    [ComVisible(true)]
    public class JavaScriptControlHelper
    {
        Window prozor;

        public JavaScriptControlHelper(Window w)
        {
            prozor = w;
        }

        public void RunFromJavascript(string param)
        {
            //prozor.doThings(param);
        }
    }
}