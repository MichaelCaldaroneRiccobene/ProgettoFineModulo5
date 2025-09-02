using UnityEngine;

public class WinBox : MonoBehaviour
{
    [ContextMenu("OnWinLevel")]
    private void OnWinLevel()
    {
        Player_Controller.CanPlayerUseInput = false;
        if (GameManager.Instance != null) GameManager.Instance.OnWinLevel();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Player_Controller controller)) OnWinLevel();
    }
}
