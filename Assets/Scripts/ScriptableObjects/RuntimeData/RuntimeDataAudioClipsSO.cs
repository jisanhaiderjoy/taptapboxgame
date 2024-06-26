using UnityEngine;

namespace PlayNoob.ScriptableObjects.RuntimeData {
	[CreateAssetMenu(menuName = "Scriptable Objects/Runtime Data/Audio/Audio Clip Data Set")]
	public class RuntimeDataAudioClipsSO : RuntimeDataArraySO<AudioClip> {
		[Tooltip("Audio Channel To Use in Audio Manager")]
		[SerializeField] private AudioChannelTypesSO channelName_;
		public AudioChannelTypesSO channelName => channelName_;
	}
}
