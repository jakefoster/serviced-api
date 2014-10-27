using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace org.ncore.ServicedApi.Container
{
    public class Instance
    {
        public static dynamic New( Type type, Injector injector )
        {
            dynamic instance;
            if( Kernel.Registry.Keys.Contains( type.FullName ) )
            {
                instance = Kernel.CreateObject<dynamic>( type.FullName );
            }
            else
            {
                instance = (dynamic)Activator.CreateInstance( type );
            }

            
            injector.Inject( instance );
            return instance;
        }

        public static dynamic New( string name, Injector injector )
        {
            dynamic instance;
            if( Kernel.Registry.Keys.Contains( name ) )
            {
                instance = Kernel.CreateObject<dynamic>( name );
            }
            else
            {
                throw new ApplicationException( "The specified name does not refer to a Type object in the Registry." );
            }

            injector.Inject( instance );
            return instance;
        }

        public static T New<T>( Injector injector )
        {
            T instance;
            if( Kernel.Registry.Keys.Contains( typeof(T).FullName ) )
            {
                instance = Kernel.CreateObject<T>();
            }
            else
            {
                instance = (dynamic)Activator.CreateInstance( typeof(T) );
            }

            injector.Inject( instance );
            return instance;
        }

        public static T New<T>( string name, Injector injector )
        {
            T instance;
            if( Kernel.Registry.Keys.Contains( name ) )
            {
                instance = Kernel.CreateObject<dynamic>( name );
            }
            else
            {
                throw new ApplicationException( "The specified name does not refer to a Type object in the Registry." );
            }

            injector.Inject( instance );
            return instance;
        }

    }
}
