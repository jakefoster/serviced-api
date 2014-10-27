using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace org.ncore.ServicedApi.Container
{
    public class InjectorType
    {
        public string Name { get; set; }
        public string Assembly { get; set; }
        public string TypeName { get; set; }
        public object Instance { get; set; }

        public InjectorType(){}

        public InjectorType( string name, Type type )
        {
            this.Name = name;
            this.Assembly = type.Assembly.FullName;
            this.TypeName = type.FullName;
        }

        public InjectorType( Type name, Type type )
        {
            this.Name = name.FullName;
            this.Assembly = type.Assembly.FullName;
            this.TypeName = type.FullName;
        }

        public InjectorType( object instance )
        {
            _validateInstance( instance );

            Type type = instance.GetType();

            this.Name = type.FullName;
            this.Assembly = type.Assembly.FullName;
            this.TypeName = type.FullName;
            this.Instance = instance;
        }

        public InjectorType(string name, object instance)
        {
            _validateInstance( instance );

            Type type = instance.GetType();

            this.Name = name;
            this.Assembly = type.Assembly.FullName;
            this.TypeName = type.FullName;
            this.Instance = instance;
        }

        public InjectorType( Type type, object instance )
        {
            _validateInstance( instance );

            this.Name = type.FullName;
            this.Assembly = type.Assembly.FullName;
            this.TypeName = type.FullName;
            this.Instance = instance;
        }

        private static void _validateInstance( object instance )
        {
            if( instance == null )
            {
                throw new ArgumentException( "The 'instance' parameter cannot be null.", "instance" );
            }
        }
    }
}
