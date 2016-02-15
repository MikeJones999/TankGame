using UnityEngine;
using System.Collections;
using System;

public class PlayerTankController : MonoBehaviour {

    public GameObject Bullet;
    private Transform Turret;
    private Transform bulletSpawnPoint;
    private float curSpeed, targetSpeed, rotSpeed;
    private float turretRotSpeed = 10.0f;
    private float maxForwardSpeed = 15.0f;  //these are reversed
    private float maxBackwardSpeed = -25.0f;
    //Bullet shooting rate    
    protected float shootRate = 0.5f;
    protected float elapsedTime;



    // Use this for initialization
    void Start ()
    {
        //Tank Settings      
        rotSpeed = 150.0f;
        //Get the turret of the tank      
        Turret = gameObject.transform.GetChild(1).GetChild(0).GetChild(0).transform;
        bulletSpawnPoint = Turret.GetChild(0).transform; 
    }

    // Update is called once per frame
    void Update ()
    {
        UpdateWeapon();
        UpdateControl();
    }

    private void UpdateControl()
    {
        //AIMING WITH THE MOUSE      
        //Generate a plane that intersects the transform's   
        //position with an upwards normal.      
        //Plane playerPlane = new Plane(Vector3.up, transform.position + new Vector3(0, 0, 0));
        Plane playerPlane = new Plane(Vector3.up, Vector3.zero);
        // Generate a ray from the cursor position      
        Ray RayCast = Camera.main.ScreenPointToRay(Input.mousePosition);
        //Determine the point where the cursor ray intersects the plane.      
        float HitDist = 0.0f;
        // If the ray is parallel to the plane, Raycast will return false.  

        if (playerPlane.Raycast(RayCast, out HitDist))
        {
            Vector3 RayHitPoint = RayCast.GetPoint(HitDist);
            Vector3 direction = RayHitPoint - Turret.transform.position;
            float rotation = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Turret.transform.rotation = Quaternion.Euler(0, rotation + 90, 0);

        }
        if (Input.GetKey(KeyCode.W))
        {
            targetSpeed = maxBackwardSpeed; //maxForwardSpeed; 
        }
        else if (Input.GetKey(KeyCode.S))
        {
            targetSpeed = maxForwardSpeed; // maxBackwardSpeed; 
        }
        else
        {
            targetSpeed = 0;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Rotate(0, -rotSpeed * Time.deltaTime, 0.0f);
        }
        else if (Input.GetKey(KeyCode.D))
        {
            transform.Rotate(0, rotSpeed * Time.deltaTime, 0.0f);
        }
        //Determine current speed      
        curSpeed = Mathf.Lerp(curSpeed, targetSpeed, 7.0f *Time.deltaTime);
        transform.Translate(Vector3.forward * Time.deltaTime * curSpeed);
    }



    private void UpdateWeapon()
    {
        if (Input.GetMouseButtonDown(0))
        {
            print("shot fired");
           // elapsedTime += Time.deltaTime; if (elapsedTime >= shootRate)
            {          
                //Reset the time          
           //     elapsedTime = 0.0f;
                //Instantiate the bullet          
                Instantiate(Bullet, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
            }
        }
    }

}
