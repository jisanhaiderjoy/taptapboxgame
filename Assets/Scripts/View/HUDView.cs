using System;
using PlayNoob.ScriptableObjects;
using PlayNoob.ScriptableObjects.GameEvents;
using PlayNoob.ScriptableObjects.RuntimeData;
using UnityEngine;

namespace PlayNoob.View {
    public class HUDView : MonoBehaviour {
        [Header("Game Cache")]
        [SerializeField] private ReadOnlyGameCacheAnchor gameCacheAnchor_;
        
        [Header("Gold View")]
        [SerializeField] private TMPro.TextMeshProUGUI goldText_;
        
        [Header("Game Events")]
        [SerializeField] private FloatGameEventSO onGoldIncreased_;

        private void OnEnable() {
            gameCacheAnchor_.onAnchorProvided += OnGameCacheAnchorProvided;
        }

        private void OnDisable() {
            gameCacheAnchor_.onAnchorProvided -= OnGameCacheAnchorProvided;
        }

        // Start is called before the first frame update
        private void Start() {
            onGoldIncreased_.RegisterFunc(UpdateGoldView);

            //Update Value On Start
            if (gameCacheAnchor_.isSet) {
                UpdateGoldView(gameCacheAnchor_.Value.goldCount);
            }
            else {
                goldText_.text = "";
            }
        }

        private void OnDestroy() {
            onGoldIncreased_.UnRegisterFunc(UpdateGoldView);
        }
        
        private void OnGameCacheAnchorProvided(IReadOnlyGameCache readOnlyGameCache) {
            UpdateGoldView(readOnlyGameCache.goldCount);
        }

        /// <summary>
        /// On Gold Amount Increase
        /// </summary>
        /// <param name="totalGold"></param>
        private void UpdateGoldView(float totalGold) {
            goldText_.text = ((int)totalGold).ToString(); //TODO: Move To View Class
        }
    }
}