using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

using DMM;

public class InitShapeWhenStart : MonoBehaviour
{
    // Start is called before the first frame update
    DMMapShape mapShape;
    Mesh mesh;
    Vector3[] vertices;
    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        vertices = mesh.vertices;

        // get max y
        var max_y = vertices[0].y;

        foreach(Vector3 vetex in vertices)
        {
            Debug.Log(vetex);
            if (vetex.y > max_y)
            {
                max_y = vetex.y;
            }
        }

        Debug.Log("max y:" + max_y);

        //Vector3[] points = { };
        ArrayList array = new ArrayList();
        HashSet<Vector3> set = new HashSet<Vector3>();
        // filter y == max_y
        foreach(Vector3 vertex in vertices)
        {
            if (vertex.y == max_y)
            {
                // transform to world space
                var pos = transform.TransformPoint(vertex);
                array.Add(pos);
                set.Add(pos);
            }
        }

        Debug.Log("array size:" + array.Count); 
        Debug.Log("set size:" + set.Count);

        //get center
        Vector3 center = findCentroid(set.ToList<Vector3>());

        // sort
        List<Vector3> points = set.OrderBy(x => Math.Atan2(x.x - center.x, x.z - center.z)).ToList();

        // init dmmapshape
        DMMapShape mapShape = gameObject.AddComponent<DMMapShape>() as DMMapShape;

        // add object
        for(int i = 0; i< points.Count; i ++)
        {
            var go = new GameObject();
            go.transform.parent = gameObject.transform;
            go.transform.position = points[i];

            mapShape.verts.Add(go);
        }
        //foreach (var obj in points)
        //{
        //    var go = new GameObject();
        //    go.transform.parent = gameObject.transform;
        //    go.transform.position = obj;

        //    mapShape.verts.Add(go);
        //}

        DMMap.instance.Generate();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //// sort points
    //// https://math.stackexchange.com/questions/978642/how-to-sort-vertices-of-a-polygon-in-counter-clockwise-order
    public Vector3 findCentroid(List<Vector3> points)
    {
        float x = 0;
        float y = points[0].y;
        float z = 0;
        foreach (Vector3 p in points)
        {
            x += p.x;
            z += p.z;
        }
        Vector3 center = new Vector3(0, 0, 0);
        center.x = x / points.Count;
        center.z = z / points.Count;
        center.y = y;
        return center;
    }

    //public List<Vector3> sortVerticies(List<Vector3> points)
    //{
    //    // get centroid
    //    Vector3 center = findCentroid(points);
    //    Collections.sort(points, (a, b)-> {
    //        double a1 = (Math.toDegrees(Math.atan2(a.x - center.x, a.y - center.y)) + 360) % 360;
    //        double a2 = (Math.toDegrees(Math.atan2(b.x - center.x, b.y - center.y)) + 360) % 360;
    //        return (int)(a1 - a2);
    //    });
    //    return points;
    //}
}
