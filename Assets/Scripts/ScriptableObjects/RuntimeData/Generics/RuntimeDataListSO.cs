using System.Collections.Generic;
using UnityEngine;

namespace PlayNoob.ScriptableObjects.RuntimeData {
    public abstract class RuntimeDataListSO<T> : DescriptionBaseSO {
        public List<T> data = new List<T>();

        public void Add(T item) {
            if (!data.Contains(item))
                data.Add(item);
        }
        
        public void InitRange(T[] item) {
            data = new List<T>(item);
        }

        public void Remove(T item) {
            if (data.Contains(item))
                data.Remove(item);
        }
    }
}
