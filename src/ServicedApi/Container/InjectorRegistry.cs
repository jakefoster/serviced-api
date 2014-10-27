using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Configuration;

namespace org.ncore.ServicedApi.Container
{
    // TODO: Should this be changed to a ConcurrentDictionary to ensure thread safety?  -JF
    public class InjectorRegistry : IDictionary<string, InjectorType>
    {
        private Dictionary<string, InjectorType> _dictionary;

        public void Add( InjectorType value )
        {
            _dictionary.Add( value.Name, value );
        }

        public void Add( string name, InjectorType value )
        {
            if( name != value.Name )
            {
                throw new ArgumentException( "The 'name' parameter must match the Name property on the supplied InjectorType.", "name" );
            }
            _dictionary.Add( value.Name, value );
        }

        public InjectorRegistry()
            : base()
        {
            this._initialize();
        }

        // NOTE: Honestly, this should really only be used for unit testing.  -JF
        public void Reset()
        {
            this._initialize();
        }

        private void _initialize()
        {
            lock( this )
            {
                _dictionary = new Dictionary<string, InjectorType>();
            }
        }

        #region IDictionary<string,InjectorType> Members


        public bool ContainsKey( string key )
        {
            return _dictionary.ContainsKey( key );
        }

        public ICollection<string> Keys
        {
            get { return _dictionary.Keys; }
        }

        public bool Remove( string key )
        {
            return _dictionary.Remove( key );
        }

        public bool TryGetValue( string key, out InjectorType value )
        {
            return _dictionary.TryGetValue( key, out value );
        }

        public ICollection<InjectorType> Values
        {
            get { return _dictionary.Values; }
        }

        public InjectorType this[ string key ]
        {
            get
            {
                return _dictionary[ key ];
            }
            set
            {
                _dictionary[ key ] = value;
            }
        }

        #endregion

        #region ICollection<KeyValuePair<string,InjectorType>> Members

        public void Add( KeyValuePair<string, InjectorType> item )
        {
            if( item.Key != item.Value.Name )
            {
                throw new ArgumentException( "The 'item.Key' parameter must match the Name property on the supplied RegistryEntry.", "item.Key" );
            }
            ( (ICollection<KeyValuePair<string, InjectorType>>)_dictionary ).Add( item );
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains( KeyValuePair<string, InjectorType> item )
        {
            return _dictionary.Contains( item );
        }

        public void CopyTo( KeyValuePair<string, InjectorType>[] array, int arrayIndex )
        {
            ( (ICollection<KeyValuePair<string, InjectorType>>)_dictionary ).CopyTo( array, arrayIndex );
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return ( (ICollection<KeyValuePair<string, InjectorType>>)_dictionary ).IsReadOnly; }
        }

        public bool Remove( KeyValuePair<string, InjectorType> item )
        {
            return ( (ICollection<KeyValuePair<string, InjectorType>>)_dictionary ).Remove( item );
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,InjectorType>> Members

        public IEnumerator<KeyValuePair<string, InjectorType>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        #endregion
    }
}

