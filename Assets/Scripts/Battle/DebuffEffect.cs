using UnityEngine;

public abstract class DebuffEffect : ScriptableObject
{
    public string nome;
    public bool after;
    public int duracao;
    public Sprite icone;

    public abstract void Aplicar(BattleUnit unidade);
}