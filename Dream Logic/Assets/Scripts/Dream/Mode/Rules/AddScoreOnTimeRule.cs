using UnityEngine;

namespace Game.Dream
{
    public class AddScoreOnTimeRule : DreamRule
    {
        [SerializeField]
        private float scoreMultiplier;

        private void Update()
        {
            DreamScore.value += Time.deltaTime / scoreMultiplier;
        }
    }
}
