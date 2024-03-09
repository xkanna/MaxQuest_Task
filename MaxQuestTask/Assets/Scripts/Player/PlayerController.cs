using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    private LineController lineController;
    private bool isMoving = true;

    private void Start()
    {
        lineController = GetComponentInChildren<LineController>();
    }

    void Update()
    {
        if (isMoving)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            mousePosition.y = transform.position.y;

            transform.position = mousePosition + new Vector3(-0.429f, 0, 0);
            /*Vector3 direction = (mousePosition - transform.position).normalized;
            if (direction.x < 0)
            {
                transform.Translate(Vector2.left * speed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector2.right * speed * Time.deltaTime);
            }*/
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (isMoving)
            {
                isMoving = false;
                var mousePosition= Camera.main.ScreenToWorldPoint(Input.mousePosition);
                lineController.CastALineToAPoint(mousePosition);
            }
            else
            {
                isMoving = true;
                lineController.PullLine();
            }
            
        }
    }
}
