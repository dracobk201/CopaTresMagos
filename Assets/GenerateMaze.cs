using UnityEngine;
using System.Collections;

public class GenerateMaze : MonoBehaviour {

	int n, m, x, z, cubos;
	bool copaCreada, harry, cedric;
	string stringToEdit = "Hello World";
	int[,] muros;
	// Use this for initialization
	private void Awake () {
		n = 50;
		m = 50;
		muros = new int[n+1, m+1]; 
	}
	
	// Update is called once per frame
	private void Start () {
		Crear ();
		Debug.Log (cubos);
		//float var = n * m;
		//if (var
	}

	void OnGUI(){
		stringToEdit = GUI.TextField (new Rect (10, 10, 200, 20), stringToEdit, 25);
	}
	private void Crear(){
		for (int i = 0; i <= n; i++) {
			for (int j = 0; j <= m; j++) {
				string name = "cube" + i+"-"+j;
				if (Random.value <= 0.33) {
					GameObject cube = GameObject.CreatePrimitive (PrimitiveType.Cube);
					cube.transform.position = new Vector3 (0 + x, 0, 0 + z);
					cube.transform.localScale = new Vector3 (1, 3, 1);
					cube.AddComponent<BoxCollider> ();
					cube.name = name;
					cubos++;
					Debug.Log (i + " " + j);
					muros [i, j] = 1;
				} else if (Random.value >= 0.8 && !harry) {
				
				} else if (Random.value >= 0.8 && !harry) {

				} else if (Random.value <= 0.2 && !copaCreada && cubos >= 300) {			//Esa condición de los 300, no va funcionar
					GameObject copa = GameObject.CreatePrimitive (PrimitiveType.Sphere);
					copa.transform.position = new Vector3 (0 + x, 0, 0 + z);
					copa.AddComponent<SphereCollider> ();
					copaCreada = true;
					muros [i, j] = 2;
				}
				muros [i, j] = 0;
				z++;
			}
			z = 0;
			x++;
		}
	}
}
