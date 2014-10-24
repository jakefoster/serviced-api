using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.Remoting;
using System.Configuration;

namespace org.ncore.ServicedApi.Container
{
    public class Kernel
    {
        // TODO: Have to change this over to use ContextStorage instead.  This is unsafe!  JF
        [ThreadStatic]
        public static KernelRegistry Registry = new KernelRegistry();

        public static T CreateObject<T>()
        {
            string name = typeof( T ).ToString();
            return CreateObject<T>( name, false );
        }

        public static T CreateObject<T>( string name )
        {
            return CreateObject<T>( name, false );
        }

        public static T CreateObject<T>( string name, bool saveInRegistry )
        {
            KernelConfiguration configuration = (KernelConfiguration)ConfigurationManager.GetSection( "kernel" );
            TypeElement targetType = configuration.Types[ name ];
            ObjectHandle handle = Activator.CreateInstance( targetType.Assembly, targetType.TypeName );
            Object target = (T)handle.Unwrap();
            // TODO: Configure params?  JF
            if( saveInRegistry )
            {
                Registry.Add( name, target );
            }
            return (T)target;
        }

        public static T GetObject<T>()
        {
            string name = typeof( T ).ToString();
            return GetObject<T>( name );
        }

        public static T GetObject<T>( string name )
        {
            Object target = Kernel.Registry[ name ];
            return (T)target;
        }


        public static T GetOrCreateObject<T>()
        {
            string name = typeof( T ).ToString();
            return GetOrCreateObject<T>( name, false );
        }

        public static T GetOrCreateObject<T>( string name )
        {
            return GetOrCreateObject<T>( name, false );
        }

        public static T GetOrCreateObject<T>( string name, bool saveInRegistry )
        {
            Object target = null;
            if( Kernel.Registry != null && Kernel.Registry.ContainsKey( name ) )
            {
                target = Kernel.Registry[ name ];
            }
            else
            {
                target = CreateObject<T>( name, saveInRegistry );
            }
            return (T)target;
        }
    }
}
