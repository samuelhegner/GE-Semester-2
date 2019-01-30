using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public GameObject targetGameObject;

    public Vector3 velocity;
    public Vector3 acceleration;
    public Vector3 target;
    public Vector3 force;

    public float maxSpeed = 5;
    public float mass = 1;
    public float slowingDistance = 10;


    void Start()
    {
        
    }



    void Update()
    {
        if(targetGameObject != null){
            target = targetGameObject.transform.position;
        }


        force = Arrive(target);


        //forward euler integration function
        acceleration = force / mass;
        velocity += acceleration * Time.deltaTime;

        transform.position += velocity * Time.deltaTime;
        

        if(velocity.magnitude > float.Epsilon){
            transform.forward = velocity;
        }
    }

    Vector3 Seek(Vector3 target){
        Vector3 toTarget = target - transform.position;
        toTarget.Normalize();
        toTarget *= maxSpeed;
        return toTarget - velocity;
    }

    Vector3 Arrive(Vector3 target){
        Vector3 toTarget = target - transform.position;
        float dist = toTarget.magnitude;

        float rampedSpeed = (dist / slowingDistance) * maxSpeed;
        float clampedSpeed = Mathf.Min(rampedSpeed, maxSpeed);
        Vector3 desired = clampedSpeed * (toTarget / dist);
        return desired - velocity;
    }

}
