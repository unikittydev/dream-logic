using UnityEngine;

namespace Game.Dream
{
    /// <summary>
    /// Счётчик очков в режиме "Беги и уклоняйся!"
    /// </summary>
    public class RunAvoidDream : MonoBehaviour
    {
        [SerializeField]
        private float scoreMultiplier;

        private float score;

        [SerializeField]
        private int scoreInt;

        private void Start()
        {
            DreamSimulation.player.gameObject.AddComponent<RunAvoidCollider>();
        }

        private void Update()
        {
            score += Time.deltaTime / scoreMultiplier;
            scoreInt = (int)score;
        }
    }
}
