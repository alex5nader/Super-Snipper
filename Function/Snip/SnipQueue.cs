using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using Function.Util;

namespace Function.Snip {

    public class SnipQueue : IReadOnlyCollection<Snip>, INotifyCollectionChanged {

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        private readonly List<Snip> _snips;

        public int Capacity { get; set; }
        public int Count => _snips.Count;

        public SnipQueue(int capacity) {
            if (capacity < 0)
                throw new ArgumentOutOfRangeException($"parameter capacity cannot be negative (capacity is {capacity})");
            Capacity = capacity;
            _snips = new List<Snip>();
        }

        public Snip this[int index] {
            get => _snips[index];
            set => _snips[index] = value;
        }

        public void Enqueue(Snip item) {
            item.RemoveCommand = new RelayCommand(
                param => Remove(item),
                param => true
            );
            _snips.Add(item);

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));

            if (Count > Capacity) {
                Dequeue();
            }
        }

        public void Dequeue() {
            var removed = _snips.First();
            _snips.Remove(_snips.First());

            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removed, 0));

            removed.Dispose();
        }

        public void Remove(Snip item) {
            var index = _snips.IndexOf(item);
            CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
            _snips.Remove(item);


            item.Dispose();
        }

        public void ForEach(Action<Snip> operation) {
            _snips.ForEach(operation);
        }

        public IEnumerator<Snip> GetEnumerator() {
            return _snips.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}
