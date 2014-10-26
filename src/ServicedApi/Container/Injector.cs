using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using org.ncore.ServicedApi;

namespace org.ncore.ServicedApi.Container
{
    public class Injector
    {
        private Dictionary<object, object> _registry = null;
        public Injector( Dictionary<object, object> registry )
        {
            _registry = registry;
        }

        public void Inject( object instance )
        {
            MemberInfo[] properties = _getMembers( instance.GetType(), MemberTypeEnum.Property );

            foreach( MemberInfo property in properties )
            {
                foreach( object attribute in property.GetCustomAttributes( true ) )
                {
                    if( attribute is InjectAttribute )
                    {
                        Debug.WriteLine( "Found: " + ( (InjectAttribute)attribute ).Name );
                        // TODO: Set it!
                    }
                }
            }

            // Do the reflection thing here -> even on private fields or private setters.
            /*
            foreach( object attribute in member.GetCustomAttributes( true ) )
            {
                if( attribute is IsTestedAttribute )
                {
                    return true;
                }
            }
            return false;
            */
        }

        public enum MemberTypeEnum
        {
            Field,
            Property
        }

        private static MemberInfo[] _getMembers( Type type, MemberTypeEnum memberType )
        {
            MemberInfo[] members;
            if( memberType == MemberTypeEnum.Field )
            {
                members = type.GetFields( BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.FlattenHierarchy );
            }
            else
            {
                members = type.GetProperties( BindingFlags.Public | BindingFlags.Instance );
            }
            return members;
        }

        private static MemberInfo _getMember( Type type, MemberInfo member, MemberTypeEnum memberType )
        {
            MemberInfo instanceMember;
            if( memberType == MemberTypeEnum.Field )
            {
                instanceMember = type.GetField( member.Name, BindingFlags.NonPublic | BindingFlags.Instance );
            }
            else
            {
                instanceMember = type.GetProperty( member.Name );
            }
            return instanceMember;
        }

        private static MemberInfo _getMemberFromBase( Type baseType, string memberName, MemberTypeEnum memberType )
        {
            MemberInfo member;
            if( memberType == MemberTypeEnum.Field )
            {
                member = baseType.GetField( memberName, BindingFlags.NonPublic | BindingFlags.Instance );
            }
            else
            {
                member = baseType.GetProperty( memberName );
            }

            if( member == null )
            {
                if( baseType == typeof( System.Object ) )
                {
                    return null;
                }
                else
                {
                    member = _getMemberFromBase( baseType.BaseType, memberName, memberType );
                }
            }
            return member;
        }
    }
}
