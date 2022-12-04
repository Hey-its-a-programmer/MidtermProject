using UnityEngine;

public class camera : MonoBehaviour
{
    [SerializeField] int sensHori;
    [SerializeField] int sensVert;

    [SerializeField] int lockVertMin;
    [SerializeField] int LockVertMax;

    [SerializeField] bool invert;

    float xRotation;
    [SerializeField] float startingFOV;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Camera.main.fieldOfView = startingFOV;
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
            Camera.main.fieldOfView = Mathf.MoveTowards(Camera.main.fieldOfView, startingFOV / 2, 250.0f * Time.deltaTime);
        }
        else
        {
            Camera.main.fieldOfView = Mathf.MoveTowards(Camera.main.fieldOfView, startingFOV, 250.0f * Time.deltaTime);
        }
    }
}
