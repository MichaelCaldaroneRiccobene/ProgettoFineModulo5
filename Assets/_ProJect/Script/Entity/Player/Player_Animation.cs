using UnityEngine;
using UnityEngine.AI;

public class Player_Animation : MonoBehaviour
{
    [SerializeField] private string parameterFloatSpeed = "Speed";
    [SerializeField] private string parameterFloatDirection = "Direction";

    [SerializeField] private string parameterTriggerSitUp = "SitUp";

    [SerializeField] private string parameterTriggerOnHit = "OnHit";
    [SerializeField] private string parameterTriggerOnDead = "OnDead";

    [SerializeField] private float smoothAnimation = 0.1f;

    private NavMeshAgent agent;
    private Animator animator;

    private Player_Attack player_Attack;
    private Player_Input player_Input;

    private bool isAttack = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponentInParent<NavMeshAgent>();

        SetUpAction();
    }

    private void Update()
    {
        Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);

        float vertical = localVelocity.z * agent.speed;
        float horizontal = localVelocity.x * agent.speed;

        vertical = Mathf.Clamp(vertical, -1f, 1f);
        horizontal = Mathf.Clamp(horizontal, -1f, 1f);

        if (animator != null) animator.SetFloat(parameterFloatSpeed, vertical, smoothAnimation, Time.deltaTime);
        if (animator != null) animator.SetFloat(parameterFloatDirection, horizontal, smoothAnimation, Time.deltaTime);
    }

    private void SetUpAction()
    {
        player_Attack = GetComponentInParent<Player_Attack>();
        if(player_Attack != null) player_Attack.OnTryAttack += OnTryAttack;

        player_Input = GetComponentInParent<Player_Input>();
        if(player_Input != null) player_Input.OnTriggerSitUp += TriggerSitUp;
    }

    private void OnTryAttack(string nameAttack)
    {
        if (isAttack) return;
        isAttack = true;
        if (animator != null) animator.SetTrigger(nameAttack);
    }
    public void DoFirstAttack() => player_Attack.OnFirstAttack();

    public void DoSecondAttack() => player_Attack.OnSecondAttack();


    public void OnFinishAttack() => isAttack = false;


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

    public void OnDead() => animator.SetTrigger(parameterTriggerOnDead);

    private void OnDisable()
    {
        player_Input.OnTriggerSitUp -= TriggerSitUp;
    }
}
