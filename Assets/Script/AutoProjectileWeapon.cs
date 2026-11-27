using UnityEngine;

namespace ScaleAndSteel.Core
{
    public class AutoProjectileWeapon : MonoBehaviour
    {
        [Header("Weapon Tuning")]
        [SerializeField] private Projectile projectilePrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float fireInterval = 0.65f;
        [SerializeField] private float searchRadius = 10f;
        [SerializeField] private LayerMask enemyLayer;

        private float _cooldownTimer;

        private void Awake()
        {
            if (firePoint == null) firePoint = transform;
        }

        private void Update()
        {
            _cooldownTimer -= Time.deltaTime;
            if (_cooldownTimer > 0f) return;

            Transform target = FindNearestEnemy();
            if (target == null) return;

            Vector2 dir = (target.position - firePoint.position).normalized;
            Projectile proj = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
            proj.Initialize(dir);

            _cooldownTimer = fireInterval;
        }

        private Transform FindNearestEnemy()
        {
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, searchRadius, enemyLayer);
            Transform closest = null;
            float bestDistSqr = float.MaxValue;

            foreach (Collider2D hit in hits)
            {
                float dSqr = (hit.transform.position - transform.position).sqrMagnitude;
                if (dSqr < bestDistSqr)
                {
                    bestDistSqr = dSqr;
                    closest = hit.transform;
                }
            }

            return closest;
        }
    }
}