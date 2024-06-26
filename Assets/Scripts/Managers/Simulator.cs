using System.Collections;
using PlayNoob.ScriptableObjects.GameEvents;
using PlayNoob.ScriptableObjects.RuntimeData;
using PlayNoob.View;
using UnityEngine;

namespace PlayNoob.Controller {
    /// <summary>
    /// Simulator Class that handles the Circle Helper's Battle Simulation.
    /// </summary>
    public class Simulator : MonoBehaviour {
        [Header("Game Cache")] 
        [SerializeField] private ReadOnlyGameCacheAnchor readOnlyGameCacheAnchor_;
        [SerializeField] private ReadOnlyCircleCacheArrayAnchor readOnlyCircleCacheArrayAnchor_;
        [SerializeField] private TransformArrayAnchorSO hitTextsAnchorSO_;
        
        [Header("Game Events")]
        [SerializeField] private IntGameEventSO onIntEnemyHit_;
        [SerializeField] private IntGameEventSO onCircleLevelUpCompleted_;
        
        [Space(6)]
        [SerializeField] private EnemyUnitView enemyUnitView_;

        [Header("Circle Helpers")] 
        [SerializeField] private TransformArrayAnchorSO spawnPointsTransformsArrayAnchorSo;
        [SerializeField] private GameObject circleUnitPrefab_;

        private CircleUnitView[] circleUnitViews_;

        private void Start() {
            StartCoroutine(UpdateRoutine());
            //Add Listener
            onCircleLevelUpCompleted_.RegisterFunc(OnCircleHelperLevelUpCompleted);
        }

        private void OnDestroy() {
            //Remove Listeners
            onCircleLevelUpCompleted_.UnRegisterFunc(OnCircleHelperLevelUpCompleted);
        }

        /// <summary>
        /// Came back from a Manager, So this is a Reliable One!
        /// </summary>
        /// <param name="helperIndex"></param>
        private void OnCircleHelperLevelUpCompleted(int helperIndex) {
            //Update the Data
            circleUnitViews_[helperIndex].OnLevelUp();
        }

        /// <summary>
        /// Spawn All the available Circle Helpers
        /// </summary>
        private void SpawnCircles() {
            Transform[] spawnPoints = spawnPointsTransformsArrayAnchorSo.Value;
            int circlesCount = readOnlyGameCacheAnchor_.Value.circleCount;
            
            circleUnitViews_ = new CircleUnitView[spawnPoints.Length];

            for (int i = 0; i < circlesCount; i++) {
                GameObject circleGo = Instantiate(circleUnitPrefab_, spawnPoints[i].position, Quaternion.identity);
                CircleUnitView unit = circleGo.GetComponent<CircleUnitView>();
                
                circleUnitViews_[i] = unit;
               
                unit.Init(enemyUnitView_.transform, readOnlyCircleCacheArrayAnchor_.Value[i], hitTextsAnchorSO_.Value[i]);
            }
        }
        
        /// <summary>
        /// Spawns A Circle Helper onto the GameScene
        /// </summary>
        public void SpawnACircle() {
            Transform[] spawnPoints = spawnPointsTransformsArrayAnchorSo.Value;
            int circlesCount = readOnlyGameCacheAnchor_.Value.circleCount;
            int index = circlesCount - 1;
            
            GameObject circleGo = Instantiate(circleUnitPrefab_, spawnPoints[index].position, Quaternion.identity);
            CircleUnitView unit = circleGo.GetComponent<CircleUnitView>();
            
            circleUnitViews_[index] = unit;
            
            unit.Init(enemyUnitView_.transform, readOnlyCircleCacheArrayAnchor_.Value[index], hitTextsAnchorSO_.Value[index]);
        }

        /// <summary>
        /// Spawns All the Available Helpers, and Starts the Battle Simulation
        /// A Managed Update Loop
        /// </summary>
        /// <returns></returns>
        private IEnumerator UpdateRoutine() {
            yield return new WaitUntil(() => spawnPointsTransformsArrayAnchorSo.isSet && readOnlyGameCacheAnchor_.isSet && hitTextsAnchorSO_.isSet);
            SpawnCircles();
            yield return new WaitForSeconds(1f);
            while (true) {
                //Updates All the Available Circle Units
                //Hit the Enemy every Second and Raise Event to the Manager
                int circlesCount = circleUnitViews_.Length;
                for (int i = 0; i < circlesCount; i++) {
                    CircleUnitView unit = circleUnitViews_[i];
                    if (unit != null) {
                        onIntEnemyHit_.RaiseEvent(i);
                        unit.UpdateView();
                    }
                    yield return new WaitForSeconds(1f / circlesCount);
                }
            }
        }
    }
}