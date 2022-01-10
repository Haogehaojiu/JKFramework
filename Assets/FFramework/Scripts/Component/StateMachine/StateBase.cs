namespace JKFramework
{
    /// <summary>
    /// 状态基类
    /// </summary>
    public abstract class StateBase
    {
        protected StateMachine stateMachine;

        /// <summary>
        /// 初始化状态
        /// 只在状态第一次创建时执行
        /// </summary>
        /// <param name="owner">宿主</param>
        /// <param name="stateType">状态类型枚举值</param>
        /// <param name="stateMachine">所属状态机</param>
        public virtual void Init(IStateMachineOwner owner, int stateType, StateMachine stateMachine) => this.stateMachine = stateMachine;
        /// <summary>
        /// 反初始化
        /// 不使用，放回对象池时执行
        /// 将引用置空，防止不能GC
        /// </summary>
        public virtual void UnInit() => this.JKObjectPushPool(); //放回对象池
        /// <summary>
        /// 状态进入
        /// 每次进入都会执行
        /// </summary>
        public virtual void Enter() { }
        /// <summary>
        /// 状态退出
        /// 每次退出都会执行
        /// </summary>
        public virtual void Exit() { }
        public virtual void Update() { }
        public virtual void LateUpdate() { }
        public virtual void FixedUpdate() { }
    }
}