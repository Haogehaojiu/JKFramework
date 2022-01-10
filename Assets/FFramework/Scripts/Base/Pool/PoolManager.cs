using System;
using System.Collections.Generic;
using UnityEngine;

namespace JKFramework
{
    public class PoolManager : ManagerBase<PoolManager>
    {
        //根节点
        [SerializeField]
        private GameObject poolRootObj;
        /// <summary>
        /// GameObject对象容器
        /// </summary>
        public Dictionary<string, GameObjectPoolData> gameObjectPoolDictionary = new Dictionary<string, GameObjectPoolData>();
        /// <summary>
        /// 普通类 对象容器
        /// </summary>
        public Dictionary<string, ObjectPoolData> objectPoolDic = new Dictionary<string, ObjectPoolData>();

        #region GameObject对象操作 五个方法
        /// <summary>
        /// 在容器中，获取GameObject
        /// </summary>
        public T GetGameObject<T>(GameObject prefab, Transform parent = null) where T : UnityEngine.Object
        {
            GameObject obj = GetGameObjet(prefab, parent);
            if (obj != null) return obj.GetComponent<T>();
            return null;
        }
        /// <summary>
        /// 在容器中，获取GameObject
        /// </summary>
        public GameObject GetGameObjet(GameObject prefab, Transform parent = null)
        {
            GameObject obj;
            var keyName = prefab.name;
            if (CheckGameObjectPoolCache(prefab)) obj = gameObjectPoolDictionary[keyName].GetGameObject(parent);
            //没有的话，实例化一个
            else
            {
                //实例化预制体
                obj = GameObject.Instantiate(prefab, parent);
                //确保实例化的游戏对象和预制体名称一致
                obj.name = keyName;
            }
            return obj;
        }
        /// <summary>
        /// 将GameObject放入容器
        /// </summary>
        /// <param name="go">游戏对象</param>
        public void PushGameObject(GameObject go)
        {
            var keyName = go.name;
            if (gameObjectPoolDictionary.ContainsKey(keyName)) gameObjectPoolDictionary[keyName].PushGameObject(go);
            else gameObjectPoolDictionary.Add(keyName, new GameObjectPoolData(go, poolRootObj));
        }
        /// <summary>
        /// 检查对象池是否存在数据
        /// </summary>
        private bool CheckGameObjectPoolCache(GameObject prefab)
        {
            var keyName = prefab.name;
            return gameObjectPoolDictionary.ContainsKey(keyName) && gameObjectPoolDictionary[keyName].PoolQueue.Count > 0;
        }
        /// <summary>
        /// 检查缓存，如果存在游戏对象，则加载游戏对象；不成功，则返回null
        /// </summary>
        public GameObject CheckCacheAndLoadGameObject(string path, Transform parent = null)
        {
            //通过路径获取预制体的名称:"UI/LoginWindow"
            var splitPath = path.Split('/');
            var prefabName = splitPath[splitPath.Length - 1];
            //对象池有对象数据
            if (gameObjectPoolDictionary.ContainsKey(prefabName) && gameObjectPoolDictionary[prefabName].PoolQueue.Count > 0) return gameObjectPoolDictionary[prefabName].GetGameObject(parent);
            return null;
        }
        #endregion

        #region 普通对象操作 三个方法
        /// <summary>
        /// 获取普通对象
        /// </summary>
        public T GetObject<T>() where T : class, new()
        {
            T obj;
            if (CheckObjectPoolCache<T>())
            {
                var fullName = typeof(T).FullName;
                obj = (T)objectPoolDic[fullName].GetObj();
                return obj;
            }
            return new T();
        }
        /// <summary>
        /// 将普通对象放入容器
        /// </summary>
        public void PushObject(object obj)
        {
            var keyName = obj.GetType().FullName;
            if (keyName != null && objectPoolDic.ContainsKey(keyName)) objectPoolDic[keyName].PushObj(obj);
            else if (keyName != null) objectPoolDic.Add(keyName, new ObjectPoolData(obj));
        }
        private bool CheckObjectPoolCache<T>()
        {
            var keyName = typeof(T).FullName;
            return keyName != null && objectPoolDic.ContainsKey(keyName) && objectPoolDic[keyName].PoolQueue.Count > 0;
        }
        #endregion

        #region 删除操作 七个方法
        /// <summary>
        /// 删除所有缓存池对象
        /// </summary>
        /// <param name="clearGameObject">删除GameObject</param>
        /// <param name="clearCSharpObject">删除普通类对象</param>
        public void Clear(bool clearGameObject = true, bool clearCSharpObject = true)
        {
            if (clearGameObject)
            {
                for(var i = 0; i < poolRootObj.transform.childCount; i++) Destroy(poolRootObj.transform.GetChild(0).gameObject);
                gameObjectPoolDictionary.Clear();
            }
            if (clearCSharpObject) objectPoolDic.Clear();
        }
        public void ClearAllGameObject() => Clear(true, false);
        public void ClearGameObject(string prefabName)
        {
            var obj = poolRootObj.transform.Find(prefabName).gameObject;
            if (obj != null)
            {
                Destroy(obj);
                gameObjectPoolDictionary.Remove(prefabName);
            }
        }
        public void ClearGameObject(GameObject prefab) => ClearGameObject(prefab.name);
        public void ClearAllObject() => Clear(false);
        public void ClearObject<T>()
        {
            var fullName = typeof(T).FullName;
            if (fullName != null) objectPoolDic.Remove(fullName);
        }
        public void ClearObject(Type type)
        {
            if (type.FullName != null) objectPoolDic.Remove(type.FullName);
        }
        #endregion
    }
}