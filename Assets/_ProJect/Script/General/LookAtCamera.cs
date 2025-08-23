using UnityEngine;
public class LookAtCamera : MonoBehaviour { private void FixedUpdate() => transform.rotation = Camera.main.transform.rotation; }


