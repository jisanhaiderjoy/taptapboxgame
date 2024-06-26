using UnityEngine;
using UnityEngine.Events;

namespace PlayNoob.ScriptableObjects.GameEvents {
	[CreateAssetMenu(menuName = "Scriptable Objects/Events/Int Event Channel")]
	public class IntGameEventSO : DescriptionBaseSO {
		private UnityEvent<int> OnEventRaised = new UnityEvent<int>();
		
		public void RaiseEvent(int value) {
			OnEventRaised?.Invoke(value);
		}

		public void RegisterFunc(UnityAction<int> func) {
			OnEventRaised.AddListener(func);
		}

		public void UnRegisterFunc(UnityAction<int> func) {
			OnEventRaised.RemoveListener(func);
		}
	}
}
