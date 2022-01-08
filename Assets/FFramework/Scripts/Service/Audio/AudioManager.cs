using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace JKFramework
{
    public class AudioManager : ManagerBase<AudioManager>
    {
        [SerializeField]
        private AudioSource bgAudioSource;

        [SerializeField]
        private GameObject prefabAudioPlay;

        //场景中所有生效的特效音乐播放器
        private List<AudioSource> audioSourceList = new List<AudioSource>();

        public override void Init()
        {
            base.Init();
            UpdateAllAudioVolume();
        }
        #region 音量、播放控制
        [SerializeField]
        [Range(0, 1)]
        [OnValueChanged("UpdateAllAudioVolume")]
        private float globalVolume;
        public float GlobalVolume {
            get => globalVolume;
            set {
                if (globalVolume == value) return;
                globalVolume = value;
                UpdateAllAudioVolume();
            }
        }
        [SerializeField]
        [Range(0, 1)]
        [OnValueChanged("UpdateBgAudioVolume")]
        private float bgVolume;
        public float BgVolume {
            get => bgVolume;
            set {
                if (bgVolume == value) return;
                bgVolume = value;
                UpdateBgAudioVolume();
            }
        }
        [SerializeField]
        [Range(0, 1)]
        [OnValueChanged("UpdateEffectAudioVolume")]
        private float effectVolume;
        public float EffectVolume {
            get => effectVolume;
            set {
                if (effectVolume == value) return;
                effectVolume = value;
                UpdateEffectAudioVolume();
            }
        }
        [SerializeField]
        [OnValueChanged("UpdateAudioMute")]
        private bool isMute;
        public bool IsMute {
            get => isMute;
            set {
                if (isMute == value) return;
                isMute = value;
                UpdateAudioMute();
            }
        }
        [SerializeField]
        [OnValueChanged("UpdateAudioLoop")]
        private bool isLoop = true;
        public bool IsLoop {
            get => isLoop;
            set {
                if (isLoop == value) return;
                isLoop = value;
                UpdateAudioLoop();
            }
        }
        private bool isPause;
        public bool IsPause {
            get => isPause;
            set {
                if (isPause == value) return;
                isPause = value;
                if (isPause) bgAudioSource.Pause();
                else bgAudioSource.UnPause();
                UpdateEffectAudioVolume();
            }
        }
        /// <summary>
        /// 更新全局音量
        /// </summary>
        private void UpdateAllAudioVolume()
        {
            UpdateBgAudioVolume();
            UpdateEffectAudioVolume();
        }
        /// <summary>
        /// 更新背景音量
        /// </summary>
        private void UpdateBgAudioVolume() { bgAudioSource.volume = GlobalVolume * BgVolume; }
        /// <summary>
        /// 更新音效音量
        /// </summary>
        private void UpdateEffectAudioVolume()
        {
            //倒序遍历
            for(var i = audioSourceList.Count - 1; i >= 0; i--)
            {
                if (audioSourceList[i] != null) SetEffectAudioPlay(audioSourceList[i]);
                else audioSourceList.RemoveAt(i);
            }
        }
        /// <summary>
        /// 设置特效音乐播放器
        /// </summary>
        private void SetEffectAudioPlay(AudioSource source, float spatial = -1)
        {
            source.mute = isMute;
            source.volume = GlobalVolume * EffectVolume;
            if (spatial != -1) source.spatialBlend = spatial;
            if (isPause) source.Pause();
            else source.UnPause();
        }
        /// <summary>
        /// 更新静音启用/禁用
        /// </summary>
        private void UpdateAudioMute()
        {
            bgAudioSource.mute = isMute;
            UpdateEffectAudioVolume();
        }
        /// <summary>
        /// 更新背景音乐循环启用/禁用
        /// </summary>
        private void UpdateAudioLoop() { bgAudioSource.loop = isLoop; }
        #endregion

        #region 背景音乐
        public void PlayBgAudio(AudioClip clip, bool loop = true, float volume = -1)
        {
            bgAudioSource.clip = clip;
            isLoop = loop;
            if (volume != -1) bgVolume = volume;
            bgAudioSource.Play();
        }
        public void PlayBgAudio(string clipPath, bool loop = true, float volume = -1)
        {
            AudioClip audioClip = ResourcesManager.LoadAsset<AudioClip>(clipPath);
            PlayBgAudio(audioClip, loop, volume);
        }
        #endregion

        #region 特效音乐
        private Transform audioPlayRoot;
        /// <summary>
        /// 获取音乐播放器
        /// </summary>
        private AudioSource GetAudioPlay(bool is3d = true)
        {
            if (audioPlayRoot == null) audioPlayRoot = new GameObject("AudioPlayRoot").transform;
            //从对象池中获取播放器
            AudioSource audioSource = PoolManager.Instance.GetGameObject<AudioSource>(prefabAudioPlay, audioPlayRoot);
            SetEffectAudioPlay(audioSource, is3d ? 1f : 0f);
            audioSourceList.Add(audioSource);
            return audioSource;
        }
        /// <summary>
        /// 回收播放器
        /// </summary>
        private void RecycleAudioPlay(AudioSource source, AudioClip clip, UnityAction callback, float time) { StartCoroutine(DoRecycleAudioPlay(source, clip, callback, time)); }
        private IEnumerator DoRecycleAudioPlay(AudioSource source, AudioClip clip, UnityAction callback, float time)
        {
            //延迟AudioClip的长度（秒）
            yield return new WaitForSeconds(clip.length);
            //放入对象池
            if (source != null)
            {
                source.JKGameObjectPushPool();
                //回调,延迟回调时间参数（秒）
                yield return new WaitForSeconds(time);
                callback?.Invoke();
            }
        }
        /// <summary>
        /// 播放一次特效音乐
        /// </summary>
        /// <param name="clip">特效音乐片段</param>
        /// <param name="component">挂载组件</param>
        /// <param name="volumeScale">音量0-1</param>
        /// <param name="is3d">是否是3D</param>
        /// <param name="callback">回调函数，在播放完成后执行</param>
        /// <param name="time">回调函数在播放完成后执行的延迟时间</param>
        public void PlayOnShot(AudioClip clip, Component component, float volumeScale = 1.0f, bool is3d = true, UnityAction callback = null, float time = 0)
        {
            //初始化音乐播放器
            AudioSource audioSource = GetAudioPlay(is3d);
            Transform audioSourceTransform = audioSource.transform;
            audioSourceTransform.SetParent(component.transform);
            audioSourceTransform.localPosition = Vector3.zero;

            //播放一次
            audioSource.PlayOneShot(clip, volumeScale);

            //播放回收以及回调函数
            RecycleAudioPlay(audioSource, clip, callback, time);
        }
        /// <summary>
        /// 播放一次特效音乐
        /// </summary>
        /// <param name="clip">特效音乐片段</param>
        /// <param name="position">播放位置</param>
        /// <param name="volumeScale">音量0-1</param>
        /// <param name="is3d">是否是3D</param>
        /// <param name="callback">回调函数，在播放完成后执行</param>
        /// <param name="time">回调函数在播放完成后执行的延迟时间</param>
        public void PlayOnShot(AudioClip clip, Vector3 position, float volumeScale = 1.0f, bool is3d = true, UnityAction callback = null, float time = 0)
        {
            //初始化音乐播放器
            AudioSource audioSource = GetAudioPlay(is3d);
            audioSource.transform.position = position;

            //播放一次
            audioSource.PlayOneShot(clip, volumeScale);

            //播放回收以及回调函数
            RecycleAudioPlay(audioSource, clip, callback, time);
        }
        /// <summary>
        /// 播放一次特效音乐
        /// </summary>
        /// <param name="clipPath">特效音乐路径</param>
        /// <param name="component">挂载组件</param>
        /// <param name="volumeScale">音量0-1</param>
        /// <param name="is3d">是否是3D</param>
        /// <param name="callback">回调函数，在播放完成后执行</param>
        /// <param name="time">回调函数在播放完成后执行的延迟时间</param>
        public void PlayOnShot(string clipPath, Component component, float volumeScale = 1.0f, bool is3d = true, UnityAction callback = null, float time = 0)
        {
            AudioClip audioClip = ResourcesManager.LoadAsset<AudioClip>(clipPath);
            if (audioClip != null) PlayOnShot(audioClip, component, volumeScale, is3d, callback, time);
        }
        /// <summary>
        /// 播放一次特效音乐
        /// </summary>
        /// <param name="clipPath">特效音乐路径</param>
        /// <param name="position">播放的位置</param>
        /// <param name="volumeScale">音量0-1</param>
        /// <param name="is3d">是否是3D</param>
        /// <param name="callback">回调函数，在播放完成后执行</param>
        /// <param name="time">回调函数在播放完成后执行的延迟时间</param>
        public void PlayOnShot(string clipPath, Vector3 position, float volumeScale = 1.0f, bool is3d = true, UnityAction callback = null, float time = 0)
        {
            AudioClip audioClip = ResourcesManager.LoadAsset<AudioClip>(clipPath);
            if (audioClip != null) PlayOnShot(audioClip, position, volumeScale, is3d, callback, time);
        }
        #endregion
    }
}