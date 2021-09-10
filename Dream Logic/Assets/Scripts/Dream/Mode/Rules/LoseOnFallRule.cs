using UnityEngine;

namespace Game.Dream
{
    public class LoseOnFallRule : DreamRule
    {
        [SerializeField]
        private float voidHeight = -5f;

        private void Update()
        {
            if (DreamGame.player.tr.position.y < voidHeight)
            {
                DreamGame.WakeUp();
            }
        }
    }
}
