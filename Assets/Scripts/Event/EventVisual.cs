using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EventVisual : MonoBehaviour
{
    [SerializeField] private TMP_Text tituloText;
    [SerializeField] private TMP_Text historiaText;

    [SerializeField] private Transform opcoesArea;
    [SerializeField] private Button optionButtonPrefab;

    public void ExibirEvento(EventData evento, EventManager manager)
    {
        tituloText.text = evento.titulo;
        historiaText.text = evento.história;

        foreach (Transform filho in opcoesArea)
        {
            Destroy(filho.gameObject);
        }

        foreach (OptionData opcao in evento.opcoes)
        {
            Button botao = Instantiate(
                optionButtonPrefab,
                opcoesArea
            );

            TMP_Text texto =
                botao.GetComponentInChildren<TMP_Text>();

            texto.text = opcao.descricao;

            OptionData opcaoSelecionada = opcao;

            botao.onClick.AddListener(
                () => manager.EscolherOpcao(opcaoSelecionada)
            );
        }
    }
}