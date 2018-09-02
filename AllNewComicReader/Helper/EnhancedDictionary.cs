using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AllNewComicReader.Helper
{
    class EnhancedDictionary<Tkey, Tvalue> : Dictionary<Tkey, Tvalue>
    {
        LinkedList<Tkey> iInsertionOrder;

        public EnhancedDictionary()
        {
            iInsertionOrder = new LinkedList<Tkey>();
        }

        public new EnhancedDictionary<Tkey, Tvalue> Add(Tkey key, Tvalue value)
        {
            base.Add(key,value);
            iInsertionOrder.AddLast(key);

            return this;
        }

        public void RemoveFromFront(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (iInsertionOrder.Count >= count)
                {
                    Remove(iInsertionOrder.First.Value);
                    iInsertionOrder.RemoveFirst();
                }
            }
        }
        
    }
}
