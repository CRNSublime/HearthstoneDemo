using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gradient : BaseMeshEffect
{
    public Color32 startColor = Color.white;
    public Color32 endColor = Color.white;
    [Range(-1,1)]
    public float transitionValue = 0.5f;
    //[SerializeField]
    public enum Direction
    {
        Horizontal,
        Vertical,
    }

    public Direction direction;

    private Gradient gradient = new Gradient();
    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
            return;
        if (vh.currentVertCount == 0)
            return;

        int vertexCount = vh.currentVertCount;
        List<UIVertex> vertexs = new List<UIVertex>();
        for (int i = 0; i < vertexCount; i++)
        {
            UIVertex v = new UIVertex();
            vh.PopulateUIVertex(ref v, i);
            vertexs.Add(v);
        }

        switch (direction)
        {
            case Direction.Horizontal:
                float top_x = vertexs[0].position.x;
                float bot_x = top_x;

                for (int i = 1; i < vertexCount; i++)
                {
                    float cur_x = vertexs[i].position.x;
                    if (top_x < cur_x)
                        top_x = cur_x;
                    else if (bot_x > cur_x)
                        bot_x = cur_x;
                }

                float offest_x = top_x - bot_x;
                for (int i = 0; i < vertexCount; i++)
                {
                    UIVertex v = vertexs[i];
                    v.color = Color.Lerp(startColor, endColor, ((v.position.x - bot_x) / offest_x) - transitionValue);
                    vh.SetUIVertex(v, i);
                }
                break;
            case Direction.Vertical:
                float top_y = vertexs[0].position.y;
                float bot_y = top_y;

                for (int i = 1; i < vertexCount; i++)
                {
                    float cur_y = vertexs[i].position.y;
                    if (top_y < cur_y)
                        top_y = cur_y;
                    else if (bot_y > cur_y)
                        bot_y = cur_y;
                }

                float offest_y = top_y - bot_y;
                for (int i = 0; i < vertexCount; i++)
                {
                    UIVertex v = vertexs[i];
                    v.color = Color.Lerp(startColor, endColor, ((v.position.y - bot_y) / offest_y) - transitionValue);
                    vh.SetUIVertex(v, i);
                }
                break;
            default:
                break;
        }
        
    }
}
