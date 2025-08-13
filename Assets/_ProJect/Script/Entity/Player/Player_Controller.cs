using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour, I_Team
{
    [SerializeField] private int teamNumber = 1;

    public int GetTeamNumber() => teamNumber;

    public void SetTarget(Transform target) { }

    public void OnDead()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("I Am Dead");
    }
}
