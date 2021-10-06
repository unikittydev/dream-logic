using UnityEngine;

namespace Game.Dream
{
    public class AddScoreOnEnemyHitRule : DreamRule
    {
        private Vector2Int enemyCost;

        private void OnEnable()
        {
            enemyCost = DreamGame.themeSwitcher.currentTheme.minMaxEnemyCost;
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
                DreamScore.value += Random.Range(enemyCost.x, enemyCost.y);
                DreamGame.pool.AddObject(data.hit.gameObject);
            }
        }
    }
}
