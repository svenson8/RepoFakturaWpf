﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Ten kod został wygenerowany przez narzędzie.
//     Wersja wykonawcza:4.0.30319.42000
//
//     Zmiany w tym pliku mogą spowodować nieprawidłowe zachowanie i zostaną utracone, jeśli
//     kod zostanie ponownie wygenerowany.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FakturaWpf.Properties {
    using System;
    
    
    /// <summary>
    ///   Klasa zasobu wymagająca zdefiniowania typu do wyszukiwania zlokalizowanych ciągów itd.
    /// </summary>
    // Ta klasa została automatycznie wygenerowana za pomocą klasy StronglyTypedResourceBuilder
    // przez narzędzie, takie jak ResGen lub Visual Studio.
    // Aby dodać lub usunąć składową, edytuj plik ResX, a następnie ponownie uruchom narzędzie ResGen
    // z opcją /str lub ponownie utwórz projekt VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        /// Zwraca buforowane wystąpienie ResourceManager używane przez tę klasę.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("FakturaWpf.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Przesłania właściwość CurrentUICulture bieżącego wątku dla wszystkich
        ///   przypadków przeszukiwania zasobów za pomocą tej klasy zasobów wymagającej zdefiniowania typu.
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
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot;?&gt;
        ///&lt;Countries&gt;
        ///  &lt;Country&gt;
        ///    &lt;symbol&gt;AL&lt;/symbol&gt;
        ///    &lt;name&gt;Albania&lt;/name&gt;
        ///  &lt;/Country&gt;
        ///  &lt;Country&gt;
        ///  &lt;symbol&gt;AR&lt;/symbol&gt;
        ///  &lt;name&gt;Argntyna&lt;/name&gt;
        ///&lt;/Country&gt;
        ///      &lt;Country&gt;
        ///  &lt;symbol&gt;BG&lt;/symbol&gt;
        ///  &lt;name&gt;Belgia&lt;/name&gt;
        ///&lt;/Country&gt;
        ///      &lt;Country&gt;
        ///  &lt;symbol&gt;CZ&lt;/symbol&gt;
        ///  &lt;name&gt;Czechy&lt;/name&gt;
        ///&lt;/Country&gt;
        ///      &lt;Country&gt;
        ///  &lt;symbol&gt;DN&lt;/symbol&gt;
        ///  &lt;name&gt;Dania&lt;/name&gt;
        ///&lt;/Country&gt;
        ///      &lt;Country&gt;
        ///  &lt;symbol&gt;HS&lt;/symbol&gt;
        ///  &lt;name&gt;Hiszpania&lt;/name&gt;
        ///&lt;/Country&gt;
        ///       [obcięto pozostałą część ciągu]&quot;;.
        /// </summary>
        internal static string Countries {
            get {
                return ResourceManager.GetString("Countries", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu &lt;?xml version=&quot;1.0&quot; encoding=&quot;UTF-8&quot;?&gt;
        ///&lt;Provinces&gt;
        ///	&lt;w&gt;dolnośląskie&lt;/w&gt;
        ///	&lt;w&gt;kujawsko-pomorskie&lt;/w&gt;
        ///	&lt;w&gt;lubelskie&lt;/w&gt;
        ///	&lt;w&gt;lubuskie&lt;/w&gt;
        ///	&lt;w&gt;łódzkie&lt;/w&gt;
        ///	&lt;w&gt;małopolskie&lt;/w&gt;
        ///	&lt;w&gt;mazowieckie&lt;/w&gt;
        ///	&lt;w&gt;opolskie&lt;/w&gt;
        ///	&lt;w&gt;podkarpackie&lt;/w&gt;
        ///	&lt;w&gt;podlaskie&lt;/w&gt;
        ///	&lt;w&gt;pomorskie&lt;/w&gt;
        ///	&lt;w&gt;śląskie&lt;/w&gt;
        ///	&lt;w&gt;świętokrzyskie&lt;/w&gt;
        ///	&lt;w&gt;warmińsko-mazurskie&lt;/w&gt;
        ///	&lt;w&gt;wielkopolskie&lt;/w&gt;
        ///	&lt;w&gt;zachodniopomorskie&lt;/w&gt;
        ///&lt;/Provinces&gt;.
        /// </summary>
        internal static string Provinces {
            get {
                return ResourceManager.GetString("Provinces", resourceCulture);
            }
        }
        
        /// <summary>
        /// Wyszukuje zlokalizowany ciąg podobny do ciągu 
        ///
        ///CREATE OR ALTER PROCEDURE [dbo].[READXML_COUNTRIES]
        ///	@filepath varchar(100)
        ///
        ///AS
        ///BEGIN
        ///DECLARE @SQL NVARCHAR(MAX)
        ///
        ///SET @SQL = N&apos;
        ///Declare @xml XML
        ///Select @xml =
        ///CONVERT(XML,bulkcolumn,2) FROM OPENROWSET(BULK &apos;&apos;&apos; +@filepath+ &apos;&apos;&apos;,SINGLE_BLOB) AS X
        ///
        ///SET ARITHABORT ON
        ///
        ///Insert into [TSlownik]
        ///(
        ///SLKOMUN1, SLKOMUN2, SLRODZ, ACTIVE, IUSERID
        ///)
        ///
        ///Select
        ///P.value(&apos;&apos;symbol[1]&apos;&apos;,&apos;&apos;VARCHAR(50)&apos;&apos;) AS symbol,
        ///P.value(&apos;&apos;name[1]&apos;&apos;,&apos;&apos;VARCHAR(50)&apos;&apos;) AS name,
        ///&apos;&apos;KRAJ&apos;&apos; as Kraj,
        ///&apos;&apos;T&apos;&apos; as Act,
        ///1 as us
        ///
        ///Fr [obcięto pozostałą część ciągu]&quot;;.
        /// </summary>
        internal static string XML_COUNT_SCR {
            get {
                return ResourceManager.GetString("XML_COUNT_SCR", resourceCulture);
            }
        }
    }
}
