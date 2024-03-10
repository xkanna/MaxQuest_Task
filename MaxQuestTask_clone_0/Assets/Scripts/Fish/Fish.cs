using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Fish : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float changeDirectionTime = 2f;
    public float minX = -7f;
    public float maxX = 7f;
    public float minY = -4f;
    public float maxY = 0f; 
    public SpriteRenderer fishSprite;

    private Rigidbody2D rb;
    private Vector2 movementDirection;
    private float timer;
    private Vector3 previousPosition;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        timer = changeDirectionTime;
        ChangeDirection();
        previousPosition = transform.position;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            ChangeDirection();
            timer = changeDirectionTime;
        }

        var currentPosition = transform.position;
        if (currentPosition.x < previousPosition.x)
        {
            fishSprite.flipX = true;
        }
        else if (currentPosition.x > previousPosition.x)
        {
            fishSprite.flipX = false;
        }
        previousPosition = currentPosition;
    }

    private void FixedUpdate()
    {
        Vector2 movement = movementDirection * moveSpeed * Time.fixedDeltaTime;
        Vector2 newPosition = rb.position + movement;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        rb.MovePosition(newPosition);
    }

    private void ChangeDirection()
    {
        var randomAngle = Random.Range(0f, 360f);
        movementDirection = Quaternion.Euler(0f, 0f, randomAngle) * Vector2.right;
    }
}
