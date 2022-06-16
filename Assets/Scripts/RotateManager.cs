using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateManager : MonoBehaviour
{
    public static RotateManager instance;
    public GameObject[] gems;
    public float speed = 0.1f;

    private void Start()
    {
        instance = this;
        gems = GameObject.FindGameObjectsWithTag("gems");
    }


    public void UpdateTags()
    {
        gems = GameObject.FindGameObjectsWithTag("gems");
    }

    private void Update()
    {
       
        
        foreach(GameObject gem in gems)
        {
            if(gem == null)
            {
                Destroy(gem);
            }

            if(gem != null)
            {
                gem.GetComponent<Transform>().Rotate(Vector3.up * speed);
            }
        }
    }
}
