using UnityEngine;

public abstract class DebuffEffect : ScriptableObject
{
    public string nome;
    public int duracaoBase;

    public abstract void Aplicar(BattleUnit unidade);
}