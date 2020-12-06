using UnityEngine;

namespace EnemySystem.ScriptableObjects.EnemyAttackStrategies
{
    public abstract class EnemyAttackStrategy : ScriptableObject, IEnemyAttackStrategy
    {
        public abstract void Attack(GameObject player, GameObject emu, int damageGiven);
    }
}
