using WeaponsSystem.MonoBehaviours;

namespace WeaponsSystem.ScriptableObjects.WeaponShootStrategies
{
    /// <summary>
    /// Interface for weapon shooting strategies. The implementors are in charge of handling shooting.<br/>
    /// This means keeping track of state, spawning actual projectiles. This is to abstract away the code so that <br/>
    /// the Player code/gameobject does not have to worry about logic related to shooting.
    /// </summary>
    public interface IWeaponShootStrategy
    {
        void Shoot(WeaponBehaviourScript weapon);
        void Reload(WeaponBehaviourScript weapon);
    }
}