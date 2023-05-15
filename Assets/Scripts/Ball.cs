using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    private Rigidbody ballRigidbody;

    void Start()
    {
        ballRigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision other)
    {
        //apply horizontal force on the outer edge of the paddle
        if (other.gameObject.CompareTag("paddle"))
        {
            Vector3 velocity = ballRigidbody.velocity;

            float direction = (transform.position.x - other.transform.position.x >= 0f) ? 1f : -1f;
            velocity.x = Mathf.Abs(velocity.x) * direction;

            // max velocity
            if (velocity.magnitude > 3.0f)
            {
                velocity = velocity.normalized * 3.0f;
            }

            ballRigidbody.velocity = velocity;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        Vector3 velocity = ballRigidbody.velocity;
        
        // after a collision we accelerate a bit
        velocity += velocity.normalized * 0.01f;
        
        // check if we are not going totally horizontal as this would lead to being stuck, we add a little vertical force
        if (Vector3.Dot(velocity.normalized, Vector3.up) < 0.1f)
        {
            velocity += velocity.y > 0 ? Vector3.up * 0.5f : Vector3.down * 0.5f;
        }

        // check if we are not going totally vertical as this would lead to being stuck, we add a little horizontal force
        if (Vector3.Dot(velocity.normalized, Vector3.up) > 0.9f)
        {
            velocity += velocity.x > 0 ? Vector3.right * 0.5f : Vector3.left * 0.5f;
        }

        // max velocity
        if (velocity.magnitude > 3.0f)
        {
            velocity = velocity.normalized * 3.0f;
        }

        ballRigidbody.velocity = velocity;
    }
}
