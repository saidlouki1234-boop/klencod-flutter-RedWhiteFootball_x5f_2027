
using UnityEngine;

namespace RedWhiteFootball.Core
{
    public class Bootstrapper : MonoBehaviour
    {
        public GameObject stadiumBuilderPrefab;
        public GameObject playerPrefab;
        public GameObject ballPrefab;
        public GameObject cameraPrefab;

        void Start()
        {
            // Create managers if missing
            if (GameManager.Instance == null)
            {
                var gm = new GameObject("GameManager");
                gm.AddComponent<GameManager>();
            }

            // Build stadium if no builder in scene
            if (FindObjectOfType<StadiumBuilder>() == null)
            {
                var sb = new GameObject("StadiumBuilder");
                sb.AddComponent<StadiumBuilder>();
            }

            // Spawn ball
            if (FindObjectOfType<RedWhiteFootball.Ball.BallController>() == null)
            {
                var ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                ball.name = "Ball"; ball.tag = "Ball";
                ball.transform.position = new Vector3(0,0.5f,0);
                ball.transform.localScale = Vector3.one * 0.45f;
                var rb = ball.AddComponent<Rigidbody>();
                rb.mass = 0.43f; rb.drag = 0.2f; rb.angularDrag = 0.4f;
                ball.AddComponent<RedWhiteFootball.Ball.BallController>();
                var col = ball.GetComponent<SphereCollider>();
                col.material = new PhysicsMaterial(){ bounciness=0.6f, dynamicFriction=0.3f };
            }

            // Spawn player TeamA (controlled)
            SpawnTeam("TeamA", new Vector3(0,0,-10), true, Color.white);
            // Spawn TeamB AI
            SpawnTeam("TeamB", new Vector3(0,0,10), false, new Color(0.05f,0.2f,0.7f));
        }

        void SpawnTeam(string tag, Vector3 center, bool isPlayerTeam, Color color)
        {
            for (int i=0;i<5;i++)
            {
                var capsule = GameObject.CreatePrimitive(PrimitiveType.Capsule);
                capsule.name = $"{tag}_Player_{i}";
                capsule.tag = tag;
                capsule.transform.position = center + new Vector3(Random.Range(-12f,12f),1f, Random.Range(-6f,6f));
                var cc = capsule.AddComponent<CharacterController>();
                cc.center = new Vector3(0,1,0); cc.height = 2;
                if (isPlayerTeam && i==0)
                {
                    var pc = capsule.AddComponent<RedWhiteFootball.Player.PlayerController>();
                    pc.isControlledByPlayer = true;
                    pc.ballControlPoint = capsule.transform;
                }
                else
                {
                    var agent = capsule.AddComponent<UnityEngine.AI.NavMeshAgent>();
                    agent.baseOffset = 1f;
                    capsule.AddComponent<RedWhiteFootball.AI.AIController>();
                }
                // Color
                var rend = capsule.GetComponent<Renderer>();
                var mat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
                mat.color = isPlayerTeam && i%2==0 ? new Color(0.82f,0.05f,0.12f) : (isPlayerTeam? Color.white : color);
                rend.material = mat;
            }

            // Goalkeeper
            var gk = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            gk.name = $"{tag}_GK";
            gk.tag = tag;
            gk.transform.position = center + new Vector3(0,1, isPlayerTeam ? -42f : 42f);
            gk.AddComponent<RedWhiteFootball.AI.GoalkeeperAI>().lineZ = isPlayerTeam ? -51f : 51f;
            var rb = gk.AddComponent<Rigidbody>();
            rb.freezeRotation = true;
        }
    }
}
