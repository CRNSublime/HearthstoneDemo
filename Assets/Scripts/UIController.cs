using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : Singleton<UIController> {

    #region  *** 拖拽指示箭头绘制  [ 模拟一条抛物线 简单绘制出箭头mesh 好丑... ]  ***
    float speed = 10f; // 
    Vector3 G = new Vector3(0, 0, 9.8f);
    float fixedTime = 0.05f;   // 间隔时间
    float arrowWidth = 0.4f;  // 箭头宽度
    float verticalWidth = 0.05f; // 箭头间断距离
	private List<Vector3> GetParabolaPos(Vector3 startPos, Vector3 endPos)
    {
        List<Vector3> posList = new List<Vector3>();

        float totalTime = Vector3.Distance(startPos, endPos) / speed;
        Vector3 startSpeed = (endPos - startPos) / totalTime + totalTime * G * 0.5f;
        for (float moveTime = 0; moveTime <= totalTime; moveTime+=fixedTime)
        {
            if (moveTime > totalTime)
                moveTime = totalTime;
            Vector3 tempPos = startPos + (startSpeed * moveTime - 0.5f * G * moveTime * moveTime);
            posList.Add(tempPos);
        }
        return posList;
    }
    // 根据两点位置和摄像机位置 计算三点组成平面的单位垂直向量
    private Vector3 GetVerticalDir(Vector3 startPos, Vector3 endPos)
    {
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 dir = endPos - startPos;
        Vector3 cdir = endPos - cameraPos;
        return Vector3.Cross(dir, cdir).normalized;
    }

    private void CreateArrow(MeshFilter meshFilter, List<Vector3> posList)
    {
        int count = posList.Count - 1;
        if (count < 1)
            return;
        float halfWidth = arrowWidth / 2;
        Vector3 dir = GetVerticalDir(posList[0], posList[count]);  // 获得横向扩展面的方向 目的是为了保持绘制的三角面一直在同一个面 保证箭头一直在视野内

        Vector3[] vertices = new Vector3[count * 4 + 7];
        Vector2[] uv = new Vector2[count * 4 + 7];
        int[] triangles = new int[count * 6 + 9];

        Vector3 vdir = (posList[1] - posList[0]).normalized;  // 箭头绘制所在平面的单位向量 用来移动顶点位置做间隔
        for (int i = 0; i < count; i++)
        {
            vertices[i * 4 + 0] = posList[i] + dir * halfWidth + vdir * verticalWidth * i;
            vertices[i * 4 + 1] = posList[i + 1] - dir * halfWidth + vdir * verticalWidth * i;
            vertices[i * 4 + 2] = posList[i + 1] + dir * halfWidth + vdir * verticalWidth * i;
            vertices[i * 4 + 3] = posList[i] - dir * halfWidth + vdir * verticalWidth * i;

            uv[i * 4 + 0] = new Vector2(0, 0);
            uv[i * 4 + 1] = new Vector2(1, 1);
            uv[i * 4 + 2] = new Vector2(1, 0);
            uv[i * 4 + 3] = new Vector2(0, 1);
        }
        // 箭头头部长方形底部顶点
        vertices[count * 4 + 0] = vertices[count * 4 - 2] + vdir * verticalWidth;
        vertices[count * 4 + 1] = vertices[count * 4 - 3] + (vdir * verticalWidth)*5;
        vertices[count * 4 + 2] = vertices[count * 4 - 2] + (vdir * verticalWidth)*5;
        vertices[count * 4 + 3] = vertices[count * 4 - 3] + vdir * verticalWidth;
        // 箭头头部三角形顶点
        vertices[count * 4 + 4] = vertices[count * 4 + 2] + dir * halfWidth;
        vertices[count * 4 + 5] = vertices[count * 4 + 1] - dir * halfWidth;
        vertices[count * 4 + 6] = vertices[count * 4 + 2] - dir * halfWidth + vdir * verticalWidth * 5;
        // 箭头三角形头部所有uv
        uv[count * 4 + 0] = new Vector2(0, 0);
        uv[count * 4 + 1] = new Vector2(1, 1);
        uv[count * 4 + 2] = new Vector2(1, 0);
        uv[count * 4 + 3] = new Vector2(0, 1);
        uv[count * 4 + 4] = new Vector2(0, 0);
        uv[count * 4 + 5] = new Vector2(1, 1);
        uv[count * 4 + 6] = new Vector2(0, 1);

        int index = 0;
        for (int i = 0; i < count; i++)
        {
            triangles[index++] = i * 4 + 0;
            triangles[index++] = i * 4 + 1;
            triangles[index++] = i * 4 + 2;
            triangles[index++] = i * 4 + 1;
            triangles[index++] = i * 4 + 0;
            triangles[index++] = i * 4 + 3;
        }

        triangles[index++] = count * 4 + 0;
        triangles[index++] = count * 4 + 1;
        triangles[index++] = count * 4 + 2;
        triangles[index++] = count * 4 + 1;
        triangles[index++] = count * 4 + 0;
        triangles[index++] = count * 4 + 3;
        triangles[index++] = count * 4 + 4;
        triangles[index++] = count * 4 + 5;
        triangles[index++] = count * 4 + 6;

        Mesh newMesh = new Mesh();
        newMesh.vertices = vertices;
        newMesh.uv = uv;
        newMesh.triangles = triangles;
        meshFilter.mesh = newMesh;
    }

    public void InitArrowSign(GameObject arrow, Vector3 startPos, Vector3 endPos)
    {
        arrow.SetActive(true);
        MeshFilter arrowMesh = arrow.GetComponent<MeshFilter>();
        MeshRenderer arrowRender = arrow.GetComponent<MeshRenderer>();
        if (!arrowMesh)
            arrowMesh = arrow.AddComponent<MeshFilter>();
        if (!arrowRender)
            arrowRender = arrow.AddComponent<MeshRenderer>();
        List<Vector3> pos = GetParabolaPos(startPos, endPos);
        CreateArrow(arrowMesh, pos);
    }
    #endregion

}
