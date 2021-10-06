using UnityEngine;

namespace Game.Dream
{
    /// <summary>
    /// Симуляция снов.
    /// </summary>
    public class DreamGame : MonoBehaviour
    {
        public static PlayerController player { get; set; }
        private static Vector3 _defaultPlayerPosition;
        public static Vector3 defaultPlayerPosition => _defaultPlayerPosition;

        [SerializeField]
        private Transform m_environment;
        private static Transform _environment;
        public static Transform environment => _environment;

        [SerializeField]
        private Transform m_tiles;
        private static Transform _tiles;
        public static Transform tiles => _tiles;

        private static DreamThemeSwitcher _themeSwitcher;
        public static DreamThemeSwitcher themeSwitcher => _themeSwitcher;
        private static DreamModeSwitcher modeSwitcher;
        private static DreamDifficulty _difficulty;
        public static DreamDifficulty difficulty => _difficulty;
        private static DreamDescriptionUI _ui;
        public static DreamDescriptionUI ui => _ui;

        private static DreamGameUI _gameUI;
        public static DreamGameUI gameUI => _gameUI;

        private static DreamCycle _cycle;
        public static DreamCycle cycle => _cycle;

        private static GamePool _pool;
        public static GamePool pool => _pool;

        private void Awake()
        {
            player = FindObjectOfType<PlayerController>();
            _defaultPlayerPosition = player.transform.position;
            _environment = m_environment;
            _tiles = m_tiles;

            _themeSwitcher = GetComponent<DreamThemeSwitcher>();
            modeSwitcher = GetComponent<DreamModeSwitcher>();
            _difficulty = GetComponent<DreamDifficulty>();
            _ui = GetComponent<DreamDescriptionUI>();
            _gameUI = GetComponent<DreamGameUI>();

            _cycle = GetComponent<DreamCycle>();

            _pool = GetComponent<GamePool>();
        }

        public static void WakeUp()
        {
            themeSwitcher.SetDefaultTheme();
            modeSwitcher.SetDefaultMode();
            DreamScore.UpdateValue();
            cycle.enabled = false;
            gameUI.Stop();
        }

        public static void Restart()
        {
            pool.Clear();
            gameUI.Restart();
            player.InstantMove(defaultPlayerPosition);
            cycle.enabled = true;
            DreamScore.value = 0f;
            difficulty.ResetDifficulty();
        }

        private void OnApplicationPause(bool pause)
        {
            if (pause)
                gameUI.Pause();
        }
    }
}
