using PlayNoob.ScriptableObjects.GameEvents;
using PlayNoob.Utils;
using PlayNoob.View;
using UnityEngine;

namespace PlayNoob.Controller {
    public class InputManager : MonoBehaviour {
        [Header("Game Events")] 
        [SerializeField] private RaycastHitGameEventSO onEnemyHitByTapSO_;

        private Camera mainCam;

        private void Start() {
            mainCam = Camera.main;
        }

        void Update() {
            if (Input.touchCount > 0) {
                Touch touch = Input.GetTouch(0);
                if (touch.phase == TouchPhase.Began) {
                    Ray raycast = mainCam.ScreenPointToRay(touch.position);
                    RaycastHit raycastHit;
                    
                    if (Physics.Raycast(raycast, out raycastHit)) {
                        //Check if enemy Got Hit
                        if (raycastHit.collider.CompareTag(Constant.ENEMY_TAG)) {
                            onEnemyHitByTapSO_.RaiseEvent(raycastHit);
                        } 
                        //Check if Helper Got Hit
                        else if (raycastHit.collider.CompareTag(Constant.HELPER_TAG)) {
                            //This Fires an Event
                            raycastHit.collider.GetComponent<IOnCircleUnitSelected>().OnCircleUnitSelected();
                        }
                    }
                }
            }
        }
    }
}