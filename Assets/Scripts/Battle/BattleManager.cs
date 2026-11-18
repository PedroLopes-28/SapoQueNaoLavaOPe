
using System;
using System.Collections;
using System.Collections.Generic;
using Core;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    public enum BattleState
    {
        AntesTurnoJogador,
        TurnoJogador,
        DepoisTurnoJogador,

        AntesTurnoInimigo,
        TurnoInimigo,
        DepoisTurnoInimigo,

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
    [SerializeField] private BattleUI battleUI;

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

    
    private bool cartaEmExecucao = false;

    public void JogarCarta(CardData carta)
    {
        if (estadoAtual != BattleState.TurnoJogador)
            return;

        if (cartaEmExecucao)
            return;

        if (carta == null || jogador == null || inimigo == null)
            return;

        if (!MaoJogador.Contains(carta))
            return;

        if (VelocidadeAtual < carta.custo)
            return;

        StartCoroutine(ExecutarCarta(carta));
    }

    private IEnumerator ExecutarCarta(CardData carta)
    {
        cartaEmExecucao = true;

        // Consome a energia da carta
        VelocidadeAtual -= carta.custo;

        // Inicia a animação do jogador
        jogadorView.AnimarEfeito(carta.corAnimacao);

        // Aguarda o momento do impacto
        yield return new WaitForSeconds(0.5f);

        // Executa os efeitos da carta
        foreach (CardEffect efeito in carta.effects)
        {
            efeito.execute(this, jogador, inimigo);
        }

        // Atualiza os visuais
        jogadorView.AtualizarVida();
        inimigoView.AtualizarVida();

        // Anima o inimigo recebendo o efeito
        inimigoView.AnimarDano();

        // Aguarda um pouco para mostrar o resultado
        yield return new WaitForSeconds(0.2f);

        // Remove a carta da mão
        MaoJogador.Remove(carta);
        Descartes.Add(carta);

        OnCartasAtualizadas?.Invoke();

        if (!VerFimCombate())
        {
            battleUI.atualizaFan();
        }

        cartaEmExecucao = false;
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
        
        AntesTurnoJogador();

        ComprarCartas(nroCartasMao);

        estadoAtual = BattleState.TurnoJogador;
    }
    
    private bool AntesTurnoJogador()
    {
        estadoAtual = BattleState.AntesTurnoJogador;

        jogador.ProcessarInicioTurno();

        jogadorView.AtualizarVida();
        inimigoView.AtualizarVida();
        jogadorView.AtualizarDebuffs();
        inimigoView.AtualizarDebuffs();

        return VerFimCombate();
    }

    public void FimTurnoJogador()
    {
        print("Fim turno jogador!");

        if (estadoAtual != BattleState.TurnoJogador)
            return;

        DepoisTurnoJogador();

        Descartes.AddRange(MaoJogador);
        MaoJogador.Clear();

        OnCartasAtualizadas?.Invoke();

        if (VerFimCombate())
            return;

        StartCoroutine(ExecutarTurnoInimigo());
    }
    
    private bool DepoisTurnoJogador()
    {
        estadoAtual = BattleState.DepoisTurnoJogador;

        jogador.ProcessarFimTurno();

        jogadorView.AtualizarVida();
        inimigoView.AtualizarVida();

        return VerFimCombate();
    }
    
    private bool AntesTurnoInimigo()
    {
        estadoAtual = BattleState.AntesTurnoInimigo;

        inimigo.ProcessarInicioTurno();

        jogadorView.AtualizarVida();
        inimigoView.AtualizarVida();
        jogadorView.AtualizarDebuffs();
        inimigoView.AtualizarDebuffs();

        return VerFimCombate();
    }


    
    private IEnumerator ExecutarTurnoInimigo()
    {
        // Início do turno do inimigo
        inimigo.bloqueio = 0;
        inimigoView.AtualizarVida();

        Debug.Log("Turno inimigo!");

        // Processa debuffs de início do turno
        if (AntesTurnoInimigo())
            yield break;

        estadoAtual = BattleState.TurnoInimigo;

        yield return new WaitForSeconds(0.5f);

        // Seleciona a ação do inimigo
        acaoInimigo = inimigoEscolhido.actions[indiceAcaoInimigo];

        Debug.Log("Ação inimiga: " + acaoInimigo.name);
        
        inimigoView.AnimarEfeito(acaoInimigo.cor);
        
        yield return new WaitForSeconds(0.5f);

        // Executa a ação do inimigo
        acaoInimigo.execute(this, inimigo, jogador);

        // Atualiza a interface de vida e bloqueio
        jogadorView.AtualizarVida();
        inimigoView.AtualizarVida();
        

        // Anima o jogador recebendo o impacto
        jogadorView.AnimarDano();

        // Verifica se o combate terminou
        if (VerFimCombate())
            yield break;

        // Avança para a próxima ação
        indiceAcaoInimigo =
            (indiceAcaoInimigo + 1) % maxIndiceAcaoInimigo;

        yield return new WaitForSeconds(0.5f);

        // Processa debuffs de fim de turno
        if (DepoisTurnoInimigo())
            yield break;

        // Inicia o próximo turno do jogador
        IniciarTurnoJogador();
    }

    
    private bool DepoisTurnoInimigo()
    {
        estadoAtual = BattleState.DepoisTurnoInimigo;

        inimigo.ProcessarFimTurno();
        

        jogadorView.AtualizarVida();
        inimigoView.AtualizarVida();
        jogadorView.AtualizarDebuffs();
        inimigoView.AtualizarDebuffs();

        return VerFimCombate();
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
