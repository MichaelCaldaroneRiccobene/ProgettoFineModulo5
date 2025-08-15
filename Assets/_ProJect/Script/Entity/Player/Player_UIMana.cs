using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player_UIMana : MonoBehaviour
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
        int tempMana = mana;
        tempMana += ammount;

        if(tempMana >= 0)
        {
            Player_Attack.canUseMagic = true;
            mana += ammount;

            OnUpdateMana?.Invoke((float)mana / stats.MaxMana);


            //OnOkMana?.Invoke();
        }
        else Player_Attack.canUseMagic = false;

        if (mana < 0 )
        {
            mana = 0;
            OnUpdateMana?.Invoke((float)mana / stats.MaxMana);
        }
    }
}
