using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Configuration;

namespace org.ncore.ServicedApi.Container
{
    // TODO: Should this be changed to a ConcurrentDictionary to ensure thread safety?  -JF
    public class KernelRegistry : IDictionary<string, RegistryEntry>
    {
        private Dictionary<string, RegistryEntry> _dictionary;

        public void Add( RegistryEntry value )
        {
            _dictionary.Add( value.Name, value );
        }

        public void Add( string name, RegistryEntry value )
        {
            if(name != value.Name)
            {
                throw new ArgumentException( "The 'name' parameter must match the Name property on the supplied RegistryEntry.", "name" );
            }
            _dictionary.Add( value.Name, value );
        }

        public KernelRegistry()
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
            lock(this)
            {
                _dictionary = new Dictionary<string, RegistryEntry>();

                KernelConfiguration configuration = (KernelConfiguration)ConfigurationManager.GetSection( "kernel" );
                foreach( TypeElement element in configuration.Types )
                {
                    RegistryEntry entry = new RegistryEntry()
                    {
                        Name = element.Name,
                        Assembly = element.Assembly,
                        TypeName = element.TypeName,
                        AllowSave = element.AllowSave,
                        Instance = null
                    };
                    this.Add( entry );
                }
            }
        }

        #region IDictionary<string,RegistryEntry> Members


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

        public bool TryGetValue( string key, out RegistryEntry value )
        {
            return _dictionary.TryGetValue( key, out value );
        }

        public ICollection<RegistryEntry> Values
        {
            get { return _dictionary.Values; }
        }

        public RegistryEntry this[ string key ]
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

        #region ICollection<KeyValuePair<string,RegistryEntry>> Members

        public void Add( KeyValuePair<string, RegistryEntry> item )
        {
            if( item.Key != item.Value.Name )
            {
                throw new ArgumentException( "The 'item.Key' parameter must match the Name property on the supplied RegistryEntry.", "item.Key" );
            }
            ((ICollection<KeyValuePair<string,RegistryEntry>>)_dictionary).Add( item );
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains( KeyValuePair<string, RegistryEntry> item )
        {
            return _dictionary.Contains( item );
        }

        public void CopyTo( KeyValuePair<string, RegistryEntry>[] array, int arrayIndex )
        {
            ((ICollection<KeyValuePair<string,RegistryEntry>>)_dictionary).CopyTo( array, arrayIndex );
        }

        public int Count
        {
            get { return _dictionary.Count; }
        }

        public bool IsReadOnly
        {
            get { return ( (ICollection<KeyValuePair<string, RegistryEntry>>)_dictionary ).IsReadOnly; }
        }

        public bool Remove( KeyValuePair<string, RegistryEntry> item )
        {
            return ( (ICollection<KeyValuePair<string, RegistryEntry>>)_dictionary ).Remove( item );
        }

        #endregion

        #region IEnumerable<KeyValuePair<string,RegistryEntry>> Members

        public IEnumerator<KeyValuePair<string, RegistryEntry>> GetEnumerator()
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
