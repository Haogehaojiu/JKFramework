using System.Collections;
using JKFramework;
using UnityEngine;

public enum PlayerState
{
    Normal,
    Reload,
    GetHit,
    Die
}
public class PlayerController : SingletonMonoBehaviour<PlayerController>
{
    [SerializeField]
    private CharacterController characterController;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private Transform firePointTransform;
    [SerializeField]
    private Transform fireForwardTransform;
    private PlayerState playerState;
    public PlayerState PlayerState { get => playerState; set { playerState = value; } }
    #region 参数
    private int currentHealthPoint;
    public int CurrentHealthPoint {
        get => currentHealthPoint;
        set {
            currentHealthPoint = value;
            //更新血条
            EventManager.EventTrigger("UpdateHealthPoint", CurrentHealthPoint, MaxHealthPoint);
        }
    }
    private int maxHealthPoint;
    public int MaxHealthPoint => maxHealthPoint;
    private float moveSpeed;
    private int attack;
    private int currentBulletNumber;
    private int maxBulletNumber;
    private float shootInterval;
    private float bulletMovePower;
    private bool canShoot = true;
    private int groundLayerMask;
    private static readonly int shoot = Animator.StringToHash("shoot");
    private static readonly int moveX = Animator.StringToHash("moveX");
    private static readonly int moveZ = Animator.StringToHash("moveZ");
    private static readonly int reLoad = Animator.StringToHash("reLoad");
    private static readonly int getHit = Animator.StringToHash("getHit");
    private static readonly int die = Animator.StringToHash("die");
    #endregion

    public void Init(PlayerConfig config)
    {
        CurrentHealthPoint = config.health;
        maxHealthPoint = config.health;
        moveSpeed = config.moveSpeed;
        attack = config.attack;

        currentBulletNumber = maxBulletNumber = config.maxBulletNumber;
        shootInterval = config.shootInterval;
        bulletMovePower = config.bulletMovePower;
        groundLayerMask = LayerMask.GetMask("Ground");
        EventManager.EventTrigger("UpdateBulletNumber", currentBulletNumber, maxBulletNumber);
    }
    private void Update()
    {
        if (Time.timeScale == 0) return;
        StateOnUpdate();
    }
    private void StateOnUpdate()
    {
        switch(PlayerState)
        {
            case PlayerState.Normal:
                Move();
                Shoot();
                if (currentBulletNumber < maxBulletNumber && Input.GetKeyDown(KeyCode.R)) PlayerState = PlayerState.Reload;
                break;
            case PlayerState.Reload:
                Move();
                if (!animator.GetBool(reLoad)) StartCoroutine(nameof(ReLoad));
                break;
            case PlayerState.GetHit:
                if (animator.GetBool(getHit)) return;
                StartCoroutine(DoGetHit());
                break;
            case PlayerState.Die:
                if (!animator.GetBool(die))
                {
                    animator.SetBool(die, true);
                    EventManager.EventTrigger("GameOver");
                }
                break;
        }
    }
    private void Move()
    {
        var v = Input.GetAxis("Horizontal");
        var h = Input.GetAxis("Vertical");
        Vector3 moveDir = new Vector3(v, 0, h).normalized;
        characterController.Move(moveDir * (moveSpeed * Time.deltaTime));
        Ray ray = CameraController.Instance.camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hitInfo, 100, groundLayerMask))
        {
            if (hitInfo.point.z < transform.position.z)
            {
                v *= -1;
                h *= -1;
            }
            Vector3 position = transform.position;
            Vector3 dir = new Vector3(hitInfo.point.x, position.y, hitInfo.point.z) - position;
            Quaternion targetQuaternion = Quaternion.FromToRotation(Vector3.forward, dir);
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetQuaternion, Time.deltaTime * 20f);
        }
        animator.SetFloat(moveX, h);
        animator.SetFloat(moveZ, v);
    }
    private void Shoot()
    {
        if (canShoot && currentBulletNumber > 0 && Input.GetMouseButton(0)) StartCoroutine(nameof(DoShoot));
        else animator.SetBool(shoot, false);
    }
    private IEnumerator DoShoot()
    {
        canShoot = false;
        --currentBulletNumber;
        EventManager.EventTrigger("UpdateBulletNumber", currentBulletNumber, maxBulletNumber);
        animator.SetBool(shoot, true);
        AudioManager.Instance.PlayOnShot("Audio/Shoot/laser_01", CameraController.Instance);
        //生成子弹
        Bullet bullet = ResourcesManager.Load<Bullet>("bullet", LevelManager.Instance.TempGameObjectTransform);
        Transform bulletTransform = bullet.transform;
        bulletTransform.position = firePointTransform.position;
        bullet.Init(fireForwardTransform, bulletMovePower, attack);
        yield return new WaitForSeconds(shootInterval);
        canShoot = true;
        if (currentBulletNumber <= 0) PlayerState = PlayerState.Reload;
    }
    private IEnumerator ReLoad()
    {
        animator.SetBool(reLoad, true);
        AudioManager.Instance.PlayOnShot("Audio/Shoot/Reload", CameraController.Instance);
        yield return new WaitForSeconds(1.9f);
        animator.SetBool(reLoad, false);
        currentBulletNumber = maxBulletNumber;
        EventManager.EventTrigger("UpdateBulletNumber", currentBulletNumber, maxBulletNumber);
        PlayerState = PlayerState.Normal;
    }

    public void OnGetHit(int damage)
    {
        if (CurrentHealthPoint <= 0) return;
        CurrentHealthPoint = CurrentHealthPoint - damage > 0 ? CurrentHealthPoint - damage : 0;
        PlayerState = CurrentHealthPoint <= 0 ? PlayerState.Die : PlayerState.GetHit;
    }
    private IEnumerator DoGetHit()
    {
        animator.SetBool(getHit, true);
        yield return new WaitForSeconds(0.2f);
        animator.SetBool(getHit, false);
        PlayerState = PlayerState.Normal;
    }
}
