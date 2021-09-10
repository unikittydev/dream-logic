using TMPro;
using UnityEngine;

namespace Game.Dream
{
    public class DreamScore : MonoBehaviour
    {
        private const string highScoreKey = "HIGH_SCORE";

        [SerializeField]
        private static float _score;
        public static float value
        {
            get => _score;
            set
            {
                _score = value;
                EventManager.OnScoreChange.Invoke(new ScoreChangedData() { value = value });
            }
        }

        [SerializeField]
        private TMP_Text _endHighScore;
        private static TMP_Text endHighScore;
        [SerializeField]
        private TMP_Text _endGameScore;
        private static TMP_Text endGameScore;
        [SerializeField]
        private TMP_Text _gameScore;
        private static TMP_Text gameScore;
        public static TMP_Text scoreText => gameScore;

        private void Awake()
        {
            endHighScore = _endHighScore;
            endGameScore = _endGameScore;
            gameScore = _gameScore;
        }

        private void OnEnable()
        {
            EventManager.OnScoreChange.AddListener(OnScoreChanged);
        }

        private void OnDisable()
        {
            EventManager.OnScoreChange.RemoveListener(OnScoreChanged);
        }

        private void OnScoreChanged(ScoreChangedData data)
        {
            gameScore.SetText(((int)data.value).ToString());
        }

        public static void UpdateValue()
        {
            if (!PlayerPrefs.HasKey(highScoreKey) || value > PlayerPrefs.GetFloat(highScoreKey))
            {
                PlayerPrefs.SetFloat(highScoreKey, value);
                AudioManager.instance.Play("lost.newRecord");
            }
            else
                AudioManager.instance.Play("lost");
            endHighScore.SetText(((int)PlayerPrefs.GetFloat(highScoreKey)).ToString());
            endGameScore.SetText(((int)value).ToString());
        }
    }
}
