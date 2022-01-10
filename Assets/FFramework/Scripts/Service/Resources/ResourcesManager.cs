using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JKFramework
{
    /// <summary>
    /// 基于Resources的资源加载，可以扩展AssetsBundle
    /// </summary>
    public static class ResourcesManager
    {
        //需要缓存的类型
        //bool值没有实际意义，只是字典需要键和值，而bool数据比较小
        private static Dictionary<Type, bool> wantCacheDic;
        static ResourcesManager() => wantCacheDic = GameRoot.Instance.GameSetting.CacheDictionary;
        /// <summary>
        /// 检查一个类型是否需要缓存
        /// </summary>
        private static bool CheckCacheDic(Type type) => wantCacheDic.ContainsKey(type);
        /// <summary>
        /// 加载Unity资源，例如：AudioClip
        /// </summary>
        public static T LoadAsset<T>(string path) where T : UnityEngine.Object => Resources.Load<T>(path);
        /// <summary>
        /// 加载实例->普通class,如果类型需要缓存，会从缓存池中获取
        /// </summary>
        public static T Load<T>() where T : class, new()
        {
            //需要缓存
            if (CheckCacheDic(typeof(T))) return PoolManager.Instance.GetObject<T>();
            return new T();
        }
        /// <summary>
        /// 获取实例->组件
        /// </summary>
        public static T Load<T>(string path, Transform parent = null) where T : Component
        {
            if (CheckCacheDic(typeof(T))) return PoolManager.Instance.GetGameObject<T>(GetPrefab(path), parent);
            return InstantiatePrefab(path).GetComponent<T>();
        }
        /// <summary>
        /// 异步加载GameObject对象
        /// </summary>
        public static void LoadGameObjectAsync<T>(string path, Action<T> callback = null, Transform parent = null) where T : UnityEngine.Object
        {
            //因为要经过对象池，不能直接使用协程
            //如果对象池中有
            if (CheckCacheDic(typeof(T)))
            {
                GameObject go = PoolManager.Instance.CheckCacheAndLoadGameObject(path, parent);
                if (go != null) callback?.Invoke(go.GetComponent<T>());
                //对象池中没有
                else MonoManager.Instance.StartCoroutine(DoLoadGameObjectAsync(path, callback, parent));
            }
            //对象池中没有
            else MonoManager.Instance.StartCoroutine(DoLoadGameObjectAsync(path, callback, parent));
        }
        static IEnumerator DoLoadGameObjectAsync<T>(string path, Action<T> callback = null, Transform parent = null) where T : UnityEngine.Object
        {
            ResourceRequest request = Resources.LoadAsync<GameObject>(path);
            yield return request;
            GameObject go = InstantiatePrefab(request.asset as GameObject, parent);
            callback?.Invoke(go.GetComponent<T>());
        }

        /// <summary>
        /// 异步加载Unity资源，例如AudioClip、Sprite、GameObject（预制体）等
        /// </summary>
        public static void LoadAssetAsync<T>(string path, Action<T> callback) where T : UnityEngine.Object => MonoManager.Instance.StartCoroutine(DoLoadAssetAsync(path, callback));
        static IEnumerator DoLoadAssetAsync<T>(string path, Action<T> callback) where T : UnityEngine.Object
        {
            ResourceRequest request = Resources.LoadAsync<T>(path);
            yield return request;
            callback?.Invoke(request.asset as T);
        }
        /// <summary>
        /// 获取预制体
        /// </summary>
        public static GameObject GetPrefab(string path)
        {
            GameObject prefab = Resources.Load<GameObject>(path);
            if (prefab != null) return prefab;
            throw new Exception($"JKFramework -> {path}有误");
        }
        /// <summary>
        /// 基于预制体实例化
        /// </summary>
        public static GameObject InstantiatePrefab(string path, Transform parent = null) => InstantiatePrefab(GetPrefab(path), parent);
        /// <summary>
        /// 基于预制体实例化
        /// </summary>
        public static GameObject InstantiatePrefab(GameObject prefab, Transform parent = null)
        {
            GameObject go = GameObject.Instantiate(prefab, parent);
            go.name = prefab.name;
            return go;
        }
    }
}