using Unity.AI.Navigation;
using UnityEngine;

public class navGet : MonoBehaviour
{
    public NavMeshSurface[] meshSurfaces;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshSurfaces = GetComponentsInChildren<NavMeshSurface>();
    }

}
