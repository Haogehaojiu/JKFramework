using UnityEngine;
using JKFramework;
using UnityEngine.UI;

[UIElement(true,"UI/mainMenuWindow",0)]
public class UIMainMenuWindow : UIWindowBase
{
    [SerializeField] private Button newGameButton;
    [SerializeField] private Button rankButton;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button settingButton;
    [SerializeField] private Button quitButton;
    
    [SerializeField] private Text titleText;
    [SerializeField] private Text newGameText;
    [SerializeField] private Text rankText;
    [SerializeField] private Text continueText;
    [SerializeField] private Text settingText;
    [SerializeField] private Text quitText;

    private const string localSetPackName = "UIMainWindow";
    
    public override void Init()
    {
        base.Init();
        newGameButton.onClick.AddListener(NewGameButtonClick);
        rankButton.onClick.AddListener(RankButtonClick);
        continueButton.onClick.AddListener(ContinueButtonClick);
        settingButton.onClick.AddListener(SettingButtonClick);
        quitButton.onClick.AddListener(QuitButtonClick);
    }
    protected override void OnUpdateLanguage()
    {
        titleText.JKLocalSet(localSetPackName, "titleText");        
        newGameText.JKLocalSet(localSetPackName, "newGameText");        
        continueText.JKLocalSet(localSetPackName, "continueText");        
        rankText.JKLocalSet(localSetPackName, "rankText");        
        settingText.JKLocalSet(localSetPackName, "settingText");        
        quitText.JKLocalSet(localSetPackName, "quitText");        
    }
    /// <summary>
    /// 播放按钮音效
    /// </summary>
    private void PlayerButtonAudio() => AudioManager.Instance.PlayOnShot("Audio/Button",UIManager.Instance);
    private void NewGameButtonClick()
    {
        PlayerButtonAudio();
        UIManager.Instance.Show<UINewGameWindow>();
    }
    private void ContinueButtonClick()
    {
        PlayerButtonAudio();
        UIManager.Instance.Show<UISaveWindow>();
    }
    private void RankButtonClick()
    {
        PlayerButtonAudio();
        UIManager.Instance.Show<UIRankWindow>();
    }
    private void SettingButtonClick()
    {
        PlayerButtonAudio();
        UIManager.Instance.Show<UISettingWindow>();
    }
    private void QuitButtonClick() => PlayerButtonAudio();
}
