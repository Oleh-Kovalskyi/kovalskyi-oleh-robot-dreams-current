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
            foreach (string item in _stringList)
            {
                listContent += item + "\n";
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

        List<string> numberStrings = new List<string>();
        List<string> letterStrings = new List<string>();
        foreach (string item in _stringList)
        {
            if (float.TryParse(item, out _))
            {
                numberStrings.Add(item);
            }
            else
            {
                letterStrings.Add(item);
            }
        }

        numberStrings.Sort((a, b) => float.Parse(a).CompareTo(float.Parse(b)));
        letterStrings.Sort();

        _stringList = new List<string>();
        _stringList.AddRange(numberStrings);
        _stringList.AddRange(letterStrings);


        string listContent = "List sorted:\n";
        
        foreach (var item in _stringList)
        {
            listContent += item + "\n";
        }
        Debug.Log(listContent);
    }
}
