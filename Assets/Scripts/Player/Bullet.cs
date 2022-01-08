using System.Collections;
using JKFramework;
using UnityEngine;

[Pool]
public class Bullet : MonoBehaviour
{
    [SerializeField]
    private new Rigidbody rigidbody;
    [SerializeField]
    private TrailRenderer trailRenderer;
    [SerializeField]
    private BoxCollider boxCollider;
    private int attack;

    public void Init(Transform transform, float movePower, int attack)
    {
        rigidbody.AddForce(transform.forward * movePower, ForceMode.VelocityChange);
        this.attack = attack;
        boxCollider.enabled = true;
        trailRenderer.emitting = true;
        Invoke(nameof(DestroyOnInit), 5);
    }
    private void OnTriggerEnter(Collider other)
    {
        CancelInvoke(nameof(DestroyOnInit));
        StartCoroutine(nameof(Destroy));
        if (other.gameObject.CompareTag("Monster")) other.GetComponent<MonsterController>().GetHit(attack);
    }
    private void OnTriggerExit(Collider other) { StartCoroutine(nameof(Destroy)); }
    private void DestroyOnInit() => StartCoroutine(nameof(Destroy));
    private IEnumerator Destroy()
    {
        trailRenderer.emitting = false;
        boxCollider.enabled = false;
        rigidbody.velocity = Vector3.zero;
        yield return new WaitForSeconds(0.2f);
        DoDestroy();
    }
    private void DoDestroy() { this.JKGameObjectPushPool(); }
}