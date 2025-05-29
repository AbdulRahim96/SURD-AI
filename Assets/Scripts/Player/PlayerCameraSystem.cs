using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraSystem : MonoBehaviour
{


    public float cameraDystance;

    public Transform normalPosition;
    public Transform aimingPosition;
    private Transform targetPosition;
    public Transform cameraTransform;
    public Transform aimTransform;
    public Transform cameraAimTransform;
    public Transform dynamicCamera;
    public float range = 100;

    void Start()
    {
        targetPosition = normalPosition;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        targetPosition = Input.GetMouseButton(1) ? aimingPosition : normalPosition;
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition.position, Time.deltaTime * 5f);
        cameraTransform.LookAt(cameraAimTransform.position);

        RaycastHit hit;
        if (Physics.Raycast(cameraTransform.position, cameraTransform.forward, out hit, range))
        {
            float distance = Vector3.Distance(cameraTransform.position, hit.point);
            if (distance >= (cameraDystance + 1f))
            {
                aimTransform.position = Vector3.Lerp(aimTransform.position, hit.point, Time.deltaTime * 10f);
            }
            else {
                aimTransform.position = cameraTransform.position + cameraTransform.forward * range;

            }
           // aimTransform.position = hit.point;
           // aimTransform.localPosition = new Vector3(aimTransform.transform.localPosition.x, aimTransform.localPosition.y, Mathf.Clamp(hit.point.z, cameraDystance + 2f, Mathf.Infinity));

        }
        else {
            aimTransform.position = cameraTransform.position + cameraTransform.forward * range;
        }

        if (Physics.Raycast(dynamicCamera.position, -dynamicCamera.forward, out hit, cameraDystance))
        {
            normalPosition.position = hit.point + normalPosition.forward * 0.2f;
        }
        else
        {
            normalPosition.localPosition = new Vector3(normalPosition.localPosition.x, normalPosition.localPosition.y, -cameraDystance);
        }


    }
}
