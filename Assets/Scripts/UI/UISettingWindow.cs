using JKFramework;
using UnityEngine;
using UnityEngine.UI;

[UIElement(true, "UI/settingWindow", 1)]
public class UISettingWindow : UIWindowBase
{
    [SerializeField]
    private Text globalVolumeText;
    [SerializeField]
    private Text bgVolumeText;
    [SerializeField]
    private Text effectVolumeText;
    [SerializeField]
    private Text languageTypeText;

    [SerializeField]
    private Button closeButton;
    [SerializeField]
    private Button backButton;
    [SerializeField]
    private Slider globalVolumeSlider;
    [SerializeField]
    private Slider bgVolumeSlider;
    [SerializeField]
    private Slider effectVolumeSlider;
    [SerializeField]
    private Dropdown languageTypeDropdown;
    private const string localSetPackName = "UISettingWindow";
    public override void Init()
    {
        closeButton.onClick.AddListener(Close);
        backButton.onClick.AddListener(OnBackButton);
        globalVolumeSlider.onValueChanged.AddListener(GlobalVolumeUpdate);
        bgVolumeSlider.onValueChanged.AddListener(BGVolumeUpdate);
        effectVolumeSlider.onValueChanged.AddListener(EffectVolumeUpdate);
        languageTypeDropdown.onValueChanged.AddListener(LanguageTypeUpdate);
    }
    /// <summary>
    /// 在游戏过程中的窗口初始化
    /// </summary>
    public void InitForGame()
    {
        closeButton.gameObject.SetActive(false);
        backButton.gameObject.SetActive(true);
    }
    public override void OnShow()
    {
        base.OnShow();
        globalVolumeSlider.value = GameManager.Instance.userSetting.globalVolume;
        bgVolumeSlider.value = GameManager.Instance.userSetting.bgVolume;
        effectVolumeSlider.value = GameManager.Instance.userSetting.effectVolume;
        languageTypeDropdown.value = (int)GameManager.Instance.userSetting.languageType;
    }
    protected override void OnUpdateLanguage()
    {
        globalVolumeText.JKLocalSet(localSetPackName, "globalVolumeText");
        bgVolumeText.JKLocalSet(localSetPackName, "bgVolumeText");
        effectVolumeText.JKLocalSet(localSetPackName, "effectVolumeText");
        languageTypeText.JKLocalSet(localSetPackName, "languageTypeText");
    }
    private void GlobalVolumeUpdate(float value)
    {
        GameManager.Instance.userSetting.globalVolume = value;
        AudioManager.Instance.GlobalVolume = value;
        SaveManager.SaveSetting(GameManager.Instance.userSetting);
    }
    private void BGVolumeUpdate(float value)
    {
        GameManager.Instance.userSetting.bgVolume = value;
        AudioManager.Instance.BgVolume = value;
        SaveManager.SaveSetting(GameManager.Instance.userSetting);
    }
    private void EffectVolumeUpdate(float value)
    {
        GameManager.Instance.userSetting.effectVolume = value;
        AudioManager.Instance.EffectVolume = value;
        SaveManager.SaveSetting(GameManager.Instance.userSetting);
    }
    private void LanguageTypeUpdate(int value)
    {
        LanguageType languageType = (LanguageType)value;
        GameManager.Instance.userSetting.languageType = languageType;
        LocalizationManager.Instance.CurrentLanguageType = languageType;
        SaveManager.SaveSetting(GameManager.Instance.userSetting);
    }
    public override void OnClose()
    {
        closeButton.gameObject.SetActive(true);
        backButton.gameObject.SetActive(false);
        AudioManager.Instance.PlayOnShot("Audio/Button", UIManager.Instance);
        base.OnClose();
    }
    private void OnBackButton()
    {
        AudioManager.Instance.PlayOnShot("Audio/Button", UIManager.Instance);
        if (Time.timeScale < 1f) Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
}