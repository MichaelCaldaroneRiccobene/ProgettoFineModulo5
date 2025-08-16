using UnityEngine;
using UnityEngine.Events;

public class Player_Mana : MonoBehaviour
{
    [SerializeField] private Player_Attack player_Attack;

    [SerializeField] private Stats_EntitySO stats;
    [SerializeField] private int regenerateMana = 1;
    [SerializeField] private float timeForRegenerateMana = 1;

    private int mana;
    private float timerRegenereteMana;

    private void Awake() => mana = stats.Mana;

    private void Start()
    {
        if(Player_Ui.Instance != null) Player_Ui.Instance.UpdateMana((float)mana / stats.MaxMana);
        timerRegenereteMana = timeForRegenerateMana;
    }

    private void Update()
    {
        if(mana < stats.MaxMana)
        {
            timerRegenereteMana -= Time.deltaTime;
            if(timerRegenereteMana <= 0)
            {
                UpdateMana(regenerateMana);
                timerRegenereteMana = timeForRegenerateMana;
            }
        }
    }

    public void UpdateMana(int ammount)
    {
        mana = Mathf.Clamp(mana + ammount,0, stats.MaxMana);
        if(Player_Ui.Instance != null) Player_Ui.Instance.UpdateMana((float)mana / stats.MaxMana);
    }

    public bool CanUseMana(int ammount) => mana > ammount;
}
