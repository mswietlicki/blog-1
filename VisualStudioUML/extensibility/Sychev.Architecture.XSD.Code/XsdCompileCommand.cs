using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Presentation;
using Microsoft.VisualStudio.ArchitectureTools.Extensibility.Uml;
using Microsoft.VisualStudio.Modeling.ExtensionEnablement;
using Microsoft.VisualStudio.Uml.Classes;

namespace Sychev.Architecture.XSD.Code
{
	/// <summary>
	/// Menu commands: http://msdn.microsoft.com/library/ee329481.aspx
	/// Stereotypes: http://msdn.microsoft.com/library/dd465143.aspx
	/// </summary>
	[Export(typeof(ICommandExtension))]
	[ClassDesignerExtension]
	public class XsdCompileCommand : ICommandExtension
	{
		// Command context: http://msdn.microsoft.com/library/ee329481.aspx
		[Import]
		public IDiagramContext DiagramContext { get; set; }

		public void QueryStatus(IMenuCommand command)
		{
			// Set command.Visible or command.Enabled to false
			// to disable the menu command.
			command.Visible = command.Enabled = true;
		}

		public string Text
		{
			get { return "Compile XSD Schema"; }
		}

		public void Execute(IMenuCommand command)
		{
			// A selection of starting points:
			IDiagram diagram = this.DiagramContext.CurrentDiagram;

			IModelStore modelStore = diagram.ModelStore;

			var classesInPackages = modelStore.AllInstances<IClass>()
				.GroupBy(i => i.Package.Name)
				.ToDictionary(i => i.Key, i => i.ToList());

			foreach (var package in classesInPackages)
			{
				var pathToWrite = this.DiagramContext.CurrentDiagram.FileName;
				pathToWrite = pathToWrite.Replace(this.DiagramContext.CurrentDiagram.Name, "").Replace(".classdiagram", "");
				new XSDWriter(pathToWrite).Write(package);
			}
		}
	}
}
