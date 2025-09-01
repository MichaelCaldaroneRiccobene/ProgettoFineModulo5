using UnityEngine;

public class Player_Interaction : MonoBehaviour
{
    [SerializeField] private Transform head;

    private Player_Controller player_Controller;

    private I_Interection currentInteraction;
    private I_Interection lastInteraction;

    private void Start() => SetUpAction();

    private void Update() => ISeeAInteraction();

    private void SetUpAction()
    {
        player_Controller = GetComponent<Player_Controller>();

        if (player_Controller != null) player_Controller = GetComponent<Player_Controller>();
        if (player_Controller != null) player_Controller.OnInteract += Interaction;
    }

    public void Interaction()
    {
        if (currentInteraction != null) currentInteraction.Interact();
    }

    private void ISeeAInteraction()
    {
        Debug.DrawRay(head.position, transform.forward * 2f, Color.black);
        if (Physics.Raycast(head.position, transform.forward, out RaycastHit hit, 2))
        {
            I_Interection interaction = hit.transform.GetComponentInChildren<I_Interection>();

            if (interaction != null)
            {
                if (lastInteraction != interaction)
                {
                    if (lastInteraction != null)
                    {
                        lastInteraction.HideInteractable();
                        lastInteraction = null;
                    }

                    currentInteraction = interaction;
                    lastInteraction = interaction;

                    currentInteraction.ShowInteractable();
                }
            }
            else if (lastInteraction != null)
            {
                lastInteraction.HideInteractable();
                currentInteraction = null;
                lastInteraction = null;
            }
        }
        else if (lastInteraction != null)
        {
            lastInteraction.HideInteractable();
            currentInteraction = null;
            lastInteraction = null;
        }
    }

    private void OnDisable()
    {
        if (player_Controller != null) player_Controller.OnInteract -= Interaction;
    }
}
