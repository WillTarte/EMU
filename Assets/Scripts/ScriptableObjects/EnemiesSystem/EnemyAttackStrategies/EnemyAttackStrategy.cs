using UnityEngine;

namespace ScriptableObjects.EnemiesSystem.EnemyAttackStrategies
{
    public abstract class EnemyAttackStrategy : ScriptableObject, IEnemyAttackStrategy
    {
        public abstract bool Attack(GameObject player, GameObject emu, int damageGiven, bool hasCollided);
    }
}
