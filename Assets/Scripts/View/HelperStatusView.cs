using PlayNoob.ScriptableObjects;
using PlayNoob.ScriptableObjects.GameEvents;
using PlayNoob.ScriptableObjects.RuntimeData;
using TMPro;
using UnityEngine;

namespace PlayNoob.View {
    /// <summary>
    /// Helper Upgrade Menu Class
    /// </summary>
    public class HelperStatusView : MonoBehaviour {
        [Header("Cache Data")]
        [SerializeField] private ReadOnlyCircleCacheArrayAnchor readOnlyCircleCacheArrayAnchor_;

        [Header("Game Events")] 
        [SerializeField] private IntGameEventSO onHelperSelectedSO_;
        [SerializeField] private IntGameEventSO onCircleLevelUpRequestedSO_;
        
        [Header("View")]
        [SerializeField] private CanvasGroup menuCanvasGroup_;

        [SerializeField] private TextMeshProUGUI HeaderTitleTxt_;
        [SerializeField] private TextMeshProUGUI BodyTxt_;
        [SerializeField] private TextMeshProUGUI UpgradeCostTxt_;
        
        private int helperIndex_ = -1;
        
        // Start is called before the first frame update
        void Start() {
            onHelperSelectedSO_.RegisterFunc(OnHelperSelected);
        }

        private void OnDestroy() {
            onHelperSelectedSO_.UnRegisterFunc(OnHelperSelected);
        }

        /// <summary>
        /// Called by an Event, To Show the Helper Upgrade Menu
        /// </summary>
        /// <param name="helperIndex"></param>
        private void OnHelperSelected(int helperIndex) {
            if (readOnlyCircleCacheArrayAnchor_.isSet) {
                helperIndex_ = helperIndex;
                IReadOnlyCircleCache readOnlyCircleCache = readOnlyCircleCacheArrayAnchor_.Value[helperIndex];
                HeaderTitleTxt_.text = $"Circle Helper {helperIndex}";
                BodyTxt_.text = $"Level : {readOnlyCircleCache.level}\nGoldPerHit : {(int)readOnlyCircleCache.goldPerHit}";
                UpgradeCostTxt_.text = ((int)readOnlyCircleCache.upgradeCost).ToString();
                
                ToggleMenu(true);
            }
        }

        /// <summary>
        /// Open/Close the Menu
        /// </summary>
        /// <param name="isEnabled"></param>
        public void ToggleMenu(bool isEnabled) {
            if (isEnabled) {
                menuCanvasGroup_.alpha = 1f;
                menuCanvasGroup_.interactable = true;
            }
            else {
                menuCanvasGroup_.alpha = 0f;
                menuCanvasGroup_.interactable = false;
            }
        }

        /// <summary>
        /// Request a Level Up to a ManagerClass to Decide if the Request Can Be Executed or not
        /// </summary>
        public void OnTryLevelUp() {
            onCircleLevelUpRequestedSO_.RaiseEvent(helperIndex_); //Request for Upgrade
            OnHelperSelected(helperIndex_); //Refresh UI
        }
    }
}