using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    
    
    public enum BattleState
    {
        InicioTurnoJogador,
        TurnoJogador,
        FimTurnoJogador,
        TurnoInimigo,
        Vitoria,
        Derrota
    }


    [SerializeField] private List<EnemyData> inimigosP;
    private EnemyData inimigoEscolhido;
    public BattleUnit jogador;
    public BattleUnit inimigo;

    private CardEffect acaoInimigo;
    private int indiceAcaoInimigo = 0;
    private int maxIndiceAcaoInimigo;
    
    //declara os visuais do jogador e inimigo
    [SerializeField] private BattleUnitVisual jogadorView;
    [SerializeField] private BattleUnitVisual inimigoView;
    private List<CardData> Deckjogador = new List<CardData>();
    private List<CardData> Descartes = new List<CardData>();
    
    private int energiaAtual = 3;
    private int maxEnergia = 3;
    public BattleState estadoAtual { get; private set; }


    private void Awake()
    {
         jogador = new BattleUnit(Core.GameManager.Run.HP, Core.GameManager.Run.MaxHP);
         
        inimigoEscolhido=inimigosP[UnityEngine.Random.Range(0, inimigosP.Count)];//pega um inimigo aleatorio dos que tão aqui
        
        inimigo= new BattleUnit(inimigoEscolhido.HP, inimigoEscolhido.maxHP);
        
        maxIndiceAcaoInimigo=inimigoEscolhido.actions.Count;//ve quantas ações no vetor de açoes do inimigo

        Deckjogador = GameManager.Gman.DeckJogador.buscaTodas();//Pega todas as cartas do deck do jogador



    }

    private void Start()
    {
        IniciarTurnoJogador();
    }

    private void IniciarTurnoJogador()
    {
        energiaAtual = maxEnergia;
        estadoAtual = BattleState.InicioTurnoJogador;
        
        //comprar as cartas
        //permitir jogar cartas
        
        estadoAtual = BattleState.TurnoJogador;
    }

    public void FimTurnoJogador()
    {
        if (estadoAtual != BattleState.TurnoJogador)
            return;

        estadoAtual = BattleState.FimTurnoJogador;

        // descartar cartas da mão
        // aplicar efeitos de fim de turno
        if (VerFimCombate())
            return;

        StartCoroutine(ExecutarTurnoInimigo());
    }
    
    private IEnumerator ExecutarTurnoInimigo()
    {
        estadoAtual = BattleState.TurnoInimigo;

        yield return new WaitForSeconds(1f);

        acaoInimigo=inimigoEscolhido.actions[indiceAcaoInimigo];
        acaoInimigo.execute(this,inimigo,jogador);
       
        if (VerFimCombate())
            yield break;
        
        //avança no vetor de ações
        indiceAcaoInimigo =(indiceAcaoInimigo + 1) % maxIndiceAcaoInimigo;

        

        yield return new WaitForSeconds(1f);

        IniciarTurnoJogador();
    }

    public void JogarCarta(CardData carta)
    {
        if (estadoAtual != BattleState.InicioTurnoJogador)
        {
            return;
        }

        if (carta == null || jogador == null || inimigo == null)
            return;

        if (energiaAtual < carta.custo)
            return;

        energiaAtual -= carta.custo;
        
        //logica de descarte da carta
        Deckjogador.Remove(carta);
        Descartes.Add(carta);

        foreach (CardEffect efeito in carta.effects)
        {
            efeito.execute(this, jogador, inimigo);
        }
        
        
        if (VerFimCombate())
            return;
    }



    private bool VerFimCombate()
    {
        if (inimigo.vidaAtual <= 0)
        {
            estadoAtual = BattleState.Vitoria;
            print("Vitoria!");
            //ao vencer a batalha finaliza o no e carrega a cena do mapa novamente
            //Core.GameManager.Gman.FinalizaNo(GameManager.Run.noAtual,jogador.vidaAtual, 50); atualiza o game manager
            //SceneManager.LoadScene("Map");//volta pro mapa
            return true;
            
        }else if (jogador.vidaAtual <= 0)
        {
            estadoAtual = BattleState.Derrota;
            print("Derrota!");
            // mostra mensagem de derrota na tela e devolve pro menu principal
            return true;
        }
        else
        {
            return false;
        }
    }

   


}