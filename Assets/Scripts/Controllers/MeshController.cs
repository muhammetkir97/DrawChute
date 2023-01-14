using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MeshController : MonoBehaviour
{
    [SerializeField] private MeshFilter meshFilter;
    private List<Vector3> parachuteSegmentVertices = new List<Vector3>()
    {
        new Vector3(1, 1, 0),       //1
        new Vector3(2, 1.7f, 0),    //2
        new Vector3(3, 1, 0),       //3
        new Vector3(4, 0, 0),       //4
        new Vector3(0, 0, 5),       //5
        new Vector3(1, 1, 5),       //6
        new Vector3(2, 1.7f, 5),    //7
        new Vector3(3, 1, 5),       //8
        new Vector3(4, 0, 5)        //9
    };

    private List<int> parachuteSegmentTriangles = new List<int>()
    {
        0, 5, 6, 
        1, 0, 6,
        2, 1, 6,
        2, 6, 7,
        2, 7, 8,
        2, 8, 3,
        3, 8, 9,
        4, 3, 9

    };

    private List<Vector3> parachuteSegmentNormals = new List<Vector3>()
    {
        Vector3.up,
        Vector3.up,
        Vector3.up,
        Vector3.up,
        Vector3.up,
        Vector3.up,
        Vector3.up,
        Vector3.up,
        Vector3.up,
        Vector3.up,

    };
    void Start()
    {
        CreateParachute(null);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CreateParachute(Vector2[] _points)
    {
        Mesh _mesh = new Mesh();
        
        List<Vector3> _verts = new List<Vector3>();
        List<int> _tris = new List<int>();
        List<Vector3> _normals = new List<Vector3>();
        Vector3 _lastPos = Vector3.zero;
        for(int i=0; i< 5; i++)
        {
            _verts.AddRange(GetSegmentVertices(_lastPos, 0));
            _tris.AddRange(GetSegmentTriangles(i));
            _normals.AddRange(parachuteSegmentNormals);
            _lastPos = _verts[_verts.Count - 1] + new Vector3(0, 0, -5);
        }
        _mesh.SetVertices(_verts);
        _mesh.SetTriangles(_tris, 0);
        _mesh.SetNormals(_normals);
        meshFilter.sharedMesh = _mesh;
    }

    private List<Vector3>  GetSegmentVertices(Vector3 _firstPos, float _rotation)
    {
        List<Vector3> _vertices = new List<Vector3>();
        Quaternion _r =  Quaternion.Euler(0, 0, _rotation);
        _vertices.Add(_firstPos);

        int _cnt = parachuteSegmentVertices.Count;
        for(int i=0; i< _cnt; i++)
        {
            _vertices.Add(_firstPos + (_r * parachuteSegmentVertices[i]));
        }

        return _vertices;

    }

    private List<int>  GetSegmentTriangles(int _segmentNo)
    {
        List<int> _tri = new List<int>();
        int _count = parachuteSegmentTriangles.Count;
        for(int i=0; i<_count; i++)
        {
            _tri.Add(parachuteSegmentTriangles[i] + (_segmentNo * (parachuteSegmentVertices.Count + 1)));
        }

        return _tri;
    }

    private Vector3 GetPointOnLine(float _distance)
    {

        return Vector3.zero;
    }

}
