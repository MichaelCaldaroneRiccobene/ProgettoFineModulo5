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
        InputPlayer();
    }

    private void InputPlayer()
    {
        InputDirectionPlayer();

        InputUseObjOrInteract();

        InputAttackOrInteract();

        InputDash();
        InputPause();
    }

    private void InputDirectionPlayer()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        OnTakeHorizontalAndVertical?.Invoke(horizontal, vertical);
        OnRotate?.Invoke();
    }

    private void InputUseObjOrInteract()
    {
        if (Input.GetKeyDown(KeyCode.Q)) OnUseItem?.Invoke();

        if (Input.GetKeyDown(KeyCode.E)) OnInteract?.Invoke();
    }

    private void InputAttackOrInteract()
    {
        if (Input.GetMouseButtonDown(0)) OnTryFirstAttack?.Invoke();
        if (Input.GetMouseButtonDown(1)) OnTrySecondAttack?.Invoke();
    }

    private void InputDash()
    {
        if (Input.GetKeyDown(KeyCode.Space)) OnDash?.Invoke();
    }

    private void InputPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) MenuInGameManager.Instance.GoToOpenMenu();
    }
}
