
using UnityEngine;
using RedWhiteFootball.Ball;
using RedWhiteFootball.Core;

namespace RedWhiteFootball.AI
{
    [RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
    public class AIController : MonoBehaviour
    {
        UnityEngine.AI.NavMeshAgent agent;
        public BallController ball;
        public float detectionRadius = 20f;
        public float attackZ = 0f;
        public float homeZ = -30f;
        Transform[] teammates;

        void Awake()
        {
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            if (ball == null) ball = FindObjectOfType<BallController>();
        }

        void Start()
        {
            // Find teammates (TeamB)
            var objs = GameObject.FindGameObjectsWithTag("TeamB");
            teammates = new Transform[objs.Length];
            for (int i=0;i<objs.Length;i++) teammates[i]=objs[i].transform;
            InvokeRepeating(nameof(UpdateAI), 0.3f, 0.35f);
        }

        void UpdateAI()
        {
            if (GameManager.Instance.State != MatchState.Playing) return;
            if (ball == null) return;

            float distToBall = Vector3.Distance(transform.position, ball.transform.position);
            bool isClosest = IsClosestToBall();

            if (distToBall < 2.2f && isClosest)
            {
                // Shoot if in opponent half
                if (transform.position.z < 0) // TeamB attacks south (-Z)
                {
                    // Pass or shoot decision
                    if (transform.position.z < -25f && Random.value > 0.5f)
                    {
                        // Shoot
                        ball.ClearOwner();
                        Vector3 goal = new Vector3(Random.Range(-3f,3f), 0.5f, -52f);
                        var rb = ball.GetComponent<Rigidbody>();
                        rb.velocity = (goal - ball.transform.position).normalized * Random.Range(14f,22f) + Vector3.up*1.2f;
                    }
                    else
                    {
                        // Pass to random teammate
                        if (teammates.Length>1)
                        {
                            var target = teammates[Random.Range(0,teammates.Length)];
                            if (target != transform)
                            {
                                var rb = ball.GetComponent<Rigidbody>();
                                rb.velocity = (target.position - ball.transform.position).normalized * 14f + Vector3.up*0.5f;
                                ball.ClearOwner();
                            }
                        }
                    }
                }
                agent.SetDestination(ball.transform.position);
            }
            else
            {
                // Positioning
                Vector3 ballPos = ball.transform.position;
                // Stay between ball and own goal, keep formation
                float zTarget = Mathf.Clamp(ballPos.z - 10f, homeZ-15f, 35f);
                float xTarget = Mathf.Clamp(ballPos.x * 0.6f + Random.Range(-6f,6f), -28f, 28f);
                agent.SetDestination(new Vector3(xTarget, 0, zTarget));
            }
        }

        bool IsClosestToBall()
        {
            float myDist = Vector3.Distance(transform.position, ball.transform.position);
            foreach (var t in teammates)
            {
                if (t == transform) continue;
                if (Vector3.Distance(t.position, ball.transform.position) < myDist - 0.5f) return false;
            }
            return true;
        }
    }
}
