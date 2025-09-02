using UnityEngine;
using UnityEngine.AI;

public static class Utility
{
    #region ChooseRandomPoint
    public static Vector3 RandomPoint(NavMeshAgent agent, Vector3 startPosition, float range)
    {
        int numberOfTentativ = 5;

        for (int i = 0; i < numberOfTentativ; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * range + startPosition;
            randomPosition.y = agent.transform.position.y;

            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 1, NavMesh.AllAreas)) return hit.position;
        }

        Debug.LogError("Nessuna posiziona Trovata");

        return agent.transform.position;
    }
    #endregion
}
