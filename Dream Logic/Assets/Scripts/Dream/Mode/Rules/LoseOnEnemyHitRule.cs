using UnityEngine;

namespace Game.Dream
{
    public class LoseOnEnemyHitRule : DreamRule
    {
        private void OnEnable()
        {
            EventManager.OnPlayerHit.AddListener(OnPlayerHit);
        }

        private void OnDisable()
        {
            EventManager.OnPlayerHit.RemoveListener(OnPlayerHit);
        }

        private void OnPlayerHit(Component player, PlayerHitData data)
        {
            if (data.hit.gameObject.CompareTag(GameTags.enemy))
            {
                AudioManager.instance.Play("hit");
                DreamGame.WakeUp();
            }
        }
    }
}
