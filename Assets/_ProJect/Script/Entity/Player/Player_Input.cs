using UnityEngine;
using UnityEngine.Events;

public class Player_Input : MonoBehaviour
{
    public UnityEvent OnTryRangeAttack;
    public UnityEvent OnInteraction;

    public UnityEvent OnRotation;
    public UnityEvent<Vector3> OnMovement;
    public UnityEvent<Vector3> OnDash;

    public UnityEvent<float, float> OnSetHorizontalAndVertical;

    private float horizontal;
    private float vertical;
    private Vector3 direction;


    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        OnSetHorizontalAndVertical?.Invoke(horizontal, vertical);

        direction = transform.forward * vertical + transform.right * horizontal;
        OnMovement?.Invoke(direction);
        OnRotation?.Invoke();

        if(Input.GetKeyDown(KeyCode.Q))  RegenerateNavMesh.Instance.UpdateNaveMeshSurface();

        if (Input.GetKeyDown(KeyCode.E)) OnInteraction?.Invoke();

        if (Input.GetMouseButtonDown(0)) OnTryRangeAttack?.Invoke();

        if (Input.GetKeyDown(KeyCode.Alpha1)) Time.timeScale = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) Time.timeScale = 0;

        if (Input.GetKeyDown(KeyCode.Space)) OnDash?.Invoke(direction);
    }
}
