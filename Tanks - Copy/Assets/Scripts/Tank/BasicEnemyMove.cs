using UnityEngine;
using System.Collections;

public class BasicEnemyMove : MonoBehaviour {

    protected Transform playerTransform;
    public Transform turret { get; set; }
    public Transform bulletSpawnPoint { get; set; }
    //Next destination position of the NPC Tank    
    protected Vector3 destPos;
    //Tank Rotation Speed   
    private float curRotSpeed;
    private Transform tank;
    private float curSpeed;
    private float maxForwardSpeed = 15.0f;  //these are reversed
    private float maxBackwardSpeed = -25.0f;

    // Use this for initialization
    void Start ()
    {
        curSpeed = 30.0f;
        curRotSpeed = 2.0f;
        GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
        playerTransform = objPlayer.transform;
        if (!playerTransform)
            print("Player doesn't exist.. Please add one " + "with Tag named 'Player'");
        else
        {
            //Get the turret of the tank   
            tank = transform;
            turret = gameObject.transform.GetChild(1).GetChild(0).GetChild(0).transform;
            bulletSpawnPoint = turret.GetChild(0).GetChild(0).transform;
        }
    }


    // Update is called once per frame
    void Update()
    {
        //model is off by 45degrees

        //turret.transform.LookAt(playerTransform);
        //Set the target position as the player position
        destPos = playerTransform.position;
        //Always Turn the turret towards the player

        Quaternion turretRotation = Quaternion.LookRotation(destPos - turret.position);
        turretRotation *= Quaternion.Euler(0, 90, 0); //adds 90 degrees
        turret.transform.rotation = Quaternion.Slerp(turret.rotation, turretRotation, Time.deltaTime * curRotSpeed);

        if (Vector3.Distance(tank.position, destPos) >= 40.0f)
        {
            //Rotate to the target point      
            Quaternion targetRotation = Quaternion.LookRotation(destPos - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * curRotSpeed);
            //Go Forward    
            transform.Translate(Vector3.forward * Time.deltaTime * maxBackwardSpeed);
        }
    }

    public void updateTargeting()
    {

    }
}
