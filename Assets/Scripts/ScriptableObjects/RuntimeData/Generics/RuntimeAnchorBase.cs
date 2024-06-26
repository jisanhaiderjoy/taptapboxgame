using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace PlayNoob.ScriptableObjects.RuntimeData {
    public class RuntimeAnchorBase<T> : DescriptionBaseSO /*where T : UnityEngine.Object*/ {
        public UnityAction<T> onAnchorProvided;

        [Header("Debug")]
    #if UNITY_EDITOR
        [ReadOnly] 
    #endif
        public bool isSet = false; // Any script can check if the transform is null before using it, by just checking this bool

    #if UNITY_EDITOR
        [ReadOnly] 
    #endif
        [SerializeField] protected internal T _value;

        public T Value => _value;

        public void Provide(T value) {
            if (value == null) {
                Debug.LogError("A null value was provided to the " + this.name + " runtime anchor.");
                return;
            }

            _value = value;
            isSet = true;

            onAnchorProvided?.Invoke(_value);
        }

        public void Unset() {
            _value = default;
            isSet = false;
        }

        private void OnDisable() {
            Unset();
        }
    }
}