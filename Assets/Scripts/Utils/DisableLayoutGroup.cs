using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace PlayNoob.Utils {
    public class DisableLayoutGroup : MonoBehaviour {
        [SerializeField] private Component[] objectsToDisable;
        [SerializeField] private float delay_ = 1f;
        void Start() {
            Invoke(nameof(DisableComponentAfterDelay), delay_);
        }

        void DisableComponentAfterDelay() {
            for (int i = 0; i < objectsToDisable.Length; i++) {
                Destroy(objectsToDisable[i]);
            }
        }
    }
}