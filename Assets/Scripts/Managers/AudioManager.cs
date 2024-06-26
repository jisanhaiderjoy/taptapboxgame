using System.Collections.Generic;
using PlayNoob.ScriptableObjects.GameEvents;
using PlayNoob.ScriptableObjects.RuntimeData;
using PlayNoob.Utils;
using UnityEngine;

namespace PlayNoob.Controller {
    public class AudioManager : MonoBehaviour {
        [SerializeField] private RuntimeDataAudioChannelsSO runtimeDataAudioChannelsSo_;
        private Dictionary<string, AudioSource> channels_;

        private Transform thisTransform;

        private void Start() {
            //caching Transform helps for future usages
            thisTransform = transform;
            
            int channelsCount = runtimeDataAudioChannelsSo_.data.Length;
            channels_ = new Dictionary<string, AudioSource>(channelsCount);
            for (int i = 0; i < channelsCount; i++) {
                CreateChannel(runtimeDataAudioChannelsSo_.data[i].AudioChannelName);
            }
        }

        private AudioSource CreateChannel(string channelName) {
            GameObject go = new GameObject(channelName);
            go.transform.SetParent(thisTransform);

            AudioSource audioSource = go.AddComponent<AudioSource>();
            audioSource.clip = null;
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            
            channels_.Add(channelName, audioSource);
            
            return audioSource;
        }

        private AudioSource GetChannel(string channelName) {
            if (channels_.TryGetValue(channelName, out AudioSource channel)) {
                return channel;
            }

            return CreateChannel(channelName);
        }

        /// <summary>
        /// Generic Play Audio Function from a RuntimeData Set
        /// </summary>
        /// <param name="clipData">ScriptableObject of AudioClip DataSet</param>
        public void PlayRandomClipOneShot(RuntimeDataAudioClipsSO clipData) {
            GetChannel(clipData.channelName.AudioChannelName).PlayOneShot(clipData.GetRandomItem());
        }
        
        /// <summary>
        /// Generic Play Audio Function
        /// </summary>
        /// <param name="clip">Unity AudioClip</param>
        public void PlayClipOneShot(AudioClip clip) {
            channels_[Constant.DEFAULT].PlayOneShot(clip);
        }

        /// <summary>
        /// Generic Play Audio Function On A Specified Channel
        /// </summary>
        /// <param name="clipData"></param>
        public void PlayClipOneShot(PlayAudioClipData clipData) {
            channels_[clipData.audioChannelTypesSo.AudioChannelName].PlayOneShot(clipData.audioClip, clipData.volume);
        }
    }
}