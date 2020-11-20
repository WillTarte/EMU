using ScriptableObjects.EnemiesSystem.EnemyAttackStrategies;
using ScriptableObjects.EnemiesSystem.EnemyMovementStrategies;
using UnityEngine;

namespace ScriptableObjects.EnemiesSystem
{
    [CreateAssetMenu(fileName = "NewEnemyBehaviourData", menuName = "ScriptableObjects/EnemiesSystem", order = 1)]
    public class EnemyBehaviourData : ScriptableObject
    {
        #region Interface Variables
        
        public EnemyBehaviourName enemyBehaviourName;
        public EnemyAttackStrategy enemyAttackStrategy;
        public EnemyMovementStrategy enemyMovementStrategy;
        public int damageGiven;
        public int healthPoints;
        
        #endregion

    }

    public enum EnemyBehaviourName
    {
        Melee,
        Ranged,
        Kamikaze
    }
}
