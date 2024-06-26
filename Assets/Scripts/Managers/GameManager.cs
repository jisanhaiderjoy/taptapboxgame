using PlayNoob.ScriptableObjects;
using PlayNoob.ScriptableObjects.GameEvents;
using PlayNoob.ScriptableObjects.RuntimeData;
using UnityEngine;

namespace PlayNoob.Controller {
	/// <summary>
	/// Owner of Important Game Data and Important Events
	/// </summary>
	public class GameManager : MonoBehaviour {
		[Header("Game Cache")]
		[SerializeField] private ReadOnlyGameCacheAnchor gameCacheAnchor;
		[SerializeField] private ReadOnlyCircleCacheArrayAnchor readOnlyCircleCacheArrayAnchor_;
		private GameCache gameCache_; //Holds the Primary Game Cache Data. Only Game Manager Can Own this
		private CircleCache[] circleCaches_;

		[Header("Game Events")] 
		[SerializeField] private VoidGameEventSO onLevelUpCompleted_;
		[SerializeField] private VoidGameEventSO onCircleBuyCompleted_;
		[SerializeField] private FloatGameEventSO onGoldIncreased_;
		[SerializeField] private IntGameEventSO onCircleLevelUpCompleted_;
		
		private void Start() {
			Application.targetFrameRate = Screen.currentResolution.refreshRate;

			//Fetch and init Data
			FetchGameScalingData();
			//InitGameCache
			GameCacheInit();
		}

		private void OnDestroy() {
			gameCacheAnchor.Unset();
			//Unset listeners
			gameCache_.UnRegisterOnLevelUp(OnLevelCompletedByGameCache);
			gameCache_.UnRegisterOnBuyCircle(OnCircleBuyCompletedByGameCache);
			gameCache_.UnRegisterOnGoldIncrease(OnGoldIncreaseByGameCache);
			gameCache_.UnRegisterOnCircleLevelUp(OnCircleLevelUpCompleted);
		}

		/// <summary>
		/// Try and Init GameCaches.
		/// If There's a Remote Data already Available, then parse and Load it OR Use Defaults
		/// </summary>
		private void GameCacheInit() {
			Scaling scalingData;
			
			string path = $"{Application.persistentDataPath}/scalingData";
			if (System.IO.File.Exists(path)) {
				string file = System.IO.File.ReadAllText(path);
				ScalingData data = JsonUtility.FromJson<ScalingData>(file);
				scalingData = data.scaling;
			}
			else {
				//Default Data
				scalingData = new Scaling() {
					constant1 = 5,
					constant2 = 2.1f,
					constant3 = 1.08f,
					circleCost = 100
				};
			}
			
			
			//Init GameCache Data and Pass Reference to the Anchor
			gameCache_ = new GameCache(scalingData);

			//add Listeners
			gameCache_.RegisterOnLevelUp(OnLevelCompletedByGameCache);
			gameCache_.RegisterOnBuyCircle(OnCircleBuyCompletedByGameCache);
			gameCache_.RegisterOnGoldIncrease(OnGoldIncreaseByGameCache);
			gameCache_.RegisterOnCircleLevelUp(OnCircleLevelUpCompleted);

			circleCaches_ = new CircleCache[5];
			for (int i = 0; i < 5; i++) {
				circleCaches_[i] = new CircleCache(i, scalingData);
			}
			//Once all the Caches are init, CacheData is Provided to the Anchors
			readOnlyCircleCacheArrayAnchor_.Provide(circleCaches_);
			gameCacheAnchor.Provide(gameCache_);
		}

		/// <summary>
		/// Fetch Remote Config Data Through the API
		/// </summary>
		private void FetchGameScalingData() {
			//To Edit the Config JSON,
			//Goto: https://www.npoint.io/docs/c1b956d757af2c158e31
			string url = "https://api.npoint.io/c1b956d757af2c158e31";
			StartCoroutine(NetworkManager.GetRequest(url, OnFetchScalingData));
		}
		
		/// <summary>
		/// OnRemote Data Fetch Success, we store the File
		/// </summary>
		/// <param name="response"></param>
		private void OnFetchScalingData(string response) {
			//Save Data into File
			System.IO.File.WriteAllText($"{Application.persistentDataPath}/scalingData",response);
		}

		/// <summary>
		/// Enemy Hit Event, Raised by player tap or helper attacks
		/// </summary>
		/// <param name="damage"></param>
		public void OnEnemyHit(RaycastHit hitInfo) {
			//Increase GoldCount
			gameCache_?.IncreaseGold();
		}
		
		/// <summary>
		/// Enemy Hit Event, Raised by player tap or helper attacks
		/// </summary>
		/// <param name="damage"></param>
		public void OnEnemyHitByHelpers(int hitterIndex) {
			if (circleCaches_ != null) {
				//Increase GoldCount
				gameCache_.IncreaseGold(circleCaches_[hitterIndex].goldPerHit);
			}
		}

		public void OnCircleLevelPurchase(int helperIndex) {
			if (circleCaches_ != null) {
				//Increase GoldCount
				gameCache_.LevelUpCircle(circleCaches_[helperIndex]);
			}
		}
		
		private void OnCircleLevelUpCompleted(int helperIndex) {
			circleCaches_[helperIndex].LevelUpCircle();
			onCircleLevelUpCompleted_.RaiseEvent(helperIndex);
		}

		/// <summary>
		/// On Level Purchased from UI
		/// </summary>
		public void OnLevelUpPurchased() {
			gameCache_?.LevelUp();
		}

		/// <summary>
		/// Raise a Reliable Event On Level Up Done.
		/// </summary>
		private void OnLevelCompletedByGameCache() {
			onLevelUpCompleted_.RaiseEvent();
		}
		
		/// <summary>
		/// Raise a Reliable Event As soon as Gold Increased
		/// </summary>
		/// <param name="totalGold"></param>
		private void OnGoldIncreaseByGameCache(float totalGold) {
			onGoldIncreased_.RaiseEvent(totalGold);
		}

		public void OnCircleBuyRequested() {
			gameCache_?.BuyCircle();
		}

		private void OnCircleBuyCompletedByGameCache() {
			onCircleBuyCompleted_.RaiseEvent();
		}
	}
}