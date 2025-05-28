using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class SURD_Aiming : MonoBehaviour
{
    [SerializeField] private float raycastRange = 100;
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private string[] tagsForObject;
    public Transform aimPoint;
    public int InteractableIndex = 0;

    [Header("Voice Settings")]
    public bool UpdateDuringVoiceCommand = true;
    public VoiceProcessor voiceProcessor;
    public bool isRecording;

    void Update()
    {
        if(UpdateDuringVoiceCommand)
        {
            if(voiceProcessor.IsRecording)
            {
                Raycasting();
            }
            if (Input.GetKeyDown(KeyCode.F1))
                Raycasting();
        }
        else
            Raycasting();
    }

    void Raycasting()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, raycastRange, hitLayer))
        {
            aimPoint.position = hit.point;
            UpdateInteraction(hit);
            /*
            if (Physics.Raycast(hit.point, Vector3.down, out RaycastHit groundHit, 5f, hitLayer))
            {
                Vector3 groundedPosition = new Vector3(aimPoint.position.x, groundHit.point.y, aimPoint.position.z);
                aimPoint.position = groundedPosition;
            }
            else
            {
                aimPoint.position = hit.point;
            }*/
        }
        else
        {
            aimPoint.position = Camera.main.transform.position + Camera.main.transform.forward * raycastRange;
        }
    }

    void UpdateInteraction(RaycastHit hit)
    {
        for (int i = 0; i < tagsForObject.Length; i++)
        {
            if (hit.transform.CompareTag(tagsForObject[i]))
            {
                print(hit.transform.name);
                Interactables.instance._objects[InteractableIndex].target = hit.transform;
                /*if(!hit.transform.GetComponent<Outline>())
                {
                    hit.transform.AddComponent<Outline>();
                }*/
                return;
            }
        }
        Interactables.instance._objects[InteractableIndex].target = GetGroundPoint();
    }

    Transform GetGroundPoint()
    {
        return aimPoint;
    }

    private void OnDrawGizmosSelected()
    {
        
    }
}
