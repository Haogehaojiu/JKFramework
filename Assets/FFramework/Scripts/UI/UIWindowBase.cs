using System;
using UnityEngine;

namespace JKFramework
{
	/// <summary>
	/// 窗口结果
	/// </summary>
	public enum WindowResult
	{
		None,
		Yes,
		No
	}
	/// <summary>
	/// 窗口基类
	/// </summary>
	public class UIWindowBase : MonoBehaviour
	{
		//窗口类型
		public Type type => GetType();
		/// <summary>
		/// 初始化
		/// </summary>
		public virtual void Init() { }
		/// <summary>
		/// 显示
		/// </summary>
		public virtual void OnShow()
		{
			OnUpdateLanguage();
			RegisterEventListener();
		}
		/// <summary>
		/// 关闭
		/// </summary>
		public void Close() => UIManager.Instance.Close(type);
		public virtual void OnClose() => CancelEventListener();
		/// <summary>
		/// 点击否/取消
		/// </summary>
		public virtual void OnCloseClick() => Close();
		/// <summary>
		/// 点击是/确认 
		/// </summary>
		public virtual void OnYesClick() => Close();
		/// <summary>
		/// 注册事件
		/// </summary>
		protected virtual void RegisterEventListener() => EventManager.AddEventListener("UpdateLanguageType", OnUpdateLanguage);
		/// <summary>
		/// 取消事件
		/// </summary>
		protected virtual void CancelEventListener() => EventManager.RemoveEventListener("UpdateLanguageType", OnUpdateLanguage);
		/// <summary>
		/// 更新语言
		/// </summary>
		protected virtual void OnUpdateLanguage() { }
	}
}