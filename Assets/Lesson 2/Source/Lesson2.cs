using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lesson2 : MonoBehaviour
{

    [SerializeField] private int _firstIntengerNumber; 
    [SerializeField] private int _secondIntengerNumber; 
    [SerializeField] private float _firstFloatNumber;
    [SerializeField] private string _firstText;
    [SerializeField] private bool _firstCheck;

    [ContextMenu("HelloWolrd")]
    private void HelloWolrd()
    {
        Debug.Log("Hello World");
    }

    [ContextMenu("Add")]
    private void Add () 
    {
    int result = _firstIntengerNumber + _secondIntengerNumber;
    Debug.Log(result);
    }
}
