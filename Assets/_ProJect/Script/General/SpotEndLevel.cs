using UnityEngine;

public class SpotEndLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Player_Controller controller))
        {
            if (Player_Ui.Instance != null) Player_Ui.Instance.FadeToWinOver();
        }
    }
}
