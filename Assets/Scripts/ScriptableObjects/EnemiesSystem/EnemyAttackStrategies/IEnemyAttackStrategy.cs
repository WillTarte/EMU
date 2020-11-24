using UnityEngine;

namespace ScriptableObjects.EnemiesSystem.EnemyAttackStrategies
{
    public interface IEnemyAttackStrategy
    {
        bool Attack(GameObject player, GameObject emu, int damageGiven, bool hasCollided);
    }
}
