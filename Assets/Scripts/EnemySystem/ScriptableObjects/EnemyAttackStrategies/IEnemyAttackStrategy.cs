using UnityEngine;

namespace EnemySystem.ScriptableObjects.EnemyAttackStrategies
{
    public interface IEnemyAttackStrategy
    {
        bool Attack(GameObject player, GameObject emu, int damageGiven, bool hasCollided);
    }
}
