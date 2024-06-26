using System;
using UnityEngine;
using UnityEngine.Events;

namespace PlayNoob.Utils {
    public class MeshButton : MonoBehaviour {
        [SerializeField] private UnityEvent<RaycastHit> onMouseDown_;
        private Camera mainCam;

        private void Start() {
            mainCam = Camera.main;
        }

        private void OnMouseDown() {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000)) {
                onMouseDown_.Invoke(hit);
            }
        }
    }
}