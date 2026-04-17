using UnityEngine;

public class Preso : MonoBehaviour
{
    public enum Estado { EnCelda, Corriendo, Escondido, Distrayendo }
    public Estado estado;

    public Guardia guardia;

    public float velocidad = 1.5f;
    public Celda miCelda;

    float tiempoDecision = 0f;

    float tiempo = 0f;
    float duracionAccion = 0f;

    void Start()
    {
        estado = Estado.EnCelda;
    }

    public void Simular()
    {
        tiempo += Time.deltaTime;
        duracionAccion += Time.deltaTime;

        // Cada 3 segundos elige acción
        if (estado == Estado.EnCelda && tiempo > 3f)
        {
            ElegirAccion();
            tiempo = 0f;
            duracionAccion = 0f;
        }

        // Ejecuta acción por máximo 3 segundos
        if (estado != Estado.EnCelda && duracionAccion > 3f)
        {
            VolverACelda();
        }
        if (estado != Estado.EnCelda)
        {
        ReaccionarAlGuardia();
        }
        switch (estado)
        {
            case Estado.Corriendo:
                transform.position += Vector3.up * velocidad * Time.deltaTime;
                break;

            case Estado.Distrayendo:
                transform.position += new Vector3(Mathf.Sin(Time.time * 5f), 0, 0) * velocidad * Time.deltaTime;
                break;

            case Estado.Escondido:
                // no se mueve
                break;
        }
    }

    void ElegirAccion()
    {
        float r = Random.value;

        if (r < 0.5f)
            estado = Estado.Corriendo;
        else if (r < 0.75f)
            estado = Estado.Escondido;
        else
            estado = Estado.Distrayendo;
    }

    public void VolverACelda()
    {
        estado = Estado.EnCelda;
        transform.position = miCelda.transform.position;

        tiempo = 0f;
        duracionAccion = 0f;
    }

    public bool PuedeSerCapturado()
    {
        return duracionAccion > 1f; // pequeña protección
    }
    void ReaccionarAlGuardia()
    {
    if (guardia == null) return;

    tiempoDecision += Time.deltaTime;

    float distancia = Vector2.Distance(transform.position, guardia.transform.position);

    // Solo decide cada 1.5 segundos
    if (distancia < 3f && tiempoDecision > 1.5f)
    {
        float r = Random.value;

        if (r < 0.5f)
        {
            estado = Estado.Escondido;
            Debug.Log("Reacciona: ESCONDERSE");
        }
        else
        {
            estado = Estado.Distrayendo;
            Debug.Log("Reacciona: DISTRAER");
        }

        tiempoDecision = 0f;
    }
    }
}