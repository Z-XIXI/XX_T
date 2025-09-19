using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Raycast Target Only")]
[RequireComponent(typeof(CanvasRenderer))]
public class UIRaycastTarget : Graphic
{
    /// <summary>
    /// ��д���ʻ�ȡ������null������Ⱦ
    /// </summary>
    public override Material material
    {
        get => null;
        set { }
    }

    /// <summary>
    /// ��д�����ʻ�ȡ
    /// </summary>
    public override Material defaultMaterial => null;

    /// <summary>
    /// ���ö�������Ϊ�գ�������Ⱦ
    /// </summary>
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear(); // ������ж��㣬����Ⱦ�κ�����
    }

    /// <summary>
    /// ȷ�����߼��ʼ������
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        raycastTarget = true; // ǿ���������߼��
    }

    /// <summary>
    /// �ڱ༭��������ʱ����
    /// </summary>
    //protected override void Reset()
    //{
    //    base.Reset();
    //    raycastTarget = true;
    //    color = new Color(0, 0, 0, 0); // ��ȫ͸��
    //}
}