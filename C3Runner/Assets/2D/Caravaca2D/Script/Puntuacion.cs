using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puntuacion : MonoBehaviour
{
    public Image[] a;

    public float aciertosProfesor;

    public CambioAEscenaPrincipal cap;
    
    // Update is called once per frame
    void Update()
    {
    }

    public void acierto()
    {
        aciertosProfesor++;
        //Debug.Log("aciertos: " + aciertosProfesor);
        
        if (aciertosProfesor > 0 && aciertosProfesor < 6)
        {
            a[(int)aciertosProfesor-1].gameObject.SetActive(true);
        }

        if (aciertosProfesor >= 5)
        {
            cap.Cambiar();
        }
    }
    
    public void fallo()
    {
        
        Debug.Log(aciertosProfesor);
        
        if (aciertosProfesor > 0 && aciertosProfesor < 6)
        {
            a[(int)aciertosProfesor-1].gameObject.SetActive(false);
        }
        
        aciertosProfesor--;
    }
     

}
