using UnityEngine;
using System.Collections;

public class controller : MonoBehaviour {
    public GenerateMaze maze;
	Vector3 positionActual;
	//public static float paso = .1f;

	Rigidbody rb;
	void Start () {
		rb = GetComponent<Rigidbody> ();
	}

    public void SetPosicion(int x, int y)
    {
        transform.position = new Vector3(x, 0, y);
    }

	// Update is called once per frame
	void Update () {
        int paso = 1;
		positionActual = this.transform.position;
		Vector3 positionNueva;
        Punto2D nuevaPosMapa;
        Punto2D posActualMapa = new Punto2D((int)positionActual.x, (int)positionActual.z);
		if (Input.GetKeyDown(KeyCode.D)) {
            positionNueva = new Vector3(positionActual.x + paso, positionActual.y, positionActual.z);
            nuevaPosMapa.x = (int)positionNueva.x;
            nuevaPosMapa.y = (int)positionNueva.z;
            if (maze.isDentroDelMapa(nuevaPosMapa.x, nuevaPosMapa.y) && maze.isPosicionLibre(nuevaPosMapa.x, nuevaPosMapa.y))
            {
                maze.moverJugador(posActualMapa, nuevaPosMapa, 3);
                this.transform.position = positionNueva;
            }
		}
		if (Input.GetKeyDown(KeyCode.A)) {
            positionNueva = new Vector3(positionActual.x - paso, positionActual.y, positionActual.z);
            nuevaPosMapa.x = (int)positionNueva.x;
            nuevaPosMapa.y = (int)positionNueva.z;
            if (maze.isDentroDelMapa(nuevaPosMapa.x, nuevaPosMapa.y) && maze.isPosicionLibre(nuevaPosMapa.x, nuevaPosMapa.y))
            {
                maze.moverJugador(posActualMapa, nuevaPosMapa, 3);
                this.transform.position = positionNueva;
            }
		}
		if (Input.GetKeyDown(KeyCode.W)) {
            positionNueva = new Vector3(positionActual.x, positionActual.y, positionActual.z + paso);
            nuevaPosMapa.x = (int)positionNueva.x;
            nuevaPosMapa.y = (int)positionNueva.z;
            if (maze.isDentroDelMapa(nuevaPosMapa.x, nuevaPosMapa.y) && maze.isPosicionLibre(nuevaPosMapa.x, nuevaPosMapa.y))
            {
                maze.moverJugador(posActualMapa, nuevaPosMapa, 3);
                this.transform.position = positionNueva;
            }
		}
		if (Input.GetKeyDown(KeyCode.S)) {
            positionNueva = new Vector3(positionActual.x, positionActual.y, positionActual.z - paso);
            nuevaPosMapa.x = (int)positionNueva.x;
            nuevaPosMapa.y = (int)positionNueva.z;
            if (maze.isDentroDelMapa(nuevaPosMapa.x, nuevaPosMapa.y) && maze.isPosicionLibre(nuevaPosMapa.x, nuevaPosMapa.y))
            {
                maze.moverJugador(posActualMapa, nuevaPosMapa, 3);
                this.transform.position = positionNueva;
            }
		}

        //float velo = (transform.position - positionActual).magnitude / Time.deltaTime;
        //Debug.LogWarning("velo " + velo);

        
	}

    void LateUpdate()
    {
        ControlesEscena();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Copa"))
        {
            GameMaster.instance.winCondition = 1;
        }
    }

    private void ControlesEscena()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        if (Input.GetKeyDown(KeyCode.P))
            Application.LoadLevel("Maze");
    }
}
