using UnityEngine;
using System.Collections;
public class Bullet : MonoBehaviour
{    //Explosion Effect   
   // public GameObject Explosion;
    public float Speed = 100.0f;
    public float LifeTime = 3.0f;
    public int damage = 10;
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
        Destroy(gameObject);
        ContactPoint contact = collision.contacts[0];
 
        GameObject objHit = collision.gameObject;
        if (objHit.tag == "AI Tank")
        {
            int health = objHit.GetComponent<StateManager>().getHealth();
            Debug.Log("****************** Health: " + health);
           
           
            Object exp = Instantiate(explosion, contact.point, Quaternion.identity);
            health -= 10;
            objHit.GetComponent<StateManager>().setHealth(health);
            Destroy(exp, 0.5f);
        }
        else if (objHit.tag == "Player")
        {
            Object exp = Instantiate(explosion, contact.point, Quaternion.identity);
            Destroy(exp, 0.5f);
        }
    }

}

