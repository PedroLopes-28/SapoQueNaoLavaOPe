using Core;
using UnityEngine;

[CreateAssetMenu(fileName = "OptionData", menuName = "Scriptable Objects/OptionData")]
public abstract class OptionData : ScriptableObject
{
    [SerializeField] public string descricao;
    public abstract void execute(GameManager manager);
}
