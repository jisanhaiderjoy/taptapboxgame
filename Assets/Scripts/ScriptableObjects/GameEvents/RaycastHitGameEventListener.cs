using UnityEngine;
using UnityEngine.Events;

namespace PlayNoob.ScriptableObjects.GameEvents {
	public class RaycastHitGameEventListener : MonoBehaviour {
		[SerializeField] private RaycastHitGameEventSO channel_ = default;
		[SerializeField] private UnityEvent<RaycastHit> onEventRaised;

		private void OnEnable() {
			if (channel_ != null)
				channel_.RegisterFunc(Respond);
		}

		private void OnDisable() {
			if (channel_ != null)
				channel_.UnRegisterFunc(Respond);
		}

		private void Respond(RaycastHit value) {
			onEventRaised?.Invoke(value);
		}
	}
}