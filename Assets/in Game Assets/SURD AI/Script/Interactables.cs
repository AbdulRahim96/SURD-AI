using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;

public class Interactables : MonoBehaviour
{
    public static Interactables instance;
    public List<Object> _objects;

    

    private void Awake()
    {
        instance = this;
    }
    public int Check(string word)
    {
        for (int i = 0; i < _objects.Count; i++)
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


    public void AddItem(string name, string description, Transform target)
    {
        Object newItem = new Object(name, description, target);
        _objects.Add(newItem);
    }
    public void RemoveItem(Transform obj)
    {
        for (int i = 0; i < _objects.Count; i++)
        {
            if (_objects[i].target == obj)
            {
                _objects.Remove(_objects[i]);
                Refresh();
                return;
            }
        }
    }

    public void RemoveItem(string name, string description)
    {
        for (int i = 0; i < _objects.Count; i++)
        {
            if (_objects[i].objectName == name && _objects[i].description == description)
            {
                _objects.Remove(_objects[i]);
                Refresh();
                return;
            }
        }
    }

    private void Refresh()
    {
        foreach (Object obj in _objects)
        {
            if (obj.target == null)
            {
                _objects.Remove(obj);
                return; // Exit the loop after removing an item
            }
        }
    }

        [System.Serializable]
    public class Object
    {
        public string objectName;
        public string description;
        public Transform target;

        public Object(string objectName, string description, Transform target)
        {
            this.objectName = objectName;
            this.description = description;
            this.target = target;
        }
    }
}
