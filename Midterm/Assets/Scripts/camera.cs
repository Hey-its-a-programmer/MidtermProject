using UnityEngine;

public class camera : MonoBehaviour
{
    [SerializeField] int sensHori;
    [SerializeField] int sensVert;

    [SerializeField] int lockVertMin;
    [SerializeField] int lockVertMax;

    [SerializeField] bool invertY;

    [Range(30, 90)] [SerializeField] float startingFOV;
    [Range(125, 500)] [SerializeField] float zoomSpeed;
    [Range(2, 6)] [SerializeField] float zoomDistance;

    float xRotation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Camera.main.fieldOfView = startingFOV;
    }

    void LateUpdate()
    {
        if (gameManager.instance.turnCameraOn)
        {


            float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHori;
            float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVert;

            if (invertY)
            {
                xRotation += mouseY;
            }
            else
            {
                xRotation -= mouseY;
            }

            Zoom();

            // clamp rotation
            xRotation = Mathf.Clamp(xRotation, lockVertMin, lockVertMax);

            //rotate the camera on the x axis
            transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

            //rotate the player
            transform.parent.Rotate(Vector3.up * mouseX);
        }
    }

    void Zoom()
    {
        if (Input.GetMouseButton(1))
        {
            Camera.main.fieldOfView = Mathf.MoveTowards(Camera.main.fieldOfView, startingFOV / zoomDistance, zoomSpeed * Time.deltaTime);
        }
        else
        {
            Camera.main.fieldOfView = Mathf.MoveTowards(Camera.main.fieldOfView, startingFOV, zoomSpeed * Time.deltaTime);
        }
    }
}
