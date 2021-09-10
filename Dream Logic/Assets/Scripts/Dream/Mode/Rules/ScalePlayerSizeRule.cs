using UnityEngine;

namespace Game.Dream
{
    public class ScalePlayerSizeRule : DreamRule
    {
        [SerializeField]
        private float scaleFactor;

        private void OnEnable()
        {
            DreamGame.player.tr.localScale *= scaleFactor;
        }
    }
}
