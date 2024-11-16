using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasuseLOck : MonoBehaviour
{

    [Range(50,500)]
    public float sens;

    public Transform body;

    float xRot = 0f;

    public Transform leaner;
    float zRot;
    bool isRotating;

    //public Fpscont playerScript;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float rotX = Input.GetAxisRaw("Mouse X") * sens * Time.deltaTime;
        float rotY = Input.GetAxisRaw("Mouse Y") * sens * Time.deltaTime;

        xRot -= rotY;

        xRot = Mathf.Clamp(xRot, -80f, 80f);
        transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);

        body.Rotate(Vector3.up * rotX);



        if (!isRotating)
        {
            zRot = Mathf.Lerp(zRot, 0f, 5 * Time.deltaTime);
           // playerScript.canMove = true;
        }

        leaner.localRotation = Quaternion.Euler(0, 0, zRot);
    }
}
