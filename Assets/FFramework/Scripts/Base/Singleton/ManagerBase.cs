using UnityEngine;
namespace JKFramework
{
    public abstract class ManagerBase : MonoBehaviour
    {
        public virtual void Init() { }
    }
    public abstract class ManagerBase<T> : ManagerBase where T : ManagerBase<T>
    {
        public static T Instance;
        /// <summary>
        /// 管理器初始化
        /// </summary>
        public override void Init() => Instance = this as T;
    }
}