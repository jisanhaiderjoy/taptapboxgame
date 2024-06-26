using System;
using System.Collections;
using System.Collections.Generic;
using PlayNoob.ScriptableObjects.RuntimeData;
using PlayNoob.ScriptableObjects;
using PlayNoob.ScriptableObjects.GameEvents;
using TMPro;
using UnityEngine;

namespace PlayNoob.View {
    public interface IOnCircleUnitSelected {
        void OnCircleUnitSelected();
    }
    public class CircleUnitView : MonoBehaviour, IOnCircleUnitSelected {
        [Header("Game Events")]
        [SerializeField] private RaycastHitGameEventSO onRaycastEnemyHit_;
        [SerializeField] private IntGameEventSO onIntHelperSelectedSO_;
        
        [Space(6)]
        [SerializeField] private Transform bulletTrail_;
        [SerializeField] private float shootSpeed_ = 20f;
        
        [Space(6)]
        [SerializeField] private float textSpeed_ = 3f;

        private IReadOnlyCircleCache readOnlyCircleCache_;
        
        private Transform enemyUnit_;
        private Transform thisTransform_;

        private Vector3 enemyPos_;
        private Vector3 initPos_;

        private Transform hitTextTransform_;
        private TextMeshPro hitText_;
        
        private Vector3 textInitOffset_ = new Vector3(0f, 1f, 0f);
        private Vector3 textTargetOffset_ = new Vector3(0f, 2f, 0f);
        
        [SerializeField] private Vector3 textScaleInit_ = new Vector3(0.35f, 0.35f, 0.35f);
        [SerializeField] private Vector3 textScaleTarget_ = new Vector3(1f, 1f, 1f);

        private Vector3 vecZero_;

        private void Start() {
            vecZero_ = new Vector3(0f,0f,0f);
        }

        public void OnCircleUnitSelected() {
            onIntHelperSelectedSO_.RaiseEvent(readOnlyCircleCache_.index);
        }

        public void Init(Transform enemy, IReadOnlyCircleCache readOnlyCircleCache, Transform hitTextTransform) {
            readOnlyCircleCache_ = readOnlyCircleCache;
            
            enemyUnit_ = enemy;
            enemyPos_ = enemyUnit_.position;
            
            thisTransform_ = transform;
            thisTransform_.LookAt(enemyUnit_);

            initPos_ = thisTransform_.position;
            
            bulletTrail_.gameObject.SetActive(false);

            hitTextTransform_ = hitTextTransform;
            hitText_ = hitTextTransform_.GetComponent<TextMeshPro>();
            hitText_.text = $"+{(int)readOnlyCircleCache_.goldPerHit}";
        }

        public void UpdateView() {
            Ray ray = new Ray(initPos_, enemyPos_ - initPos_);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000)) {
                onRaycastEnemyHit_.RaiseEvent(hit);
                StartCoroutine(ShootTrail(hit)); //Trail Effect
                StartCoroutine(HitText(hit.point));
            }
        }

        private IEnumerator HitText(Vector3 hitPoint) {
            //Init
            hitTextTransform_.gameObject.SetActive(true);

            //Position
            Vector3 initPos = hitPoint + textInitOffset_;
            Vector3 target = initPos + textTargetOffset_;
            hitTextTransform_.position = initPos;
            
            //Scale
            hitTextTransform_.localScale = textScaleInit_;
            
            while (true) {
                Vector3 position = hitTextTransform_.position;
                position = Vector3.MoveTowards(position, target, textSpeed_ * Time.deltaTime);
                hitTextTransform_.position = position;
    
                hitTextTransform_.localScale = Vector3.MoveTowards(hitTextTransform_.localScale, textScaleTarget_, textSpeed_ * Time.deltaTime);
                
                //Check if Target Reached, Then Break Loop
                float distance = Vector3.Distance(position, target);
                if (distance < 0.05f) {
                    break;
                }
                
                yield return null;
            }
            //Deactivate
            hitTextTransform_.gameObject.SetActive(false);
        }

        private IEnumerator ShootTrail(RaycastHit hit) {
            //Init
            bulletTrail_.gameObject.SetActive(true);
            bulletTrail_.localPosition = vecZero_;
            
            while (true) {
                Vector3 position = bulletTrail_.position;
                position = Vector3.MoveTowards(position, hit.point, shootSpeed_ * Time.deltaTime);
                bulletTrail_.position = position;
                
                float distance = Vector3.Distance(position, hit.point);
                if (distance < 0.05f) {
                    break;
                }
                
                yield return null;
            }
            //Deactivate
            bulletTrail_.gameObject.SetActive(false);
        }

        public void OnLevelUp() {
            hitText_.text = $"+{(int)readOnlyCircleCache_.goldPerHit}";
        }
    }
}