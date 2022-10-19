using System;
using System.Collections.Generic;
using DG.Tweening;
using Interactions;
using UnityEngine;
using UnityEngine.SceneManagement;
using Weapons;

public class PlayerController : MonoBehaviour
{
    public float maxVelocity;
    public float timeToMaxVelocity;
    public float timeToZeroVelocity;
    public float upwardDragModifier;

    // Оружия
    public List<ModuleInfo> Modules;
    
    private Dictionary<ModulePlacement, ModuleInfo> modulesData;

    private Vector2 axes;
    
    private Rigidbody2D body;
    private Animator animator;
    
    private static readonly int Movement = Animator.StringToHash("Movement");

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        modulesData = new Dictionary<ModulePlacement, ModuleInfo>();

        foreach (var weapon in Modules)
        {
            modulesData.Add(weapon.Placement, weapon);
            modulesData[weapon.Placement].Weapon = WeaponTypes.Nothing;
        }

        AddWeapon(ModulePlacement.Main, WeaponTypes.Blaster);

        GetComponent<Destroyable>().OnDestroyFinish += () =>
        {
            var sequence = DOTween.Sequence();
            sequence.AppendInterval(1f);
            sequence.AppendCallback(() => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
        };
    }

    private void Update()
    {
        axes = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        animator.SetFloat(Movement, axes.x);
    }

    public void AddWeapon(ModulePlacement placement, WeaponTypes weapon)
    {
        modulesData[placement].Module.AddComponent(WeaponManager.Weapons[weapon]);
        modulesData[placement].Weapon = weapon;
    }

    public ModuleInfo GetModule(ModulePlacement placement)
    {
        return modulesData[placement];
    }

    private void FixedUpdate()
    {
        // Берем разницу до максимальной скорости
        var targetVelocities = axes * maxVelocity - body.velocity;

        // Ускоряемся быстрее, а замедляемся медленнее!
        targetVelocities /= Mathf.Approximately(axes.magnitude, 0f) ? timeToZeroVelocity : timeToMaxVelocity;
        
        // Если мы движемся вперед, то это даётся нам труднее
        targetVelocities *= axes.y > 0f ? upwardDragModifier : 1;

        // Применяем скорость
        body.AddForce(targetVelocities, ForceMode2D.Force);
    }
}

public enum ModulePlacement
{
    Main,
    LeftUp,
    RightUp,
    LeftDown,
    RightDown
}

[Serializable]
public class ModuleInfo
{
    public ModulePlacement Placement;
    public GameObject Module;
    
    [HideInInspector]
    public WeaponTypes Weapon;
}