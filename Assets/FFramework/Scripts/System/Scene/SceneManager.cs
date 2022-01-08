using System;
using System.Collections;
using UnityEngine;

namespace JKFramework
{
    /// <summary>
    /// 场景管理器
    /// </summary>
    public static class SceneManager
    {
        /// <summary>
        /// 同步加载场景
        /// </summary>
        public static void LoadScene(string sceneName, Action callback = null)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            callback?.Invoke();
        }
        /// <summary>
        /// 异步加载场景
        /// 会自动分发进度到事件中心，事件名称"LoadingScene"
        /// </summary>
        /// <param name="sceneName"></param>
        /// <param name="callback"></param>
        public static void LoadSceneAsync(string sceneName, Action callback = null) => MonoManager.Instance.StartCoroutine(DoLoadSceneAsync(sceneName, callback));
        private static IEnumerator DoLoadSceneAsync(string sceneName, Action callback = null)
        {
            AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName);
            while(asyncOperation.isDone == false)
            {
                //把加载进度分发到事件中心
                EventManager.EventTrigger("LoadSceneProgress", asyncOperation.progress);
                yield return asyncOperation.progress;
            }
            EventManager.EventTrigger("LoadSceneProgress", 1F);
            EventManager.EventTrigger("LoadSceneSucceed");
            callback?.Invoke();
        }
    }
}