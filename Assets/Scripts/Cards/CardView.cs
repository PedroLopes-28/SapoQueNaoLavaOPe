using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{
    [SerializeField] private TMP_Text custoText;
    [SerializeField] private TMP_Text descricaoText;
    [SerializeField] private Button botao;

    private CardData carta;
    private BattleManager battleManager;

    public void Inicializar(CardData dados, BattleManager manager)
    {
        carta = dados;
        battleManager = manager;
        
        custoText.text = carta.custo.ToString();
        descricaoText.text = carta.desc;

        botao.onClick.RemoveAllListeners();
        botao.onClick.AddListener(Jogar);
    }

    private void Jogar()
    {
        battleManager.JogarCarta(carta);
    }
}