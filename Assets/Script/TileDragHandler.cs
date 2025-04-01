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

        transform.SetParent(dragParent); // 그리드에서 분리
        rectTransform.localScale = originalScale * 1.1f;

        // ?? 마우스 클릭 위치와 타일 중심 사이의 offset 저장
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
            rectTransform.anchoredPosition = localPoint + pointerOffset; // offset 유지하며 따라감
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        transform.SetParent(originalParent); // 원래 자리로
        rectTransform.anchoredPosition = originalPosition;
        rectTransform.localScale = originalScale;
    }
}
