using UnityEngine;
using Vuforia;

public class BarcodeReader : MonoBehaviour
{
    public Animator animator;
    BarcodeBehaviour mBarcodeBehaviour;
    
    void Start()
    {
        mBarcodeBehaviour = GetComponent<BarcodeBehaviour>();
        animator.enabled = false;
    }

    void Update()
    {
        if (mBarcodeBehaviour != null && mBarcodeBehaviour.InstanceData != null)
        {
            Debug.Log($"<a href=\"{mBarcodeBehaviour.InstanceData.Text}\">{mBarcodeBehaviour.InstanceData.Text}</a>");
            
            animator.enabled = true;
        }
    }
}