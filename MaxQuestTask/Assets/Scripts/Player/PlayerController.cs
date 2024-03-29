using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private LineController lineController;
    private FishCatcher fishCatcher;
    private Animator animator;
    private bool isMoving = true;

    public override void OnNetworkSpawn()
    {
        if(!IsOwner) Destroy(this);
    }

    private void Start()
    {
        lineController = GetComponent<LineController>();
        fishCatcher = GetComponent<FishCatcher>();
        animator = GetComponentInChildren<Animator>();
        
        lineController.OnLinePulled += ChangeIsMoving;
        lineController.OnCanCatchFish += TryToPullFish;
    }

    private void ChangeIsMoving()
    {
        isMoving = true;
        animator.SetBool("IsMoving", true);
    }

    private void TryToPullFish()
    {
        fishCatcher.TryToPullFish();
    }

    private void Update()
    {
        if (isMoving)
        {
            MovePlayer();
        }

        if (Input.GetMouseButtonDown(0))
        {
            if (isMoving)
            {
                ThrowLine();
            }
            else
            {
                PullLine();
            }
        }
    }

    private void MovePlayer()
    {
        var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
        if (mousePosition.x <= 6 && mousePosition.x >= -6)
        {
            mousePosition.z = 0f;
            mousePosition.y = transform.position.y;
            transform.position = mousePosition + new Vector3(-0.429f, 0, 0);
        }
    }

    private void ThrowLine()
    {
        isMoving = false;
        animator.SetBool("IsMoving", false);
        var mousePosition= Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0f;
        lineController.CastALineToAPoint(mousePosition);
    }

    private void PullLine()
    {
        lineController.PullLine();
    }

    public override void OnDestroy()
    {
        //lineController.OnLinePulled -= ChangeIsMoving;
        //lineController.OnCanCatchFish -= TryToPullFish;
    }
}
