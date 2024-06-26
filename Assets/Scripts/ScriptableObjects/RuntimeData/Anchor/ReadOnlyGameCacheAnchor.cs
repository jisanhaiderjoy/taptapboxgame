using UnityEngine;

namespace PlayNoob.ScriptableObjects.RuntimeData {
    [CreateAssetMenu(menuName = "Scriptable Objects/Runtime Anchors/ReadOnly GameCache")]
    public class ReadOnlyGameCacheAnchor : RuntimeAnchorBase<IReadOnlyGameCache> { }
}