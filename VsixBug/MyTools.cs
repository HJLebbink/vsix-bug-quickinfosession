using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.Diagnostics;
using System.Globalization;

namespace VsixBug
{
    public static class MyTools
    {
        public static IVsOutputWindowPane GetOutputPane()
        {
            IVsOutputWindow outputWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;
            if (outputWindow == null)
            {
                return null;
            }
            else
            {
                Guid paneGuid = new Guid("ec494134-84d2-44b4-a750-8a4a674aa12f");
                outputWindow.CreatePane(paneGuid, "VsixBug", 1, 0);
                outputWindow.GetPane(paneGuid, out var pane);
                return pane;
            }
        }

        /// <summary>Output message to the Output window</summary>
        public static void Output_INFO(string msg)
        {
            IVsOutputWindowPane outputPane = GetOutputPane();
            string msg2 = string.Format(CultureInfo.CurrentCulture, "{0}", msg.Trim() + Environment.NewLine);
            if (outputPane == null)
            {
                Debug.Write(msg2);
            }
            else
            {
                outputPane.OutputString(msg2);
                outputPane.Activate();
            }
        }
    }
}
