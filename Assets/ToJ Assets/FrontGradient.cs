using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;
/// <summary>
/// 字颜色上下渐变
/// </summary>
[AddComponentMenu("UI/Effects/FontGradient")]
public class FontGradient : BaseMeshEffect
{
    public enum  LerpType
    {
        Normal,
        Line,//这个算法没用，因为 text的顶点是4个，而不是 shader 面片渲染
            //暂没法像 Photoshop 一样做多个 Keys 的 Gradient
    }

    public LerpType lerpType;
    [SerializeField]
    Color32 topColor = Color.white;
    
    [SerializeField]
    Color32 bottomColor = Color.black;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive()) return;
        
        var count = vh.currentVertCount;
        if (count == 0) return;
        
        var vertexs = new List<UIVertex>();
        for (var i = 0; i < count; i++)
        {
            var vertex = new UIVertex();
            vh.PopulateUIVertex(ref vertex, i);
            vertexs.Add(vertex);
        }

        var topY = vertexs[0].position.y;
        var bottomY = vertexs[0].position.y;

        for (var i = 1; i < count; i++)
        {
            var y = vertexs[i].position.y;
            if (y > topY)
            {
                topY = y;
            }
            else if (y < bottomY)
            {
                bottomY = y;
            }
        }

        var height = topY - bottomY;
        var lineHeight = height / 3;
        Debug.LogError("lineHeight=" + lineHeight +" total=" + height +" from " + topY +"-" + bottomY);
        for (var i = 0; i < count; i++)
        {
            var vertex = vertexs[i];
            Color32 color;
            if(lerpType == LerpType.Normal)
                color = Color32.Lerp(bottomColor, topColor, (vertex.position.y - bottomY) / height);
            else
            { //这个逻辑暂时效果错乱。。。   
                Debug.LogError($"curr={vertex.position.y} bottom={bottomY} = {(vertex.position.y - bottomY)% lineHeight}");
                if ((vertex.position.y - bottomY)% lineHeight > 3f)
                    color = Color32.Lerp(bottomColor, topColor, 1);
                else
                    color = Color32.Lerp(bottomColor, topColor, 0.001f);
            }
            vertex.color = color;
            vh.SetUIVertex(vertex, i);
        }
    }
}