using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateScriptsTrigger : NetworkBehaviour
{
    public bool enabledByDefault = true;

    public enum Place // your custom enumeration
    {
        Caravaca,
        Lorca,
        Aguilas,
        Totana,
        Alhama,
        Alcantarilla,
        Murcia,
        Molina,
        Cieza,
        Jumilla,
        Yecla,
        SanPedro,
        Cartagena
    };
    public Place place = Place.Caravaca;  // this public var should appear as a drop down

    public List<GameObject> objectsWithScripts;


    void Start()
    {
        if (!enabledByDefault)
            TogglePlace2(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.GetComponent<Player3D>().isLocalPlayer)
        {
            TogglePlace2(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.GetComponent<Player3D>().isLocalPlayer)
        {
            TogglePlace2(false);
        }
    }


    void TogglePlace2(bool toggle)
    {
        foreach (var obj in objectsWithScripts)
        {
            obj.SetActive(toggle);

        }
    }

    void TogglePlace(bool toggle)
    {
        switch (place)
        {
            case Place.Caravaca: Caravava(toggle); break;
            case Place.Lorca: Lorca(toggle); break;
            //case Place.Aguilas: Aguilas(toggle); break;
            //case Place.Totana: Totana(toggle); break;
            //case Place.Alhama: Alhama(toggle); break;
            //case Place.Alcantarilla: Alcantarilla(toggle); break;
            //case Place.Murcia: Murcia(toggle); break;
            case Place.Molina: Molina(toggle); break;
                //case Place.Cieza: Cieza(toggle); break;
                //case Place.Jumilla: Jumilla(toggle); break;
                //case Place.Yecla: Yecla(toggle); break;
                //case Place.SanPedro: SanPedro(toggle); break;
                //case Place.Cartagena: Cartagena(toggle); break;
        }
    }

    void Caravava(bool toggle)
    {
        foreach (var obj in objectsWithScripts)
        {
            //Flechas
            if (obj.GetComponent<FlechasVelocidad>() != null)
            {
                obj.GetComponent<FlechasVelocidad>().enabled = toggle;
            }

            //Cavallista
            if (obj.GetComponent<WayPointController>() != null)
            {
                obj.GetComponent<WayPointController>().enabled = toggle;
                obj.GetComponent<Bouncer>().enabled = toggle;
            }

            //Bolas
            if (obj.GetComponent<SimpleRotator>() != null)
            {
                obj.GetComponent<SimpleRotator>().enabled = toggle;
                obj.transform.GetChild(0).GetComponent<Bouncer>().enabled = toggle;
            }

        }
    }

    void Lorca(bool toggle)
    {
        foreach (var obj in objectsWithScripts)
        {
            //Flechas
            if (obj.GetComponent<FlechasVelocidad>() != null)
            {
                obj.GetComponent<FlechasVelocidad>().enabled = toggle;
            }

            //Rocas
            if (obj.GetComponent<RockSpawner>() != null)
            {
                obj.GetComponent<RockSpawner>().enabled = toggle;
            }

            //Bolas
            if (obj.GetComponent<SimpleRotator>() != null)
            {
                obj.GetComponent<SimpleRotator>().enabled = toggle;
                obj.transform.GetChild(0).GetComponent<Bouncer>().enabled = toggle;
            }

        }
    }

    void Molina(bool toggle)
    {
        foreach (var obj in objectsWithScripts)
        {
            //Flechas
            if (obj.GetComponent<FlechasVelocidad>() != null)
            {
                obj.GetComponent<FlechasVelocidad>().enabled = toggle;
            }

            //Bolas
            if (obj.GetComponent<SimpleRotator>() != null)
            {
                obj.GetComponent<SimpleRotator>().enabled = toggle;
                obj.transform.GetChild(0).GetComponent<Bouncer>().enabled = toggle;
            }

        }
    }
}
