using UnityEngine;

namespace ScaleAndSteel.Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ChaserEnemy : MonoBehaviour, IDamageable
    {
        [Header("Stats")]
        [SerializeField] private float moveSpeed = 2.6f;
        [SerializeField] private float maxHp = 30f;
        [SerializeField] private float contactDamage = 10f;

        private Rigidbody2D _rb;
        private Transform _player;
        private float _currentHp;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _currentHp = maxHp;

            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                _player = playerObj.transform;
            }
        }

        private void FixedUpdate()
        {
            if (_player == null) return;

            Vector2 direction = ((Vector2)_player.position - _rb.position).normalized;
            _rb.linearVelocity = direction * moveSpeed;
        }

        public void TakeDamage(float amount)
        {
            _currentHp -= amount;
            if (_currentHp <= 0f)
            {
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player") && other.TryGetComponent<IDamageable>(out var damageable))
            {
                damageable.TakeDamage(contactDamage);
            }
        }
    }
}