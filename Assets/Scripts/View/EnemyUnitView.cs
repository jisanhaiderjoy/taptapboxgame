using System.Collections;
using PlayNoob.ScriptableObjects.GameEvents;
using PlayNoob.ScriptableObjects.RuntimeData;
using TMPro;
using UnityEngine;

namespace PlayNoob.View {
    public class EnemyUnitView : MonoBehaviour {
        [Header("Game Cache")]
        [SerializeField] private ReadOnlyGameCacheAnchor gameCacheAnchor;
        
        [Header("Game Events")]
        [SerializeField] private RaycastHitGameEventSO enemyHitByTapEvent_ = default;
        [SerializeField] private RaycastHitGameEventSO enemyHitByHelpersEvent_ = default;
        [SerializeField] private VoidGameEventSO onLevelUpCompleted_ = default;

        [Header("Game Data Anchors")]
        [SerializeField] private TransformArrayAnchorSO tapHitParticlesAnchorSO_;
        [SerializeField] private TransformArrayAnchorSO circleHitParticlesAnchorSO_;
        [SerializeField] private TransformArrayAnchorSO hitTextsAnchorSO_;

        [Header("Text Particle")]
        [SerializeField] private float textSpeed_ = 3f;
        
        private TextMeshPro[] hitTexts_;

        private Vector3 textInitOffset_ = new Vector3(0f, 1f, 0f);
        private Vector3 textTargetOffset_ = new Vector3(0f, 2f, 0f);
        
        private Vector3 textScaleInit_ = new Vector3(0.35f, 0.35f, 0.35f);
        private Vector3 textScaleTarget_ = new Vector3(1f, 1f, 1f);
        
        private Quaternion identity;
        private void Start() {
            enemyHitByTapEvent_.RegisterFunc(OnEnemyHitByTap);
            enemyHitByHelpersEvent_.RegisterFunc(OnEnemyHitByHelpers);
            onLevelUpCompleted_.RegisterFunc(OnLevelUpConfirmed);

            identity = Quaternion.identity;

            //Init Data
            StartCoroutine(InitTextHitData());
        }

        private IEnumerator InitTextHitData() {
            //Wait until all data is Set
            yield return new WaitUntil(() => gameCacheAnchor.isSet && hitTextsAnchorSO_.isSet);
            
            //Cache all the Transforms and Texts
            Transform[] hitTextTransforms = hitTextsAnchorSO_.Value;
            int textCounts = hitTextTransforms.Length;
            hitTexts_ = new TextMeshPro[textCounts];
            
            for (int i = 0; i < textCounts; i++) {
                hitTexts_[i] = hitTextTransforms[i].GetComponent<TextMeshPro>();
                hitTexts_[i].text = $"+{(int)gameCacheAnchor.Value.goldPerTap}";
            }
        }

        private void OnLevelUpConfirmed() {
            if (gameCacheAnchor.isSet && hitTextsAnchorSO_.isSet) {
                //Update all the Texts, so that we don't have to do it again
                int textCounts = hitTexts_.Length;
                for (int i = 0; i < textCounts; i++) {
                    hitTexts_[i].text = $"+{(int)gameCacheAnchor.Value.goldPerTap}";
                }
            }
        }

        private void OnDestroy() {
            enemyHitByTapEvent_.UnRegisterFunc(OnEnemyHitByTap);
            enemyHitByHelpersEvent_.UnRegisterFunc(OnEnemyHitByHelpers);
            onLevelUpCompleted_.UnRegisterFunc(OnLevelUpConfirmed);
        }

        private void OnEnemyHitByTap(RaycastHit hit) {
            if (tapHitParticlesAnchorSO_.isSet) {
                Transform particle = tapHitParticlesAnchorSO_.GetRandomItem();
                particle.SetPositionAndRotation(hit.point, identity);
                particle.gameObject.SetActive(true);

                if (gameCacheAnchor.isSet && hitTextsAnchorSO_.isSet) {
                    StartCoroutine(HitText(hit.point));
                }
            }
        }
        
        private void OnEnemyHitByHelpers(RaycastHit hit) {
            if (circleHitParticlesAnchorSO_.isSet) {
                Transform particle = circleHitParticlesAnchorSO_.GetRandomItem();
                particle.SetPositionAndRotation(hit.point, identity);
                particle.gameObject.SetActive(true);
            }
        }
        
        private IEnumerator HitText(Vector3 hitPoint) {
            //Init
            Transform hitTextTransform = hitTextsAnchorSO_.GetNextItem();
            hitTextTransform.gameObject.SetActive(true);

            //Position
            Vector3 initPos = hitPoint + textInitOffset_;
            Vector3 target = initPos + textTargetOffset_;
            hitTextTransform.position = initPos;
            
            //Scale
            hitTextTransform.localScale = textScaleInit_;
            
            while (true) {
                Vector3 position = hitTextTransform.position;
                position = Vector3.MoveTowards(position, target, textSpeed_ * Time.deltaTime);
                hitTextTransform.position = position;
    
                hitTextTransform.localScale = Vector3.MoveTowards(hitTextTransform.localScale, textScaleTarget_, textSpeed_ * Time.deltaTime);
                
                //Check if Target Reached, Then Break Loop
                float distance = Vector3.Distance(position, target);
                if (distance < 0.05f) {
                    break;
                }
                
                yield return null;
            }
            //Deactivate
            hitTextTransform.gameObject.SetActive(false);
        }
    }
}
