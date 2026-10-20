
using UnityEngine;
using TMPro;
using RedWhiteFootball.Core;

namespace RedWhiteFootball.Core
{
    public class MatchTimer : MonoBehaviour
    {
        public TextMeshProUGUI timerText;
        float currentTime = 0f;

        void Update()
        {
            if (GameManager.Instance == null || GameManager.Instance.State != MatchState.Playing) return;
            currentTime += Time.deltaTime;
            float remaining = Mathf.Max(0, GameManager.Instance.matchDuration - currentTime);

            int minutes = Mathf.FloorToInt(remaining / 60f);
            int seconds = Mathf.FloorToInt(remaining % 60f);
            if (timerText) timerText.text = $"{minutes:00}:{seconds:00}";

            if (remaining <= 0.01f)
            {
                GameManager.Instance.FinishMatch();
            }
        }

        public void ResetTimer() => currentTime = 0f;
    }
}
