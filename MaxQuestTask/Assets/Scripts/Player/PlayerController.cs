using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private LineController lineController;
    private bool isMoving = true;

    private void Start()
    {
        lineController = GetComponentInChildren<LineController>();
    }

    private void Update()
    {
        if (isMoving)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            mousePosition.y = transform.position.y;

            transform.position = mousePosition + new Vector3(-0.429f, 0, 0);
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (isMoving)
            {
                isMoving = false;
                var mousePosition= Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;
                lineController.CastALineToAPoint(mousePosition);
            }
            else
            {
                lineController.PullLine();
                lineController.OnLinePulled += () => isMoving = true;
            }
            
        }
    }
}
