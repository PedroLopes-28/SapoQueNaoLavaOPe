using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "CardDatabase", menuName = "CardDatabase/CardDatabase")]
public class CardDatabase : ScriptableObject
{
    public List<CardData> cards = new List<CardData>();
    
    public CardData buscaCard(string id)
    {
        return cards.Find(x => x.id == id);// retorna carta com base no id
    }
    
    public List<CardData> buscaTodas()//retorna todas as cartas do deck
    {
        return cards;
    }

    public List<CardData> pegarAleatorio(int qtd)// recebe um numero e retorna aquele numero de cartas aleatorias
    {
        
        return cards
            .OrderBy(x => UnityEngine.Random.value)
            .Take(qtd)
            .ToList();
    }
}
