using UnityEngine;

namespace ScaleAndSteel.Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class KnightMotor : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float moveSpeed = 5.5f;
        [SerializeField] private bool flipSpriteHorizontal = true;

        [Header("Hero Knight Animator")]
        [SerializeField] private Animator animator;
        [SerializeField] private string animStateParam = "AnimState";
        [SerializeField] private string groundedParam = "Grounded";
        [SerializeField] private int idleStateValue = 0;
        [SerializeField] private int runStateValue = 2;

        private Rigidbody2D _rb;
        private Vector2 _moveInput;
        private bool _hasAnimState;
        private bool _hasGrounded;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            if (animator == null) animator = GetComponent<Animator>();
            CacheParameters();
        }

        private void CacheParameters()
        {
            if (animator == null) return;
            foreach (var p in animator.parameters)
            {
                if (p.name == animStateParam && p.type == AnimatorControllerParameterType.Int)
                    _hasAnimState = true;
                else if (p.name == groundedParam && p.type == AnimatorControllerParameterType.Bool)
                    _hasGrounded = true;
            }
        }

        private void Start()
        {
            if (_hasGrounded) animator.SetBool(groundedParam, true);
        }

        private void Update()
        {
            float x = Input.GetAxisRaw("Horizontal");
            float y = Input.GetAxisRaw("Vertical");
            _moveInput = new Vector2(x, y).normalized;

            if (flipSpriteHorizontal && Mathf.Abs(x) > 0.01f)
            {
                Vector3 s = transform.localScale;
                s.x = Mathf.Abs(s.x) * (x > 0 ? 1f : -1f);
                transform.localScale = s;
            }

            if (animator != null && _hasAnimState)
            {
                bool moving = _moveInput.sqrMagnitude > 0.001f;
                animator.SetInteger(animStateParam, moving ? runStateValue : idleStateValue);
            }
        }

        private void FixedUpdate()
        {
            _rb.linearVelocity = _moveInput * moveSpeed;
        }
    }
}