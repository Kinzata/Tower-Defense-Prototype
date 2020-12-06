using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateConstantly : MonoBehaviour
{
    [SerializeField] Vector3 InitialRotation = Vector3.zero;
    [SerializeField] Vector3 PerSecondRotation = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(InitialRotation, Space.World);
    }

    // Update is called once per frame
    void Update()
    {
        var rotationAmount = PerSecondRotation.y * Time.deltaTime;

        transform.Rotate(new Vector3(0,rotationAmount,0), Space.World);
    }
}
