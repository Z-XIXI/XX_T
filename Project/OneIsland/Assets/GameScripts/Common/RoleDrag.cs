using UnityEngine;
using UnityEngine.EventSystems;

public class RoleDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 offset; // 偏移量
    private Camera mainCamera; // 主摄像机

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 计算初始偏移量
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(eventData.position);
        offset = transform.position - mouseWorldPos;
        // 注意：保持Z轴不变
        offset.z = 0;
        Debug.Log(offset.ToString());
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 根据鼠标移动更新位置
        Vector3 newPos = mainCamera.ScreenToWorldPoint(eventData.position) + offset;
        newPos.z = transform.position.z; // 保持原有的Z深度
        transform.position = newPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 拖拽结束时的处理（例如：检查放置位置是否有效）
        //Debug.Log("Drag ended!");
    }
}