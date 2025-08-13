using System.Collections;
using UnityEngine;

public class Player_Attack : MonoBehaviour
{
    [SerializeField] private Stats_EntitySO stats;
    [SerializeField] private Transform firePoint;

    public void OnRangeAttack() => StartCoroutine(OnRangeAttackRoutine());

    private IEnumerator OnRangeAttackRoutine()
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
}
