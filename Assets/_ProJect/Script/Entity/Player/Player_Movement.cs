using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 2f;

    [SerializeField] private int dashCost = 5;
    [SerializeField] private GameObject dashEffect;
    [SerializeField] private GameObject[] hideTargetForDash;

    public event Action<int, Action> OnDash;

    private Player_Controller player_Controller;

    private NavMeshAgent agent;
    private Vector3 direction;
    private bool isOnDash;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;

        SetUpAction();
    }

    private void Update()
    {
        if (!Player_Controller.CanPlayerUseInput)
        {
            direction = Vector3.zero;
            return;
        }

        Rotation();
        Movement();
    }

    private void Movement()
    {
        Vector3 targetVelocity = direction.normalized * agent.speed;
        agent.velocity = Vector3.MoveTowards(agent.velocity, targetVelocity, agent.acceleration * Time.deltaTime);
    }


    private void SetUpAction()
    {
        player_Controller = GetComponent<Player_Controller>();

        if (player_Controller != null)
        {
            player_Controller.OnTakeHorizontalAndVertical += OnTakeHorizontalAndVertical;
            player_Controller.OnDash += Dash;
        } 
    }

    private void Rotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 lookDirection = hit.point - agent.transform.position;
            lookDirection.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            agent.transform.rotation = Quaternion.Slerp(agent.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
        }
    }

    private void OnTakeHorizontalAndVertical(float horizontal, float vertical) => direction = transform.forward * vertical + transform.right * horizontal;

    private void Dash()
    {
        OnDash?.Invoke(dashCost, () =>
        {
            StartCoroutine(DashRoutine(direction));
        }
        );
    }

    public IEnumerator DashRoutine(Vector3 direction)
    {
        if (isOnDash) yield break;

        isOnDash = true;
        Vector3 currentPosition = agent.transform.position;
        Vector3 newPosition = currentPosition + direction.normalized * 5;
        agent.updatePosition = false;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, direction.normalized, out hit, 5)) newPosition = hit.point - direction.normalized * 1f;

        SetEffectForDash(isOnDash);
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

        isOnDash = false;

        SetEffectForDash(isOnDash);
        CameraShake.Instace.OnCameraShake(transform.position, 0.2f, 1.5f, 10);
    }

    private void SetEffectForDash(bool isDashing)
    {
        if(dashEffect != null) dashEffect.SetActive(isDashing);

        foreach (GameObject obj in hideTargetForDash)
        {
            foreach(Renderer renderer in obj.GetComponentsInChildren<Renderer>()) renderer.enabled = !isDashing;
        }
    }

    private void OnDisable()
    {
        if (player_Controller != null)
        {
            player_Controller.OnTakeHorizontalAndVertical -= OnTakeHorizontalAndVertical;
            player_Controller.OnDash -= Dash;
        }
    }
}
