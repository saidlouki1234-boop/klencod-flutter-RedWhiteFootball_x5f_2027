
using UnityEngine;
using UnityEngine.EventSystems;
using RedWhiteFootball.Player;
using RedWhiteFootball.Ball;

namespace RedWhiteFootball.Player
{
    public class TouchController : MonoBehaviour
    {
        [Header("References")]
        public PlayerController controlledPlayer;
        public BallController ball;
        public FixedJoystick joystick; // custom implementation below if not present

        [Header("Buttons")]
        public bool passPressed;
        public bool shootPressed;
        public bool sprintHeld;
        public float shootCharge = 0f;

        float shootChargeTime = 0f;
        const float maxChargeTime = 1.2f;

        void Update()
        {
            if (controlledPlayer == null) return;

            Vector2 input = Vector2.zero;
            if (joystick != null) input = new Vector2(joystick.Horizontal, joystick.Vertical);
            else
            {
                // Fallback: WASD for editor testing
                input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            }

            controlledPlayer.Move(input, sprintHeld || Input.GetKey(KeyCode.LeftShift));

            // Shoot charging
            if (shootPressed || Input.GetKey(KeyCode.Space))
            {
                shootChargeTime += Time.deltaTime;
                shootCharge = Mathf.Clamp01(shootChargeTime / maxChargeTime);
            }
            else if (shootChargeTime > 0)
            {
                // Release shoot
                float power = Mathf.Lerp(8f, 24f, shootCharge);
                TryShoot(power);
                shootChargeTime = 0; shootCharge = 0;
            }

            if (passPressed || Input.GetKeyDown(KeyCode.X))
            {
                TryPass();
                passPressed = false;
            }
        }

        void TryPass()
        {
            if (ball == null) return;
            ball.RequestPass(controlledPlayer.transform);
            controlledPlayer.PassBall(null);
        }

        void TryShoot(float power)
        {
            if (ball == null) return;
            ball.RequestShoot(controlledPlayer.transform, power);
            controlledPlayer.Shoot(power);
        }

        // UI callbacks
        public void OnSprintDown() => sprintHeld = true;
        public void OnSprintUp() => sprintHeld = false;
        public void OnPassClick() => passPressed = true;
        public void OnShootDown() { shootPressed = true; shootChargeTime = 0; }
        public void OnShootUp() { shootPressed = false; }
    }

    // Minimal joystick without external asset
    public class FixedJoystick : MonoBehaviour, IDragHandler, IPointerDownHandler, IPointerUpHandler
    {
        public RectTransform background;
        public RectTransform handle;
        public float range = 80f;
        Vector2 input = Vector2.zero;
        public float Horizontal => input.x;
        public float Vertical => input.y;

        void Awake()
        {
            if (background == null) background = GetComponent<RectTransform>();
        }

        public void OnPointerDown(PointerEventData eventData) => OnDrag(eventData);
        public void OnDrag(PointerEventData eventData)
        {
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(background, eventData.position, eventData.pressEventCamera, out pos);
            pos = Vector2.ClampMagnitude(pos, range);
            if (handle) handle.anchoredPosition = pos;
            input = pos / range;
        }
        public void OnPointerUp(PointerEventData eventData)
        {
            input = Vector2.zero;
            if (handle) handle.anchoredPosition = Vector2.zero;
        }
    }
}
