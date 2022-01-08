using JKFramework;
using UnityEngine;
using UnityEngine.UI;

[UIElement(true, "UI/newGameWindow", 1)]
public class UINewGameWindow : UIWindowBase
{
    [SerializeField]
    private Text userNameText;
    [SerializeField]
    private Text placeholderText;

    [SerializeField]
    private InputField userNameInputField;

    [SerializeField]
    private Button playButton;
    [SerializeField]
    private Button backButton;

    [SerializeField]
    private Text playText;
    [SerializeField]
    private Text backText;

    private const string localSetPackName = "UINewGameWindow";

    public override void Init()
    {
        base.Init();
        playButton.onClick.AddListener(OnYesClick);
        backButton.onClick.AddListener(OnCloseClick);
    }
    protected override void OnUpdateLanguage()
    {
        userNameText.JKLocalSet(localSetPackName, "userNameText");
        placeholderText.JKLocalSet(localSetPackName, "placeholderText");
        playText.JKLocalSet(localSetPackName, "playText");
        backText.JKLocalSet(localSetPackName, "backText");
    }
    public override void OnClose()
    {
        userNameInputField.text = "";
        base.OnClose();
    }
    public override void OnYesClick()
    {
        if (userNameInputField.text.Length < 1)
        {
            AudioManager.Instance.PlayOnShot("Audio/Button", UIManager.Instance);
            UIManager.Instance.AddTipByLocalization("checkName");
        }
        else
        {
            AudioManager.Instance.PlayOnShot("Audio/Button", UIManager.Instance);
            EventManager.EventTrigger("CreateNewSaveAndEnterGame", userNameInputField.text);
            base.OnYesClick();
        }
    }
    public override void OnCloseClick()
    {
        AudioManager.Instance.PlayOnShot("Audio/Button", UIManager.Instance);
        base.OnCloseClick();
    }
}