using PlayNoob.ScriptableObjects.RuntimeData;
using UnityEngine;

namespace PlayNoob {
    public class TransformToRuntimeDataSet : MonoBehaviour {
        [SerializeField] private TransformArrayAnchorSO runtimeTransformsArrayAnchorSo;
        [SerializeField] private Transform[] transforms_;

        void Awake() {
            runtimeTransformsArrayAnchorSo.InitRange(transforms_);
        }

        private void OnDestroy() {
            runtimeTransformsArrayAnchorSo.Unset();
        }
    }
}