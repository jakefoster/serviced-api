using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.ncore.ServicedApi.Container
{
    public class Instance
    {
        public static dynamic New( Type type, Injector injector )
        {
            dynamic instance = Kernel.CreateObject<dynamic>( type.ToString(), false );
            injector.Inject( instance );
            return instance;
        }

        public static dynamic New( string name, Injector injector )
        {
            dynamic instance = Kernel.CreateObject<dynamic>( name, false );
            injector.Inject( instance );
            return instance;
        }

        public static T New<T>( Injector injector )
        {
            T instance = Kernel.CreateObject<T>();
            injector.Inject( instance );
            return instance;
        }

        public static T New<T>( string name, Injector injector )
        {
            T instance = Kernel.CreateObject<T>( name, false );
            injector.Inject( instance );
            return instance;
        }

    }
}
