using System;
using System.Collections.Generic;
namespace JKFramework
{
    /// <summary>
    /// 事件系统管理器
    /// </summary>
    public static class EventManager
    {
        #region 内部接口、内部类
        /// <summary>
        /// 事件信息接口
        /// </summary>
        private interface IEventInfo
        {
            void Destroy();
        }
        /// <summary>
        /// 无参的事件信息
        /// </summary>
        private class EventInfo : IEventInfo
        {
            public Action action;
            public void Init(Action action) => this.action = action;
            public void Destroy()
            {
                action = null;
                this.JKObjectPushPool();
            }
        }
        /// <summary>
        /// 一个参数的事件信息
        /// </summary>
        private class EventInfo<T> : IEventInfo
        {
            public Action<T> action;
            public void Init(Action<T> action) => this.action = action;
            public void Destroy()
            {
                action = null;
                this.JKObjectPushPool();
            }
        }
        /// <summary>
        /// 两个参数的事件信息
        /// </summary>
        private class EventInfo<T, K> : IEventInfo
        {
            public Action<T, K> action;
            public void Init(Action<T, K> action) => this.action = action;
            public void Destroy()
            {
                action = null;
                this.JKObjectPushPool();
            }
        }
        /// <summary>
        /// 三个参数事件信息
        /// </summary>
        private class EventInfo<T, K, L> : IEventInfo
        {
            public Action<T, K, L> action;
            public void Init(Action<T, K, L> action) => this.action = action;
            public void Destroy()
            {
                action = null;
                this.JKObjectPushPool();
            }
        }
        #endregion

        private static readonly Dictionary<string, IEventInfo> eventInfoDic = new Dictionary<string, IEventInfo>();

        #region 添加事件的监听,要关心的某个事件，当这个事件触发，会执行传过来的Action
        /// <summary>
        /// 添加无参事件监听器
        /// </summary>
        public static void AddEventListener(string eventName, Action action)
        {
            //有对应事件可以监听
            if (eventInfoDic.ContainsKey(eventName)) ((EventInfo)eventInfoDic[eventName]).action += action;
            //没有对应事件可以监听
            else
            {
                EventInfo eventInfo = PoolManager.Instance.GetObject<EventInfo>();
                eventInfo.Init(action);
                eventInfoDic.Add(eventName, eventInfo);
            }
        }
        /// <summary>
        /// 添加一个参事件监听器
        /// </summary>
        public static void AddEventListener<T>(string eventName, Action<T> action)
        {
            //有对应事件可以监听
            if (eventInfoDic.ContainsKey(eventName)) ((EventInfo<T>)eventInfoDic[eventName]).action += action;
            //没有对应事件可以监听
            else
            {
                EventInfo<T> eventInfo = PoolManager.Instance.GetObject<EventInfo<T>>();
                eventInfo.Init(action);
                eventInfoDic.Add(eventName, eventInfo);
            }
        }
        /// <summary>
        /// 添加两个参事件监听器
        /// </summary>
        public static void AddEventListener<T, K>(string eventName, Action<T, K> action)
        {
            //有对应事件可以监听
            if (eventInfoDic.ContainsKey(eventName))
                ((EventInfo<T, K>)eventInfoDic[eventName]).action += action;
            //没有对应事件可以监听
            else
            {
                EventInfo<T, K> eventInfo = PoolManager.Instance.GetObject<EventInfo<T, K>>();
                eventInfo.Init(action);
                eventInfoDic.Add(eventName, eventInfo);
            }
        }
        /// <summary>
        /// 添加三个参事件监听器
        /// </summary>
        public static void AddEventListener<T, K, L>(string eventName, Action<T, K, L> action)
        {
            //有对应事件可以监听
            if (eventInfoDic.ContainsKey(eventName))
                ((EventInfo<T, K, L>)eventInfoDic[eventName]).action += action;
            //没有对应事件可以监听
            else
            {
                EventInfo<T, K, L> eventInfo = PoolManager.Instance.GetObject<EventInfo<T, K, L>>();
                eventInfo.Init(action);
                eventInfoDic.Add(eventName, eventInfo);
            }
        }
        #endregion

        #region 触发事件
        /// <summary>
        /// 触发无参事件
        /// </summary>
        public static void EventTrigger(string eventName)
        {
            if (eventInfoDic.ContainsKey(eventName)) ((EventInfo)eventInfoDic[eventName]).action?.Invoke();
        }
        /// <summary>
        /// 触发一个参数事件
        /// </summary>
        public static void EventTrigger<T>(string eventName, T args)
        {
            if (eventInfoDic.ContainsKey(eventName)) ((EventInfo<T>)eventInfoDic[eventName]).action?.Invoke(args);
        }
        /// <summary>
        /// 触发两个参数事件
        /// </summary>
        public static void EventTrigger<T, K>(string eventName, T args1, K args2)
        {
            if (eventInfoDic.ContainsKey(eventName)) ((EventInfo<T, K>)eventInfoDic[eventName]).action?.Invoke(args1, args2);
        }
        /// <summary>
        /// 触发三个参数事件
        /// </summary>
        public static void EventTrigger<T, K, L>(string eventName, T args1, K args2, L args3)
        {
            if (eventInfoDic.ContainsKey(eventName)) ((EventInfo<T, K, L>)eventInfoDic[eventName]).action?.Invoke(args1, args2, args3);
        }
        #endregion

        #region 取消事件的监听
        /// <summary>
        /// 取消一个无参事件的监听
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static void RemoveEventListener(string eventName, Action action)
        {
            if (eventInfoDic.ContainsKey(eventName)) ((EventInfo)eventInfoDic[eventName]).action -= action;
        }
        /// <summary>
        /// 取消一个一个参数事件的监听
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static void RemoveEventListener<T>(string eventName, Action<T> action)
        {
            if (eventInfoDic.ContainsKey(eventName)) ((EventInfo<T>)eventInfoDic[eventName]).action -= action;
        }
        /// <summary>
        /// 取消一个两个参数事件的监听
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static void RemoveEventListener<T, K>(string eventName, Action<T, K> action)
        {
            if (eventInfoDic.ContainsKey(eventName)) ((EventInfo<T, K>)eventInfoDic[eventName]).action -= action;
        }
        /// <summary>
        /// 取消一个三个参数事件的监听
        /// </summary>
        /// <param name="eventName"></param>
        /// <returns></returns>
        public static void RemoveEventListener<T, K, L>(string eventName, Action<T, K, L> action)
        {
            if (eventInfoDic.ContainsKey(eventName)) ((EventInfo<T, K, L>)eventInfoDic[eventName]).action -= action;
        }
        #endregion

        #region 移除事件
        /// <summary>
        /// 移除一个事件
        /// </summary>
        /// <param name="eventName"></param>
        public static void RemoveEventListener(string eventName)
        {
            if (eventInfoDic.ContainsKey(eventName))
            {
                eventInfoDic[eventName].Destroy();
                eventInfoDic.Remove(eventName);
            }
        }
        /// <summary>
        /// 清空事件中心(一般不需要使用)
        /// </summary>
        public static void Clear()
        {
            foreach(var eventName in eventInfoDic.Keys) eventInfoDic[eventName].Destroy();
            eventInfoDic.Clear();
        }
        #endregion
    }
}