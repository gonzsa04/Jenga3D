using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    // Rotation speed of the camera
    public float rotationSpeed = 500.0f;

    // List of GameObjects to rotate the camera around
    public List<GameObject> rotateAroundGameObjects;
    
    // Variable to keep track of the current orbit target
    private int _currentGO = 0;
    
    void Start()
    {
        // Initialize the camera orbit
        ChangeOrbit();
    }
    
    void Update()
    {
        // Check for left mouse button press to enable orbit
        if (Input.GetKey(KeyCode.Mouse0)) Orbit();
        
        // Check for middle mouse button press to change the orbit target
        if (Input.GetKeyDown(KeyCode.Mouse2)) 
            ChangeOrbit();
       
        // Lock and hide the cursor while left mouse button is held down
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }

    // Method to perform camera orbit
    private void Orbit()
    {
        if (Input.GetAxis("Mouse Y") != 0 || Input.GetAxis("Mouse X") != 0)
        {
            float verticalInput = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
            float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
            
            // Rotate the camera based on mouse input
            transform.Rotate(Vector3.right, verticalInput);
            transform.Rotate(Vector3.up, horizontalInput, Space.World);
        }
    }

    // Method to change the orbit target
    private void ChangeOrbit()
    {
        if (rotateAroundGameObjects.Count > 0) {
            // Set the camera's position to rotate around a specified GameObject
            this.transform.position = rotateAroundGameObjects[_currentGO].transform.position;
            _currentGO++;

            // Wrap around to the beginning if the end of the list is reached
            if (_currentGO >= rotateAroundGameObjects.Count) _currentGO = 0;
        }
    }
}