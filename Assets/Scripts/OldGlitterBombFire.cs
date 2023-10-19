using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitterBombFire : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform projectileSpawn;
    public float projectileSpeed = 30f;
    public float lifeTime = 3f;
    // IGNORE THIS IF THIS IS IMPORTED, SOMEONE ELSE IS WORKING ON THIS ALREADY
    void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        Fire();
    }
    private void Fire()
    {
        GameObject projectile = Instantiate(projectilePrefab);
        //projectile.AddComponent(typeof(ConstantScript));
        projectile.AddComponent<Rigidbody2D>();
        //Physics2D.IgnoreCollision(projectile.GetComponent<Collider2D>(), projectileSpawn.parent.GetComponent<Collider2D>());
        projectile.transform.position = projectileSpawn.position;
        Vector3 rotation = projectile.transform.rotation.eulerAngles;
        projectile.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z);
        projectile.GetComponent<Rigidbody>().AddForce(projectileSpawn.forward * projectileSpeed, ForceMode.Impulse);
        StartCoroutine("DestroyProjectile");


    }

    private IEnumerator DestroyProjectile(GameObject projectile, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(projectile);
    }
    
}
