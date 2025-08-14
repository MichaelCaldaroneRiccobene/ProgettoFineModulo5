using Unity.AI.Navigation;
using UnityEngine;

public class RegenerateNavMesh : MonoBehaviour
{
    public static RegenerateNavMesh Instance;

    private NavMeshSurface meshSurface;

    private void Start()
    {
        if (Instance == null) Instance = this;

        meshSurface = GetComponent<NavMeshSurface>();
        UpdateNaveMeshSurface();
    }

    public void UpdateNaveMeshSurface()
    {
        if (meshSurface == null) return; 
        meshSurface.UpdateNavMesh(meshSurface.navMeshData);
    }
}
