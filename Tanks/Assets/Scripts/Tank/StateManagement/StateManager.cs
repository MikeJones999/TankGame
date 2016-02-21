using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour {

    public GameObject missile;
    //protected StateManager manager;
    protected GameObject tank;
    protected State currentState;
    public bool isActive = false;
    public int health = 100;
    public GameObject explosion;


    // Use this for initialization
    void Start ()
    {
        print("Starting StateManager");
        currentState = new PatrolState(this);
        tank = gameObject;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (currentState != null)
        {
            currentState.updateState();
        }

        if(health <=0)
        {
            explode();
        }
	}

    public void switchState(State newState)
    {
        currentState = newState;
    }

    public Transform getTransform()
    {
        return transform;
    }

    public GameObject getMissile()
    {
        return missile;
    }

    public int getHealth()
    {
        return health;
    }


    public void setHealth(int health)
    {
        this.health = health;
    }


    protected void explode()
    {
        Debug.Log("Vehicle destroyed!!!!!!!!!!!!!!");
        Object exp = Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
        Destroy(exp, 10.0f);
        Destroy(tank);
    }

}
