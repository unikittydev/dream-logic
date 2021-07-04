
using UnityEngine;

namespace Game.Dream
{
    /// <summary>
    /// ����� "���� � �������!"
    /// </summary>
    public class RunCatchDream : DreamMode
    {
        [SerializeField]
        private float scoreMultiplier = 5f;

        private void OnEnable()
        {
            DreamSimulation.player.tr.localScale *= 2f;
            DreamSimulation.onPlayerHit.AddListener(OnPlayerHit);
        }

        private void OnDisable()
        {
            DreamSimulation.onPlayerHit.RemoveListener(OnPlayerHit);
        }

        private void OnPlayerHit(PlayerController player, ControllerColliderHit hit)
        {
            if (hit.gameObject.CompareTag(GameTags.enemy))
            {
                DreamSimulation.score += scoreMultiplier;
                Destroy(hit.gameObject);
            }
        }
    }
}
