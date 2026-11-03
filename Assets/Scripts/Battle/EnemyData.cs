using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inimigo", menuName = "Inimigo/EnemyData")]
public class EnemyData : ScriptableObject
{

    public string id;
    public int HP;
    public int maxHP;
    [SerializeField] public List<CardEffect> actions;

}
    

