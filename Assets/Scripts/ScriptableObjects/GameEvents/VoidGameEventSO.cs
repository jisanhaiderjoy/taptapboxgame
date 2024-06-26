using UnityEngine;
using UnityEngine.Events;

namespace PlayNoob.ScriptableObjects.GameEvents {
	[CreateAssetMenu(menuName = "Scriptable Objects/Events/Void Event Channel")]
	public class VoidGameEventSO : DescriptionBaseSO {
		private UnityEvent OnEventRaised = new UnityEvent();
		
		public void RaiseEvent() {
			OnEventRaised?.Invoke();
		}

		public void RegisterFunc(UnityAction func) {
			OnEventRaised.AddListener(func);
		}

		public void UnRegisterFunc(UnityAction func) {
			OnEventRaised.RemoveListener(func);
		}
	}
}
