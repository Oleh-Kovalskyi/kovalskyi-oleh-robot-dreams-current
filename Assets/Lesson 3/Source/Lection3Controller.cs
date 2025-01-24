using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lection3Controller : MonoBehaviour
{
    [SerializeField] private List<string> _stringList;
    [SerializeField] private string _value;

    [ContextMenu("Print")]
    private void Print()
    {
        if (_stringList != null && _stringList.Count > 0)
        {
            string listContent = "List:\n";
            for (int i = 0; i < _stringList.Count; ++i)
            {
                listContent += _stringList[i] + "\n";
            }
            Debug.Log(listContent);
        }
        else
        {
            Debug.Log("The list is empty.");
        }
    }

    [ContextMenu("Add")]
    private void Add()
    {
        if (!string.IsNullOrEmpty(_value))
        {
            _stringList.Add(_value);
            Debug.Log($"The string {_value} is added to the list.");
        }
        else
        {
            Debug.Log($"The string is empty, please fill the Value.");
        }
    }
    [ContextMenu("Remove")]
    private void Remove()
    {
        if (!string.IsNullOrEmpty(_value))
        {
            _stringList.Remove(_value);
            Debug.Log($"The string \"{_value}\" is removed from the list.");
        }
        else
        {
            Debug.Log($"The string is empty, please fill the Value.");
        }

    }
    [ContextMenu("Clear")]
    private void Clear()
    {
        if (_stringList.Count > 0)
        {
            _stringList.Clear();
            Debug.Log($"The List is clear");
        }
        else
        {
            Debug.Log($"The List is already clear");
        }
    }
    [ContextMenu("Sort")]
    private void Sort()
    {
        List<float> numberList = new List<float>();
        List<string> letterList = new List<string>();

        for (int i = 0; i < _stringList.Count; ++i)
        {
            if (float.TryParse(_stringList[i], out float parsedValue))
            {
                numberList.Add(parsedValue);
            }
            else
            {
                letterList.Add(_stringList[i]);
            }
        }

        numberList.Sort();
        letterList.Sort();

        _stringList.Clear();
        for (int i = 0; i < numberList.Count; ++i)
        {
            _stringList.Add(numberList[i].ToString());
        }
        _stringList.AddRange(letterList);

        string listContent = "List sorted:\n";
        for (int i = 0; i < _stringList.Count; ++i)
        {
            listContent += _stringList[i] + "\n";
        }
        Debug.Log(listContent);
    }
}
