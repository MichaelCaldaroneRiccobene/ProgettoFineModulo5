using System;
using System.Collections;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    [SerializeField] private Stats_EntitySO stats;
    [SerializeField] private Transform firePoint;

    [SerializeField] private int manaRequestFirstAttack = 10;
    [SerializeField] private int manaRequestSecondAttack = 25;

    [SerializeField] private int numberOfCubeOfGrass = 3;
    [SerializeField] private float distanceForeachCubeOfGrass = 3;
    [SerializeField] private float timeSpawnCubeOfGrass = 0.5f;

    [SerializeField] private string parameterTriggerFirstAttack = "FirstAttack";
    [SerializeField] private string parameterTriggerSecondAttack = "SecondAttack";

    public event Action<int, Action> OnAttack;
    public event Action<string> OnTryAttack;

    private Player_Input player_Input;

    private void Start()
    {
        SetUpEventAction();
    }

    private void SetUpEventAction()
    {
        player_Input = GetComponentInChildren<Player_Input>();
        if(player_Input != null) player_Input.OnTryFirstAttack += TryOnFirstAttack;
        if(player_Input != null) player_Input.OnTrySecondAttack += TryOnSecondAttack;
    }

    public void TryOnFirstAttack() =>  OnTryAttack?.Invoke(parameterTriggerFirstAttack);

    public void TryOnSecondAttack() => OnTryAttack?.Invoke(parameterTriggerSecondAttack);


    public void OnFirstAttack()
    {
        OnAttack?.Invoke(manaRequestFirstAttack, () =>
        {
            GameObject obj = ManagerPool.Instace.GetGameObjFromPool(Parameters_ObjectPool.FireBallObjForPool);
            BaseMagic weapon = obj.gameObject.GetComponentInChildren<BaseMagic>();

            if (weapon is FireBall fireball)
            {
                weapon.BasicSetUp(firePoint.position, firePoint.rotation, stats.DamageRange, transform);
                fireball.OnShoot(transform.forward);

                CameraShake.Instance.OnCameraShake(transform.position, 0.5f, 1, 5);
            }
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
            GameObject obj = ManagerPool.Instace.GetGameObjFromPool(Parameters_ObjectPool.CubeOfDirtObjForpool);

            sizeCube += obj.transform.localScale.x + distanceForeachCubeOfGrass;
            Vector3 positionToSpawn = positionStart + positionForwardStart * sizeCube;

            BaseMagic weapon = obj.gameObject.GetComponentInChildren<BaseMagic>();
            if (weapon != null) weapon.BasicSetUp(positionToSpawn, transform.rotation, stats.DamageMelee, transform);

            CameraShake.Instance.OnCameraShake(obj.transform.position,1, 1.5f, 15);
            RegenerateNavMesh.Instance.UpdateNaveMeshSurface();

            yield return new WaitForSeconds(timeSpawnCubeOfGrass);
        }
    }

    private void OnDisable()
    {
        if (player_Input != null) player_Input.OnTryFirstAttack -= TryOnFirstAttack;
        if (player_Input != null) player_Input.OnTrySecondAttack -= TryOnSecondAttack;
    }
}
