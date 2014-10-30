using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace org.ncore.ServicedApi.Container
{
    public static class New
    {
        public static dynamic Instance( Type type, Injector injector, object[] ctorParams = null )
        {
            dynamic instance;
            if( Kernel.Registry.Keys.Contains( type.FullName ) )
            {
                instance = Kernel.CreateObject<dynamic>( type.FullName, ctorParams );
            }
            else
            {
                instance = (dynamic)Activator.CreateInstance( type, ctorParams );
            }

            if(injector != null)
            {
                injector.Inject( instance );
            }
            return instance;
        }

        public static dynamic Instance( string name, Injector injector, object[] ctorParams = null )
        {
            dynamic instance;
            if( Kernel.Registry.Keys.Contains( name ) )
            {
                instance = Kernel.CreateObject<dynamic>( name, ctorParams );
            }
            else
            {
                throw new ApplicationException( "The specified name does not refer to a Type object in the Registry." );
            }

            if( injector != null )
            {
                injector.Inject( instance );
            }
            return instance;
        }

        public static T Instance<T>( Injector injector, object[] ctorParams = null )
        {
            T instance;
            if( Kernel.Registry.Keys.Contains( typeof( T ).FullName ) )
            {
                instance = Kernel.CreateObject<T>( ctorParams );
            }
            else
            {
                instance = (dynamic)Activator.CreateInstance( typeof( T ), ctorParams );
            }

            if( injector != null )
            {
                injector.Inject( instance );
            }
            return instance;
        }

        public static T Instance<T>( string name, Injector injector, object[] ctorParams = null )
        {
            T instance;
            if( Kernel.Registry.Keys.Contains( name ) )
            {
                instance = Kernel.CreateObject<dynamic>( name, ctorParams );
            }
            else
            {
                throw new ApplicationException( "The specified name does not refer to a Type object in the Registry." );
            }

            if( injector != null )
            {
                injector.Inject( instance );
            }
            return instance;
        }

        public static Service Service( Type type )
        {
            return new Service( type );
        }

        public static Service Service( string name )
        {
            return new Service( name );
        }
    }
}
