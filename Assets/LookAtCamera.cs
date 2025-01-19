using UnityEngine;

public class LookAtCamera : MonoBehaviour
{

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }
}
