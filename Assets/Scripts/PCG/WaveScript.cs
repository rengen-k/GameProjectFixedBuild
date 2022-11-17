using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// TODO GET PROPER CREDTIS FOR THIS
// Credit to Lecturer

public class WaveScript : MonoBehaviour {
    [Tooltip("Scale of waves, the height change.")]
    [SerializeField] private float scale = 0.7f;
    [Tooltip("The speed of change.")]
    [SerializeField] private float speed = 3.0f;
    private Vector3[] baseHeight;

    // Use this for initialization
    void Start () {
		transform.GetChild(0).GetComponent<Canvas>().worldCamera = GameObject.Find("UICamera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        Camera camera = Camera.main;
        transform.GetChild(0).transform.LookAt(new Vector3(camera.transform.position.x, transform.GetChild(0).transform.position.y, camera.transform.position.z));
        Waves();
		
	}

    void Waves()
    {
        //Get current cube mesh
        Mesh mesh = GetComponent<MeshFilter>().mesh;

        if (baseHeight == null)
            baseHeight = mesh.vertices;

        //Vertices for cube mesh
        Vector3[] vertices = new Vector3[baseHeight.Length];

        for (var i = 0; i < vertices.Length; i++)
        {
            Vector3 vertex = baseHeight[i];

            //Sinus value for rolling waves
            float value = Mathf.Sin(Time.time * speed + baseHeight[i].x + baseHeight[i].y + baseHeight[i].z) * scale;

            //Add sinus and update the vertex
            vertex.y += value;
            vertices[i] = vertex;

        }

        //Update cube mesh
        mesh.vertices = vertices;
        mesh.RecalculateNormals();


    }
}
