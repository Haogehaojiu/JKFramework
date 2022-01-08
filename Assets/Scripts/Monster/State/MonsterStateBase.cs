using JKFramework;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// 怪物状态基类
/// </summary>
public abstract class MonsterStateBase : StateBase
{
    protected MonsterController monster;
    protected Animator animator;
    protected NavMeshAgent navMeshAgent;
    protected PlayerController player => PlayerController.Instance;
    public override void Init(IStateMachineOwner owner, int stateType, StateMachine stateMachine)
    {
        base.Init(owner, stateType, stateMachine);
        monster = owner as MonsterController;
        if (!(monster is null))
        {
            animator = monster.MonsterAnimator;
            navMeshAgent = monster.MonsterNavMeshAgent;
        }
    }
    public override void UnInit()
    {
        animator = null;
        monster = null;
        navMeshAgent = null;
        base.UnInit();
    }
    /// <summary>
    /// 播放动画
    /// </summary>
    /// <param name="clipName"></param>
    protected void PlayAnimation(string animationName) { animator.CrossFadeInFixedTime(animationName, 0.2f); }
    /// <summary>
    /// 设置移动状态
    /// </summary>
    /// <param name="canMove"></param>
    protected void SetMoveState(bool canMove) { navMeshAgent.enabled = canMove; }
    /// <summary>
    /// 检查追击，如果达到条件，则切换到追击状态
    /// </summary>
    protected bool CheckFollowAndSwitchState()
    {
        if (Vector3.Distance(monster.transform.position, player.transform.position) < 5f)
        {
            stateMachine.SwitchState<MonsterStateFollow>((int)MonsterStateType.Follow);
            return true;
        }
        return false;
    }
}