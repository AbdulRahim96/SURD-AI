using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactables : MonoBehaviour
{
    public static Interactables instance;
    public Object[] _objects;

    private void Awake()
    {
        instance = this;
    }
    public int Check(string word)
    {
        for (int i = 0; i < _objects.Length; i++)
        {
            if (word.ToLower() == _objects[i].objectName.ToLower())
                return i;
        }
        return -1;
    }

    public Transform GetTarget(int index)
    {
        return _objects[index].target;
    }

    [System.Serializable]
    public class Object
    {
        public string objectName;
        public string description;
        public Transform target;
    }
}
