using System;
using UnityEngine;
using UnityEngine.EventSystems;
namespace JKFramework
{
    public static class JKEventListenerExtend
    {
        #region 工具函数
        private static JKEventListener GetOrAddEventListener(Component component)
        {
            var listener = component.GetComponent<JKEventListener>();
            if (listener == null) return component.gameObject.AddComponent<JKEventListener>();
            return listener;
        }
        public static void AddEventListener<T>(this Component component, FEventType eventType, Action<T, object[]> action, params object[] args)
        {
            var listener = GetOrAddEventListener(component);
            listener.AddListener(eventType, action, args);
        }
        public static void RemoveEventListener<T>(this Component component, FEventType eventType, Action<T, object[]> action, bool checkArgs = false, params object[] args)
        {
            var listener = GetOrAddEventListener(component);
            listener.RemoveListener(eventType, action, checkArgs, args);
        }
        public static void RemoveAllListener(this Component component, FEventType eventType)
        {
            var listener = GetOrAddEventListener(component);
            listener.RemoveAllListener(eventType);
        }
        public static void RemoveAllListener(this Component component)
        {
            var listener = GetOrAddEventListener(component);
            listener.RemoveAllListener();
        }
        #endregion

        #region 鼠标相关事件
        public static void OnPointerEnter(this Component component, Action<PointerEventData, object[]> action, params object[] args) { AddEventListener(component, FEventType.OnPointerEnter, action, args); }
        public static void OnPointerExit(this Component component, Action<PointerEventData, object[]> action, params object[] args) { AddEventListener(component, FEventType.OnPointerExit, action, args); }
        public static void OnPointerClick(this Component component, Action<PointerEventData, object[]> action, params object[] args) { AddEventListener(component, FEventType.OnPointerClick, action, args); }
        public static void OnPointerDown(this Component component, Action<PointerEventData, object[]> action, params object[] args) { AddEventListener(component, FEventType.OnPointerDown, action, args); }
        public static void OnPointerUp(this Component component, Action<PointerEventData, object[]> action, params object[] args) { AddEventListener(component, FEventType.OnPointerUp, action, args); }
        public static void OnDrag(this Component component, Action<PointerEventData, object[]> action, params object[] args) { AddEventListener(component, FEventType.OnDrag, action, args); }
        public static void OnBeginDrag(this Component component, Action<PointerEventData, object[]> action, params object[] args) { AddEventListener(component, FEventType.OnBeginDrag, action, args); }
        public static void OnEndDrag(this Component component, Action<PointerEventData, object[]> action, params object[] args) { AddEventListener(component, FEventType.OnEndDrag, action, args); }
        public static void RemovePointerClick(this Component component, Action<PointerEventData, object[]> action, bool checkArgs = false, params object[] args) { RemoveEventListener(component, FEventType.OnPointerClick, action, checkArgs, args); }
        public static void RemovePointerDown(this Component component, Action<PointerEventData, object[]> action, bool checkArgs = false, params object[] args) { RemoveEventListener(component, FEventType.OnPointerDown, action, checkArgs, args); }
        public static void RemovePointerUp(this Component component, Action<PointerEventData, object[]> action, bool checkArgs = false, params object[] args) { RemoveEventListener(component, FEventType.OnPointerUp, action, checkArgs, args); }
        public static void RemoveDrag(this Component component, Action<PointerEventData, object[]> action, bool checkArgs = false, params object[] args) { RemoveEventListener(component, FEventType.OnDrag, action, checkArgs, args); }
        public static void RemoveBeginDrag(this Component component, Action<PointerEventData, object[]> action, bool checkArgs = false, params object[] args) { RemoveEventListener(component, FEventType.OnBeginDrag, action, checkArgs, args); }
        public static void RemoveEndDrag(this Component component, Action<PointerEventData, object[]> action, bool checkArgs = false, params object[] args) { RemoveEventListener(component, FEventType.OnEndDrag, action, checkArgs, args); }
        #endregion

        #region 碰撞相关事件
        public static void OnCollisionEnter(this Component component, Action<Collision, object[]> action, params object[] args) { AddEventListener(component, FEventType.OnCollisionEnter, action, args); }
        public static void OnCollisionStay(this Component component, Action<Collision, object[]> action, params object[] args) { AddEventListener(component, FEventType.OnCollisionStay, action, args); }
        public static void OnCollisionExit(this Component component, Action<Collision, object[]> action, params object[] args) { AddEventListener(component, FEventType.OnCollisionExit, action, args); }
        public static void OnCollisionEnter2D(this Component component, Action<Collision, object[]> action, params object[] args) { AddEventListener(component, FEventType.OnCollisionEnter2D, action, args); }
        public static void OnCollisionStay2D(this Component component, Action<Collision, object[]> action, params object[] args) { AddEventListener(component, FEventType.OnCollisionStay2D, action, args); }
        public static void OnCollisionExit2D(this Component component, Action<Collision, object[]> action, params object[] args) { AddEventListener(component, FEventType.OnCollisionExit2D, action, args); }
        public static void RemoveCollisionEnter(this Component component, Action<Collision, object[]> action, bool checkArgs = false, params object[] args) { RemoveEventListener(component, FEventType.OnCollisionEnter, action, checkArgs, args); }
        public static void RemoveCollisionStay(this Component component, Action<Collision, object[]> action, bool checkArgs = false, params object[] args) { RemoveEventListener(component, FEventType.OnCollisionStay, action, checkArgs, args); }
        public static void RemoveCollisionExit(this Component component, Action<Collision, object[]> action, bool checkArgs = false, params object[] args) { RemoveEventListener(component, FEventType.OnCollisionExit, action, checkArgs, args); }
        public static void RemoveCollisionEnter2D(this Component component, Action<Collision2D, object[]> action, bool checkArgs = false, params object[] args) { RemoveEventListener(component, FEventType.OnCollisionEnter2D, action, checkArgs, args); }
        public static void RemoveCollisionStay2D(this Component component, Action<Collision2D, object[]> action, bool checkArgs = false, params object[] args) { RemoveEventListener(component, FEventType.OnCollisionStay2D, action, checkArgs, args); }
        public static void RemoveCollisionExit2D(this Component component, Action<Collision2D, object[]> action, bool checkArgs = false, params object[] args) { RemoveEventListener(component, FEventType.OnCollisionExit2D, action, checkArgs, args); }
        #endregion

        #region 触发相关事件
        public static void OnTriggerEnter(this Component component, Action<Collider, object[]> action, params object[] args) { AddEventListener(component, FEventType.OnTriggerEnter, action, args); }
        public static void OnTriggerStay(this Component component, Action<Collider, object[]> action, params object[] args) { AddEventListener(component, FEventType.OnTriggerStay, action, args); }
        public static void OnTriggerExit(this Component component, Action<Collider, object[]> action, params object[] args) { AddEventListener(component, FEventType.OnTriggerExit, action, args); }
        public static void OnTriggerEnter2D(this Component component, Action<Collider, object[]> action, params object[] args) { AddEventListener(component, FEventType.OnTriggerEnter2D, action, args); }
        public static void OnTriggerStay2D(this Component component, Action<Collider, object[]> action, params object[] args) { AddEventListener(component, FEventType.OnTriggerStay2D, action, args); }
        public static void OnTriggerExit2D(this Component component, Action<Collider, object[]> action, params object[] args) { AddEventListener(component, FEventType.OnTriggerExit2D, action, args); }
        public static void RemoveTriggerEnter(this Component component, Action<Collider, object[]> action, bool checkArgs = false, params object[] args) { RemoveEventListener(component, FEventType.OnTriggerEnter, action, checkArgs, args); }
        public static void RemoveTriggerStay(this Component component, Action<Collider, object[]> action, bool checkArgs = false, params object[] args) { RemoveEventListener(component, FEventType.OnTriggerStay, action, checkArgs, args); }
        public static void RemoveTriggerExit(this Component component, Action<Collider, object[]> action, bool checkArgs = false, params object[] args) { RemoveEventListener(component, FEventType.OnTriggerExit, action, checkArgs, args); }
        public static void RemoveTriggerEnter2D(this Component component, Action<Collider2D, object[]> action, bool checkArgs = false, params object[] args) { RemoveEventListener(component, FEventType.OnTriggerEnter2D, action, checkArgs, args); }
        public static void RemoveTriggerStay2D(this Component component, Action<Collider2D, object[]> action, bool checkArgs = false, params object[] args) { RemoveEventListener(component, FEventType.OnTriggerStay2D, action, checkArgs, args); }
        public static void RemoveTriggerExit2D(this Component component, Action<Collider2D, object[]> action, bool checkArgs = false, params object[] args) { RemoveEventListener(component, FEventType.OnTriggerExit2D, action, checkArgs, args); }
        #endregion


    }
}