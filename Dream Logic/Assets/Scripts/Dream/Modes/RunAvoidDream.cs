using UnityEngine;

namespace Game.Dream
{
    /// <summary>
    /// Режим "Беги и уклоняйся!"
    /// </summary>
    public class RunAvoidDream : DreamMode
    {
        [SerializeField]
        private float scoreMultiplier = 1.5f;

        private void OnEnable()
        {
            DreamSimulation.onPlayerHit.AddListener(OnPlayerHit);
        }

        private void OnDisable()
        {
            DreamSimulation.onPlayerHit.RemoveListener(OnPlayerHit);
        }

        private void Update()
        {
            DreamSimulation.score += Time.deltaTime / scoreMultiplier;
        }

        private void OnPlayerHit(PlayerController player, ControllerColliderHit hit)
        {
            if (hit.gameObject.CompareTag(GameTags.enemy))
            {
                AudioManager.instance.Play("hit");
                DreamSimulation.WakeUp();
            }
        }
    }
}
