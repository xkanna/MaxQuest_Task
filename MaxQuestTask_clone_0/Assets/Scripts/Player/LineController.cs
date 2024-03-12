using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class LineController : NetworkBehaviour
{
    [SerializeField] private Transform[] points;
    [SerializeField] private FishBaitCollision fishBaitCollider;
    [SerializeField] private GameObject redCircle;
    [SerializeField] private GameObject greenCircle;
    [SerializeField] private GameObject foundFish;
    [SerializeField] private GameObject bait;
    
    private LineRenderer lr;
    private bool canPullFish;

    public Action OnLinePulled;
    public Action OnCanCatchFish;

    private void Awake()
    {
        lr = GetComponentInChildren<LineRenderer>();
    }

    private void Start()
    {
        fishBaitCollider.OnFishInRange += ChangeCircleColor;
    }

    private void Update()
    {
        UpdateLineRendererPoints();
    }

    private void UpdateLineRendererPoints()
    {
        for (var i = 0; i < points.Length; i++)
        {
            lr.SetPosition(i, points[i].position);
        }
    }

    public void CastALineToAPoint(Vector3 point)
    {
        StartCoroutine(MoveToPointCoroutine(point));
    }

    public void PullLine()
    {
        StartCoroutine(PullToPointCoroutine(new Vector3(0.439f, 0, 0)));
    }

    private IEnumerator MoveToPointCoroutine(Vector3 targetPosition)
    {
        bait.SetActive(true);
        var startPosition = points[1].position;
        var elapsedTime = 0f;

        while (elapsedTime < 0.5f)
        {
            points[1].position = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 0.5f);

            UpdatePointPositionClientRpc(1, points[1].position);
            UpdatePointPositionServerRpc(1, points[1].position);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        points[1].position = targetPosition;
        redCircle.SetActive(true);
        fishBaitCollider.gameObject.SetActive(true);
        foundFish.SetActive(false);
    }

    private IEnumerator PullToPointCoroutine(Vector3 targetPosition)
    {
        if (canPullFish)
        {
            OnCanCatchFish?.Invoke();
            fishBaitCollider.CatchFishServerRpc();
        }
        fishBaitCollider.gameObject.SetActive(false);
        redCircle.SetActive(false);
        greenCircle.SetActive(false);

        var startPosition = points[1].localPosition;
        var elapsedTime = 0f;

        while (elapsedTime < 0.5f)
        {
            points[1].localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 0.5f);
            UpdatePointPositionClientRpc(1, points[1].position);
            UpdatePointPositionServerRpc(1, points[1].position);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        points[1].localPosition = targetPosition;
        bait.SetActive(false);
        foundFish.SetActive(false);

        OnLinePulled?.Invoke();
    }

    [ServerRpc(RequireOwnership = false)]
    private void UpdatePointPositionServerRpc(int pointIndex, Vector3 newPosition)
    {
        points[pointIndex].position = newPosition;
    }
    
    [ClientRpc]
    private void UpdatePointPositionClientRpc(int pointIndex, Vector3 newPosition)
    {
        if (IsServer) return;
        points[pointIndex].position = newPosition + new Vector3(-0.429f, 0, 0);
    }

    private void ChangeCircleColor(bool isInRange)
    {
        greenCircle.SetActive(isInRange);
        redCircle.SetActive(!isInRange);
        canPullFish = isInRange;
    }

    private void OnDestroy()
    {
        fishBaitCollider.OnFishInRange -= ChangeCircleColor;
    }
}
