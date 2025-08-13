using UnityEngine;
using UnityEngine.Events;

public class Player_Animation : MonoBehaviour
{
    [SerializeField] private string parameterFloatSpeed = "Speed";
    [SerializeField] private string parameterFloatDirection = "Direction";

    [SerializeField] private string parameterTriggerAttack = "Attck";

    [SerializeField] private float smoothAnimation = 0.1f;

    public UnityEvent OnRangeAttack;

    private Animator animator;

    private float horizontal;
    private float vertical;

    private bool isAttack = false;

    private void Start() => animator = GetComponent<Animator>();

    public void Attack()
    {
        if(isAttack) return;
        isAttack = true;
        if (animator != null) animator.SetTrigger(parameterTriggerAttack);
    }

    public void OnFinishAttack() => isAttack = false;

    public void Shoot() => OnRangeAttack?.Invoke();

    public void TakeHorizontalAndVertical(float horizontal, float vertical)
    {
        if (animator != null) animator.SetFloat(parameterFloatSpeed, vertical, smoothAnimation, Time.deltaTime);
        if (animator != null) animator.SetFloat(parameterFloatDirection, horizontal, smoothAnimation, Time.deltaTime);
    }
}
