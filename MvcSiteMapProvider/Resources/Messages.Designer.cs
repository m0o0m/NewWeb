﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.18444
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace MvcSiteMapProvider.Resources {
    using System;
    
    
    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("MvcSiteMapProvider.Resources.Messages", typeof(Messages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
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
        ///   查找类似 The current ACL module does not provide functionality for regular SiteMapNode objects. 的本地化字符串。
        /// </summary>
        internal static string AclModuleDoesNotSupportRegularSiteMapNodes {
            get {
                return ResourceManager.GetString("AclModuleDoesNotSupportRegularSiteMapNodes", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Ambiguous controller. Found multiple controller types for {0}Controller. Consider narrowing the places to search by adding you controller namespaces to ControllerBuilder.Current.DefaultNamespaces. 的本地化字符串。
        /// </summary>
        internal static string AmbiguousControllerFoundMultipleControllers {
            get {
                return ResourceManager.GetString("AmbiguousControllerFoundMultipleControllers", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Scanning assemblies for sitemap nodes is not supported in the current trust level. Consider a higher trust level or disable scanning assemblies for SiteMap nodes. 的本地化字符串。
        /// </summary>
        internal static string AssemblyScanTrustLevelNotSupported {
            get {
                return ResourceManager.GetString("AssemblyScanTrustLevelNotSupported", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Cannot enumerate a threadsafe dictionary. Instead, enumerate the keys or values collection. 的本地化字符串。
        /// </summary>
        internal static string CannotEnumerateThreadSafeDictionary {
            get {
                return ResourceManager.GetString("CannotEnumerateThreadSafeDictionary", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Root node could not be determined. Check that the XML SiteMap file exists or that there is an MvcSiteMapNode attribute defined that does not have the ParentKey defined. 的本地化字符串。
        /// </summary>
        internal static string CouldNotDetermineRootNode {
            get {
                return ResourceManager.GetString("CouldNotDetermineRootNode", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Could not resolve URL for sitemap node {0} which represents action {1} in controller {2}. Ensure that the route {3} for this sitemap node can be resolved and that its default values allow resolving the URL for the current sitemap node. 的本地化字符串。
        /// </summary>
        internal static string CouldNotResolve {
            get {
                return ResourceManager.GetString("CouldNotResolve", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Duplicate root node detected. This usually happens when a rootnode is defined in the SiteMap XML file and/or one or more MvcSiteMapNode attributes have been defined without a ParentKey. 的本地化字符串。
        /// </summary>
        internal static string DuplicateRootNodeDetected {
            get {
                return ResourceManager.GetString("DuplicateRootNodeDetected", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 An invalid element was found in the sitemap. 的本地化字符串。
        /// </summary>
        internal static string InvalidSiteMapElement {
            get {
                return ResourceManager.GetString("InvalidSiteMapElement", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 No ISiteMapNodeUrlResolver is provided for the current node. 的本地化字符串。
        /// </summary>
        internal static string NoUrlResolverProvided {
            get {
                return ResourceManager.GetString("NoUrlResolverProvided", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Relative URL is not allowed. 的本地化字符串。
        /// </summary>
        internal static string RelativeUrlNotAllowed {
            get {
                return ResourceManager.GetString("RelativeUrlNotAllowed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Site map file could not be found. Verify that the path provided in Web.config is correct. 的本地化字符串。
        /// </summary>
        internal static string SiteMapFileNotFound {
            get {
                return ResourceManager.GetString("SiteMapFileNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 An error occured while building the sitemap... Check the InnerException for more details. 的本地化字符串。
        /// </summary>
        internal static string UnknownException {
            get {
                return ResourceManager.GetString("UnknownException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   查找类似 Unknown SiteMap provider: {0} 的本地化字符串。
        /// </summary>
        internal static string UnknownSiteMapProvider {
            get {
                return ResourceManager.GetString("UnknownSiteMapProvider", resourceCulture);
            }
        }
    }
}
