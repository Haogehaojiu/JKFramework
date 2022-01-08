using System.Collections.Generic;
namespace JKFramework
{
    /// <summary>
    /// 普通类 对象 对象池数据
    /// </summary>
    public class ObjectPoolData
    {
        //对象容器
        public readonly Queue<object> PoolQueue = new Queue<object>();
        public ObjectPoolData(object obj) => PushObj(obj);
        /// <summary>
        /// 将Object对象放入容器
        /// </summary>
        /// <param name="obj">要放入的GameObject对象</param>
        public void PushObj(object obj) => PoolQueue.Enqueue(obj);
        /// <summary>
        /// 从容器中获取对象
        /// </summary>
        /// <returns>Object对象</returns>
        public object GetObj() => PoolQueue.Dequeue();
    }
}