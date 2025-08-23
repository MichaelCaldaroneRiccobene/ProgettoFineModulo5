using System;
using UnityEngine;

public class Player_Animation : MonoBehaviour
{
    [SerializeField] private string parameterFloatSpeed = "Speed";
    [SerializeField] private string parameterFloatDirection = "Direction";

    [SerializeField] private string parameterTriggerSitUp = "SitUp";

    [SerializeField] private string parameterTriggerFirstAttack = "FirstAttack";
    [SerializeField] private string parameterTriggerSecondAttack = "SecondAttack";

    [SerializeField] private string parameterTriggerOnHit = "OnHit";
    [SerializeField] private string parameterTriggerOnDead = "OnDead";

    [SerializeField] private float smoothAnimation = 0.1f;

    private Player_Attack player_Attack;
    private Player_Input player_Input;
    private Animator animator;

    private bool isAttack = false;

    private void Start()
    {
        animator = GetComponent<Animator>();

        player_Attack = GetComponentInParent<Player_Attack>();

        SetUpAction();
    }

    private void SetUpAction()
    {
        player_Input = GetComponentInParent<Player_Input>();
        player_Input.OnTakeHorizontalAndVertical += TakeHorizontalAndVertical;
        player_Input.OnTriggerSitUp += TriggerSitUp;
    }

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



    public void DoFirstAttack() => player_Attack.OnFirstAttack();

    public void DoSecondAttack() => player_Attack.OnSecondAttack();




    public void TriggerHit()
    {
        animator.SetTrigger(parameterTriggerOnHit);

        isAttack = false;
    }



    public void TriggerSitUp() => animator.SetTrigger(parameterTriggerSitUp);
    public void OnSitUp()
    {
        Player_Input.CanPlayerUseInput = true;
        Player_Ui.Instance.ShowPlayerUI();
    }


    public void TakeHorizontalAndVertical(float horizontal, float vertical)
    {
        if (animator != null) animator.SetFloat(parameterFloatSpeed, vertical, smoothAnimation, Time.deltaTime);
        if (animator != null) animator.SetFloat(parameterFloatDirection, horizontal, smoothAnimation, Time.deltaTime);
    }


    public void OnDead() => animator.SetTrigger(parameterTriggerOnDead);

    private void OnDisable()
    {
        player_Input.OnTakeHorizontalAndVertical -= TakeHorizontalAndVertical;
        player_Input.OnTriggerSitUp -= TriggerSitUp;
    }
}
