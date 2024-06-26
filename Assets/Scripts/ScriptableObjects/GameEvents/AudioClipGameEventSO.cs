using PlayNoob.ScriptableObjects.RuntimeData;
using UnityEngine;
using UnityEngine.Events;

namespace PlayNoob.ScriptableObjects.GameEvents {
    [System.Serializable]
    public struct PlayAudioClipData {
        public AudioClip audioClip;
        [Range(0, 1)] public float volume;
        public AudioChannelTypesSO audioChannelTypesSo;
    }
    
    [CreateAssetMenu(menuName = "Scriptable Objects/Events/AudioClip Event Channel")]
    public class AudioClipGameEventSO : DescriptionBaseSO {
        private UnityEvent<PlayAudioClipData> OnEventRaised = new UnityEvent<PlayAudioClipData>();
		
        public void RaiseEvent(PlayAudioClipData audioClipData) {
            OnEventRaised?.Invoke(audioClipData);
        }

        public void RegisterFunc(UnityAction<PlayAudioClipData> func) {
            OnEventRaised.AddListener(func);
        }

        public void UnRegisterFunc(UnityAction<PlayAudioClipData> func) {
            OnEventRaised.RemoveListener(func);
        }
    }
}

