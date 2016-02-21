using UnityEngine;
using System.Collections;
using System;

public class PatrolState : State
{
    public GameObject[] waypoints;

    public StateManager manager;
    protected Transform tank;
    private float maxBackwardSpeed = 20.0f;
    private float curRotSpeed = 2.0f;
    private Boolean patrolling = true;
    protected Vector3 destPos;
    protected Transform playerTransform;
    protected Boolean enemySigthted = false;


    public PatrolState(StateManager manager)
    {
        this.manager = manager;
        //find all necessary waypoints - possibly only need to this once within stateManager
        waypoints = GameObject.FindGameObjectsWithTag("WanderPoint");
        tank = manager.getTransform();
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;

    }

    public void executeState()
    { 
        if(!patrolling)
        {
            int rndIndex = UnityEngine.Random.Range(0, waypoints.Length);
            destPos = waypoints[rndIndex].transform.position;
        }
        moveToWayPoint();


    }

    //continuously called by statemanger
    public void updateState()
    {

        //if (Vector3.Distance(tank.position, playerTransform.position) <= 30.0f)
        //{
        //    Debug.Log("Switch to Attack state");
        //    if(enemySigthted == false) enemySigthted = true;
        //    //if enemy tank found switch to attack state
        //    //manager.switchState(new AttackState());
        //}
        //else 
        if (Vector3.Distance(tank.position, playerTransform.position) <= 80.0f && Vector3.Distance(tank.position, playerTransform.position) > 30.0f)
        {
            Debug.Log("Switch to Chase state");
            if (enemySigthted == false) enemySigthted = true;
            //if enemy tank found switch to attack state
            manager.switchState(new ChaseState(manager, playerTransform));

        }
        else
        executeState();

    }


    public void moveToWayPoint()
    {
       
            if ((Vector3.Distance(tank.position, destPos) >= 40.0f))
            {
            
             //   Debug.Log("Patrolling state " + tank.ToString());
                patrolling = true;             

                Quaternion targetRotation = Quaternion.LookRotation(destPos - tank.position);
              //  Debug.Log("waypoint pos: " + destPos);

                tank.rotation = Quaternion.Slerp(tank.rotation, targetRotation, Time.deltaTime * curRotSpeed);
                //Go Forward    
                tank.Translate(Vector3.forward * Time.deltaTime * maxBackwardSpeed);
            }
        else
        {
            patrolling = false;
        }
        
    }

}
