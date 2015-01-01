using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using Microsoft.VisualStudio.Modeling.Validation;
using Microsoft.VisualStudio.Uml.Classes;

namespace Sychev.Architecture.ApiValidation.General
{
	//http://msdn.microsoft.com/en-us/library/ee329482.aspx
	public class ApiValidation
	{
		public const string MethodVisibilityExceptionMessage = "Method is not public. API interface must contain only api method but not BI. Reported By ApiValidation";
		public const string MethodVisibilityExceptionNumber = "0001";

		public const string MethodParametersToMuchExceptionMessage = "Method containts more than 5 incoming parametes. Reported By ApiValidation";
		public const string MethodParametersToMuchExceptionNumber = "0002";
		public const int MaxMethodParametersInMethod = 5;

		public static string ClassMethodsToMuchExceptionMessage = "interface containts more than 15 methods. Reported By ApiValidation";
		public static string ClassMethodsToMuchExceptionNumber = "0003";
		public const int MaxMethodsInClass = 15;

		public static string ClassNoMethodExceptionMessage = "interface doesn't contain methods. Reported By ApiValidation";
		public static string ClassNoMethodExceptionNumber = "0004";

		public const int MinTypeLength = 3;
		public static string MinTypeLengtExceptionNumber = "0005";

		public static string TypeIsNotDefineExceptionNumber = "0006";
		public static string InvokeExceptionNumber = "2006";

		[Export(typeof(System.Action<ValidationContext, object>))]
		[ValidationMethod(ValidationCategories.Save | ValidationCategories.Open | ValidationCategories.Menu)]
		public void ValidateInterfaceApi(ValidationContext context, IInterface elementToValidate)
		{
			var operations = elementToValidate.OwnedOperations.ToList();
			var operationNames = new List<string>();

			CheckOperationsCount(context, elementToValidate, operations);
			CheckSpaces(context, elementToValidate);
			foreach (var operation in operations)
			{
				operationNames.Add(CheckNoDublicateOperations(context, operation, operationNames));

				CheckApiMethodVisibilite(context, operation);

				CheckOperationParametersCount(context, operation);
				foreach (var variable in operation.OwnedParameters)
				{
					if (variable.Type == null)
					{
						context.LogWarning(
							string.Format("Type is not defined for {0} in {1}. Reported by ApiValidation.ValidateInterfaceApi", variable.Name, elementToValidate.Name),
							TypeIsNotDefineExceptionNumber, elementToValidate as Microsoft.VisualStudio.Modeling.ModelElement);
						continue;
					}
					else
					{
						ValidateTypeNameLength(context, variable.Type);
					}
				}
			}

			var attributeNames = new List<string>();
			var attributes = elementToValidate.OwnedAttributes;

			foreach (var attribute in attributes)
			{
				attributeNames.Add(CheckNoDublicateAttribute(context, attribute, attributeNames));
				if (attribute.Type == null)
				{
					context.LogWarning(string.Format("Type is not defined for attribute {0} in {1}. Reported by ApiValidation.ValidateInterfaceApi", attribute.Name, elementToValidate.Name), TypeIsNotDefineExceptionNumber, elementToValidate as Microsoft.VisualStudio.Modeling.ModelElement);
					continue;
				}
				ValidateTypeNameLength(context, attribute.Type);
			}
		}

		[Export(typeof(System.Action<ValidationContext, object>))]
		[ValidationMethod(ValidationCategories.Save | ValidationCategories.Open | ValidationCategories.Menu)]
		public void ValidateClassApi(ValidationContext context, IClass elementToValidate)
		{
			ValidateClassNameSpaces(context, elementToValidate);
		}

		[Export(typeof(System.Action<ValidationContext, object>))]
		[ValidationMethod(ValidationCategories.Save | ValidationCategories.Open | ValidationCategories.Menu)]
		public void ValidateTypeApi(ValidationContext context, IType elementToValidate)
		{
			if (elementToValidate.Name.Contains(" "))
			{
				context.LogError(string.Format("Type {0} contain spaces in its name. Reported by ApiValidation.ValidateTypeApi", elementToValidate.Name), InvokeExceptionNumber, elementToValidate as Microsoft.VisualStudio.Modeling.ModelElement);
			}
			//if (string.IsNullOrEmpty(elementToValidate.Name))
			//{
			//	context.LogError(string.Format("Type name {0} is empty, in namespace {1}", elementToValidate.Name, elementToValidate.Package.Name), InvokeExceptionNumber);
			//}
		}

		[Export(typeof(System.Action<ValidationContext, object>))]
		[ValidationMethod(ValidationCategories.Save | ValidationCategories.Open | ValidationCategories.Menu)]
		public void ValidateOperationApi(ValidationContext context, IOperation elementToValidate)
		{
			if (elementToValidate.Name.Contains(" "))
			{
				context.LogError(string.Format("Operation {0} contain spaces in its name. Reported by ApiValidation.ValidateOperationApi", elementToValidate.Name),
					InvokeExceptionNumber, elementToValidate as Microsoft.VisualStudio.Modeling.ModelElement);
			}
			if (string.IsNullOrEmpty(elementToValidate.Name))
			{
				INamedElement element = elementToValidate.Class == null ? elementToValidate.Class as INamedElement : elementToValidate.Interface as INamedElement;
				context.LogError(string.Format("Operation name {0} is empty. Reported by SpacesInNamesValidator.ValidateOperationApi", element.Name), InvokeExceptionNumber, elementToValidate as Microsoft.VisualStudio.Modeling.ModelElement);
			}
		}

		[Export(typeof(System.Action<ValidationContext, object>))]
		[ValidationMethod(ValidationCategories.Save | ValidationCategories.Open | ValidationCategories.Menu)]
		public void ValidatePackageApi(ValidationContext context, IPackage elementToValidate)
		{
			if (elementToValidate.Name.Contains(" "))
			{
				context.LogError(string.Format("Package {0} contain spaces in its name. Reported by ApiValidation.ValidatePackageApi", elementToValidate.Name), InvokeExceptionNumber);
			}
			if (string.IsNullOrEmpty(elementToValidate.Name))
			{
				context.LogError(string.Format("Package name is empty. Reported by ApiValidation.ValidatePackageApi"), InvokeExceptionNumber);
			}
		}

		[Export(typeof(System.Action<ValidationContext, object>))]
		[ValidationMethod(ValidationCategories.Save | ValidationCategories.Open | ValidationCategories.Menu)]
		public void ValidatePropertyApi(ValidationContext context, IProperty elementToValidate)
		{
			if(elementToValidate.Class is Microsoft.VisualStudio.Uml.Interactions.Interaction)
				return;

			if (elementToValidate.Name.Contains(" "))
			{
				context.LogError(string.Format("Property {0} contain spaces in its name. Reported by ApiValidation.ValidatePropertyApi", elementToValidate.Name),
					InvokeExceptionNumber, elementToValidate as Microsoft.VisualStudio.Modeling.ModelElement);
			}
			if (string.IsNullOrEmpty(elementToValidate.Name))
			{
				context.LogError(string.Format("Property is empty in class {0}. Reported by ApiValidation.ValidatePropertyApi", elementToValidate.Class.Name), InvokeExceptionNumber, elementToValidate as Microsoft.VisualStudio.Modeling.ModelElement);
			}
		}

		public static void ValidateClassNameSpaces(ValidationContext context, IClass elementToValidate)
		{
			if (elementToValidate.Name.Contains(" "))
			{
				context.LogError(
					string.Format("Class {0} contain spaces in its name. Reported by SpacesInNamesValidator.ValidateInterfaceApi",
						elementToValidate.Name), InvokeExceptionNumber, elementToValidate as Microsoft.VisualStudio.Modeling.ModelElement);
			}
			if (string.IsNullOrEmpty(elementToValidate.Name))
			{
				context.LogError(string.Format("Class name {0} is empty", elementToValidate.Name), InvokeExceptionNumber, elementToValidate as Microsoft.VisualStudio.Modeling.ModelElement);
			}
		}

		public static void CheckSpaces(ValidationContext context, IInterface elementToValidate)
		{
			if (elementToValidate.Name.Contains(" "))
			{
				context.LogError(
					string.Format(
						"Interface {0} contain spaces in its name. Reported by SpacesInNamesValidator.ValidateInterfaceApi",
						elementToValidate.Name), InvokeExceptionNumber, elementToValidate as Microsoft.VisualStudio.Modeling.ModelElement);
			}
			if (string.IsNullOrEmpty(elementToValidate.Name))
			{
				context.LogError(
					string.Format("Interface name is empty in Package {0}. Reported by SpacesInNamesValidator.ValidateInterfaceApi",
						elementToValidate.Package.Name), InvokeExceptionNumber, elementToValidate as Microsoft.VisualStudio.Modeling.ModelElement);
			}
		}

		public static void ValidateTypeNameLength(ValidationContext context, IType type)
		{
			if (!string.IsNullOrEmpty(type.Name) && type.Name.Length < MinTypeLength)
			{
				context.LogError(string.Format("Type name {0} is too short. Reported by ApiValidation.ValidateTypeNameLength", type.Name), MinTypeLengtExceptionNumber, type as Microsoft.VisualStudio.Modeling.ModelElement);
			}
		}

		public static string CheckNoDublicateAttribute(ValidationContext context, IProperty attribute, List<string> attributeNames)
		{
			string name = attribute.Name;
			if (!string.IsNullOrEmpty(name) && attributeNames.Contains(name))
			{
				context.LogError(string.Format("Duplicate attribute name '{0}' in interface {1}. Reported by ApiValidation.CheckNoDublicateAttribute", name, attribute.Class.Name), "001",attribute as Microsoft.VisualStudio.Modeling.ModelElement);
			}
			return name;
		}

		public static string CheckNoDublicateOperations(ValidationContext context, IOperation operation, List<string> operationsNames)
		{
			string name = operation.Name;
			if (!string.IsNullOrEmpty(name) && operationsNames.Contains(name))
			{
				context.LogError(string.Format("Duplicate operation name '{0}' in interface {1}. Reported by ApiValidation.CheckNoDublicateOperations", name, operation.Class.Name), "001", operation as Microsoft.VisualStudio.Modeling.ModelElement);
			}
			return name;
		}

		public static void CheckOperationParametersCount(ValidationContext context, IOperation operation)
		{
			//to many incoming parameters
			if (operation.OwnedParameters.Count() > MaxMethodParametersInMethod)
			{
				var messageFullString = GetMethodTemplateMessage(operation, MethodParametersToMuchExceptionMessage);
				context.LogError(messageFullString, MethodParametersToMuchExceptionNumber, operation as Microsoft.VisualStudio.Modeling.ModelElement);
			}
		}

		public static void CheckApiMethodVisibilite(ValidationContext context, IOperation operation)
		{
			//api class must contains only public methods. It's not a BL class
			if (operation.Visibility != VisibilityKind.Public)
			{
				var messageFullString = GetMethodTemplateMessage(operation, MethodVisibilityExceptionMessage);
				context.LogError(messageFullString, MethodVisibilityExceptionNumber, operation as Microsoft.VisualStudio.Modeling.ModelElement);
			}

		}

		public static void CheckOperationsCount(ValidationContext context, IInterface elementToValidate, IList<IOperation> operations)
		{
			//
			if (operations.Count == 0)
			{
				var messageFullString = elementToValidate.Name + " " + ClassNoMethodExceptionMessage;
				context.LogError(messageFullString, ClassNoMethodExceptionNumber, elementToValidate as Microsoft.VisualStudio.Modeling.ModelElement);
				return;
			}

			if (operations.Count > MaxMethodsInClass)
			{
				var messageFullString = elementToValidate.Name + " " + ClassMethodsToMuchExceptionMessage;
				context.LogError(messageFullString, ClassMethodsToMuchExceptionNumber, elementToValidate as Microsoft.VisualStudio.Modeling.ModelElement);
			}
		}

		public static string GetMethodTemplateMessage(IOperation operation, string message)
		{
			return "Interface: " + operation.Interface.Name + " method name: " + operation.Name + " " + message;
		}

		//protected string GetPropertyTemplateMessage(IProperty property, string message)
		//{
		//	return "Class: " + property.Class.Name + "method name: " + property.Name + " " + message;
		//}
	}
}