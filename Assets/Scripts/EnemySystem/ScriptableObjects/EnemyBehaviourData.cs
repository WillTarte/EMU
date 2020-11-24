using EnemySystem.ScriptableObjects.EnemyAttackStrategies;
using EnemySystem.ScriptableObjects.EnemyMovementStrategies;
using UnityEngine;

namespace EnemySystem.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewEnemyBehaviourData", menuName = "ScriptableObjects/EnemiesSystem", order = 1)]
    public class EnemyBehaviourData : ScriptableObject
    {
        #region Interface Variables
        
        public EnemyBehaviourName enemyBehaviourName;
        public EnemyAttackStrategy enemyAttackStrategy;
        public EnemyMovementStrategy enemyMovementStrategy;
        public int damageGiven;

        #endregion

    }

    public enum EnemyBehaviourName
    {
        Melee,
        Ranged,
        Kamikaze
    }
}
