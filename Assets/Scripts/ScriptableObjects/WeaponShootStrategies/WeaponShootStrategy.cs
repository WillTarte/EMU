using UnityEngine;

public abstract class WeaponShootStrategy : ScriptableObject, IWeaponShootStrategy
{
    public abstract void Shoot(WeaponBehaviourScript weapon);
    public abstract void Reload(WeaponBehaviourScript weapon);
    protected abstract void SpawnProjectile(WeaponBehaviourScript weapon);
}