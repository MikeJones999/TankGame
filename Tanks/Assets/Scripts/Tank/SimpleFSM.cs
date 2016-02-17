﻿using UnityEngine;
using System.Collections;
using System;

public class SimpleFSM : FSM {

    public enum FSMState
    {
        None,
        Patrol,
        Chase,
        Attack,
        Dead,
    }

    //Current state that the NPC is reaching    
    public FSMState curState;
    //Speed of the tank    
    private float curSpeed;
    //Tank Rotation Speed   
    private float curRotSpeed;
    //Bullet    
    public GameObject Bullet;
    //Whether the NPC is destroyed or not    
    private bool bDead;
    private int health;

    //Initialize the Finite state machine for the NPC tank    
    protected override void Initialize ()
    {
        curState = FSMState.Patrol;
        curSpeed = 150.0f;
        curRotSpeed = 2.0f;
        bDead = false;
        elapsedTime = 0.0f;
        shootRate = 3.0f;
        health = 100;
        //Get the list of points      
        pointList =  GameObject.FindGameObjectsWithTag("WanderPoint");
        //Set Random destination point first      
        FindNextPoint();
        //Get the target enemy(Player)      
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;
        if (!playerTransform)
            print("Player doesn't exist.. Please add one " + "with Tag named 'Player'");
        //Get the turret of the tank        
        turret = gameObject.transform.GetChild(1).GetChild(0).GetChild(0).transform;
        bulletSpawnPoint = turret.GetChild(0).GetChild(0).transform;
    }

    //Update each frame    
    protected override void FSMUpdate()
    {
        switch (curState)
        {
            case FSMState.Patrol: UpdatePatrolState(); break;
            case FSMState.Chase: UpdateChaseState(); break;
            case FSMState.Attack: UpdateAttackState(); break;
            case FSMState.Dead: UpdateDeadState(); break;
        }
        //Update the time     
        elapsedTime += Time.deltaTime;
        //Go to dead state is no health left     
        if (health <= 0)
            curState = FSMState.Dead;
    }

    private void UpdateAttackState()
    {
        //Set the target position as the player position
        destPos = playerTransform.position;
        //Check the distance with the player tank
        float dist = Vector3.Distance(transform.position, playerTransform.position);
        if (dist >= 200.0f && dist < 300.0f)
        {
            //Rotate to the target point
            Quaternion targetRotation =
            Quaternion.LookRotation(destPos -
            transform.position);
            transform.rotation = Quaternion.Slerp(
            transform.rotation, targetRotation,
            Time.deltaTime * curRotSpeed);
            //Go Forward
            transform.Translate(Vector3.forward *
            Time.deltaTime * curSpeed);
            curState = FSMState.Attack;
            }
            //Transition to patrol is the tank become too far
            else if (dist >= 300.0f)
            {
            curState = FSMState.Patrol;
            }
            //Always Turn the turret towards the player
            Quaternion turretRotation =  Quaternion.LookRotation(destPos - turret.position);
            turret.rotation =  Quaternion.Slerp(turret.rotation, turretRotation, Time.deltaTime* curRotSpeed);
            //Shoot the bullets
            ShootBullet();
          }

private void ShootBullet()
{
    if (elapsedTime >= shootRate)
    {
        //Shoot the bullet
        Instantiate(Bullet, bulletSpawnPoint.position,
        bulletSpawnPoint.rotation);
        elapsedTime = 0.0f;
    }
}
        

    private void UpdateChaseState()
    {
        //Set the target position as the player position      
        destPos = playerTransform.position;
        //Check the distance with the player tank      
        float dist = Vector3.Distance(transform.position, playerTransform.position);
        if (dist >= 200.0f && dist < 300.0f)
        {
            //Rotate to the target point        
            Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);

            //Go Forward    
            transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
            curState = FSMState.Attack;
        }
        //Transition to patrol is the tank become too far      
        else if (dist >= 300.0f)
        {
            curState = FSMState.Patrol;
        }

        //Always Turn the turret towards the player      
        Quaternion turretRotation =  Quaternion.LookRotation(destPos - turret.position);
        turret.rotation = Quaternion.Slerp(turret.rotation, turretRotation, Time.deltaTime * curRotSpeed);
        //Shoot the bullets      
        ShootBullet();
    }


    protected void UpdateDeadState()
    {
        //Show the dead animation with some physics effects      
        if (!bDead)
        {
            bDead = true;
            Explode();
        }
    }

    protected void Explode()
    {
        float rndX = UnityEngine.Random.Range(10.0f, 30.0f);
        float rndZ = UnityEngine.Random.Range(10.0f, 30.0f);
        for (int i = 0; i < 3; i++)
        {
            GetComponent<Rigidbody>().AddExplosionForce(10000.0f, transform.position - new Vector3(rndX, 10.0f, rndZ), 40.0f, 10.0f);
            GetComponent<Rigidbody>().velocity = transform.TransformDirection(new Vector3(rndX, 20.0f, rndZ));
        }
        Destroy(gameObject, 1.5f);
    }


    protected void UpdatePatrolState()
    {      //Find another random patrol point if the current       
           //point is reached     
        if (Vector3.Distance(transform.position, destPos) <=       100.0f)
        {
            print("Reached to the destination point\n"+ "calculating the next point");
            FindNextPoint();
        }
        //Check the distance with player tank      
        //When the distance is near, transition to chase state      
        else if (Vector3.Distance(transform.position, playerTransform.position) <= 300.0f)
        {
            print("Switch to Chase Position");
            curState = FSMState.Chase;
        }
        //Rotate to the target point      
        Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime* curRotSpeed);
        //Go Forward    
        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }

    protected void FindNextPoint()
    {
        print("Finding next point");
        int rndIndex = UnityEngine.Random.Range(0, pointList.Length);
        float rndRadius = 10.0f; Vector3 rndPosition = Vector3.zero;
        destPos = pointList[rndIndex].transform.position + rndPosition;
        //Check Range to decide the random point       
        //as the same as before      
        if (IsInCurrentRange(destPos))
        {
            rndPosition = new Vector3(UnityEngine.Random.Range(-rndRadius, rndRadius), 0.0f, UnityEngine.Random.Range(-rndRadius, rndRadius));
            destPos = pointList[rndIndex].transform.position + rndPosition;
        }
    }
    protected bool IsInCurrentRange(Vector3 pos)
    {
        float xPos = Mathf.Abs(pos.x - transform.position.x);
        float zPos = Mathf.Abs(pos.z - transform.position.z);
        if (xPos <= 50 && zPos <= 50) return true;
        return false;
    }

    void OnCollisionEnter(Collision collision)
    {
        //Reduce health
        if (collision.gameObject.tag == "Bullet")
        {
            health -= collision.gameObject.GetComponent
            <Bullet>().damage;
        }
    }

}
