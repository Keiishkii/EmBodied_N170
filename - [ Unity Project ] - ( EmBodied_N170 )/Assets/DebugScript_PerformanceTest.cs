using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugScript_PerformanceTest : MonoBehaviour
{
    private Vector3 startPosition;
    
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = startPosition + (new Vector3(Mathf.Sin(Time.timeSinceLevelLoad), Mathf.Cos(Time.timeSinceLevelLoad)) * 0.5f);
    }
}
