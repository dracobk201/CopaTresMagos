using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GenerateMaze : MonoBehaviour {

    public Cedric cedric;
    public controller harryPower;

	public int n, m, x, z, cubos;
	bool copaCreada, cedricBool,MazeRunner;
	bool muestra = true;
	float t;

    [HideInInspector]
    /// <summary>
    /// Representa el mapa de tamaño n x m. Los valores posibles
    /// para cada casilla son
    /// 0 = espacio libre
    /// 1 = pared
    /// 2 = copa
    /// 3 = HP
    /// 4 = Cedric
    /// </summary>
    public int[,] muros;

    public Punto2D copaPos;

    [HideInInspector]
    public GameObject[,] murosGameObjects;
    public int posicionCopaX, posicionCopaY;

    bool[,] visitados;

    int[] desplazamientosX = new int[] { 1, 0, -1, 0 };
    int[] desplazamientosY = new int[] { 0, 1, 0, -1 };

    public GameObject copaGameObject;
	public Material wallTexture;

	//Esto era para usar los inputfield del canvas
	//public GameObject nMaze;
	//public GameObject mMaze;
	//public GameObject tMaze;
	[HideInInspector] public string nMaze = "10";
	[HideInInspector] public string mMaze = "10";
	[HideInInspector] public string tMaze = "0";

	// Use this for initialization
	private void Awake () {
		GameMaster.instance.canvas = GameObject.FindGameObjectWithTag ("UI");
	}
	
	// Update is called once per frame
	private void Update () {
		if (Input.GetKeyDown (KeyCode.E) && !MazeRunner) {
			Asignaciones ();
			CrearLaberinto ();

            Punto2D posicionCopa = getPosicionAleatoriayLibre();
            Debug.Log(" posicionCopa " + posicionCopa);

            muros[posicionCopa.x, posicionCopa.y] = 2;
			copaGameObject.transform.position = new Vector3 (posicionCopa.x, 0, posicionCopa.y);

            //Posicion de Harry
            Punto2D posicionHarry = getPosicionAleatoriayLibre();
            Debug.Log(" posicionHarry " + posicionHarry);

            muros[posicionHarry.x, posicionHarry.y] = 3;
            harryPower.SetPosicion(posicionHarry.x, posicionHarry.y);

			Punto2D posicionCedric = getPosicionAleatoriayLibre ();
			Debug.Log (" posicionCedric " + posicionCedric);

            cedric.SetPosicionCedric(posicionCedric.x, posicionCedric.y);
            muros[posicionCedric.x, posicionCedric.y] = 4;

            copaPos = posicionCopa;

			cedric.CalcularRuta (posicionCedric, posicionCopa);

            StartCoroutine(Evolucion());
		}
	}

    /// <summary>
    /// Obtiene una posicion dentro del mapa de forma aleatoria.
    /// </summary>
    /// <returns>Una posición 2D.</returns>
    Punto2D getPosicionAleatoria()
    {
        Punto2D punto = new Punto2D();

        punto.x = Random.Range(0, n);
        punto.y = Random.Range(0, m);

        return punto;
    }

    /// <summary>
    /// Devuelve si la coordenada x y está dentro de
    /// las coordenadas del mapa
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool isDentroDelMapa(int x, int y)
    {
        return x >= 0 && x < n && y >= 0 && y < m;
    }

    /// <summary>
    /// Devuelve verdadero si la posición x y está libre.
    /// (es decir cualquier cosa que no sea una pared)
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public bool isPosicionLibre(int x, int y)
    {
        return muros[x, y] != 1;
        //return muros[x, y] == 0;
    }


    /// <summary>
    /// Obtiene una posicion dentro del mapa de forma aleatoria.
    /// </summary>
    /// <returns>Una posición 2D.</returns>
    Punto2D getPosicionEvolucion()
    {
        Punto2D punto = getPosicionAleatoria();

        while (muros[punto.x, punto.y] != 0 && muros[punto.x, punto.y] != 1)
        {
            punto = getPosicionAleatoria();
        }

        return punto;
    }

    /// <summary>
    /// Obtiene una posicion dentro del mapa de forma aleatoria.
    /// </summary>
    /// <returns>Una posición libre.</returns>
    Punto2D getPosicionAleatoriayLibre()
    {
        Punto2D punto = getPosicionAleatoria();
        while (muros[punto.x, punto.y] != 0)
        {
            punto = getPosicionAleatoria();
        }

        return punto;
    }

    /// <summary>
    /// Mueve un jugador de una posición a otra.
    /// </summary>
    /// <param name="posicionAnterior">Posición actual.</param>
    /// <param name="posicionNueva">Posición nueva.</param>
    /// <param name="tipoJugador">Indicar si es Harry o Cedric</param>
    public void moverJugador(Punto2D posicionAnterior, Punto2D posicionNueva, int tipoJugador)
    {
        muros[posicionAnterior.x, posicionAnterior.y] = 0;

        muros[posicionNueva.x, posicionNueva.y] = tipoJugador;
    }

    /// <summary>
    /// Devuelve una permutación de direcciones de forma aleatoria, a partir de 4
    /// coordenadas (Este, Sur, Oeste, Norte).
    /// </summary>
    /// <returns>El arreglo de direcciones</returns>
    int[] permutarDirecciones()
    {
        int[] direcciones = new int[] { 0, 1, 2, 3 };//ESTE, SUR, OESTE, NORTE

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


    private void CrearLaberinto()
    {
        // Se inicializa el mapa.
        // Se asume que el mapa está lleno de paredes.
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < m; j++)
            {
                visitados[i, j] = false;
                muros[i, j] = 1;
            }
        }
        //Se genera el laberinto a partir de la posicion 0, 0
        muros[0, 0] = 0;
        visitados[0, 0] = true;
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
					cube.GetComponent<Renderer>().material = wallTexture;
                    cube.name = name;
                    murosGameObjects[i, j] = cube;
                    cubos++;
                }
                z++;
            }

            z = 0;
            x++;
        }        
    }

    /// <summary>
    /// Método que se encarga de generar un laberinto usando un recorrido en profundidad
    /// (DFS). Inicialmente el mapa está lleno de paredes y a partir de una posición se abrirá camino progresivamente, 
    /// con cada cada llamada al método.
    /// 
    /// Dada una posición x,y se verifica con sus 4 puntos cardinales si se puede crear un nuevo camino hacia esa
    /// nueva dirección. En caso de ser válido se invoca nuevamente al método con esta nueva coordenada.
    /// 
    /// Estos 4 puntos cardinales son tomados de forma aleatoria en vez de tomar siempre el mismo patrón.
    /// </summary>
    /// <param name="x">Coordenada x dentro del mapa</param>
    /// <param name="y">Coordenada y dentro del mapa</param>
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
                if (!visitados[nuevoX_2, nuevoY_2]) // Se verifica que la nueva posición no haya sido visitada/procesada, para evitar ciclos en el recorrido.
                {
                    muros[nuevoX, nuevoY] = 0;
                    muros[nuevoX_2, nuevoY_2] = 0;

                    visitados[nuevoX, nuevoY] = true;
                    visitados[nuevoX_2, nuevoY_2] = true;

                    //Se invoca recursivamente a la función.
                    DoDFS(nuevoX_2, nuevoY_2);
                }
            }
        }
    }

    /// <summary>
    /// Método que permite modificar los parámetros de entrada.
    /// </summary>
	void OnGUI() {
		if (muestra) {
			nMaze = GUI.TextField (new Rect (5, 40, 35, 20), nMaze, 3);
			mMaze = GUI.TextField (new Rect (5, 200, 35, 20), mMaze, 3);
			tMaze = GUI.TextField (new Rect (5, 364, 35, 20), tMaze, 3);
		}
	}

    /// <summary>
    /// Co-rutina para evolucionar el mapa de forma aleatoria.
    /// </summary>
    /// <returns></returns>
    IEnumerator Evolucion()
    {
        while (true)
        {
            //Después de cada t segundos intercambiar una pared por un piso o viceversa.
            yield return new WaitForSeconds(t);

            Punto2D punto = getPosicionEvolucion();
            if (muros[punto.x, punto.y] == 0)
            {
                //se genera una pared.
                string name = "cube" + punto.x + "-" + punto.y;
                GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                cube.transform.position = new Vector3(punto.x, 0, punto.y);
                cube.transform.localScale = new Vector3(1, 3, 1);
                cube.AddComponent<BoxCollider>();
                cube.GetComponent<Renderer>().material = wallTexture;
                cube.name = name;
                murosGameObjects[punto.x, punto.y] = cube;
                muros[punto.x, punto.y] = 1;

                if (cedric.isDebemosRecalcularRuta(punto)) { //Si la pared está en el recorrido de cedric debemos recalcular su camino mínimo.
                    Debug.LogError("recalculando...");
                    cedric.CalcularRuta(cedric.GetPosicionCedric(), copaPos);
                }
            }
            else if (muros[punto.x, punto.y] == 1)
            {
                //se elimina una pared.
                Destroy(murosGameObjects[punto.x, punto.y]);
                muros[punto.x, punto.y] = 0;

                if (cedric.isNoHayCamino()) { 
                    cedric.CalcularRuta(cedric.GetPosicionCedric(), copaPos);
                }
            }
        }
    }

	void Asignaciones(){
		//Esto era para usar los InputField del canvas
		/*n = int.Parse(nMaze.GetComponent<Text> ().text);
		m = int.Parse(mMaze.GetComponent<Text> ().text);
		n = int.Parse(tMaze.GetComponent<Text> ().text);*/
		n = int.Parse(nMaze);
		m = int.Parse(mMaze);
		t = float.Parse(tMaze);
		muros = new int[n + 1, m + 1];
		visitados = new bool[n, m];
        murosGameObjects = new GameObject[n, m];
		muestra = false;
		MazeRunner = true;
        GameMaster.instance.canvas.SetActive (false);
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