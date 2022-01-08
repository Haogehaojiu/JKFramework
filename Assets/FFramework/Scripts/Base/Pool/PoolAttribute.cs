using System;
namespace JKFramework
{
    /// <summary>
    /// 确定一个类是否需要对象池
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class PoolAttribute : Attribute
    {
    }
}