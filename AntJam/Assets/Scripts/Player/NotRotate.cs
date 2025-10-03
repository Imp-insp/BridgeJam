using UnityEngine;

public class NotRotate : MonoBehaviour
{


    private Quaternion initialLocalRotation;
    private Vector3 initialWorldOffset;

    void Start()
    {
        // Capture the rotation the object has at the start relative to its parent.
        initialLocalRotation = transform.localRotation;

        
        if (transform.parent != null)
        {
            initialWorldOffset = transform.position - transform.parent.position;
        }
        
        if (transform.parent == null)
        {
            Debug.LogWarning($"The object '{gameObject.name}' must have a parent for RotationStopper to work effectively.", this);
        }
    }

    void LateUpdate()
    {
        if (transform.parent)
        {
            transform.position = transform.parent.position + initialWorldOffset;
        }

        transform.localRotation = initialLocalRotation;
    }
}


