using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Unity.VisualScripting;

public class RecyclableScrollRect : UIBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    #region ��������

    [Header("��������")]
    [SerializeField] private ScrollDirection direction = ScrollDirection.Vertical;
    [SerializeField] private float spacing = 0f;
    [SerializeField] private RectOffset padding;

    [Header("Ԥ�Ƽ�")]
    [SerializeField] private GameObject itemPrefab;

    [Header("����")]
    [SerializeField] private int dataCount;

    // ����ö��
    public enum ScrollDirection
    {
        Vertical,
        Horizontal
    }

    // ������������
    public int DataCount
    {
        get => dataCount;
        set
        {
            dataCount = value;
            Refresh();
        }
    }

    #endregion

    #region ˽���ֶ�

    private ScrollRect scrollRect;
    private RectTransform viewport;
    private RectTransform content;

    private LinkedList<RectTransform> activeItems = new LinkedList<RectTransform>();
    private Dictionary<int, LinkedListNode<RectTransform>> cachedItems = new Dictionary<int, LinkedListNode<RectTransform>>();
    private Queue<RectTransform> itemPool = new Queue<RectTransform>();

    private Vector2 itemSize;
    private int firstVisibleIndex = -1;
    private int lastVisibleIndex = -1;

    private bool isDragging;
    private Vector2 lastContentPosition;

    /// <summary>
    /// item���»ص�����ί��
    /// </summary>
    /// <param name="item">�ӽڵ����</param>
    /// <param name="rowIndex">����</param>
    public delegate void ItemDelegate(RectTransform item, int rowIndex);

    /// <summary>
    /// item���»ص�����ί��
    /// </summary>
    public ItemDelegate ItemCallback;

    #endregion

    #region Unity��������

    protected override void Awake()
    {
        base.Awake();

        scrollRect = GetComponent<ScrollRect>();
        viewport = scrollRect.viewport;
        content = scrollRect.content;

        // ���ݷ�������ScrollRect
        if (direction == ScrollDirection.Vertical)
        {
            scrollRect.vertical = true;
            scrollRect.horizontal = false;
            content.anchorMin = new Vector2(0, 1);
            content.anchorMax = new Vector2(1, 1);
            content.pivot = new Vector2(0.5f, 1);
        }
        else
        {
            scrollRect.vertical = false;
            scrollRect.horizontal = true;
            content.anchorMin = new Vector2(0, 0);
            content.anchorMax = new Vector2(0, 1);
            content.pivot = new Vector2(0, 0.5f);
        }

        // ��ȡԤ�Ƽ���С
        if (itemPrefab != null)
        {
            RectTransform prefabRect = itemPrefab.GetComponent<RectTransform>();
            itemSize = prefabRect.rect.size;
        }
    }

    protected override void Start()
    {
        base.Start();
        Initialize();
    }

    protected override void OnRectTransformDimensionsChange()
    {
        base.OnRectTransformDimensionsChange();
        if (gameObject.activeInHierarchy)
        {
            Refresh();
        }
    }

    private void Update()
    {
        if (content.hasChanged)
        {
            content.hasChanged = false;
            UpdateItems();
        }
    }

    #endregion

    #region ��ʼ��

    private void Initialize()
    {
        // �����������
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        activeItems.Clear();
        cachedItems.Clear();
        itemPool.Clear();

        // �������ݴ�С
        UpdateContentSize();

        // ��ʼ��������
        UpdateVisibleItems();
    }

    #endregion

    #region ��������

    public void Refresh()
    {
        Initialize();
    }

    public void ScrollToIndex(int index, float duration = 0.5f)
    {
        if (index < 0 || index >= dataCount) return;

        float normalizedPosition = 0f;

        if (direction == ScrollDirection.Vertical)
        {
            float contentHeight = content.rect.height;
            float viewportHeight = viewport.rect.height;
            float itemPosition = index * (itemSize.y + spacing) + padding.top;

            normalizedPosition = 1 - (itemPosition + itemSize.y / 2) / (contentHeight - viewportHeight);
        }
        else
        {
            float contentWidth = content.rect.width;
            float viewportWidth = viewport.rect.width;
            float itemPosition = index * (itemSize.x + spacing) + padding.left;

            normalizedPosition = (itemPosition + itemSize.x / 2) / (contentWidth - viewportWidth);
        }

        normalizedPosition = Mathf.Clamp01(normalizedPosition);

        if (direction == ScrollDirection.Vertical)
        {
            scrollRect.verticalNormalizedPosition = normalizedPosition;
        }
        else
        {
            scrollRect.horizontalNormalizedPosition = normalizedPosition;
        }
    }

    #endregion

    #region ��ק�¼�

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ��ʵ�֣�ScrollRect�Ѿ���������ק
    }

    #endregion

    #region �����߼�

    private void UpdateContentSize()
    {
        if (direction == ScrollDirection.Vertical)
        {
            float height = padding.vertical + dataCount * itemSize.y + Mathf.Max(0, dataCount - 1) * spacing;
            content.sizeDelta = new Vector2(content.sizeDelta.x, height);
        }
        else
        {
            float width = padding.horizontal + dataCount * itemSize.x + Mathf.Max(0, dataCount - 1) * spacing;
            content.sizeDelta = new Vector2(width, content.sizeDelta.y);
        }
    }

    private void UpdateVisibleItems()
    {
        if (dataCount == 0)
        {
            ClearAllItems();
            return;
        }

        // ������ӷ�Χ
        int newFirstIndex = CalculateFirstVisibleIndex();
        int newLastIndex = CalculateLastVisibleIndex(newFirstIndex);

        // ���û�л�������ʼ��
        if (activeItems.Count == 0 && newFirstIndex >= 0)
        {
            for (int i = newFirstIndex; i <= newLastIndex; i++)
            {
                CreateItem(i);
            }

            firstVisibleIndex = newFirstIndex;
            lastVisibleIndex = newLastIndex;
            return;
        }

        // �������û�б仯������Ҫ����
        if (newFirstIndex == firstVisibleIndex && newLastIndex == lastVisibleIndex)
            return;

        // �Ƴ����ӷ�Χ֮�����
        while (activeItems.Count > 0 && firstVisibleIndex < newFirstIndex)
        {
            RecycleItem(firstVisibleIndex);
            firstVisibleIndex++;
        }

        while (activeItems.Count > 0 && lastVisibleIndex > newLastIndex)
        {
            RecycleItem(lastVisibleIndex);
            lastVisibleIndex--;
        }

        // ����½�����ӷ�Χ����
        while (firstVisibleIndex > newFirstIndex)
        {
            firstVisibleIndex--;
            CreateItem(firstVisibleIndex);
        }

        while (lastVisibleIndex < newLastIndex)
        {
            lastVisibleIndex++;
            CreateItem(lastVisibleIndex);
        }
    }

    private int CalculateFirstVisibleIndex()
    {
        if (direction == ScrollDirection.Vertical)
        {
            float contentY = content.anchoredPosition.y;
            float visibleTop = Mathf.Max(0, contentY - padding.top);
            return Mathf.FloorToInt(visibleTop / (itemSize.y + spacing));
        }
        else
        {
            float contentX = content.anchoredPosition.x;
            float visibleLeft = Mathf.Max(0, -contentX - padding.left);
            return Mathf.FloorToInt(visibleLeft / (itemSize.x + spacing));
        }
    }

    private int CalculateLastVisibleIndex(int firstIndex)
    {
        if (firstIndex < 0) return -1;

        int maxIndex = dataCount - 1;

        if (direction == ScrollDirection.Vertical)
        {
            float contentY = content.anchoredPosition.y;
            float viewportHeight = viewport.rect.height;
            float visibleBottom = contentY + viewportHeight;
            int lastIndex = Mathf.FloorToInt((visibleBottom - padding.top) / (itemSize.y + spacing));
            return Mathf.Min(lastIndex, maxIndex);
        }
        else
        {
            float contentX = content.anchoredPosition.x;
            float viewportWidth = viewport.rect.width;
            float visibleRight = -contentX + viewportWidth;
            int lastIndex = Mathf.FloorToInt((visibleRight - padding.left) / (itemSize.x + spacing));
            return Mathf.Min(lastIndex, maxIndex);
        }
    }

    private void CreateItem(int index)
    {
        if (index < 0 || index >= dataCount) return;

        RectTransform item = GetItemFromPool();
        SetupItem(item, index);

        var node = activeItems.AddLast(item);
        cachedItems.Add(index, node);
    }

    private void RecycleItem(int index)
    {
        if (cachedItems.TryGetValue(index, out var node))
        {
            ReturnItemToPool(node.Value);
            activeItems.Remove(node);
            cachedItems.Remove(index);
        }
    }

    private void SetupItem(RectTransform item, int index)
    {
        item.gameObject.SetActive(true);
        item.name = $"Item_{index}";
        if (ItemCallback == null)
        {
            Debug.Log("RecyclableScrollRect is missing an ItemCallback, cannot function", this);
            return;
        }
        else
        {
            ItemCallback(item, index);
        }

        // ����λ��
        Vector2 position = CalculateItemPosition(index);
        item.anchoredPosition = position;

        // �������������������ݵ��߼�
        // ����: item.GetComponent<ListItem>().SetData(GetData(index));
    }

    private Vector2 CalculateItemPosition(int index)
    {
        if (direction == ScrollDirection.Vertical)
        {
            float y = -padding.top - index * (itemSize.y + spacing);
            return new Vector2(padding.left, y);
        }
        else
        {
            float x = padding.left + index * (itemSize.x + spacing);
            return new Vector2(x, -padding.top);
        }
    }

    private RectTransform GetItemFromPool()
    {
        RectTransform item;
        if (itemPool.Count > 0)
        {
            item = itemPool.Dequeue();
        }
        else
        {
            item = Instantiate(itemPrefab, content).GetComponent<RectTransform>();
        }

        return item;
    }

    private void ReturnItemToPool(RectTransform item)
    {
        item.gameObject.SetActive(false);
        itemPool.Enqueue(item);
    }

    private void ClearAllItems()
    {
        while (activeItems.Count > 0)
        {
            ReturnItemToPool(activeItems.First.Value);
            activeItems.RemoveFirst();
        }

        cachedItems.Clear();
        firstVisibleIndex = -1;
        lastVisibleIndex = -1;
    }

    private void UpdateItems()
    {
        if (!isDragging && content.anchoredPosition == lastContentPosition)
            return;

        lastContentPosition = content.anchoredPosition;
        UpdateVisibleItems();
    }

    #endregion
}
public class BaseListItem
{
    private Dictionary<string, GameObject> nodeList;
    protected Dictionary<string, GameObject> NodeList
    {
        get { return nodeList; }
        set { nodeList = value; }
    }
    public BaseListItem(RectTransform item)
    {
        UINameTable uiNameTab = item.GetComponent<UINameTable>();
        if (null != uiNameTab)
        {
            NodeList = uiNameTab.ItemDic;
        }
    }
}