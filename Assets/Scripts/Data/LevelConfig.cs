using System;
using JKFramework;
using Sirenix.OdinInspector;
using UnityEngine;

/// <summary>
/// 创建怪物的配置
/// </summary>
[Serializable]
public class CreateMonsterConfig
{
    [HorizontalGroup("Group")]
    [HideLabel]
    public MonsterConfig monsterConfig;
    
    [HorizontalGroup("Group")]
    [LabelText("生成的概率-随机数大于次数则生成怪物")]
    public int probability;
}
[CreateAssetMenu(fileName = "LeveConfig", menuName = "Config/Level Config")]
public class LevelConfig : ConfigBase
{
    [LabelText("怪物生成的间隔")]
    public float createMonsterInterval;
    [LabelText("场景中存在最大的怪物数")]
    public int maxMonsterCountInScene;
    [LabelText("怪物配置，刷新概率")]
    public CreateMonsterConfig[] createMonsterConfigs;
}
