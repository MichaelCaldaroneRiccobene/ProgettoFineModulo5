using UnityEngine;

public class Player_Mana : MonoBehaviour
{
    [SerializeField] private Stats_EntitySO stats;
    [SerializeField] private int regenerateMana = 1;
    [SerializeField] private float timeForRegenerateMana = 1;

    private int mana;
    private float timerRegenereteMana;

    private Player_Attack player_Attack;
    private Player_Movement player_Movement;

    private void Awake() => mana = stats.Mana;

    private void Start()
    {
        SetUp();
    }

    private void Update()
    {
        RegenerateMana();
    }

    private void SetUp()
    {
        SetUpUI();
        SetUpAction();
    }

    private void SetUpUI()
    {
        if (Player_Ui.Instance != null) Player_Ui.Instance.UpdateMana((float)mana / stats.MaxMana);
        timerRegenereteMana = timeForRegenerateMana;
    }

    private void SetUpAction()
    {
        player_Attack = GetComponent<Player_Attack>();
        if (player_Attack != null) player_Attack.OnAttack += UseManaForAction;

        player_Movement = GetComponent<Player_Movement>();
        if (player_Movement != null) player_Movement.OnDash += UseManaForAction;
    }

    private void RegenerateMana()
    {
        if (mana < stats.MaxMana)
        {
            timerRegenereteMana -= Time.deltaTime;
            if (timerRegenereteMana <= 0)
            {
                UpdateMana(regenerateMana);
                timerRegenereteMana = timeForRegenerateMana;
            }
        }
    }

    public void UseManaForAction(int manaCost, System.Action onComplete)
    {
        if (CanUseMana(manaCost))
        {
            UpdateMana(-manaCost);
            onComplete?.Invoke();
        }
    }

    public void UpdateMana(int ammount)
    {
        mana = Mathf.Clamp(mana + ammount,0, stats.MaxMana);
        if(Player_Ui.Instance != null) Player_Ui.Instance.UpdateMana((float)mana / stats.MaxMana);
    }

    private bool CanUseMana(int ammount) => mana > ammount;

    private void OnDisable()
    {
        if (player_Attack != null) player_Attack.OnAttack -= UseManaForAction;
        if (player_Movement != null) player_Movement.OnDash -= UseManaForAction;
    }
}
