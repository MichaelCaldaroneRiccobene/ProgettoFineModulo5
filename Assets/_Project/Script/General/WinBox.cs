using UnityEngine;

public class WinBox : MonoBehaviour
{
    [ContextMenu("OnWinLevel")]
    private void OnWinLevel()
    {
        Player_Controller.CanPlayerUseInput = false;
        if (GameManager.Instace != null) GameManager.Instace.OnWinLevel();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player_Controller controller)) OnWinLevel();
    }
}
