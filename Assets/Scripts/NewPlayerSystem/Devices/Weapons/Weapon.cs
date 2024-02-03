using DG.Tweening;
using UnityEngine;
using Weapons;

public abstract class Weapon : Device
{
    private float energy = 0;
    
    private Sequence activeSequence;

    public GameObject bulletObj = Resources.Load("StandardBullet") as GameObject;

    public Weapon()
    {
        kind = ModuleKind.Weapon;
    }

    public override float CalculateConsumption(float input)
    {
        if (input == energy)
        {
            return input;
        }

        energy = input;
        RestartWeapon();


        return input;
    }

    protected virtual float getChargesToFire()
    {
        return 1f;
    }

    protected abstract void Shoot(Vector2 startPoint, Vector2 direction, float force);

    private void RestartWeapon()
    {
        activeSequence.Pause();
        activeSequence.Kill();

        activeSequence = DOTween.Sequence();
        activeSequence
            .AppendCallback(() => Shoot(GameObject.FindGameObjectWithTag("Player").transform.position, Vector2.up, 600))
            .AppendInterval(getChargesToFire() / energy)
            .SetLoops(-1);
    }
}
