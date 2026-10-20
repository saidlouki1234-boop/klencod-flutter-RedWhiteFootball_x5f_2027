
using UnityEngine;
using TMPro;
using RedWhiteFootball.Core;

namespace RedWhiteFootball.UI
{
    public class UIManager : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI teamNamesText;
        public GameObject goalPanel;
        public TextMeshProUGUI goalText;
        public GameObject finishPanel;
        public TextMeshProUGUI finalScoreText;

        void OnEnable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged += UpdateScore;
                GameManager.Instance.OnMatchFinished += OnFinish;
            }
        }

        void OnDisable()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnScoreChanged -= UpdateScore;
                GameManager.Instance.OnMatchFinished -= OnFinish;
            }
        }

        void Start()
        {
            UpdateScore(GameManager.Instance.scoreTeamA, GameManager.Instance.scoreTeamB);
            if (teamNamesText) teamNamesText.text = $"{GameManager.Instance.teamAName} {ArabicRTLHelper.Fix("ضد")} {GameManager.Instance.teamBName}";
        }

        void UpdateScore(int a, int b)
        {
            if (scoreText) scoreText.text = $"{a} - {b}";
            if (goalPanel)
            {
                goalPanel.SetActive(true);
                if (goalText) goalText.text = ArabicRTLHelper.Fix("هدف! GOAL!");
                Invoke(nameof(HideGoal), 2f);
            }
        }

        void HideGoal() { if (goalPanel) goalPanel.SetActive(false); }

        void OnFinish()
        {
            if (finishPanel) finishPanel.SetActive(true);
            if (finalScoreText) finalScoreText.text = $"النتيجة النهائية\n{GameManager.Instance.scoreTeamA} - {GameManager.Instance.scoreTeamB}";
            Time.timeScale = 0.3f;
        }

        public void OnPause() { Time.timeScale = 0; GameManager.Instance.State = MatchState.Paused; }
        public void OnResume() { Time.timeScale = 1; GameManager.Instance.State = MatchState.Playing; }
        public void OnQuit() { UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu"); Time.timeScale = 1; }
    }

    // Simple RTL helper without external lib
    public static class ArabicRTLHelper
    {
        public static string Fix(string input)
        {
            // Unity TextMeshPro handles Arabic shaping if font supports it. This is a placeholder that reverses for legacy.
            // For proper Arabic, use an Arabic-supporting TMP font (e.g. Cairo) and enable RTL in TMP.
            if (string.IsNullOrEmpty(input)) return input;
            // Detect if contains Arabic - keep as is for TMP with RTL enabled
            return input; 
        }
    }
}
