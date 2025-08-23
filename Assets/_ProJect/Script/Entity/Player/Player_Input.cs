using System;
using UnityEngine;

public class Player_Input : MonoBehaviour
{
    public static bool CanPlayerUseInput;

    private float horizontal;
    private float vertical;

    public event Action <float,float> OnTakeHorizontalAndVertical;

    public event Action OnTryFirstAttack;
    public event Action OnTrySecondAttack;

    public event Action OnDash;
    public event Action OnRotate;

    public event Action OnUseItem;
    public event Action OnInteract;

    public event Action OnTriggerSitUp;

    private void Awake() => CanPlayerUseInput = false;

    private void Update()
    {
        if (Input.anyKeyDown && !CanPlayerUseInput)
        {
            GameManager.Instance.OnStart();
            OnTriggerSitUp?.Invoke();
        }

        if (!CanPlayerUseInput) return;

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        OnTakeHorizontalAndVertical?.Invoke(horizontal, vertical);
        OnRotate?.Invoke();

        if (Input.GetKeyDown(KeyCode.Q)) OnUseItem?.Invoke();
        if (Input.GetKeyDown(KeyCode.Q)) RegenerateNavMesh.Instance.UpdateNaveMeshSurface();

        if (Input.GetKeyDown(KeyCode.E)) OnInteract?.Invoke();
        if (Input.GetMouseButtonDown(0)) OnTryFirstAttack?.Invoke();

        if (Input.GetMouseButtonDown(1)) OnTrySecondAttack?.Invoke();

        if (Input.GetKeyDown(KeyCode.Space)) OnDash?.Invoke();


        if (Input.GetKeyDown(KeyCode.Escape)) MenuInGameManager.Instance.GoToOpenMenu();
    }
}
