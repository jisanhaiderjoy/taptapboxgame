using System.ComponentModel;
using PlayNoob.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace PlayNoob.ScriptableObjects {
    [System.Serializable]
    public struct ScalingData {
        public Scaling scaling;
    }

    [System.Serializable]
    public struct Scaling {
        public float constant1;
        public float constant2;
        public float constant3;
        public int circleCost;
    }
    
    /// <summary>
    /// To make sure nobody is able to Modify Important Game Data
    /// </summary>
    public interface IReadOnlyGameCache {
        float goldCount { get; }
        int level { get; }
        int circleCount { get; }
        int circleCost { get; }
        float upgradeCost { get; }
        float goldPerTap { get; }
    }
    
    public class GameCache : IReadOnlyGameCache {
        private float goldCount_;
        public float goldCount => goldCount_;
        
        private int level_ = 1;
        public int level => level_;
        
        public float goldPerTap => scalingConstant1_ * Mathf.Pow(level_, scalingConstant2_);
        public float upgradeCost => scalingConstant1_ * Mathf.Pow(scalingConstant3_, level);

        public int circleCount => circleCount_;
        private int circleCount_ = 0;
        public int circleCost => circleCostConstant_ * (int)Mathf.Pow(10, circleCount_);
        
        private UnityEvent<float> onGoldIncreased_;
        private UnityEvent onLevelUp_;
        private UnityEvent<int> onCircleLevelUp_;
        private UnityEvent onBuyCircle_;

        private float scalingConstant1_ = 5f;
        private float scalingConstant2_ = 2.1f;
        private float scalingConstant3_ = 1.08f;
        private int circleCostConstant_ = 100;

        public GameCache(Scaling scalingData) {
            level_ = PlayerPrefs.GetInt(Constant.PLAYER_LEVEL_KEY, 1);
            circleCount_ = PlayerPrefs.GetInt(Constant.CIRCLE_COUNT_KEY, 0);
            goldCount_ = PlayerPrefs.GetFloat(Constant.GOLD_COUNT_KEY, 0f);

            scalingConstant1_ = scalingData.constant1;
            scalingConstant2_ = scalingData.constant2;
            scalingConstant3_ = scalingData.constant3;
            circleCostConstant_ = scalingData.circleCost;
            
            onGoldIncreased_ = new UnityEvent<float>();
            onLevelUp_ = new UnityEvent();
            onCircleLevelUp_ = new UnityEvent<int>(); 
            onBuyCircle_ = new UnityEvent();
        }

        public void IncreaseGold() {
            //Increase Data
            goldCount_ += goldPerTap;
            PlayerPrefs.SetFloat(Constant.GOLD_COUNT_KEY, goldCount_);
            //Raise Event
            onGoldIncreased_.Invoke(goldCount_);
        }
        
        public void IncreaseGold(float diffGoldPerTap) {
            //Increase Data
            goldCount_ += diffGoldPerTap;
            PlayerPrefs.SetFloat(Constant.GOLD_COUNT_KEY, goldCount_);
            //Raise Event
            onGoldIncreased_.Invoke(goldCount_);
        }

        public void LevelUp() {
            //Increase Data
            goldCount_ -= upgradeCost;
            level_++;
            PlayerPrefs.SetFloat(Constant.GOLD_COUNT_KEY, goldCount_);
            PlayerPrefs.SetInt(Constant.PLAYER_LEVEL_KEY, level_);
            //Raise Event for change in Gold
            onGoldIncreased_.Invoke(goldCount_);
            onLevelUp_.Invoke();
        }

        public void LevelUpCircle(IReadOnlyCircleCache readOnlyCircleCache) {
            //Update Data
            goldCount_ -= readOnlyCircleCache.upgradeCost;
            PlayerPrefs.SetFloat(Constant.GOLD_COUNT_KEY, goldCount_);
            //Raise Event for change in Gold
            onGoldIncreased_.Invoke(goldCount_);
            onCircleLevelUp_.Invoke(readOnlyCircleCache.index);
        }
        
        public void BuyCircle() {
            //Update Data
            goldCount_ -= circleCost;
            circleCount_++;
            PlayerPrefs.SetFloat(Constant.GOLD_COUNT_KEY, goldCount_);
            PlayerPrefs.SetInt(Constant.CIRCLE_COUNT_KEY, circleCount_);
            //Raise Event for change in Gold
            onGoldIncreased_.Invoke(goldCount_);
            onBuyCircle_.Invoke();
        }

        public void RegisterOnGoldIncrease(UnityAction<float> func) {
            onGoldIncreased_.AddListener(func);
        }

        public void UnRegisterOnGoldIncrease(UnityAction<float> func) {
            onGoldIncreased_.AddListener(func);
        }
        
        public void RegisterOnCircleLevelUp(UnityAction<int> func) {
            onCircleLevelUp_.AddListener(func);
        }

        public void UnRegisterOnCircleLevelUp(UnityAction<int> func) {
            onCircleLevelUp_.AddListener(func);
        }
        
        public void RegisterOnLevelUp(UnityAction func) {
            onLevelUp_.AddListener(func);
        }

        public void UnRegisterOnLevelUp(UnityAction func) {
            onLevelUp_.AddListener(func);
        }
        
        public void RegisterOnBuyCircle(UnityAction func) {
            onBuyCircle_.AddListener(func);
        }

        public void UnRegisterOnBuyCircle(UnityAction func) {
            onBuyCircle_.AddListener(func);
        }
    }
}