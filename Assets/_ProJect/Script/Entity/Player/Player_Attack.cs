using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Player_Attack : MonoBehaviour
{
    [SerializeField] private Stats_EntitySO stats;
    [SerializeField] private Transform firePoint;

    [SerializeField] private int manaRequestFirstAttack = 10;
    [SerializeField] private int manaRequestSecondAttack = 25;

    [SerializeField] private int numberOfCubeOfGrass = 3;
    [SerializeField] private float distanceForeachCubeOfGrass = 3;
    [SerializeField] private float timeSpawnCubeOfGrass = 0.5f;

    public UnityEvent<int> OnUseMana;

    public static bool canUseMagic = false;

    public void OnFirstAttack()
    {
        OnUseMana?.Invoke(-manaRequestFirstAttack);
        if(!canUseMagic) return;

        canUseMagic = false;
        StartCoroutine(OnFirstAttackRoutine());
    }

    public void OnSecondAttack()
    {
        OnUseMana?.Invoke(-manaRequestSecondAttack);
        if (!canUseMagic) return;

        canUseMagic = false;
        StartCoroutine(OnSecondAttackRoutine());
    }

    public void CanUseMagic() => canUseMagic = true;


    private IEnumerator OnFirstAttackRoutine()
    {
        GameObject obj = ManagerPool.Instace.GetGameObjFromPool(Parameters_ObjectPool.FireBallObjForPool);

        if (obj.gameObject.TryGetComponent(out FireBall fireball))
        {
            float shooterSpeed = 0;

            fireball.transform.position = firePoint.position;
            fireball.OnShoot(transform.forward, shooterSpeed, stats.DamageRange, transform);
            CameraShake.Instance.OnCameraShake(0.5f, 0.5f);
        }

        yield return null;
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

            CameraShake.Instance.OnCameraShake(1.5f, 0.5f);
            RegenerateNavMesh.Instance.UpdateNaveMeshSurface();

            yield return new WaitForSeconds(timeSpawnCubeOfGrass);
        }
    }
}
