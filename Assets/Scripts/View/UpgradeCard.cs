using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace PlayNoob.View {
    public class UpgradeCard : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI title_;
        [SerializeField] private TextMeshProUGUI priceText_;

        [SerializeField] private Button thisButton_;

        private void Start() {
            if (thisButton_ == null) {
                thisButton_ = GetComponent<Button>();
            }
        }

        public void UpdateView(string title, string price) {
            title_.text = title;
            priceText_.text = price;
        }

        public void ToggleInteractable(bool isEnabled) {
            thisButton_.interactable = isEnabled;
        }
    }
}