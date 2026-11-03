using System.Collections.Generic;
using Core;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField] private List<EventData> eventosPossiveis;
    [SerializeField] private EventVisual eventVisual;
    public EventData eventoEscolhido { get; private set; }
    private GameManager Gman;
    private bool eventoFinalizado = false;

    private void Awake()
    {
        Gman = GameManager.Gman;
    }

    private void Start()
    {
        SortearEvento();
    }

    private void SortearEvento()
    {
        if (eventosPossiveis == null || eventosPossiveis.Count == 0)
        {
            return;
        }

        int indice = Random.Range(0, eventosPossiveis.Count);

        eventoEscolhido = eventosPossiveis[indice];

        eventVisual.ExibirEvento(eventoEscolhido, this);
    }

    public void EscolherOpcao(OptionData opcao)
    {
        if (eventoFinalizado || opcao == null)
            return;

        if (eventoEscolhido == null ||
            !eventoEscolhido.opcoes.Contains(opcao))
            return;

        eventoFinalizado = true;

        opcao.execute(Gman);

        GameManager.Gman.FinalizaNo(
            GameManager.Run.noAtual,
            GameManager.Run.HP,
            GameManager.Run.gold
        );
    }
}