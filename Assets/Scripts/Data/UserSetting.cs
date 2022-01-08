using System;
using JKFramework;

[Serializable]
public class UserSetting
{
    public float globalVolume;
    public float bgVolume;
    public float effectVolume;
    public LanguageType languageType;

    public UserSetting(float globalVolume, float bgVolume, float effectVolume, LanguageType languageType)
    {
        this.globalVolume = globalVolume;
        this.bgVolume = bgVolume;
        this.effectVolume = effectVolume;
        this.languageType = languageType;
    }
}