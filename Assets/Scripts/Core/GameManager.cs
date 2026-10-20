
using UnityEngine;
using System;

namespace RedWhiteFootball.Core
{
    public enum MatchState { MainMenu, Playing, Paused, GoalScored, HalfTime, Finished }

    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;
        public MatchState State = MatchState.MainMenu;
        public float matchDuration = 90f; // seconds for demo (1.5 min = full match sim)
        public int scoreTeamA = 0;
        public int scoreTeamB = 0;
        public string teamAName = "النادي الإفريقي";
        public string teamBName = "الخصم";

        public event Action<int,int> OnScoreChanged;
        public event Action OnMatchFinished;

        void Awake()
        {
            if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
            else Destroy(gameObject);
        }

        public void StartMatch()
        {
            scoreTeamA = 0; scoreTeamB = 0;
            State = MatchState.Playing;
            Time.timeScale = 1f;
        }

        public void ScoreGoal(bool isTeamA)
        {
            if (isTeamA) scoreTeamA++; else scoreTeamB++;
            OnScoreChanged?.Invoke(scoreTeamA, scoreTeamB);
            State = MatchState.GoalScored;
            Invoke(nameof(ResumeAfterGoal), 2.5f);
        }

        void ResumeAfterGoal()
        {
            if (State != MatchState.Finished) State = MatchState.Playing;
        }

        public void FinishMatch()
        {
            State = MatchState.Finished;
            OnMatchFinished?.Invoke();
        }
    }
}
