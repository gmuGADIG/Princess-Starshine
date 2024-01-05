using UnityEngine;

class Exploder : EnemyTemplate {
    [Header("Exploder Properties")]
    [SerializeField] GameObject explosionPrefab;

    void OnDisable()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}
