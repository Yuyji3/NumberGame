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

    //    // 1. placeholder ���� (�� ���� ������)
    //    placeholder = new GameObject("Placeholder", typeof(RectTransform));
    //    placeholder.transform.SetParent(originalParent);
    //    placeholder.transform.SetSiblingIndex(originalSiblingIndex);

    //    // LayoutElement �߰��ؼ� ���̾ƿ� �ڸ� ����
    //    LayoutElement layout = placeholder.AddComponent<LayoutElement>();
    //    LayoutElement currentLayout = GetComponent<LayoutElement>();
    //    if (currentLayout != null)
    //    {
    //        layout.preferredWidth = currentLayout.preferredWidth;
    //        layout.preferredHeight = currentLayout.preferredHeight;
    //        layout.flexibleWidth = 0;
    //        layout.flexibleHeight = 0;
    //    }

    //    //2.�巡�׿� �θ�� �̵�, ���̾� �켱
    //    transform.SetParent(dragParent);
    //    transform.SetAsLastSibling();
    //    rectTransform.localScale = originalScale * 0.9f;


    //    // ���콺 offset ���
    //    RectTransformUtility.ScreenPointToLocalPointInRectangle(
    //        canvas.transform as RectTransform,
    //        eventData.position,
    //        canvas.worldCamera,
    //        out var localPointerPos
    //    );
    //    pointerOffset = rectTransform.anchoredPosition - localPointerPos;
        

    //}
    public Canvas dragCanvas; // �巡�� ���� ĵ����

    public void OnPointerDown(PointerEventData eventData)
    {
        originalScale = rectTransform.localScale;
        originalParent = transform.parent;
        originalSiblingIndex = transform.GetSiblingIndex();

        // 1. placeholder ����
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

        // 2. �巡�� ĵ������ �ű�� �켱���� ����
        transform.SetParent(dragCanvas.transform); // ���� ���� ���̵���
        transform.SetAsLastSibling(); // �� ȣ��!

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
        // 3. placeholder �ڸ��� ����
        transform.SetParent(originalParent);
        transform.SetSiblingIndex(placeholder.transform.GetSiblingIndex());
        rectTransform.localScale = originalScale;

        // placeholder ����
        Destroy(placeholder);
    }
}
