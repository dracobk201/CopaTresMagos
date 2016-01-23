using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameMaster : MonoBehaviour
{

    private static GameMaster _instance;

    #region imagenes
    public Sprite Cedric;
    public Sprite Harry;
    public Sprite Copa;
    #endregion

    #region GameObject
    public GameObject[] textosCanvas = new GameObject[5];
    public GameObject canvas;
    public int winCondition = 0; //0: EN juego, 1: Harry gana, 2: Cedric gana
    #endregion

    public static GameMaster instance
    {
        get
        {
            //If _instance hasn't been set yet, we grab it from the scene!
            //This will only happen the first time this reference is used.
            if (_instance == null)
                _instance = GameObject.FindObjectOfType<GameMaster>();
            return _instance;
        }
    }

    private void Start()
    {
        canvas.GetComponentInChildren<Image>().sprite = Copa;
        Time.timeScale = 1f;
    }

    private void LateUpdate()
    {
        if (winCondition != 0)
        {
            Time.timeScale = 0f;
            canvas.SetActive(true);
            textosCanvas[0].SetActive(false);
            textosCanvas[1].SetActive(false);
            textosCanvas[2].SetActive(false);
            textosCanvas[3].SetActive(false);
            if (winCondition == 1)
            {
                canvas.GetComponentInChildren<Image>().sprite = Harry;
                textosCanvas[4].SetActive(true);
                textosCanvas[4].GetComponent<Text>().text = "Harry toca la copa, es un traslador, se lo lleva a un cementerio y Voldemort le trollea la victoria.";
            }
            else if (winCondition == 2) {
                canvas.GetComponentInChildren<Image>().sprite = Cedric;
                textosCanvas[4].SetActive(true);
                textosCanvas[4].GetComponent<Text>().text = "Ceric toca la copa, es un traslador, se lo lleva a un cementerio y Voldemort lo mata. Que conveniente.";
            }
        }
    }

}
