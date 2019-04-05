using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ePlayerType { player = 1, ki = 2, none};

public class GameManager : GenericSingletonClass<GameManager>
{
    [Header("Game Values")]
    public float MaxYMovement = 7.8f;
    public bool GameOver { get; set; }
    public int PointsToWin = 3;
    public int PickupsNeededForNewLife = 5;
    public float KiSpeed = 50;
    public float MaxChargePower = 300;
    public float MaxBallSpeed = 10;

    [Space]
    [Header("Ball")]
    public GameObject ballPrefab;

    [Space]
    [Header("Colectables")]
    public GameObject item1Prefab;
    public GameObject item2Prefab;

    [Space]
    [Header("UI Elements")]
    public GameObject LifesPlayer;
    public GameObject LifesKi;
    public GameObject ChargeMeter;
    public TMPro.TMP_Text WinnerText;
    public GameObject GameOverPanel;

    public Image[] playerLifes;
    public RectTransform PlayerChargeMeter { get; set; }

    public Image[] kiLifes;
   


    private int currLifesPlayer = 3;
    private int currLifesKi = 3;
    private ePlayerType winner = ePlayerType.none;

    private Vector3 ballInitPos;
    public ePlayerType LastContact = ePlayerType.none;
    private GameObject instatiatedBall = null;
    public GameObject Ball { get { return instatiatedBall; } }
    
    
    void Start()
    {
        Time.timeScale = 1;
        AudioListener.pause = false;

        
        PlayerChargeMeter = ChargeMeter.GetComponent<RectTransform>();
        PlayerChargeMeter.localScale = new Vector3(0,1,1);
      


        StartRound();
    }


    void Update()
    {
        if (GameOver)
        {
            //Show Game Over UI
        }
    }

    public void OnBorderCollision(ePlayerType type)
    {
        DecLife(type);
        switch (type)
        {
            case ePlayerType.player:     
                break;
            case ePlayerType.ki:                  
                break;
            default:
                break;
        }
        
        if (!GameOver)
            instatiatedBall.GetComponent<Ball>().reset();
        //Show ui or something
    }

    private void DecLife(ePlayerType type)
    {
        switch (type)
        {
            case ePlayerType.player:
                currLifesPlayer--;
                if(currLifesPlayer <= 0)
                {
                    winner = ePlayerType.ki;
                    GameOver = true;
                }
                break;
            case ePlayerType.ki:
                currLifesKi--;
                if (currLifesKi <= 0)
                {
                    winner = ePlayerType.player;
                    GameOver = true;
                }
                break;
            case ePlayerType.none:
                break;
            default:
                break;
        }
        UpdateUI();
    }

    private void OnGameOver()
    {

    }

    public void StartRound()
    {
        instatiatedBall = Instantiate(ballPrefab);
        ballInitPos = instatiatedBall.transform.position;
    }

    private void Reset()
    {
        winner = ePlayerType.none;
        currLifesKi = 0;
        currLifesPlayer = 0;
        UpdateUI();
        //Or just reload scene 
    }

    private void SpawnRandomObsticles()
    {

    }

    private void UpdateUI()
    {
        //Player Hearts
        int i = 3 - currLifesPlayer;
        for( int j = 0; j < i; j++)
        {
            playerLifes[i-1].color = new Color32(126, 126, 126,255);
        }

        //KI Hearts
        i = 3 - currLifesKi;
        for (int j = 0; j < i; j++)
        {
            kiLifes[i - 1].color = new Color32(126, 126, 126, 255);
        }
    }

    
}
