using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] private int dashCost = 5;
    [SerializeField] private GameObject dashEffect;
    [SerializeField] private GameObject[] hideTargetForDash;

    private NavMeshAgent agent;
    private Player_Mana player_Mana;
    private Player_Input player_Input;

    private Vector3 direction;
    private bool isDashing;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player_Mana = GetComponent<Player_Mana>();
        agent.updateRotation = false;

        SetUpEventAction();
    }

    private void SetUpEventAction()
    {
        player_Input = GetComponent<Player_Input>();
        player_Input.OnTakeHorizontalAndVertical += Movement;
        player_Input.OnDash += Dash;
        player_Input.OnRotate += Rotation;
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

    public void Movement(float horizontal, float vertical)
    {
        direction = transform.forward * vertical + transform.right * horizontal;
        agent.velocity = direction.normalized * agent.speed;
    }

    public void Dash()
    {
        if (!player_Mana.CanUseMana(dashCost)) return;

        player_Mana.UpdateMana(-dashCost);
        StartCoroutine(DashRoutine(direction));
    }

    public IEnumerator DashRoutine(Vector3 direction)
    {
        if (isDashing) yield break;

        isDashing = true;
        Vector3 currentPosition = agent.transform.position;
        Vector3 newPosition = currentPosition + direction.normalized * 5;
        agent.updatePosition = false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction.normalized, out hit, 5)) newPosition = hit.point - direction.normalized * 1f;

        SetEffectForDash(isDashing);

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
        SetEffectForDash(isDashing);
        CameraShake.Instance.OnCameraShake(transform.position, 0.2f, 1.5f,10);
    }

    private void SetEffectForDash(bool isDashing)
    {
        dashEffect.SetActive(isDashing);
        foreach(GameObject obj in hideTargetForDash) obj.SetActive(!isDashing);
    }

    private void OnDisable()
    {
        player_Input.OnTakeHorizontalAndVertical -= Movement;
        player_Input.OnDash -= Dash;
        player_Input.OnRotate -= Rotation;
    }
}
