using System.Collections.Generic;
using UnityEngine;
namespace JKFramework
{
    /// <summary>
    /// GameObject对象池数据
    /// </summary>
    public class GameObjectPoolData
    {
        //对象池中，父节点PoolRoot的子级
        private GameObject parentGameObject;
        //对象容器
        public Queue<GameObject> PoolQueue;
        public GameObjectPoolData(GameObject go, GameObject poolRootObj)
        {
            PoolQueue = new Queue<GameObject>();
            //创建父节点，并设置为缓存池根节点子级
            parentGameObject = new GameObject(go.name);
            parentGameObject.transform.SetParent(poolRootObj.transform);

            //对象首次创建时，放入容器        
            PushGameObject(go);
        }
        /// <summary>
        /// 将GameObject对象放入容器
        /// </summary>
        /// <param name="go">要放入的GameObject对象</param>
        public void PushGameObject(GameObject go)
        {
            //将对象放进容器
            PoolQueue.Enqueue(go);
            //设置父级
            go.transform.SetParent(parentGameObject.transform);
            //将放进容器的对象取消激活
            go.SetActive(false);
        }
        /// <summary>
        /// 从容器中获取对象
        /// </summary>
        /// <returns>GameObject对象</returns>
        public GameObject GetGameObject(Transform parent = null)
        {
            var go = PoolQueue.Dequeue();
            //激活取出容器的对象
            go.SetActive(true);
            //设置父级
            go.transform.SetParent(parent);
            //将对象放入默认场景（非不可销毁场景）
            if (parent == null) UnityEngine.SceneManagement.SceneManager.MoveGameObjectToScene(go, UnityEngine.SceneManagement.SceneManager.GetActiveScene());
            return go;
        }
    }
}