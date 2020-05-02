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

			//this.uniqueKeyMap = this.LoadUniqueKeyMap("uniqueKeyMap.txt");
			//this.nonUniqueKeyMap = this.LoadNonUniqueKeyMap("nonUniqueKeyMap.txt");

			RefactorTables();
		}

		private void RefactorTables()
		{
			Directory.SetCurrentDirectory(@"C:\Users\stwhi\source\repos\win32-pr\desktop-src\direct3ddxgi");

			//FileInfo[] mdfiles = dir.GetFiles("*.md", SearchOption.AllDirectories);
			//FileInfo[] tocFiles = dir.GetFiles("toc.yml", SearchOption.AllDirectories);

			//var editor = new Editor(FileInfo fileInfo);

			OpenMarkdownFile("format-support-for-direct3d-feature-level-10-1-hardware.md", "MISSING format-support-for-direct3d-feature-level-10-1-hardware.md");
		}

		private static void OpenMarkdownFile(string fileName, string notFoundMessage = null)
		{
			FileInfo fileInfo = new FileInfo(fileName);
			if (!fileInfo.Exists)
			{
				ProgramBase.ConsoleWrite(notFoundMessage ?? "Could not find " + fileName, ConsoleWriteStyle.Error);
				throw new MDSDKException();
			}

			using (StreamReader streamReader = fileInfo.OpenText())
			{
				while (!streamReader.EndOfStream)
				{
					streamReader.ReadLine();
				}
			}
		}
	}
}