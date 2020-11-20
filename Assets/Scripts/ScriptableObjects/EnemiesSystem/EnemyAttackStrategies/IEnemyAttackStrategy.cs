using UnityEngine;

namespace ScriptableObjects.EnemiesSystem.EnemyAttackStrategies
{
    public interface IEnemyAttackStrategy
    {
        void Attack(Collider2D other, int damageGiven);
    }
}
