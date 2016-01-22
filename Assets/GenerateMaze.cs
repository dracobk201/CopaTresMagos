using UnityEngine;
using System.Collections;

public class GenerateMaze : MonoBehaviour {
    
    public Cedric cedric;

	public int n, m, x, z, cubos;
	bool copaCreada, harry, cedricBool;
	string stringToEdit = "Hello World";

    [HideInInspector]
    /// <summary>
    /// Representa el mapa. Los valores posibles son
    /// 0 = espacio libre
    /// 1 = pared
    /// 2 = copa
    /// </summary>
    public int[,] muros;

    public int posicionCopaX, posicionCopaY;

    bool[,] visitados;

    int[] desplazamientosX = new int[] { 1, 0, -1, 0 };
    int[] desplazamientosY = new int[] { 0, 1, 0, -1 };

    public GameObject copaGameObject;

	// Use this for initialization
	private void Awake () {
        n = 50;
		m = 50;
        muros = new int[n + 1, m + 1];
        visitados = new bool[n, m];
	}
	
	// Update is called once per frame
	private void Start () {
		//Crear ();
        Crear2();
		Debug.Log (cubos);
		//float var = n * m;
		//if (var

        Punto2D posicionCedric = getPosicionAleatoriayLibre();
        Debug.Log(" posicionCedric " + posicionCedric);
        Punto2D posicionCopa = getPosicionAleatoriayLibre();
        Debug.Log(" posicionCopa " + posicionCopa);

        copaGameObject.transform.position = new Vector3(posicionCopa.x, 0, posicionCopa.y);
        cedric.CalcularRuta(posicionCedric, posicionCopa);
	}

	void OnGUI(){
		stringToEdit = GUI.TextField (new Rect (10, 10, 200, 20), stringToEdit, 25);
	}
	private void Crear(){
		for (int i = 0; i <= n; i++) {
			for (int j = 0; j <= m; j++) {
				string name = "cube" + i+"-"+j;
                muros [i, j] = 0;
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

                } /*else if (Random.value <= 0.2 && !copaCreada && cubos >= 300) {			//Esa condición de los 300, no va funcionar
                    GameObject copa = GameObject.CreatePrimitive (PrimitiveType.Sphere);
                    copa.transform.position = new Vector3 (0 + x, 0, 0 + z);
                    copa.AddComponent<SphereCollider> ();
                    copaCreada = true;
                    muros [i, j] = 2;
                }*/
				
				z++;
			}
			z = 0;
			x++;
		}
	}

    /// <summary>
    /// Obtiene una posicion dentro del mapa de forma aleatoria.
    /// </summary>
    /// <returns>Una posición libre.</returns>
    Punto2D getPosicionAleatoriayLibre()
    {
        Punto2D punto = new Punto2D();

        punto.x = Random.Range(0, n);
        punto.y = Random.Range(0, m);
        while (muros[punto.x, punto.y] == 1)
        {
            punto.x = Random.Range(0, n);
            punto.y = Random.Range(0, m);
        }

        return punto;
    }

    /// <summary>
    /// Devuelve una permutación de direcciones a partir de 4
    /// coordenadas (Este, Sur, Oeste, Norte).
    /// </summary>
    /// <returns></returns>
    int[] permutarDirecciones()
    {
        int[] direcciones = new int[] { 0, 1, 2, 3 };

        for (int i = 0; i < 3; i++)
        {
            int temp = direcciones[i];
            int numeroRandom = Random.Range(0, 4);

            //Hacer un shuffle.
            direcciones[i] = direcciones[numeroRandom];
            direcciones[numeroRandom] = temp;
        }

        return direcciones;
    }


    private void Crear2()
    {
        // Se inicializa el mapa.
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                visitados[i, j] = false;
                muros[i, j] = 1;
            }
        }
        //Se genera el laberinto a partir de la posicion 0, 0
        DoDFS(0, 0);
        x = 0;
        z = 0;
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                string name = "cube" + i + "-" + j;
                if (muros[i, j] == 1) // Si es una pared añade un cubo.
                {
                    GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    cube.transform.position = new Vector3(0 + x, 0, 0 + z);
                    cube.transform.localScale = new Vector3(1, 3, 1);
                    cube.AddComponent<BoxCollider>();
                    cube.name = name;
                    cubos++;
                    //Debug.Log(i + " " + j);
                }
                z++;
            }

            z = 0;
            x++;
        }        
    }

    private void DoDFS(int x, int y)
    {
        int n2 = n;
        int m2 = m;
        int[] direcciones = permutarDirecciones();

        for (int i = 0; i < direcciones.Length; i++)
        {
            int nuevoX = x + desplazamientosX[direcciones[i]];
            int nuevoY = y + desplazamientosY[direcciones[i]];
            int nuevoX_2 = x + desplazamientosX[direcciones[i]] * 2;
            int nuevoY_2 = y + desplazamientosY[direcciones[i]] * 2;

            //Aca se verifica que la nueva posicion sea válida (se encuentre dentro de los límites del mapa).
            if (nuevoX >= 0 && nuevoX < n2 && nuevoY >= 0 && nuevoY < m2 &&
                nuevoX_2 >= 0 && nuevoX_2 < n2 && nuevoY_2 >= 0 && nuevoY_2 < m2)

            {
                if (!visitados[nuevoX_2, nuevoY_2])
                {
                    muros[nuevoX, nuevoY] = 0;
                    muros[nuevoX_2, nuevoY_2] = 0;

                    visitados[nuevoX_2, nuevoY_2] = true;
                    DoDFS(nuevoX_2, nuevoY_2);
                }
            }
        }
    }
}

public struct Punto2D
{
    public int x, y;
    public Punto2D(int _x, int _y)
    {
        x = _x;
        y = _y;
    }

    public override string ToString()
    {
        return "x: " + x + "; y: " + y;
    }
}