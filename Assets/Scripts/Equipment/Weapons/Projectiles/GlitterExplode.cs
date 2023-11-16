using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitterExplode : MonoBehaviour
{
    float damage;
    string explodeSound = "Glitter_Bomb_Explode";

    void Start()
    {
        SoundManager.Instance.PlaySoundGlobal(explodeSound);
        StartCoroutine(Coroutine());
        IEnumerator Coroutine()
        {
            yield return new WaitForSeconds(0.25f);
            Destroy(gameObject);
        }
    }

    public void Create(float newDamage, Vector3 scale)
    {
        this.transform.localScale = 2.5f*scale;
        this.damage = newDamage;
        this.GetComponent<ProjectileCollision>().SetDamage(newDamage);
    }
}