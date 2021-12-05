using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    [SerializeField] Vector3 RotationDirection = Vector3.up;
    [SerializeField] float rotationSpeed = 20f;
    [SerializeField] GameObject target;

    // Rotates the object around a set object by a determined rotation factor
    void Update() 
    { 
        transform.RotateAround(target.transform.position, RotationDirection, rotationSpeed * Time.deltaTime);   
    }
  
}
