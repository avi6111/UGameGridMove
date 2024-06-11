using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 字跳动效果
/// </summary>
[AddComponentMenu("UI/Effects/FontBeat")]
public class FontBeat : BaseMeshEffect
{
    [SerializeField] int StartIndex;
    [SerializeField] int EndIndex;

    [SerializeField] float Speed;
    [SerializeField] float Density;
    [SerializeField] float Magnitude;

    private Text m_text;
    List<UIVertex> vertices = new List<UIVertex>();
    List<UIVertex> newVerts = new List<UIVertex>();

    public override void ModifyMesh(VertexHelper vh)
    {
        vertices.Clear();
        newVerts.Clear();
        vh.GetUIVertexStream(vertices);

        for (int i = 0; i < (vertices.Count / 6); i++)
        {
            if (i >= StartIndex && i < EndIndex)
            {
                newVerts.AddRange(Characters(vertices.GetRange(i * 6, 6), i));
            }
            else
            {
                newVerts.AddRange(vertices.GetRange(i * 6, 6));
            }
        }

        vh.Clear();
        vh.AddUIVertexTriangleStream(newVerts);
    }

    protected override void Start()
    {
        m_text = GetComponent<Text>();
        base.Start();
    }

    List<UIVertex> Characters(List<UIVertex> verts, int characterindex)
    {
        for (int i = 0; i < verts.Count; i++)
        {
            UIVertex c = verts[i];
            c.position = c.position + new Vector3(1, Magnitude * Mathf.Sin((Time.timeSinceLevelLoad * Speed) + (characterindex * Density)), 1);
            verts[i] = c;
        }

        return verts;
    }

    void Update()
    {
        UpdateBeat();
    }

    public void UpdateBeat()
    {
        m_text.SetVerticesDirty();
    }
}