using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inimigo", menuName = "Inimigo/EnemyData")]
public class EnemyData : ScriptableObject
{

    public string id;
    public int hp;
    public List<string> CardEffects;
    
    
    public List<CardEffect> effects;

}
    

