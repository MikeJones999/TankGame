using UnityEngine;
using System.Collections;

public class StateManager : MonoBehaviour {

    //protected StateManager manager;
    protected State currentState;
    public bool isActive = false;

	// Use this for initialization
	void Start ()
    {
        print("Starting StateManager");
        currentState = new PatrolState(this);
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (currentState != null)
        {
            currentState.updateState();
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

}
