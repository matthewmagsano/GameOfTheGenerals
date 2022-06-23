using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Arbitor : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] Vector3 playerPosition;
    [SerializeField] Vector3 enemyPosition;
    [Space(10)]
    [SerializeField] GameObject EnemyPieceRoot;
    [SerializeField] GameObject PlayerIndicator;
    [SerializeField] GameObject EnemyIndicator;
    [SerializeField] GamePiece[] playerPieces;
    [SerializeField] GamePiece[] enemyPieces;

    public static Arbitor Instance;
    // Start is called before the first frame update

    public GamePiece[] PlayerPieces { get { return playerPieces; } }
    public GamePiece[] EnemyPieces { get { return enemyPieces; } }
    public GameObject[] PlayerIcon;
    public GameObject[] EnemyIcon;


    void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        EnemyPieceRoot.SetActive(false);
    }   
    public void ComparePiece(GamePiece agressor, GamePiece defendant,Tile prize)
    {

        if (agressor.Power > defendant.Power)
        {
            Debug.Log("Agressor Wins");
            if (defendant.Power == -1){
                Debug.Log("It was the flag");
                MatchManager.Instance.EndGame(agressor.Friendly);
            }
            agressor.transform.DOMove(prize.transform.position, .04f);
            defendant.transform.DOMove(defendant.OriginalPosition, .04f);
            prize.SetPiece(agressor);



        }
        else
        {
            Debug.Log("Defendant Wins");
            if (agressor.Power == -1)
            {
                Debug.Log("It was the flag");
                MatchManager.Instance.EndGame(defendant.Friendly);
            }
            agressor.transform.DOMove(agressor.OriginalPosition, .04f);
            prize.SetPiece(defendant);
        }
    }
    public void CheckPieces()
    {
        bool canStartGame = true;

        if(MatchManager.MatchState.SetUp_Player == MatchManager.Instance.CurrentState)
        {
            foreach (GamePiece piece in playerPieces)
            {
                if (piece.OriginalPosition == piece.PreviousPosition)
                    canStartGame = false;

            }
            if (canStartGame)
            {
                TweenCamera("Enemy");
                MatchManager.Instance.ChangeStates(MatchManager.MatchState.SetUp_Enemy);
            }
        }

        if(MatchManager.MatchState.SetUp_Enemy == MatchManager.Instance.CurrentState)
        {
            foreach (GamePiece piece in enemyPieces)
            {
                if(piece.OriginalPosition == piece.PreviousPosition)
                    canStartGame = false;

            }
            if (canStartGame)
            {
                TweenCamera("Player");
                MatchManager.Instance.ChangeStates(MatchManager.MatchState.Turn_Player);
            }
        }
    }
    public void TogglePieces(GamePiece[] pieces ,bool toggle)
    {
        foreach (GamePiece piece in pieces)
        {
            piece.canMove = toggle;
        }
        
    }
    public void TweenCamera(string whichCamera)
    {
        if(whichCamera == "Player")
        {
            Camera.main.transform.DOMove(playerPosition, 1f);
        }
        else
        {
            Camera.main.transform.DOMove(enemyPosition, 1f).OnComplete(()=> {

                EnemyPieceRoot.SetActive(true);
            });
        }
    }
    public void HidePieces(bool whichTeam,bool toggle)
    {
        //Player
        if (whichTeam)
        {
            foreach (GameObject icon in PlayerIcon)
            {
                icon.SetActive(toggle);
            }
        }
        else
        {
            foreach (GameObject icon in EnemyIcon)
            {
                icon.SetActive(toggle);
            }
        }



    }

}
