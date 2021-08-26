using UnityEngine;

namespace Game
{
    /// <summary>
    /// ��������� ����������.
    /// </summary>
    public abstract class EnemyBehaviour : MonoBehaviour
    {
        protected EnemyController ec;

        private void Awake()
        {
            ec = GetComponent<EnemyController>();
        }
    }
}
