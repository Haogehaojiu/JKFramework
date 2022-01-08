using JKFramework;
using UnityEngine;

public class CameraController : SingletonMonoBehaviour<CameraController>
{
    [SerializeField]
    private PlayerController player;
    public new Camera camera { get; private set; }
    private Vector3 offset = new Vector3(0, 9, -7.5f);
    private float speed = 2;
    protected override void Awake()
    {
        base.Awake();
        player = PlayerController.Instance;
        camera = GetComponent<Camera>();
    }
    private void Start() => player = PlayerController.Instance;

    private void LateUpdate() => FollowPlayer();
    private void FollowPlayer() => transform.position = Vector3.Lerp(transform.position, player.transform.position + offset, Time.deltaTime * speed);

}
