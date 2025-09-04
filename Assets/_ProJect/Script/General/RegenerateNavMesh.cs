using Unity.AI.Navigation;
using UnityEngine;

public class RegenerateNavMesh : MonoBehaviour
{
    public static RegenerateNavMesh Instace;

    private NavMeshSurface meshSurface;

    private void Awake()
    {
        if (Instace != null && Instace != this) { Destroy(gameObject); return; }
        else Instace = this;

        meshSurface = GetComponent<NavMeshSurface>();
        UpdateNaveMeshSurface();
    }

    public void UpdateNaveMeshSurface()
    {
        if (meshSurface == null) return;
        meshSurface.UpdateNavMesh(meshSurface.navMeshData);
    }
}
