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

// TODO

// Normalize topic
// Normalize BOM and encoding.
// Replace double space with single, recursively (exception: ignore *leading* whitespace within ```; OTHER CASES?).
// Replace double carriage-return with single, recursively.
// Replace yml delimters with plain
// Read yml fields into dictionary, sort some (like req), write out in a rational order (e.g. title, desc, date together).
// [!Note] => [!NOTE], so they're easier for the writer to spot.
// Etc.

// Normalize table
// Replace double hypthen with single.
// Cellpadding: single space after |, and if there are contents a single space after that (before the next |).
// Etc.

// To transform table. Table class with collection of col headers and collection of rows (each a collection of cells; one for each col).
// Load table into class. Read a list of col names that are repeated, and so ignore second and subsequent ones.
// Refactor each row into one new class (and a heading).
// Write out each class, normalized.

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
			var projectDirectoryInfo = new DirectoryInfo(@"C:\Users\stwhi\source\repos\win32-pr\desktop-src\direct3ddxgi");
			Directory.SetCurrentDirectory(projectDirectoryInfo.FullName);

			Editor editor = EditorBase.GetEditorForTopicFileName(projectDirectoryInfo, "hardware-support-for-direct3d-12-1-formats.md");

			//FileInfo[] mdfiles = dir.GetFiles("*.md", SearchOption.AllDirectories);
			//FileInfo[] tocFiles = dir.GetFiles("toc.yml", SearchOption.AllDirectories);

			Table firstTable = editor.CutTable();
			if (firstTable != null)
			{
				firstTable.RemoveRowNumberOneBased(firstTable.RowCount);
				firstTable.RemoveRedundantColumns(@"\#", @"Format ( DXGI\_FORMAT\_\* )");
			}
		}
	}
}