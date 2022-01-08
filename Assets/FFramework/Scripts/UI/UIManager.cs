using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace JKFramework
{
    public class UIManager : ManagerBase<UIManager>
    {
        #region 内部类、接口
        [Serializable]
        private class UILayer
        {
            public Transform root;
            public Image maskImage;
            private int count;
            public void OnShow()
            {
                ++count;
                Update();
            }
            public void OnClose()
            {
                --count;
                Update();
            }
            private void Update()
            {
                maskImage.raycastTarget = count != 0;
                var posIndex = root.childCount - 2;
                maskImage.transform.SetSiblingIndex(posIndex < 0 ? 0 : posIndex);
            }
        }
        #endregion
        /// <summary>
        /// 元素资源k库
        /// </summary>
        public Dictionary<Type, UIElement> UIElementDic => GameRoot.Instance.GameSetting.uiElementsDic;

        private const string TipLocalizationPackageName = "Tip";
        [SerializeField]
        private UILayer[] UILayers;

        // 提示窗
        [SerializeField]
        private UITips uiTips;
        public override void Init()
        {
            base.Init();
            uiTips = transform.Find("System/UIRoot/Tips").GetComponent<UITips>();
        }
        /// <summary>
        /// 展示提示信息
        /// </summary>
        /// <param name="info"></param>
        public void AddTip(string info) => uiTips.AddTip(info);
        /// <summary>
        /// 基于本地化进行展示提示信息
        /// </summary>
        /// <param name="info"></param>
        public void AddTipByLocalization(string tipKeyName) => uiTips.AddTip(LocalizationManager.Instance.GetContent<L_Text>(TipLocalizationPackageName, tipKeyName).content);

        /// <summary>
        /// 显示窗口
        /// </summary>
        /// <param name="layer">层级，-1为不设置</param>
        /// <typeparam name="T">窗口类型</typeparam>
        public T Show<T>(int layer = -1) where T : UIWindowBase => Show(typeof(T), layer) as T;
        /// <summary>
        /// 显示窗口 
        /// </summary>
        /// <param name="type">窗口类型</param>
        /// <param name="layer">层级，-1为不设置</param>
        public UIWindowBase Show(Type type, int layer = -1)
        {
            if (UIElementDic.ContainsKey(type))
            {
                UIElement uiElement = UIElementDic[type];
                var layerNumber = layer == -1 ? uiElement.layerLevel : layer;
                //获取/实例化对象，保证窗口实例存在
                if (uiElement.objInstance != null)
                {
                    uiElement.objInstance.gameObject.SetActive(true);
                    uiElement.objInstance.transform.SetParent(UILayers[layerNumber].root);
                    uiElement.objInstance.transform.SetAsLastSibling();
                    uiElement.objInstance.OnShow();
                } else
                {
                    UIWindowBase window = ResourcesManager.InstantiatePrefab(uiElement.prefab, UILayers[layerNumber].root).GetComponent<UIWindowBase>();
                    uiElement.objInstance = window;
                    window.Init();
                    window.OnShow();
                }
                uiElement.layerLevel = layerNumber;
                UILayers[layerNumber].OnShow();
                return uiElement.objInstance;
            }
            //资源库中没有，意味着不允许显示
            return null;
        }
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <typeparam name="T">窗口类型</typeparam>
        public void Close<T>() => Close(typeof(T));
        /// <summary>
        /// 关闭窗口
        /// </summary>
        /// <param name="type">窗口类型</param>
        public void Close(Type type)
        {
            if (UIElementDic.ContainsKey(type))
            {
                UIElement uiElement = UIElementDic[type];
                if (uiElement.objInstance == null) return;
                uiElement.objInstance.OnClose();
                //需要缓存则隐藏
                if (uiElement.isCache)
                {
                    uiElement.objInstance.transform.SetAsFirstSibling();
                    uiElement.objInstance.gameObject.SetActive(false);
                }
                //不需要缓存则销毁
                else
                {
                    Destroy(uiElement.objInstance.gameObject);
                    uiElement.objInstance = null;
                }
                UILayers[uiElement.layerLevel].OnClose();
            }

        }
        /// <summary>
        /// 关闭全部窗口
        /// </summary>
        public void CloseAll()
        {
            //处理缓存中所有状态的逻辑
            Dictionary<Type, UIElement>.Enumerator enumerator = UIElementDic.GetEnumerator();
            while(enumerator.MoveNext())
                if (enumerator.Current.Value.objInstance != null && enumerator.Current.Value.objInstance.gameObject.activeSelf)
                    enumerator.Current.Value.objInstance.Close();
        }
    }
}