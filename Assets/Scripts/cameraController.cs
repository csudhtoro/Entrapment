using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{

    public Transform target;
    public Vector3 offset;
    public bool useOffsetValues;
    public float rotateSpeed;
    public Transform pivot;
    public float maxViewAngle;
    public float minViewAngle;

    public bool invertY;

    // Start is called before the first frame update
    void Start()
    {
        if (!useOffsetValues) {
            offset = target.position - transform.position;
        }

        pivot.transform.position = target.transform.position;
        //pivot.transform.parent = target.transform;
        pivot.transform.parent = null;

        //removes the mouse icon from the scene in play mode
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        //allow camera to stay in place separate from the player
        pivot.transform.position = target.transform.position;

        //get x position of the mouse and rotate the pivot
        float horizontal = Input.GetAxis("Mouse X") * rotateSpeed;
        pivot.Rotate(0f, horizontal, 0f);

        //get the y position of the mouse & ritate the pivot
        float vertical = Input.GetAxis("Mouse Y") * rotateSpeed;

        //invert the camera look up/down controls
        if(invertY) {
            pivot.Rotate(vertical, 0f, 0f);
        }
        else {
            pivot.Rotate(-vertical, 0f, 0f);
        }

        //limit the up/down camera rotation
        if(pivot.rotation.eulerAngles.x > maxViewAngle && pivot.rotation.eulerAngles.x < 180) {
            pivot.rotation = Quaternion.Euler(maxViewAngle, 0f, 0f);
;        }

        if(pivot.rotation.eulerAngles.x > 180f && pivot.rotation.eulerAngles.x < 360f + minViewAngle) {
            pivot.rotation = Quaternion.Euler(360f + minViewAngle, 0f, 0f);
        }

        //move the camera based on the current rotation of the target and the original offset
        float desiredYAngle = pivot.eulerAngles.y;
        float desiredXAngle = pivot.eulerAngles.x;
        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0f);
        transform.position = target.position - (rotation * offset);


        //to prevent the camera from clipping through the floor
        if (transform.position.y < target.position.y) {
            transform.position = new Vector3(transform.position.x, target.position.y, transform.position.z);
        }

        transform.LookAt(target);
    }
}
