using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interaction : MonoBehaviour
{
    [SerializeField] private Transform head;

    private Player_Input player_Input;
    private void Start() => SetUpAction();

    private void SetUpAction()
    {
        player_Input = GetComponent<Player_Input>();
        player_Input.OnInteract += Interaction;
    }

    public void Interaction()
    {
        if (Physics.Raycast(head.position, transform.forward, out RaycastHit hit, 2))
        {
            Debug.Log(hit.collider.name);
            Debug.DrawRay(head.position, hit.point, Color.black, 1);

            if (hit.collider.TryGetComponent(out I_Interection interection))
            {
                interection.Interact();
            }
        }
        else Debug.DrawRay(head.position, transform.forward, Color.black, 1);
    }

    private void OnDisable()
    {
        player_Input.OnInteract -= Interaction;
    }
}
