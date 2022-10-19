using Interactions;
using UnityEngine;

[RequireComponent(typeof(Damaging))]
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{
    private Rigidbody2D body;
    private Damaging dmg;

    public void Launch(float force, Vector2 dir, float damage)
    {
        dmg.Damage = damage;
        body.AddForce(dir.normalized * force, ForceMode2D.Force);
    }

    private void DamageHandler()
    {
        Destroy(gameObject);
    }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        dmg = GetComponent<Damaging>();

        dmg.OnDamage = DamageHandler;
    }
}
