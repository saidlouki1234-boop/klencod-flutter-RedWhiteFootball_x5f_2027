
using UnityEngine;
using RedWhiteFootball.Ball;
using RedWhiteFootball.Core;

namespace RedWhiteFootball.AI
{
    public class GoalkeeperAI : MonoBehaviour
    {
        public BallController ball;
        public Transform goalCenter;
        public float lineZ = -51f; // for team B goalkeeper
        public float maxSide = 6f;
        public float reactionSpeed = 4.5f;
        Vector3 startPos;
        Rigidbody rb;

        void Awake()
        {
            startPos = transform.position;
            rb = GetComponent<Rigidbody>();
            if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            if (ball == null) ball = FindObjectOfType<BallController>();
            if (goalCenter == null) goalCenter = transform;
        }

        void Update()
        {
            if (GameManager.Instance != null && GameManager.Instance.State != MatchState.Playing) return;
            if (ball == null) return;

            Vector3 ballPos = ball.transform.position;
            Rigidbody ballRb = ball.GetComponent<Rigidbody>();

            // Predict ball X when it reaches goal line
            bool isThreat = false;
            if (ballRb != null)
            {
                Vector3 vel = ballRb.velocity;
                if (Mathf.Abs(vel.z) > 1f)
                {
                    float timeToGoal = (lineZ - ballPos.z) / vel.z;
                    if (timeToGoal > 0 && timeToGoal < 2.5f)
                    {
                        isThreat = true;
                        float predictedX = ballPos.x + vel.x * timeToGoal;
                        float targetX = Mathf.Clamp(predictedX, goalCenter.position.x - maxSide, goalCenter.position.x + maxSide);
                        // Dive if close
                        if (Mathf.Abs(predictedX - transform.position.x) > 2f && timeToGoal < 0.8f)
                        {
                            // Dive
                            Vector3 diveDir = new Vector3(targetX - transform.position.x, 0.8f, 0);
                            rb.velocity = diveDir.normalized * 9f;
                        }
                        else
                        {
                            // Shuffle
                            Vector3 target = new Vector3(targetX, transform.position.y, lineZ + (lineZ < 0 ? 1.2f : -1.2f));
                            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * reactionSpeed);
                        }
                    }
                }
            }

            if (!isThreat)
            {
                // Idle track ball
                float xFollow = Mathf.Clamp(ballPos.x * 0.35f, goalCenter.position.x - 3f, goalCenter.position.x + 3f);
                Vector3 idleTarget = new Vector3(xFollow, transform.position.y, lineZ + (lineZ < 0 ? 1f : -1f));
                transform.position = Vector3.Lerp(transform.position, idleTarget, Time.deltaTime * 2f);
            }
        }
    }
}
