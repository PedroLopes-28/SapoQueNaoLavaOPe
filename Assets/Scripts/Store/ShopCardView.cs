using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopCardView : MonoBehaviour
{
    [SerializeField] private Image cardImage;
    [SerializeField] private TMP_Text custoText;
    [SerializeField] private TMP_Text descricaoText;

    public CardData Carta { get; private set; }

    public void Inicializar(CardData carta)
    {
        if (carta == null)
        {
            Debug.LogError("Carta da loja é nula!");
            return;
        }

        Carta = carta;

        cardImage.sprite = carta.sprite;
        cardImage.preserveAspect = true;

        custoText.text = carta.custo.ToString();
        descricaoText.text = carta.desc;
    }
}