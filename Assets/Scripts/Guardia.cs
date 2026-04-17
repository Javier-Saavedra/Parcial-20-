using UnityEngine;

public class Guardia : MonoBehaviour
{
    public Preso[] presos;
    public float velocidad = 2f;

    float direccion = 1f;
    float cooldown = 0f;

    Vector3 puntoInicial;

    void Start()
    {
        puntoInicial = transform.position;
    }

    public void Simular()
    {
        cooldown += Time.deltaTime;

        Preso objetivo = ObtenerPresoCercano();

        if (objetivo == null)
        {
            Patrullar();
            return;
        }

        float distancia = Vector2.Distance(transform.position, objetivo.transform.position);

        if (cooldown < 3f)
        {
            VolverAPatrulla();
            return;
        }

        if ((objetivo.estado == Preso.Estado.Corriendo ||
             objetivo.estado == Preso.Estado.Distrayendo)
            && distancia < 6f)
        {
            Perseguir(objetivo);
        }
        else
        {
            Patrullar();
        }
    }

    Preso ObtenerPresoCercano()
    {
        Preso objetivo = null;
        float menorDistancia = Mathf.Infinity;

        foreach (Preso p in presos)
        {
            if (p == null) continue;

            float distancia = Vector2.Distance(transform.position, p.transform.position);

            if (distancia < menorDistancia)
            {
                menorDistancia = distancia;
                objetivo = p;
            }
        }

        return objetivo;
    }

    void Patrullar()
    {
        transform.position += new Vector3(direccion, 0, 0) * velocidad * Time.deltaTime;

        if (transform.position.x > 4) direccion = -1;
        if (transform.position.x < -4) direccion = 1;
    }

    void VolverAPatrulla()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            puntoInicial,
            velocidad * Time.deltaTime
        );
    }

    void Perseguir(Preso objetivo)
    {
        transform.position = Vector2.MoveTowards(
            transform.position,
            objetivo.transform.position,
            velocidad * Time.deltaTime
        );

        float distancia = Vector2.Distance(transform.position, objetivo.transform.position);

        if (distancia < 0.5f && objetivo.PuedeSerCapturado())
        {
            objetivo.VolverACelda();
            cooldown = 0f;
        }
    }
}