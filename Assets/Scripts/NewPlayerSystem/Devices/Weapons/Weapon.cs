using DG.Tweening;
using UnityEngine;
using Weapons;

public abstract class Weapon : Device
{
    public float energy = 0;

    public int chargesToFire = 1;

    public Sequence activeSequence;

    public Weapon()
    {
        kind = ModuleKind.Weapon;
    }

    public override float CalculateConsumption(float input)
    {
        if (input == energy)
        {
            return 0;
        }

        energy = input;
        RestartWeapon();


        return 0;
    }

    protected abstract void Shoot(Vector2 startPoint, Vector2 direction, float force);

    private void RestartWeapon()
    {
        activeSequence.Kill();

        activeSequence = DOTween.Sequence();
        activeSequence
            .AppendCallback(() => Shoot(GameObject.FindGameObjectWithTag("Player").transform.position, Vector2.up, 1000))
            .AppendInterval(energy / chargesToFire)
            .SetLoops(-1);
    }
}
