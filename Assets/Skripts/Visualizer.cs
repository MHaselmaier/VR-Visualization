using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour {

    float[,] data = new float[10000, 3];

    void Awake()
    {
        for (int i = 0; data.GetLength(0) > i; ++i)
        {
            do
            {
                for (int j = 0; 3 > j; ++j)
                {
                    data[i, j] = Random.Range(-5f, 5f);
                }
            } while (5 < Mathf.Sqrt(data[i, 0] * data[i, 0] + data[i, 1] * data[i, 1] + data[i, 2] * data[i, 2]));
        }
    }

	// Use this for initialization
	void Start () {
		for (int i = 0; data.GetLength(0) > i; ++i)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.parent = this.gameObject.transform;
            sphere.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            sphere.transform.position = new Vector3(data[i, 0], data[i, 1], data[i, 2]);
            sphere.GetComponent<Renderer>().material.color = new Color(Mathf.Abs(data[i, 0]) / 10f, Mathf.Abs(data[i, 1]) / 10f, Mathf.Abs(data[i, 2]) / 10f);
        }
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(Vector3.down * Time.deltaTime * 10);
        transform.Rotate(Vector3.back * Time.deltaTime * 10);
        transform.Rotate(Vector3.right * Time.deltaTime * 10);
    }
}
