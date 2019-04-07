using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

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
    public float BallStartSpeed = 12;
    public float SpawnBoundaryY;
    public float SpawnBoundaryX;
    public float maxObsticleLenght;
    public float minObsticleLenght;
    public float maxDistortion1;
    public float maxDistortion2;
    public float minDelay;
    public float maxDelay;

    [Space]
    [Header("Camera")]
    public GameObject Camera;
    private PostProcessProfile postP;

    [Space]
    [Header("Prefabs")]
    public GameObject ballPrefab;
    public GameObject obstaclePrefab;


    [Space]
    [Header("UI Elements")]
    public GameObject LifesPlayer;
    public GameObject LifesKi;
    public GameObject ChargeMeter;
    public TMPro.TMP_Text WinnerText;
    public GameObject GameOverPanel;
    public GameObject StartPanel;
    public GameObject IngamePanel;

    public Image[] playerLifes;
    public RectTransform PlayerChargeMeter { get; set; }

    public Image[] kiLifes;

    public Canvas canvas;


    public bool gameStarted = false;

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

        IngamePanel.SetActive(false);
        GameOverPanel.SetActive(false);

        instatiatedBall = Instantiate(ballPrefab);
        
        

    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Time.timeScale = 0;
            GameOverPanel.SetActive(true);
            WinnerText.SetText("");
            GameOver = true;
            OnGameOver();
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
        {
            instatiatedBall.GetComponent<Ball>().reset();
            StopAllCoroutines();
            StartCoroutine(SpawnRandomObsticles());
        }
            
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
                    OnGameOver();
                }
                break;
            case ePlayerType.ki:
                currLifesKi--;
                if (currLifesKi <= 0)
                {
                    winner = ePlayerType.player;
                    GameOver = true;
                    OnGameOver();
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
        Time.timeScale = 0;
        GameOverPanel.SetActive(true);
        switch (winner)
        {
            case ePlayerType.player:
                WinnerText.SetText("YOU WON!");
                break;
            case ePlayerType.ki:
                WinnerText.SetText("YOU LOST..");
                break;
            case ePlayerType.none:
                break;
            default:
                break;
        }

        Cursor.visible = true;
    }

    private void OnApplicationPause(bool pause)
    {
        StopAllCoroutines();
    }

    private void OnApplicationQuit()
    {
        StopAllCoroutines();
    }

    public void StartRound()
    {

        Cursor.visible = false;
        canvas.GetComponent<Canvas>().planeDistance = 1;
        instatiatedBall.GetComponent<Ball>().reset();
        ballInitPos = instatiatedBall.transform.position;
        StartCoroutine(SpawnRandomObsticles());
        IngamePanel.SetActive(true);
        StartPanel.SetActive(false);
        gameStarted = true;
    }

    public void Reset()
    {
        //winner = ePlayerType.none;
        //currLifesKi = 0;
        //currLifesPlayer = 0;
        //UpdateUI();
        //StopAllCoroutines();
        //Or just reload scene 
        SceneManager.LoadScene(0);
    }

   

    IEnumerator SpawnRandomObsticles()
    {
        //Spawn standard obsticle:
        yield return new WaitForSeconds(3);
        Quaternion randRotation;
        int i = Random.Range(0, 1);
        if(i == 0)
        {
            randRotation = Quaternion.Euler(0, 0, Random.Range(-10, -8));
        }
        else
        {
            randRotation = Quaternion.Euler(0, 0, Random.Range(8, 10));
        }
     

        GameObject tmpGO = Instantiate(obstaclePrefab, Vector3.zero, randRotation);
        tmpGO.transform.localScale = new Vector3(Random.Range(minObsticleLenght, maxObsticleLenght), Random.Range(minObsticleLenght, maxObsticleLenght), 1);
        Destroy(tmpGO, 4);

        while(true)
        {
            float randomDelay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(randomDelay);
            SpawnObsticle();
            
        }

        yield return null;
    }

    Vector3 ChooseSpawnLocation()
    {
        bool isValidPosition = false;
        int tries = 0;
        Vector3 pos = new Vector3(Random.Range(-SpawnBoundaryX, SpawnBoundaryX), Random.Range(-SpawnBoundaryY, SpawnBoundaryY), 0);
        while(!isValidPosition)
        {
            tries++;
            Collider[] colliders = Physics.OverlapSphere(pos, 1);
            if (colliders.Length == 0)
                isValidPosition = true;
            if(tries > 100)
            {
                return Vector3.zero;
            }
        }


        return pos;
    }

    void SpawnObsticle()
    {

        Vector3 spawnPos = ChooseSpawnLocation();
        GameObject go = Instantiate(obstaclePrefab, spawnPos, Quaternion.Euler(0, 0, Random.Range(-20, 20)));
        go.transform.localScale = new Vector3(Random.Range(0.5f, 2) , Random.Range(minObsticleLenght, maxObsticleLenght), 1);

        Destroy(go, Random.Range(3, 10));
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

    public void Quit()
    {
        StopAllCoroutines();
        Application.Quit();
    }

    
}
