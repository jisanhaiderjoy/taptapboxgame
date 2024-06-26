using UnityEngine;
using UnityEngine.Events;

namespace PlayNoob.ScriptableObjects.GameEvents {
	public class FloatGameEventListener : MonoBehaviour {
		[SerializeField] private FloatGameEventSO channel_ = default;
		[SerializeField] private UnityEvent<float> onEventRaised;

		private void OnEnable() {
			if (channel_ != null)
				channel_.RegisterFunc(Respond);
		}

		private void OnDisable() {
			if (channel_ != null)
				channel_.UnRegisterFunc(Respond);
		}

		private void Respond(float value) {
			onEventRaised?.Invoke(value);
		}
	}
}