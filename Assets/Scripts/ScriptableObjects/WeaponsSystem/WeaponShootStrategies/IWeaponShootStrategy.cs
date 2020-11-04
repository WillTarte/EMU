using MonoBehaviours.WeaponsSystem;

namespace ScriptableObjects.WeaponsSystem.WeaponShootStrategies
{
    /// <summary>
    /// Interface for weapon shooting strategies. The implementors are in charge of handling shooting.<br/>
    /// This means keeping track of state, spawning actual projectiles. This is to abstract away the code so that <br/>
    /// the Player code/gameobject does not have to worry about logic related to shooting.
    /// </summary>
    public interface IWeaponShootStrategy
    {
        //todo?
        void Shoot(/*Player player,*/ WeaponBehaviourScript weapon);
        void Reload(WeaponBehaviourScript weapon);
    }
}