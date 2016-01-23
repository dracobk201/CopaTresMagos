using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cedric : MonoBehaviour {

    
    public GenerateMaze maze;

    public Punto2D posicionCedric;

    Rigidbody rb;

    struct PosicionMapa
    {
        public Punto2D posicion;
        public List<Punto2D> camino;
        public override string ToString()
        {
            return posicion.ToString();
        }
    }

    bool[,] visitados;

    int[] desplazamientosX = new int[] { 1, 0, -1, 0 };
    int[] desplazamientosY = new int[] { 0, 1, 0, -1 };

    List<Punto2D> caminoACopa;

    int casillaActual = -1;
    bool moviendo;

    void Start()
    {
        moviendo = false;
        caminoACopa = new List<Punto2D>();

        rb = GetComponent<Rigidbody>();
        StartCoroutine(MoverCedric());
	}
	
    public Punto2D GetPosicionCedric()
    {
        return posicionCedric;
    }

	// Update is called once per frame
	void Update () {
        if (!moviendo)
        {
            return;
        }


	}

    IEnumerator MoverCedric()
    {
        while (true)
        {
            if (!moviendo || caminoACopa.Count < 2)
            {
                yield return null; 
                continue;
            }


            if (casillaActual >= caminoACopa.Count || caminoACopa.Count == 0)
            {
                if (caminoACopa.Count == 0)
                {
                    Debug.Log("CalcularRuta dentro de mover");
                    CalcularRuta(posicionCedric, maze.copaPos);
                    if (caminoACopa.Count == 0)
                    {
                        yield return null;
                        continue;
                    }
                }
                yield return null; 
                continue;
            }
            yield return new WaitForSeconds(0.25f);

            if (casillaActual >= caminoACopa.Count)
            {
                Debug.LogError("index fuera del rango " + casillaActual + " ; " + caminoACopa.Count);

                yield return null;
                continue;
            }

            Punto2D posicionAnterior = posicionCedric;
            Punto2D posicionNueva = caminoACopa[casillaActual];
            casillaActual++;

            if (posicionNueva.x == maze.copaPos.x && posicionNueva.y == maze.copaPos.y)
            {
                SetPosicionCedric(posicionNueva.x, posicionNueva.y);
                moviendo = false;
                //aqui.
                GameMaster.instance.winCondition = 2; //Gana Cedric
                yield return null;
                continue;
            }

            maze.moverJugador(posicionAnterior, posicionNueva, 4);
            SetPosicionCedric(posicionNueva.x, posicionNueva.y);

            if (casillaActual >= caminoACopa.Count)
            {

                break;
            }

        }
        yield return null;
    }
    public bool isNoHayCamino()
    {
        return caminoACopa.Count == 0;
    }
    public bool isDebemosRecalcularRuta(Punto2D nuevaPared)
    {
        if (caminoACopa.Count == 0) return true;
        for (int i = casillaActual; i < caminoACopa.Count; i++)
        {
            if (caminoACopa[i].x == nuevaPared.x && caminoACopa[i].y == nuevaPared.y)
                return true;
        }
        return false;
    }

    public void SetPosicionCedric(int x, int y)
    {
        posicionCedric.x = x;
        posicionCedric.y = y;

        transform.position = new Vector3(x, 0, y);
    }

    public void CalcularRuta(Punto2D posicionIncial, Punto2D copa)
    {
        moviendo = false;
        DoBFS(posicionIncial.x, posicionIncial.y, copa.x, copa.y);
        casillaActual = 0;
        //SetPosicionCedric(posicionIncial.x, posicionIncial.y);

        Debug.Log("longitud camino " + caminoACopa.Count);

        //for (int i = 0; i < caminoACopa.Count; i++)
        //{
        //    Debug.Log("punto " + caminoACopa[i]);
        //}

        moviendo = true;
    }

    /// <summary>
    /// Algoritmo de búsqueda en anchura para calcular el camino más corto hacia la copa.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="posCopaX"></param>
    /// <param name="posCopaY"></param>
    private void DoBFS(int x, int y, int posCopaX, int posCopaY)
    {
        visitados = new bool[maze.n, maze.m];
        Queue<PosicionMapa> q = new Queue<PosicionMapa>();

        PosicionMapa posInicial = new PosicionMapa();
        posInicial.posicion.x = x;
        posInicial.posicion.y = y;
        posInicial.camino = new List<Punto2D>();
        posInicial.camino.Add(new Punto2D(x, y));

        visitados[x, y] = true;
        q.Enqueue(posInicial);

        while (q.Count > 0)
        {
            PosicionMapa posActual = q.Dequeue();
            //Debug.LogWarning("recorriendo pos " + posActual);
            if (posActual.posicion.x == posCopaX && posActual.posicion.y == posCopaY)
            {
                //Enconttramos el camino a al copa. Guardamos la ruta.
                caminoACopa = new List<Punto2D>(posActual.camino);
                return;
            }

            for (int i = 0; i < 4; i++)
            {
                int nuevoX = posActual.posicion.x + desplazamientosX[i];
                int nuevoY = posActual.posicion.y + desplazamientosY[i];

                if (nuevoX >= 0 && nuevoX < maze.n && nuevoY >= 0 && nuevoY < maze.m)
                {
                    //if ((maze.muros[nuevoX, nuevoY] == 0 || maze.muros[nuevoX, nuevoY] == 2) && !visitados[nuevoX, nuevoY])
                    if ((maze.muros[nuevoX, nuevoY] != 1) && !visitados[nuevoX, nuevoY])
                    {
                        visitados[nuevoX, nuevoY] = true;

                        PosicionMapa nuevaPos = new PosicionMapa();
                        nuevaPos.posicion.x = nuevoX;
                        nuevaPos.posicion.y = nuevoY;
                        nuevaPos.camino = new List<Punto2D>(posActual.camino);
                        nuevaPos.camino.Add(nuevaPos.posicion);

                        q.Enqueue(nuevaPos);
                    }
                }
            }
        }

        caminoACopa.Clear();
    }
}
