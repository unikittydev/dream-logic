using UnityEngine;

namespace Game.Dream
{
    /// <summary>
    /// Сон.
    /// </summary>
    public class Dream : MonoBehaviour
    {
        private float maxTime;
        private float timeCounter;

        private DreamTheme theme;
        private DreamBehaviour rules;

        private void Update()
        {
            timeCounter += Time.deltaTime;
            if (timeCounter >= maxTime)
            {
                DreamSimulation.StartNewDreamCycle();
            }
        }
    }
}
