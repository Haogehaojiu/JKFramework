using System;
using System.Collections.Generic;
using System.Reflection;
using Sirenix.OdinInspector;
using UnityEngine;

namespace JKFramework
{
    /// <summary>
    /// 框架层面的设置
    /// 对象缓存池设置、UI元素的设置、默认分辨率等
    /// </summary>
    [CreateAssetMenu(fileName = "GameSetting", menuName = "JKFramework/Config/GameSetting")]
    public class GameSetting : ConfigBase
    {
        [LabelText("对象池设置")]
        [DictionaryDrawerSettings(KeyLabel = "类型", ValueLabel = "皆可缓存")]
        public Dictionary<Type, bool> CacheDictionary = new Dictionary<Type, bool>();

        [LabelText("UI窗口设置")]
        [DictionaryDrawerSettings(KeyLabel = "类型", ValueLabel = "UI窗口数据")]
        public Dictionary<Type, UIElement> uiElementsDic = new Dictionary<Type, UIElement>();

// #if UNITY_EDITOR
        /// <summary>
        /// 编译时执行函数
        /// </summary>
        [Button(Name = "Init", ButtonHeight = 50)]
        [GUIColor(0, 0, 1)]
        public void InitForEditor()
        {
            PoolAttributeOnEditor();
            UIElementAttributeOnEditor();
        }
        /// <summary>
        /// 将有PoolAttribute特性的对象加入缓存池字典
        /// </summary>
        private void PoolAttributeOnEditor()
        {
            CacheDictionary.Clear();
            //获取所有程序集
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            //遍历程序集
            foreach(Assembly assembly in assemblies)
            {
                //遍历程序集下的每一个类型
                Type[] types = assembly.GetTypes();
                foreach(Type type in types)
                {
                    // 获取PoolAttribute特性
                    PoolAttribute poolAttribute = type.GetCustomAttribute<PoolAttribute>();
                    if (poolAttribute != null) CacheDictionary.Add(type, true);
                }
            }
        }
        private void UIElementAttributeOnEditor()
        {
            uiElementsDic.Clear();
            //获取所有程序集
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var baseType = typeof(UIWindowBase);
            //遍历程序集
            foreach(Assembly assembly in assemblies)
            {
                //遍历程序集下的每一个类型
                Type[] types = assembly.GetTypes();
                foreach(Type type in types)
                {
                    if (baseType.IsAssignableFrom(type) && !type.IsAbstract)
                    {
                        UIElementAttribute uiElementAttribute = type.GetCustomAttribute<UIElementAttribute>();
                        if (uiElementAttribute != null)
                        {
                            uiElementsDic.Add(type, new UIElement
                            {
                                isCache = uiElementAttribute.isCache,
                                prefab = Resources.Load<GameObject>(uiElementAttribute.resourcePath),
                                layerLevel = uiElementAttribute.layerLevel
                            });
                        }
                    }
                }
            }
        }
// #endif
    }
}