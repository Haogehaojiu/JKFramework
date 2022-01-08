using JKFramework;
public class MonsterStateAttack : MonsterStateBase
{
    //用于接收怪物view层的动画事件
    private int monsterEventId;

    public override void Init(IStateMachineOwner owner, int stateType, StateMachine stateMachine)
    {
        base.Init(owner, stateType, stateMachine);
        monsterEventId = monster.transform.GetInstanceID();
    }
    public override void Enter()
    {
        //修改移动状态
        SetMoveState(false);
        //播放对应动画
        PlayAnimation("attack");
        //监听攻击动画的结束
        EventManager.AddEventListener($"EndAttack{monsterEventId}", OnAttackOver);
    }
    public override void Exit() { EventManager.RemoveEventListener($"EndAttack{monsterEventId}", OnAttackOver); }
    /// <summary>
    /// 当攻击结束时执行的逻辑
    /// </summary>
    private void OnAttackOver() { stateMachine.SwitchState<MonsterStateFollow>((int)MonsterStateType.Follow); }

}