using System;
using System.Collections;
using UnityEngine;
public enum PlayerAttacks
{
    None = 0, FireBall = 1, Earth = 2
}

public class Player_Attack : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private Stats_EntitySO stats;
    [SerializeField] private Transform firePoint;

    [SerializeField] private int manaRequestFirstAttack = 10;
    [SerializeField] private int manaRequestSecondAttack = 25;

    [SerializeField] private int numberOfCubeOfGrass = 3;
    [SerializeField] private float distanceForeachCubeOfGrass = 3;
    [SerializeField] private float timeSpawnCubeOfGrass = 0.5f;

    public event Action<int, Action> OnAttack;
    public event Action<PlayerAttacks> OnTryAttack;

    private Player_Controller player_Controller;
    private Player_Animation player_Animation;

    private void Start() => SetUpEventAction();

    private void SetUpEventAction()
    {
        player_Controller = GetComponent<Player_Controller>();
        if (player_Controller != null) player_Controller.OnTryAttack += TryAttack;

        player_Animation = GetComponentInChildren<Player_Animation>();
        if (player_Animation != null) player_Animation.OnDoAttack += OnDoAttack;
    }

    public void TryAttack(PlayerAttacks playerAttacks) => OnTryAttack?.Invoke(playerAttacks);

    private void OnDoAttack(PlayerAttacks playerAttacks)
    { 
        switch (playerAttacks)
        {
            case PlayerAttacks.None:
                break;
            case PlayerAttacks.FireBall:
                OnFirstAttack();
                break;
            case PlayerAttacks.Earth:
                OnSecondAttack();
                break;
        }
    }

    public void OnFirstAttack()
    {
        OnAttack?.Invoke(manaRequestFirstAttack, () =>
        {
            GameObject obj = ManagerPool.Instace.GetGameObjFromPool(StaticName.Parameters_ObjectPool.FireBallObjForPool);
            if (obj == null) return;

            BaseMagic weapon = obj.gameObject.GetComponentInChildren<BaseMagic>();

            if (weapon is FireBall fireball)
            {
                weapon.BasicSetUp(firePoint.position, firePoint.rotation, stats.DamageRange, transform);
                fireball.OnShoot(transform.forward);
            }

            if (CameraShake.Instance != null) CameraShake.Instance.OnCameraShake(transform.position, 0.5f, 1, 5);
        });
    }

    public void OnSecondAttack()
    {
        OnAttack?.Invoke(manaRequestSecondAttack, () =>
        {
            StartCoroutine(OnSecondAttackRoutine());
        });
    }

    private IEnumerator OnSecondAttackRoutine()
    {
        float sizeCube = 0;

        Vector3 positionStart = transform.position;
        Vector3 positionForwardStart = transform.forward;

        for (int i = 0; i < numberOfCubeOfGrass; i++)
        {
            GameObject obj = ManagerPool.Instace.GetGameObjFromPool(StaticName.Parameters_ObjectPool.CubeOfDirtObjForpool);
            if (obj == null) yield break;

            sizeCube += obj.transform.localScale.x + distanceForeachCubeOfGrass;
            Vector3 positionToSpawn = positionStart + positionForwardStart * sizeCube;

            BaseMagic weapon = obj.gameObject.GetComponentInChildren<BaseMagic>();
            if (weapon != null) weapon.BasicSetUp(positionToSpawn, transform.rotation, stats.DamageMelee, transform);

            if (CameraShake.Instance != null) CameraShake.Instance.OnCameraShake(obj.transform.position, 1, 1.5f, 15);

            yield return new WaitForSeconds(timeSpawnCubeOfGrass);
        }
    }

    private void OnDisable()
    {
        if (player_Controller != null) player_Controller.OnTryAttack -= TryAttack;

        if (player_Animation != null) player_Animation.OnDoAttack -= OnDoAttack;
    }
}
