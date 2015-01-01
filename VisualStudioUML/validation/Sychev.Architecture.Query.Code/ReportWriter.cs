using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Microsoft.VisualStudio.Uml.Classes;
using Microsoft.VisualStudio.Uml.Components;

namespace Sychev.Architecture.Query.Code
{
	public class ReportWriter
	{
		public static void WriteReport(string pathToWrite, List<IComponent> components)
		{
			using (var streamWriter = new StreamWriter(pathToWrite))
			{
				foreach (var ipackage in components)
				{
					Debug.WriteLine("Package {0}", ipackage.Name);
					streamWriter.WriteLine("Package {0}", ipackage.Name);
					Debug.WriteLine("	Provided Interfaces:");
					streamWriter.WriteLine("	Provided Interfaces:");
					foreach (var iinterface in ipackage.Provided)
					{
						WriteInterfaces(streamWriter, iinterface);
					}
					Debug.WriteLine("	Required Interfaces:");
					streamWriter.WriteLine("	Required Interfaces:");
					foreach (var iinterface in ipackage.Required)
					{
						WriteInterfaces(streamWriter, iinterface);
					}
				}
				streamWriter.Flush();
			}
		}

		private static void WriteInterfaces(StreamWriter streamWriter, IInterface iinterface)
		{
			Debug.WriteLine("		{0}", iinterface.Name);
			streamWriter.WriteLine("		{0}", iinterface.Name);
			Debug.WriteLine("	Required Interfaces:");
			streamWriter.WriteLine("	Required Interfaces:");
			foreach (var ioperation in iinterface.OwnedOperations)
			{
				var returnTypeName = string.Empty;

				var strToWrite = string.Empty;
				foreach (var parameter in ioperation.OwnedParameters)
				{
					if (parameter.Type == null)
					{
						if (parameter.Direction == ParameterDirectionKind.Return)
						{
							returnTypeName = "notDefined";
						}
						else
						{
							strToWrite += string.Format("{0} {1}, ", "notDefined", "notDefined");
						}
					}
					else
					{
						if (parameter.Direction == ParameterDirectionKind.Return)
						{
							returnTypeName = parameter.Type.Name;
						}
						else
						{
							strToWrite += string.Format("{0} {1}, ", parameter.Type.Name, parameter.Name);
						}
					}	
				}
				if (string.IsNullOrEmpty(returnTypeName))
				{
					returnTypeName = "void";
				}

				strToWrite = strToWrite.TrimEnd(',', ' ');
				strToWrite = string.Format("				{1} {0} (", ioperation.Name, returnTypeName) + strToWrite + ")";
				Debug.WriteLine(strToWrite);
				streamWriter.WriteLine(strToWrite);
			}
		}
	}
}