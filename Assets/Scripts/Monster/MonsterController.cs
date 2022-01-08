using JKFramework;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 怪物的状态类型
/// </summary>
public enum MonsterStateType
{
    Idle = 1,
    Patrol = 2,
    Follow = 3,
    Attack = 4,
    GetHit = 5,
    Die = 6
}

[Pool]
public class MonsterController : MonoBehaviour, IStateMachineOwner
{
    #region 自身组件
    [SerializeField]
    private NavMeshAgent monsterNavMeshAgent;
    public NavMeshAgent MonsterNavMeshAgent => monsterNavMeshAgent;
    private StateMachine stateMachine;
    #endregion

    #region View组件
    private MonsterView monsterView;
    private Animator animator;
    public Animator MonsterAnimator => animator;
    #endregion

    #region 数值
    private int hp;
    #endregion

    public void Init(MonsterConfig monsterConfig)
    {
        monsterView = PoolManager.Instance.GetGameObject<MonsterView>(monsterConfig.monsterPrefab, transform);
        var monsterViewTrans = monsterView.transform;
        monsterViewTrans.localPosition = Vector3.zero;
        monsterViewTrans.localEulerAngles = Vector3.zero;
        monsterView.Init(monsterConfig.attack);
        animator = monsterView.Animator;
        hp = monsterConfig.hp;
        //初始化状态机
        stateMachine = ResourcesManager.Load<StateMachine>();
        stateMachine.Init(this);
        stateMachine.SwitchState<MonsterStateIdle>((int)MonsterStateType.Idle);
    }
    /// <summary>
    /// 获取巡逻目标点
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPatrolTarget() { return MonsterManager.Instance.GetPatrolTarget(); }
    /// <summary>
    /// 受到攻击
    /// </summary>
    public void GetHit(int damage)
    {
        if (hp <= 0) return;
        hp = hp - damage < 0 ? 0 : hp - damage;
        if (hp <= 0) stateMachine.SwitchState<MonsterStateDie>((int)MonsterStateType.Die);
        else stateMachine.SwitchState<MonsterStateGetHit>((int)MonsterStateType.GetHit);
    }
    /// <summary>
    /// 逻辑调用的死亡销毁
    /// </summary>
    public void Die()
    {
        stateMachine.Destroy();
        stateMachine = null;
        monsterView.Destroy();
        monsterView = null;
        this.JKGameObjectPushPool();
    }
    /// <summary>
    /// 因为场景切换导致的销毁，只能把非GameObject上的脚本放进对象池
    /// </summary>
    private void OnDestroy() => stateMachine?.Destroy();
}
