using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MatchManager : MonoBehaviour
{
    public enum MatchState { SetUp_Board,SetUp_Player, SetUp_Enemy, Turn_Player, Turn_Enemy, End };
    public GameObject prompt;
    public TextMeshProUGUI winnerText;
    MatchState currentState;
    public MatchState CurrentState { get { return currentState; } }

    bool playerToggle=true;
    public static MatchManager Instance;
    // Start is called before the first frame update
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

        currentState = MatchState.SetUp_Board;
    }


    public void ChangeStates(MatchState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case MatchState.SetUp_Player:
                //Show Available 
                Board.Instance.OpenSetUpPosition(true);

                //Turn Off Enemy Pawns
                Arbitor.Instance.TogglePieces(Arbitor.Instance.EnemyPieces, false);
                //Turn On Friendly Pawns
                Arbitor.Instance.TogglePieces(Arbitor.Instance.PlayerPieces, true);
                Arbitor.Instance.HidePieces(true, true);
                Arbitor.Instance.HidePieces(false, false);
                break;
            case MatchState.SetUp_Enemy:
                Board.Instance.ClearBoard();
                //Show Available 
                Board.Instance.OpenSetUpPosition(false);
                //Turn On Enemy Pawns
                Arbitor.Instance.TogglePieces(Arbitor.Instance.EnemyPieces, true);
                //Turn Off Friendly Pawns
                Arbitor.Instance.TogglePieces(Arbitor.Instance.PlayerPieces, false);
                Arbitor.Instance.HidePieces(true, false);
                Arbitor.Instance.HidePieces(false, true);
                break;
            case MatchState.Turn_Player:
                Board.Instance.ClearBoard();

                Arbitor.Instance.TweenCamera("Player");
                //Turn Off Enemy Pawns
                Arbitor.Instance.TogglePieces(Arbitor.Instance.EnemyPieces, false);
                //Turn On Friendly Pawns
                Arbitor.Instance.TogglePieces(Arbitor.Instance.PlayerPieces, true);
                Arbitor.Instance.HidePieces(true, true);
                Arbitor.Instance.HidePieces(false, false);
                break;

            case MatchState.Turn_Enemy:
                Board.Instance.ClearBoard();

                Arbitor.Instance.TweenCamera("Enemy");
                //Turn On Enemy Pawns
                Arbitor.Instance.TogglePieces(Arbitor.Instance.EnemyPieces, true);
                //Turn Off Friendly Pawns
                Arbitor.Instance.TogglePieces(Arbitor.Instance.PlayerPieces, false);
                Arbitor.Instance.HidePieces(true, false);
                Arbitor.Instance.HidePieces(false, true);
                break;
            case MatchState.End:
                prompt.SetActive(true);
                break;

        }
    }

    public void SwitchTurn()
    {
        playerToggle = !playerToggle;

        if (playerToggle)
            ChangeStates(MatchState.Turn_Player);
        else
            ChangeStates(MatchState.Turn_Enemy);

    }

    public void EndGame(bool WinnerTeam)
    {
        Debug.Log("Ending");
        if (WinnerTeam)
        {
            Debug.Log("White Wins");
            winnerText.SetText("White Wins");
        }
        else
        {
            Debug.Log("Black Wins");
            winnerText.SetText("Black Wins");
        }


        ChangeStates(MatchState.End);
    }
}
