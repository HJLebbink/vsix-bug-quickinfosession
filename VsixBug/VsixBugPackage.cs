using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.VisualStudio.Shell;

namespace VsixBug
{
    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    //[InstalledProductRegistration("#110", "#112", "0.0.9", IconResourceID = 400)] // Info on this package for Help/About
    [Guid(PackageGuidString)]
    [SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
    public sealed class VsixBugPackage : AsyncPackage
    {
        public const string PackageGuidString = "ec494134-84d2-44b4-a750-8a4a674aa12f";
        internal const string MyContentType = "xyzzy!";

        public VsixBugPackage()
        {
            Debug.WriteLine(string.Format(CultureInfo.CurrentCulture, "=========================================\nINFO: VsixBugPackage: Entering constructor"));

            StringBuilder sb = new StringBuilder();
            sb.Append("INFO: Loaded VsixBug version " + typeof(VsixBugPackage).Assembly.GetName().Version + "\n");
            sb.Append("INFO: Example code to pinpoint a bug described at https://github.com/HJLebbink/vsix-bug-quickinfosession \n");
            sb.Append("----------------------------------");
            MyTools.Output_INFO(sb.ToString());
        }

        protected override async System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await base.InitializeAsync(cancellationToken, progress);
        }
   }
}
