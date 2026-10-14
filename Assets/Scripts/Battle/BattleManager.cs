using System;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public BattleUnit jogador;
    public BattleUnit inimigo;
    

    public int energiaAtual = 3;


    private void Awake()
    {
        
        jogador.vidaAtual = Core.GameManager.Run.HP;
        inimigo.vidaAtual = 100;//colocar hp ligado no enemyData de algum jeito
    }

    public void JogarCarta(CardData carta)
    {
        if (carta == null || jogador == null || inimigo == null)
            return;

        if (energiaAtual < carta.custo)
            return;

        energiaAtual -= carta.custo;

        foreach (CardEffect efeito in carta.effects)
        {
            efeito.execute(this, jogador, inimigo);
        }
    }

    public void FinalizarTurno()
    {
       

        energiaAtual = 3;
    }
}