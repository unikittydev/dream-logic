using UnityEngine;

namespace Game
{
    /// <summary>
    /// Поведение противника.
    /// </summary>
    public abstract class EnemyBehaviour : MonoBehaviour
    {
        protected EnemyController ec;

        protected virtual void Awake()
        {
            ec = GetComponent<EnemyController>();
        }
    }
}
