using UnityEngine;

namespace Game.Dream
{
    /// <summary>
    /// ����� "����, �������".
    /// </summary>
    public class IdleDream : MonoBehaviour
    {
        [SerializeField]
        private float scoreMultiplier = 2f;

        private void Update()
        {
            DreamSimulation.score += Time.deltaTime / scoreMultiplier;
        }
    }
}
