using JKFramework;
using JetBrains.Annotations;
using UnityEngine;

public class MonsterView : MonoBehaviour
{
    [SerializeField]
    private Animator animator;
    public Animator Animator => animator;

    [SerializeField]
    private Transform weaponCollider;
    private bool canHit;
    private int attack;

    public void Init(int attack)
    {
        this.attack = attack;
        weaponCollider.OnTriggerEnter(OnWeaponColliderTriggerEnter);
    }
    private void OnWeaponColliderTriggerEnter(Collider other, params object[] args)
    {
        if (!canHit) return;
        if (!other.CompareTag("Player")) return;
        canHit = false;
        AudioManager.Instance.PlayOnShot("Audio/Monster/monsterHit", transform);
        PlayerController.Instance.OnGetHit(attack);
    }
    /// <summary>
    /// 放回对象池
    /// </summary>
    public void Destroy()
    {
        this.JKGameObjectPushPool();
        weaponCollider.RemoveAllListener();
    }
    #region 动画事件
    [UsedImplicitly]
    private void Footstep() => AudioManager.Instance.PlayOnShot("Audio/Monster/monsterRun", transform, 0.4f);
    [UsedImplicitly]
    private void StartHit() => canHit = true;
    [UsedImplicitly]
    private void StopHit() => canHit = false;
    [UsedImplicitly]
    private void EndAttack() => EventManager.EventTrigger($"EndAttack{transform.parent.GetInstanceID()}");
    [UsedImplicitly]
    private void EndGetHit() => EventManager.EventTrigger($"EndGetHit{transform.parent.GetInstanceID()}");
    #endregion
}