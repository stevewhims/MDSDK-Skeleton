using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using MDSDK;
using MDSDKBase;

namespace MDSDK_Skeleton
{
}

namespace MDSDKDerived
{
    using MDSDK_Skeleton;
    using Microsoft.VisualBasic;
    using System.Diagnostics;

    /// <summary>
    /// See the xml docs for ProgramBase.
    /// </summary>
    internal class Program : ProgramBase
    {
        // Logs
        private Log exampleLog = null;

        // Data
        private DocSet win10Docs = null;
        //private Dictionary<string, string> uniqueKeyMap = null;
        //private Dictionary<string, List<string>> nonUniqueKeyMap = null;

        static int Main(string[] args)
        {
            return (new Program()).Run();
        }

        protected override void OnRun()
        {
            // Load a docset.
            this.win10Docs = DocSet.CreateDocSet(DocSetType.ConceptualAndReference, Platform.UWPWindows10, "Win10 docs");

            this.exampleLog = new Log()
            {
                Label = "Text log containing info too verbose for the console.",
                Filename = "Example_Log.txt",
                AnnouncementStyle = ConsoleWriteStyle.Default
            };
            this.RegisterLog(this.exampleLog);
            this.exampleLog.Add("Example message.");

            var processStartInfo = new ProcessStartInfo("cmd");
            processStartInfo.CreateNoWindow = true;
            processStartInfo.RedirectStandardInput = true;
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.UseShellExecute = false;
            processStartInfo.WorkingDirectory = $"{ProgramBase.MyContentReposFolderDirectoryInfo.FullName}\\{ProgramBase.ContentRepo}";

            using (var createBranchProcess = Process.Start(processStartInfo))
            {
                createBranchProcess.StandardInput.WriteLine("git pull -p");
                createBranchProcess.StandardInput.WriteLine("git checkout -b stwhi-master/refactor-mega-tables-3 origin/master");
                //createBranchProcess.StandardInput.WriteLine("git push --set-upstream origin stwhi-master/refactor-mega-tables-3");
                createBranchProcess.StandardInput.WriteLine("git status");
                createBranchProcess.StandardInput.Close();
                createBranchProcess.WaitForExit();
                ProgramBase.ConsoleWrite(createBranchProcess.StandardOutput.ReadToEnd(), ConsoleWriteStyle.OutputFromAnotherProcess);
                createBranchProcess.WaitForExit();
            }

            using (var pushProcess = Process.Start(processStartInfo))
            {
                pushProcess.StandardInput.WriteLine("git add .");
                pushProcess.StandardInput.WriteLine("git commit -a -m \"commit message\"");
                pushProcess.StandardInput.WriteLine("git push");
                pushProcess.StandardInput.WriteLine("git status");
                pushProcess.StandardInput.Close();
                pushProcess.WaitForExit();
                ProgramBase.ConsoleWrite(pushProcess.StandardOutput.ReadToEnd(), ConsoleWriteStyle.OutputFromAnotherProcess);
                pushProcess.WaitForExit();
            }

            using (var deleteBranchProcess = Process.Start(processStartInfo))
            {
                deleteBranchProcess.StandardInput.WriteLine("git checkout master");
                deleteBranchProcess.StandardInput.WriteLine("git branch -D stwhi-master/refactor-mega-tables-3");
                deleteBranchProcess.StandardInput.WriteLine("git pull -p");
                deleteBranchProcess.StandardInput.WriteLine("git status");
                deleteBranchProcess.StandardInput.Close();
                deleteBranchProcess.WaitForExit();
                ProgramBase.ConsoleWrite(deleteBranchProcess.StandardOutput.ReadToEnd(), ConsoleWriteStyle.OutputFromAnotherProcess);
                deleteBranchProcess.WaitForExit();
            }

            //this.uniqueKeyMap = this.LoadUniqueKeyMap("uniqueKeyMap.txt");
            //this.nonUniqueKeyMap = this.LoadNonUniqueKeyMap("nonUniqueKeyMap.txt");
        }
    }
}