using UnityEngine;
using UnityEngine.Events;

namespace PlayNoob.ScriptableObjects.GameEvents {
	public class VoidGameEventListener : MonoBehaviour {
		[SerializeField] private VoidGameEventSO channel_ = default;
		[SerializeField] private UnityEvent OnEventRaised;
		[SerializeField] private float raiseDelay_ = 0f;

		private void OnEnable() {
			if (channel_ != null)
				channel_.RegisterFunc(Respond);
		}

		private void OnDisable() {
			if (channel_ != null)
				channel_.UnRegisterFunc(Respond);
		}

		private void Respond() {
			if (raiseDelay_ > 0) {
				Invoke(nameof(DelayedRespond), raiseDelay_);
			}
			else {
				OnEventRaised?.Invoke();
			}
		}

		private void DelayedRespond() {
			OnEventRaised?.Invoke();
		}
		
		private void OnDestroy() {
			CancelInvoke();
		}
	}
}