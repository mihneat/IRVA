using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.ARFoundation;
using Google.XR.ARCoreExtensions;
using TMPro;

public class AnchorCreatedEvent : UnityEvent<Transform> { }

/* TODO 1. Enable ARCore Cloud Anchors API on Google Cloud Platform */
public class ARCloudAnchorManager : MonoBehaviour
{
    [SerializeField]
    private Camera arCamera = null;

    [SerializeField]
    TMP_Text statusUpdate;

    private ARAnchorManager  arAnchorManager = null;
    private List<ARAnchor> pendingHostAnchors = new();
    private List<string> anchorIdsToResolve;
    private AnchorCreatedEvent anchorCreatedEvent = null;
    public static ARCloudAnchorManager Instance { get; private set; }
    public GameObject middle;
    public GameObject main;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        anchorCreatedEvent = new AnchorCreatedEvent();
        anchorCreatedEvent.AddListener((t) => CloudAnchorObjectPlacement.Instance.RecreatePlacement(t));
    }

    private Pose GetCameraPose()
    {
        return new Pose(arCamera.transform.position, arCamera.transform.rotation);
    }
    public void QueueAnchor(ARAnchor arAnchor)
    {
        pendingHostAnchors.Add(arAnchor);
    }

    public IEnumerator DisplayStatus(string text)
    {
        statusUpdate.text = text;
        yield return new WaitForSeconds(3);
        statusUpdate.text = "";
    }

    public void HostAnchor()
    {
        /* TODO 3.1 Get FeatureMapQuality */
        FeatureMapQuality quality = arAnchorManager.EstimateFeatureMapQualityForHosting(GetCameraPose());
        StartCoroutine(DisplayStatus("HostAnchor call in progress. Feature Map Quality: " + quality));

        if (quality != FeatureMapQuality.Insufficient)
        {
            /* TODO 3.2 Start the hosting process */
            anchorIdsToResolve = new List<string>();
            new List<ARAnchor>(pendingHostAnchors).ForEach(pendingHostAnchor =>
            {
                HostCloudAnchorPromise cloudAnchor = arAnchorManager.HostCloudAnchorAsync(pendingHostAnchor, 365);

                /* Wait for the promise to solve (Hint! Pass the HostCloudAnchorPromise variable to the coroutine) */
                StartCoroutine(WaitHostingResult(pendingHostAnchor, cloudAnchor));
            });
        }
    }

    public void Resolve()
    {
        StartCoroutine(DisplayStatus("Resolve call in progress"));

        /* TODO 5 Start the resolve process and wait for the promise */
        new List<string>(anchorIdsToResolve).ForEach(anchorIdToResolve =>
        {
            ResolveCloudAnchorPromise resolvePromise = arAnchorManager.ResolveCloudAnchorAsync(anchorIdToResolve);

            StartCoroutine(WaitResolvingResult(resolvePromise));
        });

    }
    
    /* TODO 3.3 Wait for the promise. Save the id if the hosting succeeded */
    private IEnumerator WaitHostingResult(ARAnchor pendingAnchor, HostCloudAnchorPromise hostingPromise)
    {
        /* Wait for the promise. Save the id if the hosting succeeded */
        yield return hostingPromise;
 
        if (hostingPromise.State == PromiseState.Cancelled)
        {
            yield break;
        }
 
        var result = hostingPromise.Result;
 
        if (result.CloudAnchorState == CloudAnchorState.Success)
        {
            anchorIdsToResolve.Add(result.CloudAnchorId);
            
            // Remove the corresponding pending one
            pendingHostAnchors.Remove(pendingAnchor);
            
            Debug.Log("Anchor hosted successfully!");
        }
        else
        {
            Debug.Log(string.Format("Error in hosting the anchor: {0}", result.CloudAnchorState));
        }
    }

    private IEnumerator WaitResolvingResult(ResolveCloudAnchorPromise resolvePromise)
    {
        yield return resolvePromise;

        if (resolvePromise.State == PromiseState.Cancelled) yield break;
        var result = resolvePromise.Result;

        if (result.CloudAnchorState == CloudAnchorState.Success)
        {
            anchorCreatedEvent?.Invoke(result.Anchor.transform);
            StartCoroutine(DisplayStatus("Anchor resolved successfully!"));
        }
        else
        {
            StartCoroutine(DisplayStatus("Error while resolving cloud anchor"));
        }
    }
}
