//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PwC.Data.SqlClient.AlwaysEncrypted.AzureKeyVaultProvider
{
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
    internal class Strings
    {

        private static global::System.Resources.ResourceManager resourceMan;

        private static global::System.Globalization.CultureInfo resourceCulture;

        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings()
        {
        }

        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Microsoft.Data.SqlClient.AlwaysEncrypted.AzureKeyVaultProvider.Strings", typeof(Strings).Assembly);
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
        internal static global::System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to CipherText length does not match the RSA key size..
        /// </summary>
        internal static string CiphertextLengthMismatch
        {
            get
            {
                return ResourceManager.GetString("CiphertextLengthMismatch", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Empty column encryption key specified..
        /// </summary>
        internal static string EmptyCek
        {
            get
            {
                return ResourceManager.GetString("EmptyCek", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Empty encrypted column encryption key specified..
        /// </summary>
        internal static string EmptyCekv
        {
            get
            {
                return ResourceManager.GetString("EmptyCekv", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Internal error: Empty encrypted column encryption key specified..
        /// </summary>
        internal static string EmptyCekvInternal
        {
            get
            {
                return ResourceManager.GetString("EmptyCekvInternal", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to encryptedColumnEncryptionKey length should not be zero..
        /// </summary>
        internal static string EncryptedCekEmpty
        {
            get
            {
                return ResourceManager.GetString("EncryptedCekEmpty", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Signed hash length does not match the RSA key size..
        /// </summary>
        internal static string HashLengthMismatch
        {
            get
            {
                return ResourceManager.GetString("HashLengthMismatch", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Invalid Azure Key Vault key path specified: &apos;{0}&apos;. Valid trusted endpoints: {1}..
        /// </summary>
        internal static string InvalidAkvKeyPathTrustedTemplate
        {
            get
            {
                return ResourceManager.GetString("InvalidAkvKeyPathTrustedTemplate", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Invalid Azure Key Vault key path specified: &apos;{0}&apos;..
        /// </summary>
        internal static string InvalidAkvPathTemplate
        {
            get
            {
                return ResourceManager.GetString("InvalidAkvPathTemplate", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Invalid url specified: &apos;{0}&apos;..
        /// </summary>
        internal static string InvalidAkvUrlTemplate
        {
            get
            {
                return ResourceManager.GetString("InvalidAkvUrlTemplate", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Specified encrypted column encryption key contains an invalid encryption algorithm version &apos;{0}&apos;. Expected version is &apos;{1}&apos;..
        /// </summary>
        internal static string InvalidAlgorithmVersionTemplate
        {
            get
            {
                return ResourceManager.GetString("InvalidAlgorithmVersionTemplate", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The specified encrypted column encryption key&apos;s ciphertext length ({0}) does not match the ciphertext length ({1}) when using column master key (Azure Key Vault key) in &apos;{2}&apos;. The encrypted column encryption key may be corrupt, or the specified Azure Key Vault key path may be incorrect..
        /// </summary>
        internal static string InvalidCiphertextLengthTemplate
        {
            get
            {
                return ResourceManager.GetString("InvalidCiphertextLengthTemplate", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Invalid key encryption algorithm specified: &apos;{0}&apos;. Expected value: &apos;{1}&apos;..
        /// </summary>
        internal static string InvalidKeyAlgorithm
        {
            get
            {
                return ResourceManager.GetString("InvalidKeyAlgorithm", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Invalid signature of the encrypted column encryption key computed..
        /// </summary>
        internal static string InvalidSignature
        {
            get
            {
                return ResourceManager.GetString("InvalidSignature", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The specified encrypted column encryption key&apos;s signature length ({0}) does not match the signature length ({1}) when using column master key (Azure Key Vault key) in &apos;{2}&apos;. The encrypted column encryption key may be corrupt, or the specified Azure Key Vault key path may be incorrect..
        /// </summary>
        internal static string InvalidSignatureLengthTemplate
        {
            get
            {
                return ResourceManager.GetString("InvalidSignatureLengthTemplate", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to The specified encrypted column encryption key signature does not match the signature computed with the column master key (Asymmetric key in Azure Key Vault) in &apos;{0}&apos;. The encrypted column encryption key may be corrupt, or the specified path may be incorrect..
        /// </summary>
        internal static string InvalidSignatureTemplate
        {
            get
            {
                return ResourceManager.GetString("InvalidSignatureTemplate", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to trustedEndPoints cannot be null or empty..
        /// </summary>
        internal static string InvalidTrustedEndpointsList
        {
            get
            {
                return ResourceManager.GetString("InvalidTrustedEndpointsList", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Invalid trusted endpoint specified: &apos;{0}&apos;; a trusted endpoint must have a value..
        /// </summary>
        internal static string InvalidTrustedEndpointTemplate
        {
            get
            {
                return ResourceManager.GetString("InvalidTrustedEndpointTemplate", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Cannot use a non-RSA key: &apos;{0}&apos;..
        /// </summary>
        internal static string NonRsaKeyTemplate
        {
            get
            {
                return ResourceManager.GetString("NonRsaKeyTemplate", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Azure Key Vault key path cannot be null..
        /// </summary>
        internal static string NullAkvPath
        {
            get
            {
                return ResourceManager.GetString("NullAkvPath", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Key encryption algorithm cannot be null..
        /// </summary>
        internal static string NullAlgorithm
        {
            get
            {
                return ResourceManager.GetString("NullAlgorithm", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Internal error: Key encryption algorithm cannot be null..
        /// </summary>
        internal static string NullAlgorithmInternal
        {
            get
            {
                return ResourceManager.GetString("NullAlgorithmInternal", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Column encryption key cannot be null..
        /// </summary>
        internal static string NullCek
        {
            get
            {
                return ResourceManager.GetString("NullCek", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Encrypted column encryption key cannot be null..
        /// </summary>
        internal static string NullCekv
        {
            get
            {
                return ResourceManager.GetString("NullCekv", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Internal error: Encrypted column encryption key cannot be null..
        /// </summary>
        internal static string NullCekvInternal
        {
            get
            {
                return ResourceManager.GetString("NullCekvInternal", resourceCulture);
            }
        }

        /// <summary>
        ///   Looks up a localized string similar to Hash should not be null while decrypting encrypted column encryption key..
        /// </summary>
        internal static string NullHash
        {
            get
            {
                return ResourceManager.GetString("NullHash", resourceCulture);
            }
        }
    }
}
