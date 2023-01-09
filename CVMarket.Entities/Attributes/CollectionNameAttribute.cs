using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVMarket.Entities.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class CollectionNameAttribute : Attribute
    {
        private string _collectionName;

        public string CollectionName { get => _collectionName; }

        public CollectionNameAttribute(string collectionName)
        {
            _collectionName = collectionName;
        }
    }
}
