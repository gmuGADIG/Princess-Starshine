using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShooterScript : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform projectileSpawn;
    public float projectileSpeed = 30f;
    public float lifeTime = 3f;
    public float fireTimer = 10f;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        fireTimer -= 1 * Time.deltaTime;
        if (fireTimer <= 0)
        {
            Fire();
            fireTimer = 1f;
        }
    }
    public void Fire()
        { 
        GameObject projectile = Instantiate(projectilePrefab, projectileSpawn.position, Quaternion.identity);
        projectile.AddComponent(typeof(TempConstantScript));
        projectile.AddComponent<CircleCollider2D>();
        projectile.AddComponent<Rigidbody2D>();
        Physics2D.IgnoreCollision(projectile.GetComponent<CircleCollider2D>(), projectileSpawn.GetComponent<Collider2D>());
        projectile.transform.position = projectileSpawn.position;
        Vector3 rotation = projectile.transform.rotation.eulerAngles;
        projectile.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
        //projectile.GetComponent<Rigidbody2D>().AddForce(projectileSpawn.forward * projectileSpeed * 5, ForceMode2D.Impulse);
        DestroyProjectile(projectile, lifeTime);


    }

    private IEnumerator DestroyProjectile(GameObject projectile, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(projectile);
    }

}