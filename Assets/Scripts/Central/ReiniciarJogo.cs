using UnityEngine;
using Core;

public class ReiniciarJogo : MonoBehaviour
{
    public void Reiniciar()
    {
        GameManager.Gman.NovaRun();
    }
}