﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Este código fue generado por una herramienta.
//     Versión de runtime:4.0.30319.42000
//
//     Los cambios en este archivo podrían causar un comportamiento incorrecto y se perderán si
//     se vuelve a generar el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Nt.Core.Resources {
    using System;
    
    
    /// <summary>
    ///   Clase de recurso fuertemente tipado, para buscar cadenas traducidas, etc.
    /// </summary>
    // StronglyTypedResourceBuilder generó automáticamente esta clase
    // a través de una herramienta como ResGen o Visual Studio.
    // Para agregar o quitar un miembro, edite el archivo .ResX y, a continuación, vuelva a ejecutar ResGen
    // con la opción /str o recompile su proyecto de VS.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class ExceptionMessages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ExceptionMessages() {
        }
        
        /// <summary>
        ///   Devuelve la instancia de ResourceManager almacenada en caché utilizada por esta clase.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Nt.Core.Resources.ExceptionMessages", typeof(ExceptionMessages).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Reemplaza la propiedad CurrentUICulture del subproceso actual para todas las
        ///   búsquedas de recursos mediante esta clase de recurso fuertemente tipado.
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
        ///   Busca una cadena traducida similar a Los objectos Ninjascript siempre deben ser cargados a través del método &quot;Load&quot; antes de que sus métodos sean invocados desde los controladores de eventos de Ninjatrader. La carga del Ninjascript debe ser realizada en el controlador de eventos &quot;NinjaTrader.NinjaScript.OnStateChanged&quot;, dentro del &quot;State.Load&quot;..
        /// </summary>
        internal static string BarUpdateLoadException {
            get {
                return ResourceManager.GetString("BarUpdateLoadException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a El constructor de objetos Ninjascripts, Builder, debe ser creado por un Script que herede de TScript y por un Builder que herede de TBuilder. Si las dos condiciones no se cumplen el objeto no puede ser creado..
        /// </summary>
        internal static string CreateBuilderAssignableException {
            get {
                return ResourceManager.GetString("CreateBuilderAssignableException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a El constructor del objeto builder ha fallado al ser invocado. Los parámetros deben ser erróneos..
        /// </summary>
        internal static string CreateBuilderCtorInvokeException {
            get {
                return ResourceManager.GetString("CreateBuilderCtorInvokeException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a La reflexión ha fallado. El constructor del objecto builder no se ha podido obtener..
        /// </summary>
        internal static string CreateBuilderGetConstructorException {
            get {
                return ResourceManager.GetString("CreateBuilderGetConstructorException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a El objecto Ninjascript no ha sido cargado a través del método Ninjascript.Load(ninjascript,bars)..
        /// </summary>
        internal static string LoadException {
            get {
                return ResourceManager.GetString("LoadException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Los parámetros del método BaseNinjascript.Load no pueden ser nulos. Estos parámetros son la conexión de dicho objeto con los objetos &quot;NinjaTrader.NinjaScriptBase&quot; y &quot;NinjaTrader.Chart.Bars&quot; y son fundamentales para su correcto funcionamiento..
        /// </summary>
        internal static string LoadParameterException {
            get {
                return ResourceManager.GetString("LoadParameterException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a Los objectos Ninjascript siempre deben ser cargados a través del método &quot;Load&quot; antes de que sus métodos sean invocados desde los controladores de eventos de Ninjatrader. La carga del Ninjascript debe ser realizada en el controlador de eventos &quot;NinjaTrader.NinjaScript.OnStateChanged&quot;, dentro del &quot;State.Load&quot;..
        /// </summary>
        internal static string MarketDataLoadException {
            get {
                return ResourceManager.GetString("MarketDataLoadException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a No se han podido asignar las propiedades porque el objecto Ninjascript no está configurado..
        /// </summary>
        internal static string SetDefaultConfigureException {
            get {
                return ResourceManager.GetString("SetDefaultConfigureException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a No se han podido asignar las propiedades por defecto, ya que el objecto &quot;NinjaTrader.NinjascriptBase&quot; tiene una referencia nula..
        /// </summary>
        internal static string SetDefaultNinjaScriptException {
            get {
                return ResourceManager.GetString("SetDefaultNinjaScriptException", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Busca una cadena traducida similar a El Ninjascript no se ha configurado. Para llevar a cabo su configuración se debe ejcutar el método Ninjascript.SetOptions desde alguno de los siguientes métodos del NinjascriptBuilder: IBuilder.ctor, IBuilder.Configure y IBuilder.Build..
        /// </summary>
        internal static string SetOptionsCallerException {
            get {
                return ResourceManager.GetString("SetOptionsCallerException", resourceCulture);
            }
        }
    }
}
