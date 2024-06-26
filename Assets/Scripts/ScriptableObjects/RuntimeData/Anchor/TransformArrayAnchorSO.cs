using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayNoob.ScriptableObjects.RuntimeData {
    [CreateAssetMenu(menuName = "Scriptable Objects/Runtime Data/Transforms Data Set")]
    public class TransformArrayAnchorSO : RuntimeAnchorBase<Transform[]> {
        private int lastRandIndex_ = 0;
        public void InitRange(Transform[] items) {
            Provide(items);
        }
        
        /// <summary>
        ///	Helper function
        /// Get a Unique Random Item from the Data Set
        /// </summary>
        /// <returns></returns>
        public Transform GetRandomItem() {
            int newRand = Random.Range(12324, 1999999) % _value.Length;
            if (newRand == lastRandIndex_) {
                newRand = (newRand + 1) % _value.Length;
            }

            lastRandIndex_ = newRand;
            return _value[newRand];
        }
        
        /// <summary>
        ///	Helper function
        /// Get a Unique Random Item from the Data Set
        /// </summary>
        /// <returns></returns>
        public Transform GetNextItem() {
            lastRandIndex_ = (lastRandIndex_ + 1) % _value.Length;
            return _value[lastRandIndex_];
        }
    }
}