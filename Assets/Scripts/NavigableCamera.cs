using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigableCamera : MonoBehaviour
{

    [SerializeField] float rotationSpeed;
    [SerializeField] float movementSpeed;
    float speedFactor = 1f;

    float xChange;
    float yChange;

    float yMin = -90;
    float yMax = 90;
    private void Start()
    {
        Vector3 rotationChange = transform.rotation.eulerAngles;
        xChange = rotationChange.x;
        yChange = rotationChange.y;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        CameraRotation();
        CameraMovement();

    }

    // Depending on the mouse movement, the camera will rotate up down, left and right
    private void CameraRotation()
    {
        xChange += Input.GetAxisRaw("Mouse X") * (rotationSpeed * Time.deltaTime);
        yChange += -Input.GetAxisRaw("Mouse Y") * (rotationSpeed * Time.deltaTime);

        // limiting the camera rotation on the y axis
        yChange = Mathf.Clamp(yChange, yMin, yMax);

        transform.rotation = Quaternion.Euler(yChange, xChange, 0f);
    }

    // Depending on key inputs, the camera will move forward, backward, left and right
    private void CameraMovement()
    {
        Vector3 movementVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

        //while holding left shift, the rate at which the camera moves is increased
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speedFactor = 3;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            speedFactor = 1;

        }

        transform.Translate(movementVector * speedFactor * movementSpeed);

        // if space is pressed, the camera will move straight up
        if (Input.GetKey(KeyCode.Space))
        {
            transform.Translate(0, speedFactor * movementSpeed, 0);
        }
    }

}
