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
		private Log refactoredTables = null;

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

			this.refactoredTables = new Log()
			{
				Label = "Unreadable tables refactored into readable tables.",
				Filename = "refactored-tables-log.txt",
				AnnouncementStyle = ConsoleWriteStyle.Default
			};
			this.RegisterLog(this.refactoredTables);

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

			List<Table> tablePerRow = null;
			List<List<string>> skippedCellsPerRow = null;
			(tablePerRow, skippedCellsPerRow) = firstTable.SliceHorizontally(new List<string>{ "Target", "Support" }, 2);

			for (int tableIndex = 0; tableIndex < tablePerRow.Count; ++tableIndex)
			{
				string heading = $"{Environment.NewLine}## {skippedCellsPerRow[tableIndex][1]} ({skippedCellsPerRow[tableIndex][0]})";
				this.refactoredTables.Add(heading);
				this.refactoredTables.Add(tablePerRow[tableIndex].RenderAsMarkdown());
			}
		}
	}
}