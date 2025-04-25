using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Networking;

/* TODO 1 Create in Unity an image database with minimum 3 images */
/* TODO 2 Augment the database */
public class ARRuntimeImageLibrary : MonoBehaviour
{
    private RuntimeReferenceImageLibrary _runtimeReferenceImageLibrary;
    private RuntimeReferenceImageLibrary _generatedLibrary;
    
    private ARTrackedImageManager _trackImageManager;
    public GameObject m_PlacedPrefab;

    private bool _usingRuntimeDatabase = true;

    private List<Texture2D> _runtimeTextures = new();
    
    void Start()
    {
        /* TODO 3.1 Download minimum one image from the internet */
        var url1 = "https://static.wixstatic.com/media/a2777d_7fe3b836e77a49ccbb63340187bc482e~mv2.png/v1/fill/w_453,h_239,fp_0.52_0.35,q_85,usm_0.66_1.00_0.01,enc_avif,quality_auto/Website%20-%20Geluff.png";
        var url2 = "https://static.wixstatic.com/media/a2777d_f6c7a641c338480d9bcf51f2f2521dd6~mv2.png/v1/fill/w_453,h_239,fp_0.41_0.31,q_85,usm_0.66_1.00_0.01,enc_avif,quality_auto/Website%20-%20Vicrow.png";
        var url3 = "https://static.wixstatic.com/media/a2777d_2838d69f1d9747618ba3809273fba834~mv2.png/v1/fill/w_453,h_239,fp_0.49_0.25,q_85,usm_0.66_1.00_0.01,enc_avif,quality_auto/Website%20-%20Frigon.png";
        var url4 = "https://static.wixstatic.com/media/a2777d_5e7a3da6beb24684afc0f317412e59a8~mv2.png/v1/fill/w_453,h_239,al_c,q_85,usm_0.66_1.00_0.01,enc_avif,quality_auto/Website%20-%20Sarathin.png";
        
        /* TODO 3.2 Destroy the previous ARTrackedImageManager component.
         * Hint! What's the difference between Destroy() and DestroyImmediate()? */
        _trackImageManager = gameObject.GetComponent<ARTrackedImageManager>();
        _runtimeReferenceImageLibrary = _trackImageManager.referenceLibrary as RuntimeReferenceImageLibrary;
        
        DestroyImmediate(gameObject.GetComponent<ARTrackedImageManager>());

        // /* TODO 3.3 Attach a new ARTrackedImageManager component */
        _trackImageManager = gameObject.AddComponent<ARTrackedImageManager>();

        /* TODO 3.4 Create a new runtime library */
        _generatedLibrary = _trackImageManager.CreateRuntimeLibrary();
            
        /* Set the maximum number of moving images */
        _trackImageManager.requestedMaxNumberOfMovingImages = 3;

        /* TODO 3.6 Set the new library as the reference library */
        _trackImageManager.referenceLibrary = _generatedLibrary;

        /* TODO 3.7 Enable the new ARTrackedImageManager component */
        _trackImageManager.trackedImagePrefab = m_PlacedPrefab;

        /* Attach the event handling */
        _trackImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        
        StartCoroutine(DownloadImage(url1, "Geluff"));
        StartCoroutine(DownloadImage(url2, "Vicrow"));
        StartCoroutine(DownloadImage(url3, "Frigon"));
        StartCoroutine(DownloadImage(url4, "Sarathin"));
    }

    /* Download and create an image database */
    IEnumerator DownloadImage(string url, string imgName)
    {
        Texture2D imageToAdd;

        /* UnityWebRequest API will be used to download the image */
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {
            Debug.Log(request.error);
        }
        else
        {
            /* Downloaded image */
            imageToAdd = ((DownloadHandlerTexture)request.downloadHandler).texture;
            _runtimeTextures.Add(imageToAdd);

            /* TODO 3.5 Add the image to the database*/
            if (_generatedLibrary  is MutableRuntimeReferenceImageLibrary mutableLibrary)
                mutableLibrary.ScheduleAddImageWithValidationJob(imageToAdd, imgName, 0.5f /* 50 cm */);
        }
    }

    void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            /* TODO 3.8 Instantiate a new object in scene so that it always follows the tracked image
             * Hint! Use SetParent() method */
            Instantiate(m_PlacedPrefab, trackedImage.transform);

            trackedImage.destroyOnRemoval = true;
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            /* Handle update event */
        }

        foreach (ARTrackedImage removedImage in eventArgs.removed)
        {
        }
    }

    public void SwitchDatabases()
    {
        Debug.Log("[ARRuntimeImageLibrary] Switching databases..");
        
        _usingRuntimeDatabase = !_usingRuntimeDatabase;

        // Find all tracked images and delete them
        foreach (var arTrackedImage in FindObjectsOfType<ARTrackedImage>())
            Destroy(arTrackedImage.gameObject);
        
        if (_usingRuntimeDatabase)
        {
            DestroyImmediate(GetComponent<ARTrackedImageManager>());
            
            _trackImageManager = gameObject.AddComponent<ARTrackedImageManager>();

            _trackImageManager.referenceLibrary = _generatedLibrary;
            
            _trackImageManager.requestedMaxNumberOfMovingImages = 3;

            _trackImageManager.trackedImagePrefab = m_PlacedPrefab;
        }
        else
        {
            DestroyImmediate(GetComponent<ARTrackedImageManager>());
            
            _trackImageManager = gameObject.AddComponent<ARTrackedImageManager>();

            _trackImageManager.referenceLibrary = _runtimeReferenceImageLibrary;
            
            _trackImageManager.requestedMaxNumberOfMovingImages = 3;
            
            _trackImageManager.trackedImagePrefab = m_PlacedPrefab;
        }
    }
}
