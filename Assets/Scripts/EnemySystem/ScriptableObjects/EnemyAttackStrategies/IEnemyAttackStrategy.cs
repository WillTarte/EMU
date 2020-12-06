using UnityEngine;

namespace EnemySystem.ScriptableObjects.EnemyAttackStrategies
{
    public interface IEnemyAttackStrategy
    {
        void Attack(GameObject player, GameObject emu, int damageGiven);
    }
}
