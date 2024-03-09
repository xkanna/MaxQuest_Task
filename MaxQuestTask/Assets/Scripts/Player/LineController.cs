using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineController : MonoBehaviour
{
    private LineRenderer lr;
    [SerializeField] private Transform[] points;

    private void Awake()
    {
        lr = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        for (var i = 0; i < points.Length; i++)
        {
            lr.SetPosition(i, points[i].position);
        }
    }

    public void CastALineToAPoint(Vector3 point)
    {
        points[1].position = point;
    }

    public void PullLine()
    {
        points[1].localPosition = new Vector3(0.439f,0,0);
    }
}
