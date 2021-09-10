using UnityEngine;

namespace Game.Dream
{
    public class RespawnOnFallRule : DreamRule
    {
        [SerializeField]
        private float voidHeight = -10f;

        private void Update()
        {
            if (DreamGame.player.tr.position.y < voidHeight)
            {
                DreamGame.player.InstantMove(DreamGame.defaultPlayerPosition);
            }
        }
    }
}
