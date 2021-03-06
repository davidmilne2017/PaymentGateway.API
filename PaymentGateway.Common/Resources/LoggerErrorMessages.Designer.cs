//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PaymentGateway.Common.Resources {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    public class LoggerErrorMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal LoggerErrorMessages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        public static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("PaymentGateway.Common.Resources.LoggerErrorMessages", typeof(LoggerErrorMessages).Assembly);
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
        ///   Looks up a localized string similar to Could not create payment.
        /// </summary>
        public static string CouldNotCreatePayment {
            get {
                return ResourceManager.GetString("CouldNotCreatePayment", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Could not retrieve payment.
        /// </summary>
        public static string CouldNotRetrievePayment {
            get {
                return ResourceManager.GetString("CouldNotRetrievePayment", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fatal exception with bank repository create payment.
        /// </summary>
        public static string CreatePaymentRepositoryException {
            get {
                return ResourceManager.GetString("CreatePaymentRepositoryException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Null request body.
        /// </summary>
        public static string NullRequestBody {
            get {
                return ResourceManager.GetString("NullRequestBody", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fatal exception with payment validators.
        /// </summary>
        public static string PaymentValidatorsException {
            get {
                return ResourceManager.GetString("PaymentValidatorsException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fatal exception with bank repository retrieve payment details.
        /// </summary>
        public static string RetrievePaymentDetailsRepositoryException {
            get {
                return ResourceManager.GetString("RetrievePaymentDetailsRepositoryException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to TransactionId not recognised.
        /// </summary>
        public static string TransactionIdNotFound {
            get {
                return ResourceManager.GetString("TransactionIdNotFound", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Validation errors.
        /// </summary>
        public static string ValidationErrors {
            get {
                return ResourceManager.GetString("ValidationErrors", resourceCulture);
            }
        }
    }
}
