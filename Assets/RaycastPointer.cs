 using UnityEngine;
 using System.Collections;
 
 public class RaycastPointer : MonoBehaviour 
 {
    Ray ray;
    RaycastHit hit;
    
    void Start()
    {

    }

    void FixedUpdate()
    {
        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);       
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity))
        { 
            if(Input.GetMouseButtonDown(0)) 
            {
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject.name.Contains("Node")) 
                {
                    Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow, 1F);
                    Debug.Log(CreateTree.SearchNodeWithName(hit.collider.gameObject.name));
                }
            }
        }     
        /*
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(transform.position, transform.forward, Color.green);

        if(Physics.Raycast(ray, out hit))
        {
            Debug.DrawRay(transform.position, transform.forward, Color.green);
            print(hit.collider.name);
            if(Input.GetMouseButtonDown(0))
                print(hit.collider.name);
        }
        */
    }
 }
 