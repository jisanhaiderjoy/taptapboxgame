using PlayNoob.Utils;
using UnityEngine;

namespace PlayNoob.ScriptableObjects.RuntimeData {
    [CreateAssetMenu(menuName = "Scriptable Objects/Runtime Data/Audio/Audio Channel Types")]
    public class AudioChannelTypesSO : DescriptionBaseSO {
        public string AudioChannelName = Constant.DEFAULT;
    }
}
