﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ClearUnusedGUIDs {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class MyStrings___Copy {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal MyStrings___Copy() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("ClearUnusedGUIDs.MyStrings - Copy", typeof(MyStrings___Copy).Assembly);
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
        public static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Команда доступна только в режиме редактирования семейства..
        /// </summary>
        public static string ErrorAvailableOnlyInFamilyEditor {
            get {
                return ResourceManager.GetString("ErrorAvailableOnlyInFamilyEditor", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не указан файл общих параметров..
        /// </summary>
        public static string ErrorNoSharedParamsFile {
            get {
                return ResourceManager.GetString("ErrorNoSharedParamsFile", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Выберите семейство-аналог.
        /// </summary>
        public static string MessageSelectFamilyAnalog {
            get {
                return ResourceManager.GetString("MessageSelectFamilyAnalog", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Не удалось добавить.
        /// </summary>
        public static string ResultFailedToAdd {
            get {
                return ResourceManager.GetString("ResultFailedToAdd", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Успешно добавлено параметров.
        /// </summary>
        public static string ResultParametersAdded {
            get {
                return ResourceManager.GetString("ResultParametersAdded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Уже присутствовали в семействе.
        /// </summary>
        public static string ResultParamsAlreadyAdded {
            get {
                return ResourceManager.GetString("ResultParamsAlreadyAdded", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Добавление параметра.
        /// </summary>
        public static string TransactionAddParameter {
            get {
                return ResourceManager.GetString("TransactionAddParameter", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Удаление определений общих параметров.
        /// </summary>
        public static string TransactionDeleteGuids {
            get {
                return ResourceManager.GetString("TransactionDeleteGuids", resourceCulture);
            }
        }
    }
}
