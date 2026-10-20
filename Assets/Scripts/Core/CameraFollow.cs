
using UnityEngine;
using RedWhiteFootball.Ball;

namespace RedWhiteFootball.Core
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform target; // ball or player
        public BallController ball;
        public Vector3 offset = new Vector3(0, 12f, -14f);
        public float smooth = 4f;
        public float lookAhead = 3f;

        void LateUpdate()
        {
            if (target == null && ball != null) target = ball.transform;
            if (target == null) return;

            Vector3 vel = Vector3.zero;
            if (ball != null)
            {
                var rb = ball.GetComponent<Rigidbody>();
                if (rb) vel = rb.velocity;
            }
            Vector3 desiredPos = target.position + offset + vel.normalized * lookAhead;
            transform.position = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * smooth);
            transform.LookAt(target.position + Vector3.up*0.5f);
        }
    }
}
