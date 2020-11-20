using UnityEngine;

namespace ScriptableObjects.EnemiesSystem.EnemyAttackStrategies
{
    public abstract class EnemyAttackStrategy : ScriptableObject, IEnemyAttackStrategy
    {
        public abstract void Attack(Collider2D other, int damageGiven);
    }
}
