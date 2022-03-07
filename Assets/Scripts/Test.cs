using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public static event Action testAction;
    // Start is called before the first frame update
    void Start()
    {
        testAction += Debug1;
        testAction += Debug3;
        testAction += Debug4;
        testAction += Debug2;


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            testAction?.Invoke();
        }
    }
    private void Debug4()
    {
        Debug.Log("asd4");
    }

    private void Debug1()
    {
        Debug.Log("asd1");
    }
    private void Debug2()
    {
        Debug.Log("asd2");
    }
    private void Debug3()
    {
        Debug.Log("asd3");
    }
}
