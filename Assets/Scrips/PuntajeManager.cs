using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuntajeManager : MonoBehaviour
{
    private float puntos;

    private static PuntajeManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public float GetPuntos()
    {
        return puntos;
    }

    public void SumarPuntos(float puntosEntrada)
    {
        puntos += puntosEntrada;
    }
}
