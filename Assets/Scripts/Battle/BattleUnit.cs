using System;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine.Rendering;

public

    class BattleUnit
{
    public int vidaAtual;
    public int vidaMaxima;
    public int bloqueio;
    public List<DebuffEffect> debuffs = new List<DebuffEffect>();
    
    public event Action<int> OnDanoRecebido;

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
        OnDanoRecebido?.Invoke(danoRestante);

    }

    public void GanharBloqueio(int quantidade)
    {

        bloqueio += quantidade;
    }

    public void ProcessarInicioTurno()
    {
        foreach (var v in debuffs)
        {
            if (!v.after)
            {
                v.Aplicar(this);
            }
        }

        RemoverDebuffsExpirados();
    }



    public void ProcessarFimTurno()
    {
        foreach (var v in debuffs)
        {
            if (v.after)
            {
                v.Aplicar(this);
            }
        }

        RemoverDebuffsExpirados();

    }


    public void RemoverDebuffsExpirados()
    {
        debuffs.RemoveAll(v => v.duracao <= 0);
    }

}