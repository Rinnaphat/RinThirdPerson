using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float mouseSensitivity = 3.0f;

    private float rotationX;
    private float rotationY;

    [SerializeField] private Transform target;
    [SerializeField] private float distanceFromTarget = 3.0f;

    private Vector3 currentRotation;
    private Vector3 smoothVelocity = Vector3.zero;

    [SerializeField] private float smoothTime = 0.2f;
    [SerializeField] private Vector2 rotationXminMax = new Vector2 (-20, 40);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationY += mouseX;
        rotationX -= mouseY;

        rotationX = Mathf.Clamp(rotationX, rotationXminMax.x, rotationXminMax.y);

        Vector3 nextRotation = new Vector3 (rotationX, rotationY);

        currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation, ref smoothVelocity, smoothTime);

        transform.localEulerAngles = currentRotation;
        transform.position = target.position - transform.forward * distanceFromTarget;
    }
}
