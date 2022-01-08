namespace JKFramework
{
    /// <summary>
    /// 逻辑管理器基类
    /// </summary>
    public abstract class LogicManagerBase<T> : SingletonMonoBehaviour<T> where T : LogicManagerBase<T>
    {
        /// <summary>
        /// 注册事件的监听
        /// </summary>
        protected abstract void RegisterEventListener();
        /// <summary>
        /// 取消注册事件的监听
        /// </summary>
        protected abstract void CancelEventListener();

        protected virtual void OnEnable() => RegisterEventListener();

        protected virtual void OnDisable() => CancelEventListener();


    }
}