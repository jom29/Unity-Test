using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public GameObject[] gems;

    public void Reswpawn()
    {
        GameObject clone;
        float x = Random.Range(10, -17);
        float z = Random.Range(17, -5);
        clone = Instantiate(gems[Random.Range(0, gems.Length - 1)], new Vector3(x, 0, z), transform.rotation) as GameObject;
    }
}
