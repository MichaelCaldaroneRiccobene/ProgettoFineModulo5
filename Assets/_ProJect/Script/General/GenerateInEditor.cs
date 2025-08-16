using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
[CustomEditor(typeof(GenerateInEditor))]
public class GenerateInEditorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        GenerateInEditor script = (GenerateInEditor)target;

        if (GUILayout.Button("Regenerate Obj")) script.GenerateObjs();
    }
}


[ExecuteInEditMode]
public class GenerateInEditor : MonoBehaviour
{
    [Header("General Setting")]
    [SerializeField] private GameObject[] objs;

    [SerializeField] private int xNumber;
    [SerializeField] private int zNumber;
    [SerializeField] private int yNumber;

    [SerializeField] private float space = 1;

    [Header("Random Rotation Setting")]
    [SerializeField] private bool isRotationWithObjParent;
    [SerializeField] private bool isRandomRotationAll;
    [SerializeField] private bool isRandomRotationX;
    [SerializeField] private bool isRandomRotationY;
    [SerializeField] private bool isRandomRotationZ;

    private Vector3 startPos;
    private Vector3 upDatePos;

    private float maxRot = 360;
    private float minRot = 0;

    public void GenerateObjs()
    {
        startPos = transform.position;
        upDatePos = transform.position;


        for (int i = transform.childCount - 1; i >= 0; i--) DestroyImmediate(transform.GetChild(i).gameObject);

        for (int i = 0; i < zNumber; i++)
        {
            for (int j = 0; j < yNumber; j++)
            {
                for (float k = 0; k < xNumber; k++)
                {
                    SpawnObj();
                }
                upDatePos.x = startPos.x;
                upDatePos.y += space;
            }
            upDatePos.y = startPos.y;
            upDatePos.z += space;
        }
    }

    private void SpawnObj()
    {
        int randomObj = Random.Range(0, objs.Length);
        GameObject obj = Instantiate(objs[randomObj], upDatePos, Quaternion.identity, transform);

        if(isRotationWithObjParent) obj.transform.rotation = transform.rotation;
        else ObjRotation(obj);

        upDatePos.x += space;
    }

    private void ObjRotation(GameObject obj)
    {
        if (isRandomRotationAll)
        {
            obj.transform.rotation = Quaternion.Euler(Random.Range(minRot, maxRot), Random.Range(minRot, maxRot), Random.Range(minRot, maxRot));
            return;
        }
        Vector3 finalRotation = obj.transform.eulerAngles;

        if (isRandomRotationX) finalRotation.x = Random.Range(minRot, maxRot);
        if (isRandomRotationY) finalRotation.y = Random.Range(minRot, maxRot);
        if (isRandomRotationZ) finalRotation.z = Random.Range(minRot, maxRot);

        obj.transform.rotation = Quaternion.Euler(finalRotation);
    }

    #endif
}
