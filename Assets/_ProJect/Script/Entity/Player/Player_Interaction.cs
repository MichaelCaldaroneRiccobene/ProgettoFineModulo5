using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player_Interaction : MonoBehaviour
{
    [SerializeField] private Transform head;

    private Player_Input player_Input;

    private I_Interection currentInteraction;
    private I_Interection lastInteraction;

    private void Start() => SetUpAction();

    private void Update()
    {
        ISeeAInteraction();
    }

    private void SetUpAction()
    {
        player_Input = GetComponent<Player_Input>();
        player_Input.OnInteract += Interaction;
    }

    public void Interaction()
    {
        if (currentInteraction != null) currentInteraction.Interact();
    }

    private void ISeeAInteraction()
    {
        if (Physics.Raycast(head.position, transform.forward, out RaycastHit hit, 2))
        {
            Debug.DrawRay(head.position, hit.point, Color.black, 1);

            I_Interection interaction = hit.transform.GetComponentInChildren<I_Interection>();

            if(interaction != null)
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
        player_Input.OnInteract -= Interaction;
    }
}
