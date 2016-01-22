using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cedric : MonoBehaviour {

    
    public GenerateMaze maze;

    public Punto2D posicionCedric;


    struct PosicionMapa
    {
        public Punto2D posicion;
        public int cantidadPasos;
        public List<Punto2D> camino;
        public override string ToString()
        {
            return posicion.ToString() + "\n cantidadPasos " + cantidadPasos;
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

        StartCoroutine(MoverCedric());
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
            if (!moviendo || caminoACopa.Count == 0)
            {
                yield return null; 
                continue;
            }

            yield return new WaitForSeconds(0.5f);

            SetPosicionCedric(caminoACopa[casillaActual].x, caminoACopa[casillaActual].y);
            casillaActual++;

            if (casillaActual >= caminoACopa.Count)
            {
                break;
            }
        }
        yield return null;
    }

    public void SetPosicionCedric(int x, int y)
    {
        posicionCedric.x = x;
        posicionCedric.y = y;

        transform.position = new Vector3(x, 0, y);
    }

    public void CalcularRuta(Punto2D posicion, Punto2D copa)
    {
        moviendo = false;
        DoBFS(posicion.x, posicion.y, copa.x, copa.y);
        casillaActual = 0;
        SetPosicionCedric(posicion.x, posicion.y);

        Debug.Log("longitud camino " + caminoACopa.Count);

        for (int i = 0; i < caminoACopa.Count; i++)
        {
            Debug.Log("punto " + caminoACopa[i]);
        }

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
        posInicial.cantidadPasos = 0;
        posInicial.camino = new List<Punto2D>();
        posInicial.camino.Add(new Punto2D(x, y));

        visitados[x, y] = true;
        q.Enqueue(posInicial);

        while (q.Count > 0)
        {
            PosicionMapa posActual = q.Dequeue();
            Debug.LogWarning("recorriendo pos " + posActual);
            if (posActual.posicion.x == posCopaX && posActual.posicion.y == posCopaY)
            {
                //Enconttramos el camino a al copa. Guardamos la ruta.
                caminoACopa = new List<Punto2D>(posActual.camino);
                break;
            }

            for (int i = 0; i < 4; i++)
            {
                int nuevoX = posActual.posicion.x + desplazamientosX[i];
                int nuevoY = posActual.posicion.y + desplazamientosY[i];

                if (nuevoX >= 0 && nuevoX < maze.n && nuevoY >= 0 && nuevoY < maze.m)
                {
                    if (maze.muros[nuevoX, nuevoY] != 1 && !visitados[nuevoX, nuevoY])
                    {
                        visitados[nuevoX, nuevoY] = true;

                        PosicionMapa nuevaPos = new PosicionMapa();
                        nuevaPos.posicion.x = nuevoX;
                        nuevaPos.posicion.y = nuevoY;
                        nuevaPos.cantidadPasos = posActual.cantidadPasos + 1;
                        nuevaPos.camino = new List<Punto2D>(posActual.camino);
                        nuevaPos.camino.Add(nuevaPos.posicion);

                        q.Enqueue(nuevaPos);
                    }
                }
            }
        }
    }
}
