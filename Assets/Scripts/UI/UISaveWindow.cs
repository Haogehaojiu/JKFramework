using System.Collections.Generic;
using JKFramework;
using UnityEngine;
using UnityEngine.UI;

[UIElement(true, "UI/saveWindow", 1)]
public class UISaveWindow : UIWindowBase
{
    [SerializeField]
    private Button closeButton;

    [SerializeField]
    private Transform itemParent;

    private List<UISaveItem> uiSaveItemList = new List<UISaveItem>();

    private bool isNeedUpdate = true;
    public override void Init()
    {
        base.Init();
        closeButton.onClick.AddListener(Close);
    }
    public override void OnShow()
    {
        base.OnShow();
        if (isNeedUpdate) UpdateAllSaveItem();
    }
    public override void OnClose()
    {
        AudioManager.Instance.PlayOnShot("Audio/Button", UIManager.Instance);
        base.OnClose();
    }
    protected override void RegisterEventListener()
    {
        base.RegisterEventListener();
        EventManager.AddEventListener("UpdateSaveItem", UpdateSaveItemFlag);
    }
    private void UpdateSaveItemFlag()
    {
        isNeedUpdate = true;
        //如果我当前是激活状态，那么立即刷新所有存档数据，而不是等到下次打开刷新
        if (gameObject.activeInHierarchy) UpdateAllSaveItem();
    }
    /// <summary>
    /// 更新全部存档项
    /// </summary>
    private void UpdateAllSaveItem()
    {
        //清空已有的存档
        for(var i = 0; i < uiSaveItemList.Count; i++) uiSaveItemList[i].Destroy();
        uiSaveItemList.Clear();
        //放置新的
        List<SaveItem> saveItems = SaveManager.GetAllSaveItemByUpdateTime();
        for(var i = 0; i < saveItems.Count; i++)
        {
            UISaveItem item = ResourcesManager.Load<UISaveItem>("UI/saveItem", itemParent);
            item.Init(saveItems[i]);
            uiSaveItemList.Add(item);
        }
        //已更新全部存档，频繁切存档窗口，不需要频繁更新存档
        isNeedUpdate = false;
    }
}