using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TileDrag : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector3 originalScale;
    private Vector2 pointerOffset;

    public Transform dragParent;
    private Transform originalParent;
    private GameObject placeholder;
    private int originalSiblingIndex;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    originalScale = rectTransform.localScale;
    //    originalParent = transform.parent;
    //    originalSiblingIndex = transform.GetSiblingIndex();

    //    // 1. placeholder 생성 (빈 공간 유지용)
    //    placeholder = new GameObject("Placeholder", typeof(RectTransform));
    //    placeholder.transform.SetParent(originalParent);
    //    placeholder.transform.SetSiblingIndex(originalSiblingIndex);

    //    // LayoutElement 추가해서 레이아웃 자리 유지
    //    LayoutElement layout = placeholder.AddComponent<LayoutElement>();
    //    LayoutElement currentLayout = GetComponent<LayoutElement>();
    //    if (currentLayout != null)
    //    {
    //        layout.preferredWidth = currentLayout.preferredWidth;
    //        layout.preferredHeight = currentLayout.preferredHeight;
    //        layout.flexibleWidth = 0;
    //        layout.flexibleHeight = 0;
    //    }

    //    //2.드래그용 부모로 이동, 레이어 우선
    //    transform.SetParent(dragParent);
    //    transform.SetAsLastSibling();
    //    rectTransform.localScale = originalScale * 0.9f;


    //    // 마우스 offset 계산
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(
    //        canvas.transform as RectTransform,
    //        eventData.position,
    //        canvas.worldCamera,
    //        out var localPointerPos
    //    );
    //    pointerOffset = rectTransform.anchoredPosition - localPointerPos;
        

    //}
    public Canvas dragCanvas; // 드래그 전용 캔버스

    public void OnPointerDown(PointerEventData eventData)
    {
        originalScale = rectTransform.localScale;
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();

        // 1. placeholder 생성
        placeholder = new GameObject("Placeholder", typeof(RectTransform));
        placeholder.transform.SetParent(originalParent);
        placeholder.transform.SetSiblingIndex(originalSiblingIndex);

        LayoutElement layout = placeholder.AddComponent<LayoutElement>();
        LayoutElement currentLayout = GetComponent<LayoutElement>();
        if (currentLayout != null)
        {
            layout.preferredWidth = currentLayout.preferredWidth;
            layout.preferredHeight = currentLayout.preferredHeight;
            layout.flexibleWidth = 0;
            layout.flexibleHeight = 0;
        }

        // 2. 드래그 캔버스로 옮기고 우선순위 조정
        transform.SetParent(dragCanvas.transform); // 가장 위에 보이도록
        transform.SetAsLastSibling(); // 꼭 호출!

        rectTransform.localScale = originalScale * 0.9f;

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
            rectTransform.anchoredPosition = localPoint + pointerOffset;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // 3. placeholder 자리로 복귀
        transform.SetParent(originalParent);
        transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        rectTransform.localScale = originalScale;

        // placeholder 제거
        Destroy(placeholder);
    }
}
