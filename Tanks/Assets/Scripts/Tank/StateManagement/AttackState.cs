using UnityEngine;

public class AttackState : State
{
    private StateManager manager;
    private Transform playerTransform;
    private Transform tank;
    protected Transform turret;
    protected Transform bulletSpawnPoint;
    private Vector3 destPos;
    private float maxBackwardSpeed = 20.0f;
    private float curRotSpeed = 2.0f;
    private float elapsedTime;
    private float shootRate;


    public AttackState(StateManager manager, Transform playerTransform)
    {
        this.manager = manager;
        this.playerTransform = playerTransform;
        tank = manager.getTransform();
        turret = tank.transform.GetChild(1).GetChild(0).GetChild(0).transform;
        bulletSpawnPoint = turret.GetChild(0).GetChild(0).transform;
        elapsedTime = 0.0f;
        shootRate = 3.0f;
    }

    public void executeState()
    {
        destPos = playerTransform.position;
        targetAndFire();

    }

  

    public void updateState()
    {
        elapsedTime += Time.deltaTime;
        if (Vector3.Distance(tank.position, playerTransform.position) > 60  && Vector3.Distance(tank.position, playerTransform.position) <= 80)
        {
            Debug.Log("Switch to Chase state");
            //if enemy tank found switch to attack state
            manager.switchState(new ChaseState(manager, playerTransform));
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

    private void targetAndFire()
    {
        fire();

        Quaternion turretRotation = Quaternion.LookRotation(destPos - turret.position);
        turretRotation *= Quaternion.Euler(0, 90, 0); //adds 90 degrees
        turret.transform.rotation = Quaternion.Slerp(turret.rotation, turretRotation, Time.deltaTime * curRotSpeed);

        Debug.Log("Chasing state " + tank.ToString());  

        Quaternion targetRotation = Quaternion.LookRotation(destPos - tank.position);
        Debug.Log("waypoint pos: " + destPos);

        tank.rotation = Quaternion.Slerp(tank.rotation, targetRotation, Time.deltaTime * curRotSpeed);
        //Go Forward    
        tank.Translate(Vector3.forward * Time.deltaTime * maxBackwardSpeed);
    }

    private void fire()
    {
        if (elapsedTime >= shootRate)
        {
            //Shoot the bullet
            GameObject.Instantiate (manager.getMissile(), bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            elapsedTime = 0.0f;
        }
    }

}
