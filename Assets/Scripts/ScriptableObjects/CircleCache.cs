using PlayNoob.Utils;
using UnityEngine;

namespace PlayNoob.ScriptableObjects {
	public interface IReadOnlyCircleCache {
		int index { get; }
		int level { get; }
		float goldPerHit { get; }
		float upgradeCost { get; }
	}
	
	[System.Serializable]
	public class CircleCache : IReadOnlyCircleCache {
		private int index_ = -1;
		public int index => index_;
		
		private int level_ = 1;
		public int level => level_;

		public float goldPerHit => scalingConstant1_ * Mathf.Pow(level_, scalingConstant2_);
		public float upgradeCost => scalingConstant1_ * Mathf.Pow(scalingConstant3_, level);

		private string cacheKey_ = "";
		
		private float scalingConstant1_ = 5f;
		private float scalingConstant2_ = 2.1f;
		private float scalingConstant3_ = 1.08f;

		public CircleCache(int index, Scaling scalingData) {
			index_ = index;
			cacheKey_ = Constant.CIRCLE_LEVEL_KEY + index;
			level_ = PlayerPrefs.GetInt(cacheKey_, 1);
			
			scalingConstant1_ = scalingData.constant1;
			scalingConstant2_ = scalingData.constant2;
			scalingConstant3_ = scalingData.constant3;
		}
		
		public void LevelUpCircle() {
			//Increase Data
			level_++;
			PlayerPrefs.SetInt(cacheKey_, level_);
		}
	}
}
