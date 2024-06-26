using PlayNoob.ScriptableObjects;
using PlayNoob.ScriptableObjects.GameEvents;
using PlayNoob.ScriptableObjects.RuntimeData;
using UnityEngine;

namespace PlayNoob.View {
    public class UpgradeablesView : MonoBehaviour {
        [Header("Game Cache")]
        [SerializeField] private ReadOnlyGameCacheAnchor gameCacheAnchor_;

        [Header("View Refs")] 
        [SerializeField] private CanvasGroup canvasGroup_;
        [SerializeField] private UpgradeCard upgradeLevel_;
        [SerializeField] private UpgradeCard buyCircleHelpers_;
        
        [Header("Game Events")]
        [SerializeField] private VoidGameEventSO onLevelUpCompleted_;
        [SerializeField] private VoidGameEventSO onCircleBuyCompleted_;

        private void OnEnable() {
            gameCacheAnchor_.onAnchorProvided += OnGameCacheAnchorProvided;
        }

        private void OnDisable() {
            gameCacheAnchor_.onAnchorProvided -= OnGameCacheAnchorProvided;
        }

        private void OnDestroy() {
            onLevelUpCompleted_.UnRegisterFunc(UpdateLevelUpView);
            onCircleBuyCompleted_.UnRegisterFunc(UpdateBuyCircleView);
        }
        
        public void Start() {
            if (gameCacheAnchor_.isSet) {
                ToggleUpgradeablesUI(true);
                UpdateLevelUpView();
                UpdateBuyCircleView();
            }
            else {
                ToggleUpgradeablesUI(false);
            }
            
            onLevelUpCompleted_.RegisterFunc(UpdateLevelUpView);
            onCircleBuyCompleted_.RegisterFunc(UpdateBuyCircleView);
        }
        
        private void OnGameCacheAnchorProvided(IReadOnlyGameCache readOnlyGameCache) {
            ToggleUpgradeablesUI(true);
            UpdateLevelUpView();
            UpdateBuyCircleView();
        }

        private void ToggleUpgradeablesUI(bool isEnabled) {
            if (isEnabled) {
                canvasGroup_.alpha = 1f;
                canvasGroup_.interactable = true;
            }
            else {
                canvasGroup_.alpha = 0f;
                canvasGroup_.interactable = false;
            }
        }

        /// <summary>
        /// Updates Purchase Menu Based on GameCache Data
        /// Updates OnLevelUp
        /// </summary>
        private void UpdateLevelUpView() {
            if (gameCacheAnchor_.isSet) {
                upgradeLevel_.UpdateView("Level Up", ((int)gameCacheAnchor_.Value.upgradeCost).ToString());
            }
        }
        
        /// <summary>
        /// Updates Purchase Menu Based on GameCache Data
        /// Updates OnLevelUp
        /// </summary>
        private void UpdateBuyCircleView() {
            if (gameCacheAnchor_.isSet) {
                IReadOnlyGameCache readOnlyGameCache = gameCacheAnchor_.Value;

                if (readOnlyGameCache.circleCount >= 5) {
                    buyCircleHelpers_.ToggleInteractable(false);
                    buyCircleHelpers_.UpdateView("Circle Maxed!", "--");
                }
                else {
                    buyCircleHelpers_.UpdateView("Buy Circle", readOnlyGameCache.circleCost.ToString());
                }
            }
        }
    }
}