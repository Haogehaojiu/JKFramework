using System;
namespace JKFramework
{
    /// <summary>
    /// UI元素的特性
    /// 每个UI窗口都应该添加，不允许多个
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class UIElementAttribute : Attribute
    {
        public bool isCache;
        public string resourcePath;
        public int layerLevel;
    
        public UIElementAttribute(bool isCache, string resourcePath, int layerLevel)
        {
            this.isCache = isCache;
            this.layerLevel = layerLevel;
            this.resourcePath = resourcePath;
        }
    }
}