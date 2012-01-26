﻿/*  Copyright 2012 PerceiveIT Limited
 *  This file is part of the Scryber library.
 *
 *  You can redistribute Scryber and/or modify 
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 3 of the License, or
 *  (at your option) any later version.
 * 
 *  Scryber is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 * 
 *  You should have received a copy of the GNU General Public License
 *  along with Scryber source code in the COPYING.txt file.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.239
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Scryber.Generation {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Errors {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Errors() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Scryber.Generation.Errors", typeof(Errors).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An attribute definition was in the elements collection..
        /// </summary>
        internal static string AttributeDefinitionInElementsCollection {
            get {
                return ResourceManager.GetString("AttributeDefinitionInElementsCollection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An object of type &apos;{0}&apos; cannot be cast to the required &apos;{1}&apos; type..
        /// </summary>
        internal static string CannotConvertObjectToType {
            get {
                return ResourceManager.GetString("CannotConvertObjectToType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to An instance of type &apos;{0}&apos; could not be created. {1}.
        /// </summary>
        internal static string CannotCreateInstanceOfType {
            get {
                return ResourceManager.GetString("CannotCreateInstanceOfType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The value &apos;{0}&apos; is invalid for an event binding on attribute &apos;{1}&apos;. You cannot specify data binding expressions on event attributes..
        /// </summary>
        internal static string CannotSpecifyBindingExpressionsOnEvents {
            get {
                return ResourceManager.GetString("CannotSpecifyBindingExpressionsOnEvents", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type reference &apos;{0}&apos; cannot be used as it resolves to a remote type rather than an actual type. Use the actual type name..
        /// </summary>
        internal static string CannotUseRemoteTypeReferencesInATypeAttribute {
            get {
                return ResourceManager.GetString("CannotUseRemoteTypeReferencesInATypeAttribute", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Can only parse a component as an element.
        /// </summary>
        internal static string CanOnlyParseComponentAsElement {
            get {
                return ResourceManager.GetString("CanOnlyParseComponentAsElement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not determine the type of the reflected property.
        /// </summary>
        internal static string CouldNotDeterminePropertyType {
            get {
                return ResourceManager.GetString("CouldNotDeterminePropertyType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The text property &apos;{0}&apos; could not be set on type &apos;{1}&apos;. Check the permissions and scope..
        /// </summary>
        internal static string CouldNotSetTextPropertyValue {
            get {
                return ResourceManager.GetString("CouldNotSetTextPropertyValue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type &apos;{0}&apos; does not support databinding. To support databinding a type must implement the IPDFBindableComponent interface..
        /// </summary>
        internal static string DatabindingIsNotSupportedOnType {
            get {
                return ResourceManager.GetString("DatabindingIsNotSupportedOnType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type &apos;{1}&apos; has multiple default elements defined. The property &apos;{0}&apos; cannot have a PDFElement attribute with a null or empty name because another property has already declared one. Either specify a name, or user the PDFIgnore attribute..
        /// </summary>
        internal static string DuplicateDefaultElementOnClass {
            get {
                return ResourceManager.GetString("DuplicateDefaultElementOnClass", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The XPath expression &apos;{0}&apos; could not be evaluated. Please check the statement..
        /// </summary>
        internal static string InvalidXPathExpression {
            get {
                return ResourceManager.GetString("InvalidXPathExpression", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No &apos;Add&apos; method was found on the collection type &apos;{0}&apos; accepting a single parameter of type &apos;{1}&apos;. This method is required for a parsed collection, unless the collection implements the IList interface..
        /// </summary>
        internal static string NoAddMethodFoundOnCollection {
            get {
                return ResourceManager.GetString("NoAddMethodFoundOnCollection", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not get the Content property definition from the template type.
        /// </summary>
        internal static string NoContentPropertyDefined {
            get {
                return ResourceManager.GetString("NoContentPropertyDefined", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No PDFComponent was found with the declared name of &apos;{0}&apos; in the namespace &apos;{1}&apos;.
        /// </summary>
        internal static string NoPDFComponentDeclaredWithNameInNamespace {
            get {
                return ResourceManager.GetString("NoPDFComponentDeclaredWithNameInNamespace", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No Type could be found in the namespace &apos;{1}&apos; that declares a component name &apos;{0}&apos;. Please check the file and required type..
        /// </summary>
        internal static string NoTypeFoundWithPDFComponentNameInNamespace {
            get {
                return ResourceManager.GetString("NoTypeFoundWithPDFComponentNameInNamespace", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type &apos;{0}&apos; is declared as parsable with the PDFParsableValueAttribute, but no static method can be found that matched the signature  &apos;Parse(string):T&apos;.
        /// </summary>
        internal static string ParsableValueMustHaveParseMethod {
            get {
                return ResourceManager.GetString("ParsableValueMustHaveParseMethod", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The parsed type &apos;{0}&apos; does not contain a definiton for the {2} with name &apos;{1}&apos;.
        /// </summary>
        internal static string ParsedTypeDoesNotContainDefinitionFor {
            get {
                return ResourceManager.GetString("ParsedTypeDoesNotContainDefinitionFor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The type of property &apos;{0}&apos; in class &apos;{1}&apos; must either be of a known simple type  or be declared with the PDFParsableValueAttribute. Attributes cannot contain complex defintion..
        /// </summary>
        internal static string ParserAttributeMustBeSimpleOrCustomParsableType {
            get {
                return ResourceManager.GetString("ParserAttributeMustBeSimpleOrCustomParsableType", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The attribute name of a PDFAttribute cannot be null or empty. Please specify a name on property &apos;{0}&apos; of type &apos;{1}&apos; .
        /// </summary>
        internal static string ParserAttributeNameCannotBeEmpty {
            get {
                return ResourceManager.GetString("ParserAttributeNameCannotBeEmpty", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The file parser could not find an assembly with the name &apos;{0}&apos;.
        /// </summary>
        internal static string ParserCannotFindAssemblyWithName {
            get {
                return ResourceManager.GetString("ParserCannotFindAssemblyWithName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The required attribute &apos;{0}&apos; was not found on the component definition &apos;{1}&apos;.
        /// </summary>
        internal static string RequiredAttributeNoFoundOnElement {
            get {
                return ResourceManager.GetString("RequiredAttributeNoFoundOnElement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The return type of XPath expression &apos;{0}&apos; could not be determined. Expressions for simple properties must return a simple node set, boolean, integer or string values..
        /// </summary>
        internal static string ReturnTypeOfXPathExpressionCouldNotBeDetermined {
            get {
                return ResourceManager.GetString("ReturnTypeOfXPathExpressionCouldNotBeDetermined", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Neither the source path or source type were set on the remote reference. One of these attributes is required.
        /// </summary>
        internal static string SourcePathOrTypeMustBeSet {
            get {
                return ResourceManager.GetString("SourcePathOrTypeMustBeSet", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The parent Component of a template Component must be an instance of an IPDFContainerComponent.
        /// </summary>
        internal static string TemplateComponentParentMustBeContainer {
            get {
                return ResourceManager.GetString("TemplateComponentParentMustBeContainer", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The template generator has not been initialised. The InitTemplate method must be called before any instaniation..
        /// </summary>
        internal static string TemplateHasNotBeenInitialised {
            get {
                return ResourceManager.GetString("TemplateHasNotBeenInitialised", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The expected {0} property &apos;{1}&apos; was not found on the text literal type &apos;{2}&apos;. Change the generator settings, or define the property..
        /// </summary>
        internal static string TextLiteralTextPropertyNotFound {
            get {
                return ResourceManager.GetString("TextLiteralTextPropertyNotFound", resourceCulture);
            }
        }
    }
}
