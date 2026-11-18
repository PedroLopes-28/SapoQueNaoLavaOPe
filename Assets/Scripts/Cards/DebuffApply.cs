using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Cards/Effects/Debuff")]
public class DebuffApply : CardEffect
{
    [SerializeField] public DebuffEffect debuff;

    public override void execute(
        BattleManager battle,
        BattleUnit usuario,
        BattleUnit alvo
    )
    {
        DebuffEffect d = debuff;
        alvo.debuffs.Add(d);
    }
}