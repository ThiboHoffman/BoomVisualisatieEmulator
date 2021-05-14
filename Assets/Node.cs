using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    private readonly GameObject _obj;
    private readonly Color _color;
    public GameObject Obj { get { return _obj; } }
    public Color Color { get { return _color; } }

    public Node(GameObject prefab, GameObject obj) {
        _obj = obj;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
