using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinimapCameraScript : MonoBehaviour
{
    public GameObject MinimapCameraObject;
    public Transform CenterCameraObject;
    public Vector3 CameraOffset;
    public bool ShowNavmesh = false;

    private NavMeshTriangulation triangulation;
    private Material mat;

    public float X
    {
        get { return gameObject.transform.position.x; }
        set
        {
            MinimapCameraObject.transform.position = new Vector3(
            value,
            gameObject.transform.position.y,
            gameObject.transform.position.z);
        }
    }
    public float Y
    {
        get { return gameObject.transform.position.y; }
        set
        {
            MinimapCameraObject.transform.position = new Vector3(
                gameObject.transform.position.x,
                value,
                gameObject.transform.position.z);
        }
    }
    public float Z
    {
        get { return gameObject.transform.position.z; }
        set
        {
            MinimapCameraObject.transform.position = new Vector3(
                gameObject.transform.position.x,
                gameObject.transform.position.y,
                value);
        }
    }

    // Use this for initialization
    void Start()
    {
        triangulation = NavMesh.CalculateTriangulation();
        if (!mat)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things. In this case, we just want to use
            // a blend mode that inverts destination colors.			
            var shader = Shader.Find("Hidden/Internal-Colored");
            mat = new Material(shader);
            mat.hideFlags = HideFlags.HideAndDontSave;
            // Set blend mode to invert destination colors.
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcColor);
            mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.DstColor);
            mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.DstAlpha);
            // Turn off backface culling, depth writes, depth test.
            mat.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            mat.SetInt("_ZWrite", 0);
            mat.SetInt("_ZTest", (int)UnityEngine.Rendering.CompareFunction.Always);
        }
        //MinimapCameraObject = GameObject.Find("Minimap Camera");
        //CenterCameraObject = GameObject.Find("Player").transform;
        //CameraOffset = new Vector3(-0.0f, 8.0f, -8.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if (CenterCameraObject != null)
        {
            X = CenterCameraObject.transform.position.x + CameraOffset.x;
            Y = CenterCameraObject.transform.position.y + 10 + CameraOffset.y;
            Z = CenterCameraObject.transform.position.z + CameraOffset.z;
        }
    }

    private void OnPostRender()
    {
        if (!ShowNavmesh) return;
        if (mat == null)
        {
           return;
        }
        GL.PushMatrix();

        mat.SetPass(0);
        GL.Begin(GL.TRIANGLES);
        for (int i = 0; i < triangulation.indices.Length; i += 3)
        {
            var triangleIndex = i / 3;
            var i1 = triangulation.indices[i];
            var i2 = triangulation.indices[i + 1];
            var i3 = triangulation.indices[i + 2];
            var p1 = triangulation.vertices[i1];
            var p2 = triangulation.vertices[i2];
            var p3 = triangulation.vertices[i3];
            var areaIndex = triangulation.areas[triangleIndex];
            Color color;
            bool ok = true;
            if (areaIndex == 1) Debug.Log("qwe");
            switch (areaIndex)
            {
                case 0:
                    
                    color = Color.gray; break;
                case 1:
                    ok = false;
                    color = Color.black; break;
                default:
                    color = Color.red; break;
            }
            if (!ok) continue;
            GL.Color(color);
            GL.Vertex(p1);
            GL.Vertex(p2);
            GL.Vertex(p3);
        }
        GL.End();

        GL.PopMatrix();
    }

}
