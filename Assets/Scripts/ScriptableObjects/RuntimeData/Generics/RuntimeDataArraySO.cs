using UnityEngine;

namespace PlayNoob.ScriptableObjects.RuntimeData {
	public class RuntimeDataArraySO<T> : DescriptionBaseSO {
		public T[] data;
		
		/// <summary>
		///	Helper function
		/// Get a Random Item from the Data Set
		/// </summary>
		/// <returns></returns>
		public T GetRandomItem() {
			return data[Random.Range(12324, 1999999) % data.Length];
		}
	}
}
