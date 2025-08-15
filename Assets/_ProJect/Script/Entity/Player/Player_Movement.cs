using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Player_Movement : MonoBehaviour
{
    private NavMeshAgent agent;
    private bool isDashing;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    public void Rotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 lookDirection = hit.point - transform.position;
            lookDirection.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 3.5f);
        }
    }    

    public void Movement(Vector3 direction) => agent.velocity = direction.normalized * agent.speed;

    public void Dash(Vector3 direction) => StartCoroutine(DashRoutine(direction));

    public IEnumerator DashRoutine(Vector3 direction)
    {
        if (isDashing) yield break;

        isDashing = true;
        Vector3 currentPosition = agent.transform.position;
        Vector3 newPosition = currentPosition + direction.normalized * 5;
        agent.updatePosition = false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction.normalized, out hit, 5)) newPosition = hit.point - direction.normalized * 1f;


        float distanceDash = Vector3.Distance(currentPosition, newPosition);
        float progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime * 20 / distanceDash;
            agent.transform.position = Vector3.Lerp(currentPosition, newPosition, progress);

            yield return null;
        }

        agent.updatePosition = true;
        agent.Warp(newPosition);

        isDashing = false;
    }
}
