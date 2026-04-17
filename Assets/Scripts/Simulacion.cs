using UnityEngine;

public class Simulacion : MonoBehaviour
{
    public Preso[] presos;
    public Guardia[] guardias;

    void Update()
    {
        foreach (Preso p in presos)
        {
            if (p != null)
                p.Simular();
        }

        foreach (Guardia g in guardias)
        {
            if (g != null)
                g.Simular();
        }
    }
}