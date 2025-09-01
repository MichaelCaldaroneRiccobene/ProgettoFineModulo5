using System;
using System.Collections;
using UnityEngine;

public class Player_Animation : Npc_Animation
{
    [Header("Setting Player_Animation")]
    [SerializeField] private string parameterFloatDirection = "Direction";
    [SerializeField] private string parameterTriggerOnSitUp = "OnSitUp";

    [Header("Setting Name Attack")]
    [SerializeField] private string parameterTriggerFirstAttack = "FirstAttack";
    [SerializeField] private string parameterTriggerSecondAttack = "SecondAttack";

    public event Action <PlayerAttacks> OnDoAttack;

    private Player_Attack player_Attack;
    private Player_Controller player_Controller;
   
    private PlayerAttacks playerAttacks;

    public override void SetUpAction()
    {
        base.SetUpAction();

        player_Attack = GetComponentInParent<Player_Attack>();
        if (player_Attack != null) player_Attack.OnTryAttack += OnTryAttack;

        player_Controller = GetComponentInParent<Player_Controller>();
        if(player_Controller != null) player_Controller.OnTriggerSitUp += OnTriggerSitUp;
    }

    public override void AnimationMoving()
    {
        base.AnimationMoving();

        horizontal = Mathf.Clamp(horizontal, -1f, 1f);
        float speedHorizontal = (agent.velocity.magnitude / agent.speed) * horizontal;

        if (animator != null) animator.SetFloat(parameterFloatDirection, speedHorizontal, smoothAnimation, Time.deltaTime);
    }

    private void OnTryAttack(PlayerAttacks playerAttacks)
    {
        if (isAttacking) return;

        this.playerAttacks = playerAttacks;
        isAttacking = true;

        SelectTryAttack();
    }

    private void SelectTryAttack()
    {
        switch (playerAttacks)
        {
            case PlayerAttacks.None:
                break;
            case PlayerAttacks.FireBall:
                DoAttack(parameterTriggerFirstAttack);
                break;
            case PlayerAttacks.Earth:
                DoAttack(parameterTriggerSecondAttack);
                break;
        }
    }

    public void OnSelectDoAttack() => OnDoAttack?.Invoke(playerAttacks);

    public void OnTriggerSitUp() => animator.SetTrigger(parameterTriggerOnSitUp);
    public void OnSitUp() => Player_Controller.CanPlayerUseInput = true;

    public override void OnDisable()
    {
        base.OnDisable();

        if (player_Attack != null) player_Attack.OnTryAttack -= OnTryAttack;
        if (player_Controller != null) player_Controller.OnTriggerSitUp -= OnTriggerSitUp;
    }
}
