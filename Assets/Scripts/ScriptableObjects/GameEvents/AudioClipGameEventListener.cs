using UnityEngine;
using UnityEngine.Events;

namespace PlayNoob.ScriptableObjects.GameEvents {
    public class AudioClipGameEventListener : MonoBehaviour {
        [SerializeField] private AudioClipGameEventSO channel_ = default;
        [SerializeField] private UnityEvent<PlayAudioClipData> OnEventRaised;
        [SerializeField] private float raiseDelay_ = 0f;

        private PlayAudioClipData audioClip_;

        private void OnEnable() {
            if (channel_ != null)
                channel_.RegisterFunc(Respond);
        }

        private void OnDisable() {
            if (channel_ != null)
                channel_.UnRegisterFunc(Respond);
        }

        private void Respond(PlayAudioClipData audioClipData) {
            if (raiseDelay_ > 0) {
                audioClip_ = audioClipData;
                Invoke(nameof(DelayedRespond), raiseDelay_);
            }
            else {
                OnEventRaised?.Invoke(audioClipData);
            }
        }

        private void DelayedRespond() {
            OnEventRaised?.Invoke(audioClip_);
        }
		
        private void OnDestroy() {
            CancelInvoke();
        }
    }
}
