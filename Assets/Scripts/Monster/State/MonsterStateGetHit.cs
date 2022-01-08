using JKFramework;

public class MonsterStateGetHit : MonsterStateBase
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
        //修改状态
        SetMoveState(false);
        //播放对应动画
        PlayAnimation("getHit");
        //监听攻击动画的结束
        EventManager.AddEventListener($"EndGetHit{monsterEventId}", OnEndGetHit);
    }
    public override void Exit() => EventManager.RemoveEventListener($"EndGetHit{monsterEventId}", OnEndGetHit);
    private void OnEndGetHit() { stateMachine.SwitchState<MonsterStateFollow>((int)MonsterStateType.Follow); }
}