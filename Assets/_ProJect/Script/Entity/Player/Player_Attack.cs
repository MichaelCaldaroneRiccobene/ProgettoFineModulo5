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


    private Player_Input player_Input;

    private Player_Mana player_Mana;
    private Player_Animation player_Animation;

    private void Start()
    {
        player_Mana = GetComponent<Player_Mana>();

        player_Animation = GetComponentInChildren<Player_Animation>();

        SetUpEventAction();
    }

    private void SetUpEventAction()
    {
        player_Input = GetComponentInChildren<Player_Input>();
        player_Input.OnTryFirstAttack += TryOnFirstAttack;
        player_Input.OnTrySecondAttack += TryOnSecondAttack;
    }

    public void TryOnFirstAttack() => player_Animation.FirstAttack();

    public void TryOnSecondAttack() => player_Animation.SecondAttack();


    public void OnFirstAttack()
    {
        if(!player_Mana.CanUseMana(manaRequestFirstAttack)) return;

        player_Mana.UpdateMana(-manaRequestFirstAttack);

        GameObject obj = ManagerPool.Instace.GetGameObjFromPool(Parameters_ObjectPool.FireBallObjForPool);

        if (obj.gameObject.TryGetComponent(out FireBall fireball))
        {
            float shooterSpeed = 0;

            fireball.transform.position = firePoint.position;
            fireball.transform.rotation = firePoint.rotation;
            fireball.OnShoot(transform.forward, shooterSpeed, stats.DamageRange, transform);
            CameraShake.Instance.OnCameraShake(transform.position, 0.5f, 1, 5);
        }
    }

    public void OnSecondAttack()
    {
        if (!player_Mana.CanUseMana(manaRequestSecondAttack)) return;

        player_Mana.UpdateMana(-manaRequestSecondAttack);
        StartCoroutine(OnSecondAttackRoutine());
    }

    private IEnumerator OnSecondAttackRoutine()
    {
        float sizeCube = 0;

        Vector3 positionStart = transform.position;
        Vector3 positionForwardStart = transform.forward;

        for (int i = 0; i < numberOfCubeOfGrass; i++)
        {
            GameObject obj = ManagerPool.Instace.GetGameObjFromPool(Parameters_ObjectPool.CubeOfGrassObjForpool);

            sizeCube += obj.transform.localScale.x + distanceForeachCubeOfGrass;
            Vector3 positionToSpawn = positionStart + positionForwardStart * sizeCube;

            obj.transform.position = positionToSpawn;
            obj.transform.rotation = transform.rotation;

            CameraShake.Instance.OnCameraShake(obj.transform.position,1, 1.5f, 15);
            RegenerateNavMesh.Instance.UpdateNaveMeshSurface();

            yield return new WaitForSeconds(timeSpawnCubeOfGrass);
        }
    }

    private void OnDisable()
    {
        player_Input.OnTryFirstAttack -= TryOnFirstAttack;
        player_Input.OnTrySecondAttack -= TryOnSecondAttack;
    }
}
