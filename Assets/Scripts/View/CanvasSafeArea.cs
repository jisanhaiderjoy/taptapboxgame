using UnityEngine;

namespace PlayNoob.View {
    public class CanvasSafeArea : MonoBehaviour {
        private RectTransform rectTransform_;
        private Rect safeArea_;
        private Vector2 minAnchor_;
        private Vector2 maxAnchor_;
        
        void Awake() {
            rectTransform_ = GetComponent<RectTransform>();
            safeArea_ = Screen.safeArea;
            minAnchor_ = safeArea_.position;
            maxAnchor_ = minAnchor_ + safeArea_.size;

            minAnchor_.x /= Screen.width;
            minAnchor_.y /= Screen.height;
            maxAnchor_.x /= Screen.width;
            maxAnchor_.y /= Screen.height;

            rectTransform_.anchorMin = minAnchor_;
            rectTransform_.anchorMax = maxAnchor_;
        }
    }
}