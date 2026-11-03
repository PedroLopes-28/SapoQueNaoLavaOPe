
using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

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

    [SerializeField] private BattleUnitVisual jogadorView;
    [SerializeField] private BattleUnitVisual inimigoView;
    [SerializeField] private VictoryUI victoryUI;
    [SerializeField] private DefeatUI defeatUI;

    [SerializeField] private int nroCartasMao = 5;

    private List<CardData> Deckjogador = new List<CardData>();
    private List<CardData> MaoJogador = new List<CardData>();
    private List<CardData> Descartes = new List<CardData>();

    public int QtdDeck => Deckjogador.Count;
    public int QtdMao => MaoJogador.Count;
    public int QtdDescartes => Descartes.Count;

    public int VelocidadeAtual = 3;
    public int MaxVelocidade = 3;

    public IReadOnlyList<CardData> CartasNaMao => MaoJogador;

    public event Action OnCartasAtualizadas;

    public BattleState estadoAtual { get; private set; }

    private void Awake()
    {
        // Reinicia os dados locais da batalha.
        MaoJogador.Clear();
        Descartes.Clear();

        indiceAcaoInimigo = 0;
        VelocidadeAtual = MaxVelocidade;

        // Recupera a vida atual do jogador.
        jogador = new BattleUnit(
            GameManager.Run.MaxHP,
            GameManager.Run.HP
        );

        // Escolhe um inimigo aleatório.
        inimigoEscolhido = inimigosP[
            UnityEngine.Random.Range(0, inimigosP.Count)
        ];

        inimigo = new BattleUnit(
            inimigoEscolhido.maxHP,
            inimigoEscolhido.HP
        );

        maxIndiceAcaoInimigo = inimigoEscolhido.actions.Count;

        // Cria uma cópia do deck original da run.
        // Assim, as alterações durante a batalha não afetam o deck permanente.
        Deckjogador = new List<CardData>(
            GameManager.Gman.deckJogador
        );

        if (Deckjogador.Count == 0)
        {
            Debug.LogError("O deck está vazio!");
            return;
        }

        Embaralhar(Deckjogador);
    }

    private void Start()
    {
        jogadorView.Inicializar(jogador);
        inimigoView.Inicializar(inimigo);

        IniciarTurnoJogador();
    }

    private void Embaralhar(List<CardData> cartas)
    {
        for (int i = cartas.Count - 1; i > 0; i--)
        {
            int j = UnityEngine.Random.Range(0, i + 1);

            (cartas[i], cartas[j]) = (cartas[j], cartas[i]);
        }
    }

    public void JogarCarta(CardData carta)
    {
        if (estadoAtual != BattleState.TurnoJogador)
            return;

        if (carta == null || jogador == null || inimigo == null)
            return;

        if (!MaoJogador.Contains(carta))
            return;

        if (VelocidadeAtual < carta.custo)
            return;

        VelocidadeAtual -= carta.custo;

        foreach (CardEffect efeito in carta.effects)
        {
            efeito.execute(this, jogador, inimigo);
        }

        MaoJogador.Remove(carta);
        Descartes.Add(carta);

        jogadorView.AtualizarVida();
        inimigoView.AtualizarVida();

        OnCartasAtualizadas?.Invoke();

        if (VerFimCombate())
            return;
    }

    private void ComprarCartas(int qtd)
    {
        for (int i = 0; i < qtd; i++)
        {
            if (Deckjogador.Count == 0)
            {
                if (Descartes.Count == 0)
                    break;

                Deckjogador.AddRange(Descartes);
                Descartes.Clear();

                Embaralhar(Deckjogador);
            }

            CardData card = Deckjogador[0];

            Deckjogador.RemoveAt(0);
            MaoJogador.Add(card);
        }

        OnCartasAtualizadas?.Invoke();
    }

    private void IniciarTurnoJogador()
    {
        jogador.bloqueio = 0;

        jogadorView.AtualizarVida();

        VelocidadeAtual = MaxVelocidade;

        estadoAtual = BattleState.InicioTurnoJogador;

        ComprarCartas(nroCartasMao);

        estadoAtual = BattleState.TurnoJogador;
    }

    public void FimTurnoJogador()
    {
        print("Fim turno jogador!");

        if (estadoAtual != BattleState.TurnoJogador)
            return;

        estadoAtual = BattleState.FimTurnoJogador;

        Descartes.AddRange(MaoJogador);
        MaoJogador.Clear();

        OnCartasAtualizadas?.Invoke();

        if (VerFimCombate())
            return;

        StartCoroutine(ExecutarTurnoInimigo());
    }

    private IEnumerator ExecutarTurnoInimigo()
    {
        inimigo.bloqueio = 0;

        inimigoView.AtualizarVida();

        print("Turno inimigo!");

        estadoAtual = BattleState.TurnoInimigo;

        yield return new WaitForSeconds(1f);

        acaoInimigo = inimigoEscolhido.actions[indiceAcaoInimigo];

        print(acaoInimigo);

        acaoInimigo.execute(this, inimigo, jogador);

        jogadorView.AtualizarVida();
        inimigoView.AtualizarVida();

        if (VerFimCombate())
            yield break;

        indiceAcaoInimigo =
            (indiceAcaoInimigo + 1) % maxIndiceAcaoInimigo;

        yield return new WaitForSeconds(1f);

        IniciarTurnoJogador();
    }

    private bool VerFimCombate()
    {
        if (inimigo.vidaAtual <= 0)
        {
            if (estadoAtual == BattleState.Vitoria)
                return true;

            estadoAtual = BattleState.Vitoria;

            GameManager.Run.HP = jogador.vidaAtual;

            print("Vitoria!");

            victoryUI.MostrarVictory();

            return true;
        }
        else if (jogador.vidaAtual <= 0)
        {
            estadoAtual = BattleState.Derrota;

            print("Derrota!");

            defeatUI.MostraDefeat();

            return true;
        }

        return false;
    }
    
    public void VoltarAoMapa()
    {

        GameManager.Gman.FinalizaNo(
            GameManager.Run.noAtual,
            jogador.vidaAtual,
            GameManager.Run.gold
        );
    }
}
