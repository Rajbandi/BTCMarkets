using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace BtcMarkets.Wallet.Models
{
    public class Grouping<K, T> : ObservableCollection<T>
    {
        public K Key { get; private set; }

        public Grouping(K key)
        {
            Key = key;
        }
        public Grouping(K key, IEnumerable<T> items)
        {
            Key = key;
            foreach (var item in items)
                this.Items.Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            foreach (var item in items)
                this.Items.Add(item);
        }
    }
}
