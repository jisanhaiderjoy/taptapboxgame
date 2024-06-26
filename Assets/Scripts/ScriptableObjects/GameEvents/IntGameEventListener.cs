using UnityEngine;
using UnityEngine.Events;

namespace PlayNoob.ScriptableObjects.GameEvents {
	public class IntGameEventListener : MonoBehaviour {
		[SerializeField] private IntGameEventSO channel_ = default;
		[SerializeField] private UnityEvent<int> OnEventRaised;

		private void OnEnable() {
			if (channel_ != null)
				channel_.RegisterFunc(Respond);
		}

		private void OnDisable() {
			if (channel_ != null)
				channel_.UnRegisterFunc(Respond);
		}

		private void Respond(int value) {
			OnEventRaised?.Invoke(value);
		}
	}
}