using UnityEngine;
using UnityEngine.EventSystems;

public class RoleDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 offset; // ƫ����
    private Camera mainCamera; // �������

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // �����ʼƫ����
        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(eventData.position);
        offset = transform.position - mouseWorldPos;
        // ע�⣺����Z�᲻��
        offset.z = 0;
        Debug.Log(offset.ToString());
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ��������ƶ�����λ��
        Vector3 newPos = mainCamera.ScreenToWorldPoint(eventData.position) + offset;
        newPos.z = transform.position.z; // ����ԭ�е�Z���
        transform.position = newPos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // ��ק����ʱ�Ĵ������磺������λ���Ƿ���Ч��
        //Debug.Log("Drag ended!");
    }
}