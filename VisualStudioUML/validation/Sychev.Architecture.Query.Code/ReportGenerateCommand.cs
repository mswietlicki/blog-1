using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Presentation;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.Modeling.ExtensionEnablement;

namespace Sychev.Architecture.Query.Code
{
	[Export(typeof(ICommandExtension))]
	[ComponentDesignerExtension]
	public class ReportGenerateCommand : ICommandExtension
	{
		[Import]
		public IDiagramContext DiagramContext { get; set; }

		public void QueryStatus(IMenuCommand command)
		{
			command.Visible = command.Enabled = true;
		}

		public string Text
		{
			get { return "Generate Report"; }
		}

		public void Execute(IMenuCommand command)
		{
			// A selection of starting points:
			IDiagram diagram = this.DiagramContext.CurrentDiagram;

			IModelStore modelStore = diagram.ModelStore;

			var components = modelStore.AllInstances<Microsoft.VisualStudio.Uml.Components.IComponent>()
				.ToList();

			//var componentsNotI = components.Select(i => i as Microsoft.VisualStudio.Uml.Components.Component).ToList();

			//var serializer = new XmlSerializer(typeof(List<Microsoft.VisualStudio.Uml.Components.Component>));
			//using (var str=new StreamWriter(@"c:\temp\1.txt"))
			//{
			//	serializer.Serialize(str, componentsNotI);
			//	str.Flush();
			//}
			
			
			var pathToWrite = GetPathToWrite();

			ReportWriter.WriteReport(pathToWrite, components);	
		}

		private string GetPathToWrite()
		{
			var pathToWrite = this.DiagramContext.CurrentDiagram.FileName;
			pathToWrite = pathToWrite.Replace(".componentdiagram", "");
			if (!Directory.Exists(pathToWrite))
			{
				Directory.CreateDirectory(pathToWrite);
			}

			pathToWrite = Path.Combine(pathToWrite, "log.txt");
			return pathToWrite;
		}
	}
}
