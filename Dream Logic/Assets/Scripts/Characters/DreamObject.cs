using UnityEngine;

namespace Game
{
    /// <summary>
    /// ������ ���. ������ ��� ��������� � ����������.
    /// </summary>
    public class DreamObject : MonoBehaviour
    {
        private void OnEnable()
        {
            ParticleManager.Poof(transform.position + new Vector3(0f, transform.localScale.y * .5f, 0f));
        }

        private void OnDisable()
        {
            ParticleManager.Poof(transform.position + new Vector3(0f, transform.localScale.y * .5f, 0f));
        }

    }
}
