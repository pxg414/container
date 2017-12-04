namespace Unity.Container.Storage
{
    public class LinkedMap<TKey, TValue> : LinkedNode<TKey, TValue>,
                                           IMap<TKey, TValue>
    {
        #region Constructors

        public LinkedMap()
        {
        }

        public LinkedMap(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }


        #endregion


        #region IMap

        public virtual TValue this[TKey key]
        {
            get
            {
                for (var node = (LinkedNode<TKey, TValue>)this; node != null; node = node.Next)
                {
                    if (Equals(node.Key, key))
                        return node.Value;
                }

                return default(TValue);
            }
            set
            {
                if (null == Key)
                {
                    Key = key;
                    Value = value;
                    return;
                }

                for (var node = (LinkedNode<TKey, TValue>)this; node != null; node = node.Next)
                {
                    if (Equals(node.Key, key))
                    {
                        // Found it
                        node.Value = value;
                        return;
                    }
                }

                Next = new LinkedNode<TKey, TValue>
                {
                    Key = key,
                    Next = Next,
                    Value = value
                };
            }
        }

        #endregion
    }
}
