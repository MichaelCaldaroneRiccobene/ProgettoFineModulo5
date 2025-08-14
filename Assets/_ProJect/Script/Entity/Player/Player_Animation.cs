using UnityEngine;
using UnityEngine.Events;

public class Player_Animation : MonoBehaviour
{
    [SerializeField] private string parameterFloatSpeed = "Speed";
    [SerializeField] private string parameterFloatDirection = "Direction";

    [SerializeField] private string parameterTriggerFirstAttack = "FirstAttack";
    [SerializeField] private string parameterTriggerSecondAttack = "SecondAttack";

    [SerializeField] private float smoothAnimation = 0.1f;

    public UnityEvent OnFirstAttack;
    public UnityEvent OnSecondAttack;

    private Animator animator;

    private bool isAttack = false;

    private void Start() => animator = GetComponent<Animator>();
    public void FirstAttack()
    {
        if (isAttack) return;
        isAttack = true;
        if (animator != null) animator.SetTrigger(parameterTriggerFirstAttack);
    }

    public void SecondAttack()
    {
        if(isAttack) return;
        isAttack = true;
        if (animator != null) animator.SetTrigger(parameterTriggerSecondAttack);
    }


    public void OnFinishAttack() => isAttack = false;

    public void ShootFirstAttack() => OnFirstAttack?.Invoke();

    public void ShootSecondAttack() => OnSecondAttack?.Invoke();

    public void TakeHorizontalAndVertical(float horizontal, float vertical)
    {
        if (animator != null) animator.SetFloat(parameterFloatSpeed, vertical, smoothAnimation, Time.deltaTime);
        if (animator != null) animator.SetFloat(parameterFloatDirection, horizontal, smoothAnimation, Time.deltaTime);
    }
}
