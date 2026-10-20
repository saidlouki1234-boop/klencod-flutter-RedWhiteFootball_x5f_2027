
using UnityEngine;
using RedWhiteFootball.Core;

namespace RedWhiteFootball.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        public bool isControlledByPlayer = true;
        public float walkSpeed = 3.5f;
        public float sprintSpeed = 6.2f;
        public float rotationSpeed = 720f;
        public float gravity = -18f;

        CharacterController cc;
        Animator anim;
        Vector3 velocity;
        float verticalVelocity;

        // For ball control
        public Transform ballControlPoint;
        public float ballControlDistance = 1.2f;

        void Awake()
        {
            cc = GetComponent<CharacterController>();
            anim = GetComponentInChildren<Animator>();
        }

        public void Move(Vector2 input, bool sprint)
        {
            if (GameManager.Instance != null && GameManager.Instance.State != MatchState.Playing) return;

            Vector3 move = new Vector3(input.x, 0, input.y);
            if (move.magnitude < 0.05f)
            {
                if (anim) anim.SetFloat("Speed", 0);
                ApplyGravity();
                return;
            }

            float speed = sprint ? sprintSpeed : walkSpeed;
            move = Vector3.ClampMagnitude(move, 1f) * speed;

            // Rotate
            Quaternion targetRot = Quaternion.LookRotation(move);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);

            // Move
            cc.Move(move * Time.deltaTime);
            ApplyGravity();

            if (anim) anim.SetFloat("Speed", sprint ? 1f : 0.5f);
        }

        void ApplyGravity()
        {
            if (cc.isGrounded && verticalVelocity < 0) verticalVelocity = -2f;
            verticalVelocity += gravity * Time.deltaTime;
            cc.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
        }

        public void PassBall(Transform target)
        {
            // handled by BallController
            if (anim) anim.SetTrigger("Pass");
        }

        public void Shoot(float power)
        {
            if (anim) anim.SetTrigger("Shoot");
        }
    }
}
