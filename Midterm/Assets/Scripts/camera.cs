using UnityEngine;

public class camera : MonoBehaviour
{
    [SerializeField] int sensHori;
    [SerializeField] int sensVert;

    [SerializeField] int lockVertMin;
    [SerializeField] int LockVertMax;

    [SerializeField] bool invert;

    float xRotation;
    float startingFOV;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        startingFOV = 60.0f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHori;
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVert;


        if (invert)
        {
            xRotation += mouseY;
        }
        else
        {
            xRotation -= mouseY;
        }

        // clamp rotaion
        xRotation = Mathf.Clamp(xRotation, lockVertMin, LockVertMax);

        //rotate the camera on the x axis
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        //rotate the player
        transform.parent.Rotate(Vector3.up * mouseX);

        if (Input.GetMouseButton(1))
        {
            Camera.main.fieldOfView = startingFOV / 2;
        }
        else
        {
            Camera.main.fieldOfView = startingFOV;
        }
    }
}
