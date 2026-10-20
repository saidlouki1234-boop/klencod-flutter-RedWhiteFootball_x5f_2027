
using UnityEngine;
using RedWhiteFootball.Core;
using RedWhiteFootball.Ball;

namespace RedWhiteFootball.Core
{
    public class GoalSystem : MonoBehaviour
    {
        public bool isTeamAGoal = false; // if true, scoring here = goal for Team A
        public ParticleSystem goalFX;
        public AudioSource goalSound;

        void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Ball")) return;
            var ball = other.GetComponent<BallController>();
            if (ball == null) return;

            // Prevent double counting
            if (GameManager.Instance.State != MatchState.Playing) return;

            // isTeamAGoal = the goal owned by Team A, so scoring into it is for Team B? Actually standard: if ball enters Team B goal, Team A scores.
            // Here simplified: isTeamAGoal means this is Team A's net, so goal for Team B
            bool goalForTeamA = !isTeamAGoal;

            GameManager.Instance.ScoreGoal(goalForTeamA);
            if (goalFX) goalFX.Play();
            if (goalSound) goalSound.Play();

            // Reset ball after delay
            ball.ResetBallDelayed(2f);
        }
    }
}
