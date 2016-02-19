using UnityEngine;
using System.Collections;
public class Bullet : MonoBehaviour
{    //Explosion Effect   
   // public GameObject Explosion;
    public float Speed = 100.0f;
    public float LifeTime = 3.0f;
    public int damage = 50;
    public GameObject explosion;

    void Start()
    {
        Destroy(gameObject, LifeTime);
    }
    void Update()
    {
        transform.position += transform.forward * Speed * Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        GameObject objHit = collision.gameObject;
        if (objHit.tag == "AI Tank" || objHit.tag == "Player")
        {
            ContactPoint contact = collision.contacts[0];
            Instantiate(explosion, contact.point, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    void Explode()
    {
        var exp = GetComponent<ParticleSystem>();
        exp.Play();
        Destroy(gameObject, exp.duration);
    }
}

