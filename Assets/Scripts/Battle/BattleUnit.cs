using System;
using Unity.VisualScripting.FullSerializer;

public

class BattleUnit
{
    public int vidaAtual;
    public int vidaMaxima;
    public int bloqueio;

    public BattleUnit(int vidaMaxima, int vidaAtual)
    {
        this.vidaMaxima = vidaMaxima;
        this.vidaAtual = vidaAtual;
        this.bloqueio = 0;
    }

    public void ReceberDano(int dano)
    {
        
        int danoRestante = UnityEngine.Mathf.Max(0, dano - bloqueio);
        Console.Write(danoRestante);
        bloqueio = UnityEngine.Mathf.Max(0, bloqueio - dano);
        vidaAtual = UnityEngine.Mathf.Max(0, vidaAtual - danoRestante);
        

    }

    public void GanharBloqueio(int quantidade)
    {
        
        bloqueio +=quantidade;
    }

}