using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class RandomHue : MonoBehaviour
{
    void Start()
    {
        var mayo = transform.Find("Mayo11anim").Find("Mayo");

        foreach (Material material in mayo.GetComponent<Renderer>().materials)
        {
            if (material.name.Contains("Clothes"))
            {
                material.SetFloat("_Seed", Random.Range(0f, 100f)/100f);
            }
        }

    }



    void Update()
    {

    }
}
