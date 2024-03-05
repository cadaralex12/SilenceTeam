using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float speed;
    private float startingSpeed; 
    private float horizontal; 
    private Rigidbody2D rb;
    private float startTime; 

    void Start()
    {
        speed = startingSpeed = 10000f; 
        horizontal = 1f; 
        rb = GetComponent<Rigidbody2D>();
        startTime = Time.time; 
        rb.AddForce(new Vector2(horizontal * speed, 0f), ForceMode2D.Force);
    }

    private void FixedUpdate()
    {
        float elapsedTime = Time.time - startTime;

        rb.AddForce(new Vector2(horizontal * elapsedTime, 0f), ForceMode2D.Force);

    }
}