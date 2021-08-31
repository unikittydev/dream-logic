using UnityEngine;

namespace Game
{
    /// <summary>
    /// ��������� ����������.
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
