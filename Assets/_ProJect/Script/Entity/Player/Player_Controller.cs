using System;
using UnityEngine;

public class Player_Controller : MonoBehaviour, I_Team
{
    [Header("Setting I_Team")]
    [SerializeField] private bool canBeAFollow = true;
    [SerializeField] private int teamNumber = 1;

    [Header("Setting CameraShake")]
    [SerializeField] private float durationCameraShake = 0.4f;
    [SerializeField] private float intensityCameraShake = 1.1f;
    [SerializeField] private float distanceCameraShake = 2f;

    private LifeSistem lifeSistem;

    private float horizontal;
    private float vertical;

    public event Action<float, float> OnTakeHorizontalAndVertical;

    public event Action <PlayerAttacks> OnTryAttack;
    public event Action OnDash;
    public event Action OnInteract;
    public event Action OnTriggerSitUp;

    public static bool CanPlayerUseInput { get; set; }

    private void Awake() => CanPlayerUseInput = false;

    private void Start() => SetUpAction();

    private void Update()
    {
        if (Input.anyKeyDown && !CanPlayerUseInput)
        {
            //GameManager.Instance.OnStart();
            OnTriggerSitUp?.Invoke();

            if(Player_Ui.Instance != null) Player_Ui.Instance.ShowPlayerUI();
            if(GameManager.Instance != null) GameManager.Instance.OffStaticCamera();
        }

        if (!CanPlayerUseInput) return;
        InputPlayer();
    }

    private void SetUpAction()
    {
        lifeSistem = GetComponent<LifeSistem>();

        if(lifeSistem != null ) lifeSistem.OnUpdateHp += OnUpdateHp;
        if (lifeSistem != null) lifeSistem.OnHit += OnHit;
        if (lifeSistem != null) lifeSistem.OnDead += OnDead;
    }

    #region LifePlayer

    private void OnUpdateHp(float hp) { if (Player_Ui.Instance != null) Player_Ui.Instance.UpdateHp(hp); }

    private void OnHit() => CameraShake.Instance.OnCameraShake(transform.position, durationCameraShake, intensityCameraShake, distanceCameraShake);

    private void OnDead() => CanPlayerUseInput = false;
    #endregion

    #region Player Input
    private void InputPlayer()
    {
        InputDirectionPlayer();

        InputAttack();
        InputInteract();

        InputDash();
        InputPause();



        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LifeSistem lifeSistem = transform.GetComponent<LifeSistem>();
            lifeSistem.Damage(-10);
        }
        if (Input.GetKey(KeyCode.Alpha1)) Time.timeScale = 0.1f;
        if (Input.GetKey(KeyCode.Alpha2)) Time.timeScale = 1f;
    }

    private void InputDirectionPlayer()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        OnTakeHorizontalAndVertical?.Invoke(horizontal, vertical);
    }

    private void InputInteract() { if (Input.GetKeyDown(KeyCode.E)) OnInteract?.Invoke(); }

    private void InputAttack()
    {
        if (Input.GetMouseButtonDown(0)) OnTryAttack?.Invoke(PlayerAttacks.FireBall);
        if (Input.GetMouseButtonDown(1)) OnTryAttack?.Invoke(PlayerAttacks.Earth);
    }

    private void InputDash() { if (Input.GetKeyDown(KeyCode.Space)) OnDash?.Invoke(); }

    private void InputPause()
    {
        //if (Input.GetKeyDown(KeyCode.Escape)) MenuInGameManager.Instance.GoToOpenMenu();
    }

    #endregion

    #region I_Team
    public void ClearTarget() { }
    public void ClearLastTarget() { }

    public void ClearAllied() { }

    public void SetLastTarget(Transform lastTarget) { }
    public void SetAllied(Transform allied) { }
    public void SetTarget(Transform target) { }

    public void SetPriorityTarget(Transform target) { }


    public int GetTeamNumber() => teamNumber;

    public Transform GetAllied() => null;

    public Transform GetTarget() => null;

    public Transform GetLastTarget() => null;

    public bool CanBeFollow() => canBeAFollow;

    public bool HasTarget() => false;
    #endregion

    private void OnDisable()
    {
        if (lifeSistem != null) lifeSistem.OnUpdateHp -= OnUpdateHp;
        if (lifeSistem != null) lifeSistem.OnHit -= OnHit;
        if (lifeSistem != null) lifeSistem.OnDead -= OnDead;
    }
}
