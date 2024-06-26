using UnityEngine;
using UnityEngine.Events;

namespace PlayNoob.ScriptableObjects.GameEvents {
	[CreateAssetMenu(menuName = "Scriptable Objects/Events/Float Event Channel")]
	public class FloatGameEventSO : DescriptionBaseSO {
		private UnityEvent<float> onEventRaised = new UnityEvent<float>();
		
		public void RaiseEvent(float value) {
			onEventRaised?.Invoke(value);
		}

		public void RegisterFunc(UnityAction<float> func) {
			onEventRaised.AddListener(func);
		}

		public void UnRegisterFunc(UnityAction<float> func) {
			onEventRaised.RemoveListener(func);
		}
	}
}
