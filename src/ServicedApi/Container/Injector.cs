using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

using org.ncore.Extensions;
using org.ncore.ServicedApi;

namespace org.ncore.ServicedApi.Container
{
    public class Injector
    {
        public bool SearchKernel { get; set; }
        public InjectorRegistry Registry { get; set; }

        public Injector()
        {
            Registry = new InjectorRegistry();
            SearchKernel = true;
        }

        public Injector(bool searchKernel)
        {
            Registry = new InjectorRegistry();
            SearchKernel = searchKernel;
        }

        public Injector( InjectorRegistry registry )
        {
            Registry = registry;
            SearchKernel = true;
            _initialize();
        }

        public Injector( InjectorRegistry registry, bool searchKernel )
        {
            Registry = registry;
            SearchKernel = searchKernel;
            _initialize();
        }

        public void RefreshRegistry()
        {
            _initialize();
        }

        private void _initialize()
        {
            foreach( InjectorType injectorType in Registry.Values )
            {
                // NOTE: Non-obvious but the whole point is to ensure that everything 
                //  in the InjectorRegistry has an instance.  -JF
                if( injectorType.Instance == null )
                {
                    injectorType.Instance = _createInstance( injectorType );
                }
            }
        }

        private static object _createInstance( InjectorType injectorType )
        {
            ObjectHandle handle = Activator.CreateInstance( injectorType.Assembly, injectorType.TypeName );
            Object target = handle.Unwrap();
            return target;
        }
        
        public void Inject( object instance )
        {
            PropertyInfo[] properties = instance.GetType().GetProperties( BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance );

            foreach( PropertyInfo property in properties )
            {
                Debug.WriteLine( "Property: " + property.Name );
                foreach( object attribute in property.GetCustomAttributes( true ) )
                {
                    Debug.WriteLine( "-> Attribute: " + attribute.GetType().Name );
                    if( attribute is InjectAttribute )
                    {
                        Debug.WriteLine( "--> Found injector attribute " + ( (InjectAttribute)attribute ).Name );
                        
                        string name = ( (InjectAttribute)attribute ).Name;

                        if( name.IsEmptyOrNull() )
                        {
                            name = property.Name;
                        }

                        Debug.WriteLine( "---> Resolves to name " + name );

                        object injectable = null;
                        if( Registry.Keys.Contains(name) )
                        {
                            injectable = Registry[ name ].Instance;
                            Debug.WriteLine( "---> Retrieved instance from InjectorRegistry" );
                        }
                        else if( SearchKernel && Kernel.Registry.Keys.Contains( name ) )
                        {
                            injectable = Kernel.GetOrCreateObject<object>( name );
                            Debug.WriteLine( "---> Retrieved instance from KernelRegistry" );
                        }

                        if(injectable != null)
                        {
                            if( property.GetSetMethod(true) != null)
                            {
                                property.SetValue( instance, injectable );
                                Debug.WriteLine( "---> Set value!" );
                            }
                            else
                            {
                                PropertyInfo baseProperty = _getPropertyFromBase( instance.GetType().BaseType, property.Name );
                                baseProperty.SetValue( instance, injectable );
                                Debug.WriteLine( "---> Set value!" );
                            }
                        }
                    }
                }
            }

            FieldInfo[] fields = _getFields( instance.GetType() );
            
            foreach( FieldInfo field in fields )
            {
                Debug.WriteLine( "Field: " + field.Name );
                foreach( object attribute in field.GetCustomAttributes( true ) )
                {
                    Debug.WriteLine( "-> Attribute: " + attribute.GetType().Name );
                    if( attribute is InjectAttribute )
                    {
                        Debug.WriteLine( "--> Found injector attribute " + ( (InjectAttribute)attribute ).Name );

                        string name = ( (InjectAttribute)attribute ).Name;

                        if( name.IsEmptyOrNull() )
                        {
                            name = field.Name;
                        }

                        Debug.WriteLine( "---> Resolves to name " + name );

                        object injectable = null;
                        if( Registry.Keys.Contains( name ) )
                        {
                            injectable = Registry[ name ].Instance;
                            Debug.WriteLine( "---> Retrieved instance from InjectorRegistry" );
                        }
                        else if( SearchKernel && Kernel.Registry.Keys.Contains( name ) )
                        {
                            injectable = Kernel.GetOrCreateObject<object>( name );
                            Debug.WriteLine( "---> Retrieved instance from KernelRegistry" );
                        }

                        if( injectable != null )
                        {
                            field.SetValue( instance, injectable );
                            Debug.WriteLine( "---> Set value!" );
                        }
                    }
                }
            }
        }

        private static FieldInfo[] _getFields( Type type )
        {
            FieldInfo[] fields = type.GetFields( BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance );
            if( type.BaseType != typeof( System.Object ) )
            {
                FieldInfo[] baseFields = _getFields( type.BaseType );
                if( baseFields != null )
                {
                    fields = fields.Concat( baseFields ).ToArray();
                }
            }
            return fields;
        }

        private static PropertyInfo _getPropertyFromBase( Type baseType, string propertyName )
        {
            PropertyInfo property = baseType.GetProperty( propertyName );
            if( property == null )
            {
                if( baseType == typeof( System.Object ) )
                {
                    return null;
                }
                else
                {
                    property = _getPropertyFromBase( baseType.BaseType, propertyName );
                }
            }
            return property;
        }
    }
}
