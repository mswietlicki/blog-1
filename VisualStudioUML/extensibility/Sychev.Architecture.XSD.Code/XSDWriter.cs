using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.Uml.Classes;

namespace Sychev.Architecture.XSD.Code
{
	public sealed class XSDWriter
	{
		private const string XmlNameSpacePrepertyName = "Namespace";
		private const string XmlNameSpaceStereotypeName = "XSDschema";

		//private const string ComplexTypeStereotypeName = "XSDcomplexType";

		private const string ElementValueName = "XSDcomplexTypeValue";
		private const string ElementPropertyName = "XSDcomplexTypeName";
		private const string ElementStereotypeName = "XSDsequence";

		private readonly string _filePathTemplate;

		public XSDWriter(string filePathTemplate)
		{
			_filePathTemplate = filePathTemplate;
		}

		public void Write(KeyValuePair<string, List<IClass>> packageWithClasses)
		{
			var package = packageWithClasses.Value.First().Package;

			//IStereotype packageNamespaceStereotype = package.ApplicableStereotypes.First(st => st.Name == XmlNameSpaceStereotypeName);
			using (var streamWriter = new StreamWriter(Path.Combine(_filePathTemplate, package.Name + ".xsd")))
			{
				WriteHeader(streamWriter);

				foreach (IClass element in packageWithClasses.Value)
				{
					WriteClass(streamWriter, element);
				}

				WriteFooter(streamWriter);
			}
		}

		private void WriteHeader(StreamWriter streamWriter)
		{
			streamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
			streamWriter.WriteLine("    <xs:schema xmlns:xs=\"http://www.w3.org/2001/XMLSchema\" elementFormDefault=\"qualified\"");
			streamWriter.WriteLine(string.Format("		targetNamespace=\"urn:www.bank.ru:schemas:services:sbpm\""));
			// streamWriter.WriteLine(string.Format("targetNamespace=\"{0}\">", packageNamespaceStereotype.Properties.First(i => i.Name == XmlNameSpacePrepertieName)));
			streamWriter.WriteLine("        xmlns=\"urn:www.bank.ru:schemas:services:sbpm\"> ");
		}

		private void WriteClass(StreamWriter streamWriter, IClass element)
		{
			streamWriter.WriteLine(string.Format("	<xs:complexType name=\"{0}\">", element.Name));

			WriteAttributes(streamWriter, element);
			WriteSequence(streamWriter, element);

			streamWriter.WriteLine("	</xs:complexType>");
		}

		private void WriteAttributes(StreamWriter streamWriter, IClass element)
		{
			var attributes = element.OwnedAttributes;
			foreach (var attribute in attributes)
			{
				//на будущее надо вытащить имена, а пока обойдемся
				//var stereotype = attribute.ApplicableStereotypes.First(i => i.Name == ElementStereotypeName);
				//var propertyName = stereotype.Properties.First(i => i.Name == ElementPropertyName);
				//var prepertyValue = stereotype.Properties.First(i => i.Name == ElementValueName);

				var attributeTypeName = attribute.Name;

				var attributeType = ConvertType(attribute.Type);
				streamWriter.WriteLine(string.Format("			<xs:attribute name=\"{0}\" type=\"{1}\"/>", attributeTypeName, attributeType));
			}
		}

		private void WriteSequence(StreamWriter streamWriter, IClass element)
		{
			//streamWriter.WriteLine("		<xs:sequence>");
			//streamWriter.WriteLine("		</xs:sequence>");
		}

		private void WriteFooter(StreamWriter streamWriter)
		{
			streamWriter.WriteLine("</xs:schema>");
			streamWriter.Flush();
		}

		private string ConvertType(IType type)
		{
			var typeString = type.Name.ToLower();
			if (typeString == "string")
				return "xs:string";
			if (typeString == "boolean")
				return "xs:boolean";
			if (typeString == "integer")
				return "xs:integer";

			throw new ArgumentException();
		}
	}
}
