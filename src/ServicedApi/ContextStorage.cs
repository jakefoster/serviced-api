using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace org.ncore.ServicedApi
{
    /// <summary>
    /// Class  used to store various Context.Current instances safely, regardless
    ///     of whether they're being used from an application with an HttpContext
    ///     or not.
    /// </summary>
    public class ContextStorage : Dictionary<string,Object>
    {
        private const string HTTP_CONTEXT_KEY = "org.ncore.ServicedApi.ContextStorage";

        /// <summary>
        /// This static member field should never be used except by the public static
        /// property accessor.
        /// </summary>
        [ThreadStatic]
        private static ContextStorage _nonHttpContextStorage = null;
        
        /// <summary>
        /// Property accessor which intelligently sets/gets the current ContextStorage field using
        /// the HttpContext.Current.Items cache if it is called within an HttpContext, or using
        /// the private field _nonHttpContextStorage if not called from an HttpContext.
        /// </summary>
        public static ContextStorage Current
        {
            get
            {
                if( HttpContext.Current == null )
                {
                    if( _nonHttpContextStorage == null )
                    {
                        _nonHttpContextStorage = new ContextStorage();
                    }
                    return _nonHttpContextStorage;
                }
                else
                {
                    if( !HttpContext.Current.Items.Contains( HTTP_CONTEXT_KEY ) )
                    {
                        HttpContext.Current.Items[ HTTP_CONTEXT_KEY ] = new ContextStorage();
                    }
                    return (ContextStorage)HttpContext.Current.Items[ HTTP_CONTEXT_KEY ];
                }
            }
            set
            {
                if( HttpContext.Current == null )
                {
                    _nonHttpContextStorage = value;
                }
                else
                {
                    HttpContext.Current.Items[ HTTP_CONTEXT_KEY ] = value;
                }
            }
        }
    }
}