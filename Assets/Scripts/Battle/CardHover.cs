using UnityEngine;
using UnityEngine.EventSystems;

public class CardHover : MonoBehaviour,
    IPointerEnterHandler,
    IPointerExitHandler
{
    private HandFanLayout handFan;

    public Vector3 EscalaBase { get; private set; }

    private void Awake()
    {
        EscalaBase = transform.localScale;
    }

    private void Start()
    {
        handFan = GetComponentInParent<HandFanLayout>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (handFan != null)
            handFan.Destacar(this);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (handFan != null)
            handFan.RemoverDestaque(this);
    }
}