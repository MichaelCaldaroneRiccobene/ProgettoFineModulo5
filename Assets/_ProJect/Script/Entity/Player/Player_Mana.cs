using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player_Mana : MonoBehaviour
{
    [SerializeField] private Stats_EntitySO stats;
    [SerializeField] private int regenerateMana = 1;
    [SerializeField] private float timeForRegenerateMana = 1;

    public UnityEvent <float> OnUpdateMana;
    public UnityEvent OnOkMana;

    private int mana;
    private float timerRegenereteMana;

    private void Awake() => mana = stats.Mana;

    private void Start()
    {
        OnUpdateMana?.Invoke((float)mana / stats.MaxMana);
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
        mana += ammount;

        OnUpdateMana?.Invoke((float)mana / stats.MaxMana);
        if (GetMana()) OnOkMana?.Invoke();
        else mana = 0;

        if (SetMaxMana()) mana = stats.MaxMana;
    }

    public bool GetMana() => mana > 0;

    private bool SetMaxMana() => mana > stats.MaxMana;
}
