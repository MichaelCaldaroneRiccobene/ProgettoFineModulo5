using UnityEngine;
public class CameraFollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void LateUpdate() { if(target != null) transform.position = target.position; }
}
