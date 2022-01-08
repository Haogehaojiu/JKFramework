using System.Collections.Generic;
using System.Linq;
using JKFramework;
using UnityEngine;
using UnityEngine.UI;

[UIElement(true, "UI/rankWindow", 1)]
public class UIRankWindow : UIWindowBase
{
    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Transform itemParent;
    private readonly List<UIRankItem> uiRankItemList = new List<UIRankItem>();
    private bool isNeedUpdate = true;
    public override void Init() => closeButton.onClick.AddListener(Close);
    public override void OnShow()
    {
        base.OnShow();
        if (isNeedUpdate) UpdateAllRankItem();
    }
    public override void OnClose()
    {
        AudioManager.Instance.PlayOnShot("Audio/Button", UIManager.Instance);
        base.OnClose();
    }
    protected override void RegisterEventListener()
    {
        base.RegisterEventListener();
        EventManager.AddEventListener("UpdateRankItem", UpdateRankItemFlag);
    }
    private void UpdateRankItemFlag()
    {
        isNeedUpdate = true;
        //如果我当前是激活状态，那么立即刷新所有存档数据，而不是等到下次打开刷新
        if (gameObject.activeInHierarchy) UpdateAllRankItem();
    }
    /// <summary>
    /// 更新全部排行信息
    /// </summary>
    private void UpdateAllRankItem()
    {
        //清空已有的存档
        for(var i = 0; i < uiRankItemList.Count; i++) uiRankItemList[i].Destroy();
        uiRankItemList.Clear();
        //获取所有存档
        List<SaveItem> saveItemList = SaveManager.GetAllSaveItem();
        List<UserData> userDataList = new List<UserData>(saveItemList.Count);
        //获取所有存档对应的用户数据
        for(var i = 0; i < saveItemList.Count; i++) userDataList.Add(SaveManager.LoadObject<UserData>(saveItemList[i]));
        //对用户数据进行排序
        userDataList = userDataList.OrderByDescending(userData => userData.score).ToList();
        //实例化所有Item
        for(var i = 0; i < userDataList.Count; i++)
        {
            var userData = userDataList[i];
            UIRankItem item = ResourcesManager.Load<UIRankItem>("UI/rankItem", itemParent);
            item.Init(userData, i + 1);
            uiRankItemList.Add(item);
        }

        //已更新全部存档，频繁切存档窗口，不需要频繁更新存档
        isNeedUpdate = false;
    }

}