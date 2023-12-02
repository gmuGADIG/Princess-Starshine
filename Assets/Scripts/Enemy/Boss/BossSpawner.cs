using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [SerializeField] float spawnTime = 300f;
    [SerializeField] GameObject bossPrefab;

    Transform playerTransform;

    private void Start()
    {
        StartCoroutine(SpawnBossCoroutine());
        playerTransform = FindObjectOfType<Player>().transform;
    }

    IEnumerator SpawnBossCoroutine()
    {
        yield return new WaitForSeconds(spawnTime);
        Instantiate(bossPrefab, playerTransform.position + Vector3.right * 5, Quaternion.identity);
    }
}
