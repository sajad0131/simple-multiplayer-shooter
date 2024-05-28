using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject virtualCam;
    public Transform lookRoot;
    [SerializeField]
    private float sensivity;
    [SerializeField]
    private float smoothing;
    [SerializeField]
    private float DownMouseLimit, UpMouseLimit;
    public Vector2 mouseLook;
    private Vector2 smoothV;
    private float lastRotation;



    // Start is called before the first frame update
    private void Start()
    {
        sensivity = PlayerPrefs.GetFloat("AimSensitivity");
    }
    public void VirtualCamera(Vector2 aimAxis, Vector2 moveAxis)
    {
        var MouseLook = new Vector2(aimAxis.x, aimAxis.y);



        lookRoot.rotation *= Quaternion.AngleAxis(MouseLook.x * PlayerPrefs.GetFloat("AimSensitivity"), Vector3.up);
        lookRoot.rotation *= Quaternion.AngleAxis(MouseLook.y * PlayerPrefs.GetFloat("AimSensitivity") * -1, Vector3.right);
        var angles = lookRoot.localEulerAngles;
        angles.z = 0;
        var angle = lookRoot.localEulerAngles.x;

        //Clamp the Up/Down rotation
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }


        lookRoot.localEulerAngles = angles;
        if (moveAxis.x > 0.5 || moveAxis.x < -0.5 || moveAxis.y > 0.5 || moveAxis.y < -0.5)
        {
            lookRoot.parent = transform;
            
            transform.rotation = Quaternion.Euler(0, lookRoot.rotation.eulerAngles.y, 0);
            lookRoot.localEulerAngles = new Vector3(angles.x, 0, 0);


        }
        virtualCam = GameObject.FindGameObjectWithTag("TPSVirtualCamera");
        virtualCam.GetComponent<CinemachineVirtualCamera>().Follow = lookRoot;
        //virtualCam.GetComponent<CinemachineVirtualCamera>().LookAt = camOrigin;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
