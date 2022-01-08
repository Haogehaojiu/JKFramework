using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JKFramework
{
    public enum FEventType
    {
        OnPointerEnter,
        OnPointerClick,
        OnPointerExit,
        OnPointerDown,
        OnPointerUp,
        OnDrag,
        OnBeginDrag,
        OnEndDrag,
        OnCollisionEnter,
        OnCollisionStay,
        OnCollisionExit,
        OnCollisionEnter2D,
        OnCollisionStay2D,
        OnCollisionExit2D,
        OnTriggerEnter,
        OnTriggerStay,
        OnTriggerExit,
        OnTriggerEnter2D,
        OnTriggerStay2D,
        OnTriggerExit2D
    }

    public interface IMouseEvent : IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
    }

    /// <summary>
    /// 事件工具
    /// 可以添加鼠标、碰撞、触发等时间
    /// </summary>
    public class JKEventListener : MonoBehaviour, IMouseEvent
    {
        #region 内部类、接口等
        /// <summary>
        /// 某个事件中，一个事件的数据包装类
        /// </summary>
        private class FEventListenerEventInfo<T>
        {
            /// <summary>
            /// T：事件本身的参数（PointerEventData、Collision、Collision2D、Collider、Collider2D）
            /// </summary>
            public Action<T, object[]> action;
            public object[] args;
            public void Init(Action<T, object[]> action, object[] args)
            {
                this.action = action;
                this.args = args;
            }
            public void Destroy()
            {
                action = null;
                args = null;
                this.JKObjectPushPool();
            }
            public void TriggerEvent(T eventData) { action?.Invoke(eventData, args); }
        }
        private interface IFEventListenerEventInfos
        {
            void RemoveAll();
        }
        /// <summary>
        /// 一类事件的数据包装，包含多个FEventListenerEventInfo
        /// </summary>
        private class FEventListenerEventInfos<T> : IFEventListenerEventInfos
        {
            //所有的事件
            private List<FEventListenerEventInfo<T>> eventList = new List<FEventListenerEventInfo<T>>();
            /// <summary>
            /// 添加事件（监听器）
            /// </summary>
            public void AddListener(Action<T, object[]> action, params object[] args)
            {
                FEventListenerEventInfo<T> info = PoolManager.Instance.GetObject<FEventListenerEventInfo<T>>();
                info.Init(action, args);
                eventList.Add(info);
            }
            /// <summary>
            /// 移除事件（监听器）
            /// </summary>
            public void RemoveListener(Action<T, object[]> action, bool checkArgs = false, params object[] args)
            {
                for(var i = 0; i < eventList.Count; i++)
                {
                    if (eventList[i].action.Equals(action))
                    {
                        //是否需要检查参数
                        if (checkArgs && args.Length > 0)
                        {
                            //参数如果相等
                            if (args.ArrayEquals(eventList[i].args))
                            {
                                //移除
                                eventList[i].Destroy();
                                eventList.RemoveAt(i);
                                return;
                            }
                        } else
                        {
                            //移除全部action
                            eventList[i].Destroy();
                            eventList.RemoveAt(i);
                            return;
                        }
                    }
                }
            }
            /// <summary>
            /// 移除全部事件（监听器）,放入对象池
            /// </summary>
            public void RemoveAll()
            {
                for(var i = 0; i < eventList.Count; i++) eventList[i].Destroy();
                eventList.Clear();
                this.JKObjectPushPool();
            }
            public void TriggerEvent(T eventData)
            {
                for(var i = 0; i < eventList.Count; i++) eventList[i].TriggerEvent(eventData);
            }
        }
        /// <summary>
        /// 比较器
        /// </summary>
        private class FEventTypeEnumComparer : Singleton<FEventTypeEnumComparer>, IEqualityComparer<FEventType>
        {

            public bool Equals(FEventType x, FEventType y) { return x == y; }
            public int GetHashCode(FEventType obj) { return (int)obj; }
        }
        #endregion

        private Dictionary<FEventType, IFEventListenerEventInfos> eventInfosDic = new Dictionary<FEventType, IFEventListenerEventInfos>(FEventTypeEnumComparer.Instance);

        #region 外部访问
        /// <summary>
        /// 添加事件
        /// </summary>
        public void AddListener<T>(FEventType eventType, Action<T, object[]> action, params object[] args)
        {
            if (eventInfosDic.ContainsKey(eventType)) (eventInfosDic[eventType] as FEventListenerEventInfos<T>)?.AddListener(action, args);
            else
            {
                FEventListenerEventInfos<T> infos = PoolManager.Instance.GetObject<FEventListenerEventInfos<T>>();
                infos.AddListener(action, args);
                eventInfosDic.Add(eventType, infos);
            }
        }
        /// <summary>
        /// 移除事件
        /// </summary>
        public void RemoveListener<T>(FEventType eventType, Action<T, object[]> action, bool checkArgs = false, params object[] args)
        {
            if (eventInfosDic.ContainsKey(eventType)) (eventInfosDic[eventType] as FEventListenerEventInfos<T>)?.RemoveListener(action, checkArgs, args);
        }
        /// <summary>
        /// 移除某事件类型全部
        /// </summary>
        public void RemoveAllListener(FEventType eventType)
        {
            if (eventInfosDic.ContainsKey(eventType))
            {
                eventInfosDic[eventType].RemoveAll();
                eventInfosDic.Remove(eventType);
            }
        }
        /// <summary>
        /// 移除全部事件
        /// </summary>
        public void RemoveAllListener()
        {
            foreach(IFEventListenerEventInfos infos in eventInfosDic.Values) infos.RemoveAll();
            eventInfosDic.Clear();
        }
        #endregion

        /// <summary>
        /// 触发事件
        /// </summary>
        private void TriggerEvent<T>(FEventType eventType, T eventData)
        {
            if (eventInfosDic.ContainsKey(eventType)) (eventInfosDic[eventType] as FEventListenerEventInfos<T>)?.TriggerEvent(eventData);
        }

        #region 鼠标事件
        public void OnPointerEnter(PointerEventData eventData) => TriggerEvent(FEventType.OnPointerEnter, eventData);
        public void OnPointerClick(PointerEventData eventData) => TriggerEvent(FEventType.OnPointerClick, eventData);
        public void OnPointerExit(PointerEventData eventData) => TriggerEvent(FEventType.OnPointerExit, eventData);
        public void OnPointerDown(PointerEventData eventData) => TriggerEvent(FEventType.OnPointerDown, eventData);
        public void OnPointerUp(PointerEventData eventData) => TriggerEvent(FEventType.OnPointerUp, eventData);
        public void OnBeginDrag(PointerEventData eventData) => TriggerEvent(FEventType.OnBeginDrag, eventData);
        public void OnEndDrag(PointerEventData eventData) => TriggerEvent(FEventType.OnEndDrag, eventData);
        public void OnDrag(PointerEventData eventData) => TriggerEvent(FEventType.OnDrag, eventData);
        #endregion

        #region 碰撞事件
        private void OnCollisionEnter(Collision other) => TriggerEvent(FEventType.OnCollisionEnter, other);
        private void OnCollisionStay(Collision other) => TriggerEvent(FEventType.OnCollisionStay, other);
        private void OnCollisionExit(Collision other) => TriggerEvent(FEventType.OnCollisionExit, other);
        private void OnCollisionEnter2D(Collision2D other) => TriggerEvent(FEventType.OnCollisionEnter2D, other);
        private void OnCollisionStay2D(Collision2D other) => TriggerEvent(FEventType.OnCollisionStay2D, other);
        private void OnCollisionExit2D(Collision2D other) => TriggerEvent(FEventType.OnCollisionExit2D, other);
        #endregion

        #region 触发事件
        private void OnTriggerEnter(Collider other) => TriggerEvent(FEventType.OnTriggerEnter, other);
        private void OnTriggerStay(Collider other) => TriggerEvent(FEventType.OnTriggerStay, other);
        private void OnTriggerExit(Collider other) => TriggerEvent(FEventType.OnTriggerExit, other);
        private void OnTriggerEnter2D(Collider2D other) => TriggerEvent(FEventType.OnTriggerEnter2D, other);
        private void OnTriggerStay2D(Collider2D other) => TriggerEvent(FEventType.OnTriggerStay2D, other);
        private void OnTriggerExit2D(Collider2D other) => TriggerEvent(FEventType.OnTriggerExit2D, other);
        #endregion
    }
}