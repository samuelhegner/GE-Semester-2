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

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, force * 5);
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, velocity * 5);
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

        /*
         * get the toTarget velocity
         * normalise and multiply it by max speed (to target vector at max speed)
         * calculate the desired velocity by taking away the current velocity
        */

        Vector3 toTarget = target - transform.position;
        toTarget.Normalize();
        Vector3 desired = toTarget *= maxSpeed;
        return desired - velocity;
    }

    Vector3 Arrive(Vector3 target){

        /*
         * generates a breaking force inside the slowing circle
         * get the to target
         * then get the distance   |T - B|
         * distance/slowing distance * max speed (the closer you are to the target the slower you will get)
         * outside the slowing circle the speed is too high therefore we clamp it
         * minimum of the ramped speed and the max speed (.Min returns the smaller value)
         * then normailse the clamped speed by * it by the totarget/distance
         * the return the desired - the current velocity
        */

        Vector3 toTarget = target - transform.position;
        float dist = toTarget.magnitude;

        float rampedSpeed = (dist / slowingDistance) * maxSpeed;
        float clampedSpeed = Mathf.Min(rampedSpeed, maxSpeed);
        Vector3 desired = clampedSpeed * (toTarget / dist);
        return desired - velocity;
    }

}
