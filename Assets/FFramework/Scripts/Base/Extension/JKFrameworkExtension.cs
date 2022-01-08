using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace JKFramework
{
    public static class JKFrameworkExtension
    {
        #region 通用
        /// <summary>
        /// 获取特性
        /// </summary>
        public static T GetAttribute<T>(this object obj) where T : Attribute => obj.GetType().GetCustomAttribute<T>();
        /// <summary>
        /// 获取特性
        /// </summary>
        /// <param name="type">特性所在的类型</param>
        public static T GetAttribute<T>(this object obj, Type type) where T : Attribute => type.GetCustomAttribute<T>();
        /// <summary>
        /// 数组想等对比
        /// </summary>
        public static bool ArrayEquals(this object[] objects, object[] otherObjects)
        {
            if (otherObjects == null || objects.GetType() != otherObjects.GetType()) return false;
            if (objects.Length == otherObjects.Length)
            {
                for(var i = 0; i < objects.Length; i++)
                    if (!objects[i].Equals(otherObjects[i]))
                        return false;
            } else return false;
            return true;
        }
        #endregion
        #region 资源管理
        /// <summary>
        /// GameObject放入对象池
        /// </summary>
        public static void JKGameObjectPushPool(this GameObject gameObject) { PoolManager.Instance.PushGameObject(gameObject); }
        /// <summary>
        /// GameObject放入对象池
        /// </summary>
        public static void JKGameObjectPushPool(this Component component) { JKGameObjectPushPool(component.gameObject); }
        /// <summary>
        /// 普通类放入对象池
        /// </summary>
        /// <param name="obj"></param>
        public static void JKObjectPushPool(this object obj) { PoolManager.Instance.PushObject(obj); }
        #endregion
        #region 本地化
        /// <summary>
        /// 从本地化系统中修改内容
        /// </summary>
        /// <param name="dataPackageName"></param>
        /// <param name="contentName"></param>
        public static void JKLocalSet(this Text text, string dataPackageName, string contentName) => text.text = LocalizationManager.Instance.GetContent<L_Text>(dataPackageName, contentName).content;
        /// <summary>
        /// 从本地化系统中修改内容
        /// </summary>
        /// <param name="dataPackageName"></param>
        /// <param name="contentName"></param>
        public static void JKLocalSet(this Image image, string dataPackageName, string contentName) => image.sprite = LocalizationManager.Instance.GetContent<L_Image>(dataPackageName, contentName).content;
        /// <summary>
        /// 从本地化系统中修改内容
        /// </summary>
        /// <param name="dataPackageName"></param>
        /// <param name="contentName"></param>
        public static void JKLocalSet(this AudioSource audioSource, string dataPackageName, string contentName) => audioSource.clip = LocalizationManager.Instance.GetContent<L_Audio>(dataPackageName, contentName).content;
        /// <summary>
        /// 从本地化系统中修改内容
        /// </summary>
        /// <param name="dataPackageName"></param>
        /// <param name="contentName"></param>
        public static void JKLocalSet(this VideoPlayer videoPlayer, string dataPackageName, string contentName) => videoPlayer.clip = LocalizationManager.Instance.GetContent<L_Video>(dataPackageName, contentName).content;
        #endregion
        #region Mono
        /// <summary>
        /// 添加Update监听
        /// </summary>
        public static void OnUpdate(this object obj, Action action) => MonoManager.Instance.AddUpdateListener(action);
        /// <summary>
        /// 移除Update监听
        /// </summary>
        public static void RemoveUpdate(this object obj, Action action) => MonoManager.Instance.RemoveUpdateListener(action);
        /// <summary>
        /// 添加LateUpdate监听
        /// </summary>
        public static void OnLateUpdate(this object obj, Action action) => MonoManager.Instance.AddLateUpdateListener(action);
        /// <summary>
        /// 移除LateUpdate监听
        /// </summary>
        public static void RemoveLateUpdate(this object obj, Action action) { MonoManager.Instance.RemoveLateUpdateListener(action); }
        /// <summary>
        /// 添加FixedUpdate监听
        /// </summary>
        public static void OnFixedUpdate(this object obj, Action action) { MonoManager.Instance.AddFixedUpdateListener(action); }
        /// <summary>
        /// 移除FixedUpdate监听
        /// </summary>
        public static void RemoveFixedUpdate(this object obj, Action action) { MonoManager.Instance.RemoveFixedUpdateListener(action); }

        public static Coroutine StartCoroutine(this object obj, IEnumerator routine) { return MonoManager.Instance.StartCoroutine(routine); }
        public static void StopCoroutine(this object obj, Coroutine routine) { MonoManager.Instance.StopCoroutine(routine); }
        public static void StopAllCoroutine(this object obj) { MonoManager.Instance.StopAllCoroutines(); }
        #endregion
    }
}