using UnityEngine;
using UnityEngine.EventSystems;

public class TileDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector3 originalPosition;
    private Vector3 originalScale;
    private Vector2 pointerOffset;

    public Transform dragParent;
    private Transform originalParent;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        originalScale = rectTransform.localScale;
        originalParent = transform.parent;

        transform.SetParent(dragParent); // �׸��忡�� �и�
        rectTransform.localScale = originalScale * 1.1f;

        // ?? ���콺 Ŭ�� ��ġ�� Ÿ�� �߽� ������ offset ����
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out var localPointerPos
        );
        pointerOffset = rectTransform.anchoredPosition - localPointerPos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            eventData.position,
            canvas.worldCamera,
            out var localPoint))
        {
            rectTransform.anchoredPosition = localPoint + pointerOffset; // offset �����ϸ� ����
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.SetParent(originalParent); // ���� �ڸ���
        rectTransform.anchoredPosition = originalPosition;
        rectTransform.localScale = originalScale;
    }
}
