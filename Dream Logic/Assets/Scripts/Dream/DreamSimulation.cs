using System.Collections;
using UnityEngine;

namespace Game.Dream
{
    /// <summary>
    /// ��������� ����.
    /// </summary>
    public class DreamSimulation : MonoBehaviour
    {
        public static PlayerController player { get; set; }

        private static Transform _environment;
        public static Transform environment => _environment;

        private static float timeCounter;
        private static float maxTime = 15f;

        private static DreamThemeSwitcher themeSwitcher;

        private void Awake()
        {
            player = FindObjectOfType<PlayerController>();
            _environment = GameObject.FindGameObjectWithTag(GameTags.environment).transform;

            themeSwitcher = GetComponent<DreamThemeSwitcher>();

            timeCounter = maxTime;
        }

        private void Update()
        {
            if (timeCounter >= maxTime)
            {
                StartCoroutine(StartNewDreamCycle());
                timeCounter = 0f;
            }
            else
            {
                timeCounter += Time.deltaTime;
            }
        }

        public static void WakeUp()
        {
            print("You woke up");
            themeSwitcher.SetDefaultTheme();
            timeCounter = -float.NegativeInfinity;
        }

        public IEnumerator StartNewDreamCycle()
        {
            themeSwitcher.SetDefaultTheme();
            yield return new WaitForSeconds(5f);
            themeSwitcher.SetRandomTheme();
        }
    }
}
