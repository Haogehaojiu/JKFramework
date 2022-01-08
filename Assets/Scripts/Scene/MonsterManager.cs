using JKFramework;
using UnityEngine;

/// <summary>
/// 怪物管理器
/// </summary>
public class MonsterManager : LogicManagerBase<MonsterManager>
{
    [SerializeField]
    private Transform monsterSpawn;
    private int monsterNumber;
    private LevelConfig levelConfig;
    [SerializeField]
    private Transform[] targets;

    private void Start()
    {
        levelConfig = ConfigManager.Instance.GetConfig<LevelConfig>("Level");
        InvokeRepeating(nameof(CreateMonster), levelConfig.createMonsterInterval, 2);
    }
    protected override void RegisterEventListener() { EventManager.AddEventListener("MonsterDie", OnMonsterDie); }
    protected override void CancelEventListener() { EventManager.RemoveEventListener("MonsterDie"); }
    //每x秒生成一直怪物
    private void CreateMonster()
    {
        //当前场景怪物数量为达到上限
        if (monsterNumber < levelConfig.maxMonsterCountInScene)
        {
            var random = Random.Range(0, 100);
            for(var i = 0; i < levelConfig.createMonsterConfigs.Length; i++)
            {
                //当前随机数大于配置中的概率
                if (random < levelConfig.createMonsterConfigs[i].probability)
                {
                    //生成这只怪物
                    MonsterController monsterController = ResourcesManager.Load<MonsterController>("Monster", LevelManager.Instance.TempGameObjectTransform);
                    var monsterTransform = monsterController.transform;
                    var spawnTransform = monsterSpawn.transform;
                    monsterTransform.localPosition = spawnTransform.localPosition;
                    monsterTransform.localRotation = spawnTransform.localRotation;
                    monsterController.Init(levelConfig.createMonsterConfigs[i].monsterConfig);
                    ++monsterNumber;
                    return;
                }
            }
        }
    }
    /// <summary>
    /// 获取巡逻目标点
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPatrolTarget() { return targets[Random.Range(0, targets.Length)].position; }
    private void OnMonsterDie() { --monsterNumber; }
}