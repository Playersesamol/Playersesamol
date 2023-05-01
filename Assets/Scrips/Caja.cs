// Script Caja
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Caja : MonoBehaviour
{
    public GameObject animalPrefab;

    public void Break()
    {
        // Instanciar el animalPrefab en la misma posición de la caja
        Instantiate(animalPrefab, transform.position, Quaternion.identity);
        // Destruir la caja
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Llamar al método Break de la caja
            Break();
        }
    }
}
