﻿using System;
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

        public static T CreateObject<T>( object[] ctorParams = null )
        {
            string name = typeof( T ).ToString();
            return CreateObject<T>( name, false, ctorParams );
        }

        public static T CreateObject<T>( bool saveInRegistry, object[] ctorParams = null )
        {
            string name = typeof( T ).ToString();
            return CreateObject<T>( name, saveInRegistry, ctorParams );
        }

        public static T CreateObject<T>( string name, object[] ctorParams = null )
        {
            return CreateObject<T>( name, false, ctorParams );
        }

        public static T CreateObject<T>( string name, bool saveInRegistry, object[] ctorParams = null )
        {
            KernelType kernelType = Registry[ name ];
            ObjectHandle handle = Activator.CreateInstance( kernelType.Assembly, kernelType.TypeName, 
                                                            false, 0, null, ctorParams, null, null );
            Object target = (T)handle.Unwrap();
            // TODO: Use Injector to configure?  -JF
            if( saveInRegistry )
            {
                if( !kernelType.AllowSave )
                {
                    throw new ApplicationException( "The 'saveInRegistry' parameter was true but the underlying RegistryEntry for this type does not allow saving an instance to the registry." );
                }

                if( kernelType.Instance != null )
                {
                    throw new ApplicationException( "The 'saveInRegistry' parameter was true but there is already an instance saved in this RegistryEntry." );
                }

                kernelType.Instance = target;
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
            KernelType target = Kernel.Registry[ name ];
            if( target.Instance == null )
            {
                throw new ApplicationException( "The specified entry in the KernalRegistry does not have a saved instance." );
            }
            return (T)target.Instance;
        }


        // TODO: I really want to remove these. I don't like that there's ambiguity.
        //  That said, the ServicedAPI generator makes heavy use of GetOrCreate and 
        //  I need to understand the implications.  -JF
        public static T GetOrCreateObject<T>()
        {
            string name = typeof( T ).ToString();
            return GetOrCreateObject<T>( name, false );
        }

        public static T GetOrCreateObject<T>( bool saveInRegistry )
        {
            string name = typeof( T ).ToString();
            return GetOrCreateObject<T>( name, saveInRegistry );
        }

        public static T GetOrCreateObject<T>( string name )
        {
            return GetOrCreateObject<T>( name, false );
        }

        public static T GetOrCreateObject<T>( string name, bool saveInRegistry )
        {
            Object target = null;
            if( Kernel.Registry != null && Kernel.Registry.ContainsKey( name ) && Kernel.Registry[name].Instance != null )
            {
                target = Kernel.Registry[ name ].Instance;
            }
            else
            {
                target = CreateObject<T>( name, saveInRegistry );
            }
            return (T)target;
        }
    }
}
