using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Interaction : MonoBehaviour
{
    [SerializeField] private Transform head;

    public void Interction()
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
}
