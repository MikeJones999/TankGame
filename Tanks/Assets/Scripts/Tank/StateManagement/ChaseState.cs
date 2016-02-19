using UnityEngine;
using System.Collections;
using System;

public class ChaseState : State
{

    public StateManager manager;
    protected Transform tank;
    private float maxBackwardSpeed = 20.0f;
    private float curRotSpeed = 2.0f;
    private Boolean patrolling = true;
    protected Vector3 destPos;
    protected Transform playerTransform;
    protected Boolean enemySigthted = false;
    protected Transform turret;
    protected Transform bulletSpawnPoint;


    public ChaseState(StateManager manager, Transform playerUnit)
    {
        this.manager = manager;
        tank = manager.getTransform();
        playerTransform = playerUnit;
        turret = tank.transform.GetChild(1).GetChild(0).GetChild(0).transform;
        bulletSpawnPoint = turret.GetChild(0).GetChild(0).transform;

    }

    public void executeState()
    {
        destPos = playerTransform.position;
        moveToWayPoint();
    }

    //continuously called by statemanger
    public void updateState()
    {

        if (Vector3.Distance(tank.position, playerTransform.position) <= 60.0f)
        {
            Debug.Log("Switch to Attack state");
            //if enemy tank found switch to attack state
            manager.switchState(new AttackState(manager, playerTransform));
        }
        else
        if (Vector3.Distance(tank.position, playerTransform.position) > 80.0f || playerTransform == null)
        {
            Debug.Log("Switch to Patrol state");
            //if enemy tank found switch to attack state
            manager.switchState(new PatrolState(manager));

        }
        else
            executeState();

    }


    public void moveToWayPoint()
    {

        Quaternion turretRotation = Quaternion.LookRotation(destPos - turret.position);
        turretRotation *= Quaternion.Euler(0, 90, 0); //adds 90 degrees
        turret.transform.rotation = Quaternion.Slerp(turret.rotation, turretRotation, Time.deltaTime * curRotSpeed);

            Debug.Log("Chasing state " + tank.ToString());
            patrolling = true;

            Quaternion targetRotation = Quaternion.LookRotation(destPos - tank.position);
            Debug.Log("waypoint pos: " + destPos);

            tank.rotation = Quaternion.Slerp(tank.rotation, targetRotation, Time.deltaTime * curRotSpeed);
            //Go Forward    
            tank.Translate(Vector3.forward * Time.deltaTime * maxBackwardSpeed);
 
    }

}

