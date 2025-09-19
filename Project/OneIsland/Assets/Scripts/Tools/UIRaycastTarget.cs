using UnityEngine;
using UnityEngine.UI;

[AddComponentMenu("UI/Raycast Target Only")]
[RequireComponent(typeof(CanvasRenderer))]
public class UIRaycastTarget : Graphic
{
    /// <summary>
    /// 重写材质获取，返回null避免渲染
    /// </summary>
    public override Material material
    {
        get => null;
        set { }
    }

    /// <summary>
    /// 重写主材质获取
    /// </summary>
    public override Material defaultMaterial => null;

    /// <summary>
    /// 设置顶点数据为空，避免渲染
    /// </summary>
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear(); // 清除所有顶点，不渲染任何内容
    }

    /// <summary>
    /// 确保射线检测始终启用
    /// </summary>
    protected override void Awake()
    {
        base.Awake();
        raycastTarget = true; // 强制启用射线检测
    }

    /// <summary>
    /// 在编辑器中重置时设置
    /// </summary>
    //protected override void Reset()
    //{
    //    base.Reset();
    //    raycastTarget = true;
    //    color = new Color(0, 0, 0, 0); // 完全透明
    //}
}