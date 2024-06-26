using UnityEngine;
using UnityEngine.Pool;

namespace PlayNoob.ScriptableObjects.RuntimeData {
    [CreateAssetMenu(menuName = "Scriptable Objects/Runtime Data/GameObjects Data Set")]
    public class RuntimeDataGameObjectsSO : RuntimeDataArraySO<GameObject> { }
}