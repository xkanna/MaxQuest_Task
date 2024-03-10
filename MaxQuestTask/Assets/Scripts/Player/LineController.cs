using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class LineController : MonoBehaviour
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
        lr = GetComponent<LineRenderer>();
    }

    private void Start()
    {
        fishBaitCollider.OnFishInRange += ChangeCircleColor;
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
            OnCanCatchFish.Invoke();
            fishBaitCollider.CatchFish();
        }
        fishBaitCollider.gameObject.SetActive(false);
        redCircle.SetActive(false);
        greenCircle.SetActive(false);
        
        var startPosition = points[1].localPosition;
        var elapsedTime = 0f;

        while (elapsedTime < 0.5f)
        {
            points[1].localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / 0.5f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        points[1].localPosition = targetPosition;
        bait.SetActive(false);
        foundFish.SetActive(false);
        
        OnLinePulled.Invoke();
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
