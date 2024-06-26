using UnityEngine;
using UnityEngine.Events;

namespace PlayNoob.ScriptableObjects.GameEvents {
	[CreateAssetMenu(menuName = "Scriptable Objects/Events/RaycastHit Event Channel")]
	public class RaycastHitGameEventSO : DescriptionBaseSO {
		private UnityEvent<RaycastHit> onEventRaised = new UnityEvent<RaycastHit>();
		
		public void RaiseEvent(RaycastHit value) {
			onEventRaised?.Invoke(value);
		}

		public void RegisterFunc(UnityAction<RaycastHit> func) {
			onEventRaised.AddListener(func);
		}

		public void UnRegisterFunc(UnityAction<RaycastHit> func) {
			onEventRaised.RemoveListener(func);
		}
	}
}
