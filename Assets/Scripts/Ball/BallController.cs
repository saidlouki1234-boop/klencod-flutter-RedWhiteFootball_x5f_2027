
using UnityEngine;
using RedWhiteFootball.Player;
using System.Collections;

namespace RedWhiteFootball.Ball
{
    [RequireComponent(typeof(Rigidbody))]
    public class BallController : MonoBehaviour
    {
        Rigidbody rb;
        Transform owner;
        public float followLerp = 12f;
        public float passSpeed = 14f;
        public float shootCurve = 0.1f;
        Vector3 startPos;
        bool isResetting = false;

        void Awake()
        {
            rb = GetComponent<Rigidbody>();
            startPos = transform.position;
            rb.maxAngularVelocity = 50f;
        }

        void FixedUpdate()
        {
            // If has owner, stick near owner
            if (owner != null && !isResetting)
            {
                Vector3 target = owner.position + owner.forward * 0.9f + Vector3.up * 0.1f;
                Vector3 dir = target - transform.position;
                rb.velocity = dir * followLerp;
                // Small auto dribble
                if (dir.magnitude > 0.2f) rb.angularVelocity = Vector3.Cross(Vector3.up, dir).normalized * 8f;
            }
        }

        public void SetOwner(Transform newOwner) => owner = newOwner;
        public void ClearOwner() => owner = null;

        public void RequestPass(Transform fromPlayer)
        {
            if (owner != fromPlayer && owner != null) return; // only owner can pass
            ClearOwner();
            // Find nearest teammate
            var teammates = GameObject.FindGameObjectsWithTag("TeamA");
            Transform best = null; float bestDist = Mathf.Infinity;
            foreach (var t in teammates)
            {
                if (t.transform == fromPlayer) continue;
                float d = Vector3.Distance(t.transform.position, fromPlayer.position);
                if (d < bestDist && d < 25f) { bestDist = d; best = t.transform; }
            }
            Vector3 target = best ? best.position : fromPlayer.position + fromPlayer.forward * 12f;
            Vector3 dir = (target - transform.position); dir.y = 0.2f;
            rb.velocity = dir.normalized * passSpeed;
            rb.angularVelocity = Random.insideUnitSphere * 3f;
        }

        public void RequestShoot(Transform fromPlayer, float power)
        {
            if (owner != fromPlayer && owner != null) return;
            ClearOwner();
            // Aim to opponent goal (assume at +Z)
            Vector3 goalPos = new Vector3(Random.Range(-3f,3f), 0.8f, 52f); // north goal
            Vector3 dir = (goalPos - transform.position);
            dir.y = Mathf.Lerp(0.3f, 2.5f, power/24f);
            rb.velocity = dir.normalized * power;
            rb.AddTorque(Vector3.Cross(dir.normalized, Vector3.up) * shootCurve * 100f);
        }

        void OnCollisionEnter(Collision c)
        {
            if (c.gameObject.CompareTag("Player") || c.gameObject.CompareTag("TeamA") || c.gameObject.CompareTag("TeamB"))
            {
                // Auto attach if slow
                if (rb.velocity.magnitude < 6f)
                {
                    owner = c.transform;
                }
            }
        }

        public void ResetBallDelayed(float delay)
        {
            if (isResetting) return;
            StartCoroutine(ResetRoutine(delay));
        }

        IEnumerator ResetRoutine(float delay)
        {
            isResetting = true;
            yield return new WaitForSeconds(delay);
            transform.position = startPos;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            owner = null;
            isResetting = false;
        }
    }
}
