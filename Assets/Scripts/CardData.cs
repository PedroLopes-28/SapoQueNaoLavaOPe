using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Nova_Carta", menuName = "Cards/CardData")]
public class CardData : ScriptableObject
{
    public string id;
    public Sprite sprite;
    public cardRarity rarity;
    public int custo;
    
    
    public List<CardEffect> effects;

}

public enum cardRarity
{
    Normal,
    Legendary,
    Cursed,
}