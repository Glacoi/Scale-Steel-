using UnityEngine;

namespace ScaleAndSteel.Core
{
    public class ArenaSpawner : MonoBehaviour
    {
        [Header("Spawning")]
        [SerializeField] private ChaserEnemy enemyPrefab;
        [SerializeField] private float spawnInterval = 1.5f;
        [SerializeField] private float spawnRadius = 9f;

        private Transform _player;
        private float _spawnTimer;

        private void Start()
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                _player = playerObj.transform;
            }
        }

        private void Update()
        {
            if (_player == null || enemyPrefab == null) return;

            _spawnTimer -= Time.deltaTime;
            if (_spawnTimer > 0f) return;

            Vector2 offset = Random.insideUnitCircle.normalized * spawnRadius;
            Vector3 spawnPosition = _player.position + new Vector3(offset.x, offset.y, 0f);
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            _spawnTimer = spawnInterval;
        }
    }
}