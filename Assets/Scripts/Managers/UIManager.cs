using PlayNoob.ScriptableObjects;
using PlayNoob.ScriptableObjects.GameEvents;
using PlayNoob.ScriptableObjects.RuntimeData;
using UnityEngine;

namespace PlayNoob.Controller {
    public class UIManager : MonoBehaviour {
        [Header("Game Cache")]
        [SerializeField] private ReadOnlyGameCacheAnchor gameCacheAnchor_;
        [SerializeField] private ReadOnlyCircleCacheArrayAnchor readOnlyCircleCacheArrayAnchor_;

        [Header("Game Events")] 
        [SerializeField] private VoidGameEventSO onLevelUp_;
        [SerializeField] private VoidGameEventSO onCircleBuy_;
        [SerializeField] private IntGameEventSO onCircleLevelUpForwardedSO_;
        [SerializeField] private AudioClipGameEventSO onPlayAudioClip_;

        [Header("Audio Clip Data")] 
        [SerializeField] private PlayAudioClipData clipDataOnDenialClicks;
        [SerializeField] private PlayAudioClipData clipDataOnSuccessClicks;

        /// <summary>
        /// Check if User is eligible for Level up or not
        /// </summary>
        public void TryLevelUp() {
            if (gameCacheAnchor_.isSet) {
                IReadOnlyGameCache readOnlyGameCache = gameCacheAnchor_.Value;
                if (readOnlyGameCache.goldCount >= readOnlyGameCache.upgradeCost) {
                    onLevelUp_.RaiseEvent();
                    //Play Audio
                    onPlayAudioClip_.RaiseEvent(clipDataOnSuccessClicks);
                }
                else {
                    //Play Audio
                    onPlayAudioClip_.RaiseEvent(clipDataOnDenialClicks);
                }
            } else {
                //Play Audio
                onPlayAudioClip_.RaiseEvent(clipDataOnDenialClicks);
            }
        }

        /// <summary>
        /// Check if User is eligible for Level up or not To make a New Circle Helper Purchase
        /// </summary>
        public void TryBuyCircles() {
            if (gameCacheAnchor_.isSet) {
                IReadOnlyGameCache readOnlyGameCache = gameCacheAnchor_.Value;
                if (readOnlyGameCache.goldCount >= readOnlyGameCache.circleCost) {
                    onCircleBuy_.RaiseEvent();
                    //Play Audio
                    onPlayAudioClip_.RaiseEvent(clipDataOnSuccessClicks);
                }
                else {
                    //Play Audio
                    onPlayAudioClip_.RaiseEvent(clipDataOnDenialClicks);
                }
            }
            else {
                //Play Audio
                onPlayAudioClip_.RaiseEvent(clipDataOnDenialClicks);
            }
        }
        
        /// <summary>
        /// Check if the User is Eligible to Level Up The Selected Circle
        /// </summary>
        /// <param name="helperIndex"></param>
        public void TryLevelUpACircle(int helperIndex) {
            if (gameCacheAnchor_.isSet && readOnlyCircleCacheArrayAnchor_.isSet) {
                IReadOnlyGameCache readOnlyGameCache = gameCacheAnchor_.Value;
                IReadOnlyCircleCache readOnlyCircleCache = readOnlyCircleCacheArrayAnchor_.Value[helperIndex];
                if (readOnlyGameCache.goldCount >= readOnlyCircleCache.upgradeCost) {
                    onCircleLevelUpForwardedSO_.RaiseEvent(helperIndex);
                    //Play Audio
                    onPlayAudioClip_.RaiseEvent(clipDataOnSuccessClicks);
                }
                else {
                    //Play Audio
                    onPlayAudioClip_.RaiseEvent(clipDataOnDenialClicks);
                }
            }
            else {
                //Play Audio
                onPlayAudioClip_.RaiseEvent(clipDataOnDenialClicks);
            }
        }
    }
}