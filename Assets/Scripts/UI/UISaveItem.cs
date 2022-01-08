using JKFramework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[Pool]
public class UISaveItem : MonoBehaviour
{
    private SaveItem saveItem;
    private UserData userData;

    [SerializeField]
    private Image saveItemImage;
    [SerializeField]
    private Text userNameText;
    [SerializeField]
    private Text scoreText;
    [SerializeField]
    private Text timeText;
    [SerializeField]
    private Text deleteText;
    [SerializeField]
    private Button deleteButton;

    private static Color normalColor = new Color(0.33f, 0.66f, 1f, 0.33f);
    private static Color selectColor = new Color(0.33f, 0.66f, 1f, 0.66f);

    private void Start()
    {
        deleteButton.onClick.AddListener(DeleteButtonClick);
        this.OnPointerEnter(MouseEnter);
        this.OnPointerExit(MouseExit);
        this.OnPointerClick(Click);
    }
    private void OnEnable() => deleteText.JKLocalSet("UISaveWindow", "delete");
    public void Init(SaveItem saveItem)
    {
        this.saveItem = saveItem;
        timeText.text = saveItem.lastSaveTime.ToString("g");
        //用户数据
        userData = SaveManager.LoadObject<UserData>(saveItem);
        userNameText.text = userData.userName;
        scoreText.text = userData.score.ToString();
    }
    private void Click(PointerEventData arg1, object[] arg2)
    {
        AudioManager.Instance.PlayOnShot("Audio/Button", UIManager.Instance);
        EventManager.EventTrigger("EnterGame", saveItem, userData);
    }
    private void MouseEnter(PointerEventData arg1, object[] arg2)
    {
        AudioManager.Instance.PlayOnShot("Audio/Button", UIManager.Instance);
        saveItemImage.color = selectColor;
    }
    private void MouseExit(PointerEventData arg1, object[] arg2)
    {
        AudioManager.Instance.PlayOnShot("Audio/Button", UIManager.Instance);
        saveItemImage.color = normalColor;
    }
    private void DeleteButtonClick()
    {
        AudioManager.Instance.PlayOnShot("Audio/Button", UIManager.Instance);
        SaveManager.DeleteSaveItem(saveItem);
        EventManager.EventTrigger("UpdateSaveItem");
        EventManager.EventTrigger("UpdateRankItem");
    }
    public void Destroy()
    {
        saveItem = null;
        userData = null;
        this.JKGameObjectPushPool();
    }
}