using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{

    [SerializeField] GameObject prefab;
    [SerializeField] Transform root;
    List<GameObject> tiles = new List<GameObject>();

    List<Tile> wholeBoard = new List<Tile>();
    List<Tile> playerStartingPosition = new List<Tile>();
    List<Tile> enemyStartingPosition = new List<Tile>();




    public static Board Instance;


    // Start is called before the first frame update
    void Awake()
    {
        DOTween.SetTweensCapacity(3000, 200);
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
        // Start is called before the first frame update
    void Start()
    {
        GenerateBoard();
    }

    private void GenerateBoard()
    {
        for (int j = 0; j < 8; j++)
        {
            for (int i = 0; i < 9; i++)
            {

                GameObject temp = Instantiate(prefab, new Vector3(i, j, 0), Quaternion.identity);
                Tile tempTile = temp.GetComponent<Tile>();
                tempTile.SetGridPos(new Vector2(i, j));
                tempTile.SetVacancy(false);
                if (j <= 2)
                {
                    playerStartingPosition.Add(tempTile);
                }
                else if (j >= 5)
                {
                    enemyStartingPosition.Add(tempTile);
                }




                temp.transform.position = new Vector2(i * 1.5f, j);

                temp.transform.SetParent(root);
                // temp.transform.localScale = Vector3.zero;
                tiles.Add(temp);
            }
        }
        MatchManager.Instance.ChangeStates(MatchManager.MatchState.SetUp_Player);
    }
    public void OpenSetUpPosition(bool whichSide)
    {
        if (whichSide)
        {

            foreach(Tile tile in playerStartingPosition)
            {
                tile.SetVacancy(true);
            }

        }
        else
        {
            foreach (Tile tile in enemyStartingPosition)
            {
                tile.SetVacancy(true);
            }
        }

        ShowVacantTiles(0.1f);
    }
     public void ShowVacantTiles(float delay)
    {
        StartCoroutine(LoadVacantTiles(delay));
    }
    void CheckPositions(Vector2 currentPos)
    {

    }
    public void MovePiece(GamePiece piece, Tile desiredTile)
    {
        if (desiredTile.GetPiece() != null)
        {
            Debug.Log("There is a piece");
            if (piece.Friendly == desiredTile.GetPiece().Friendly)
            {
                piece.transform.DOMove(piece.PreviousPosition, .25f);

            }
            if (piece.Friendly != desiredTile.GetPiece().Friendly)
            {
                Arbitor.Instance.ComparePiece(piece, desiredTile.GetPiece(), desiredTile);
            }
        }

        //the vacany is set to true for some reason - Dae Han
        else if (desiredTile.Vacancy)
        {
            
            //Clear Tile
            if (piece.PreviousPosition != piece.OriginalPosition)
                ClearTile(new Vector2(piece.PreviousPosition.x/ 1.5f, piece.PreviousPosition.y));

                piece.transform.DOMove(desiredTile.transform.position, .25f);
                desiredTile.SetPiece(piece);
                piece.SetPrevPosition(desiredTile.transform.position);
        }
        else
        {
            if (desiredTile.GetPiece().Friendly != piece.Friendly)
            {
                Debug.Log(desiredTile.GetPiece());
                //Clear Tile
                if (piece.PreviousPosition != piece.OriginalPosition)
                    ClearTile(new Vector2(piece.PreviousPosition.x / 1.5f, piece.PreviousPosition.y));
                //Move Piece then Fight
                piece.transform.DOMove(desiredTile.transform.position, .25f).OnComplete(()=> { 
                    Arbitor.Instance.ComparePiece(piece, desiredTile.GetPiece(), desiredTile);
                });
            }
            else
            {
                piece.transform.DOMove(piece.PreviousPosition, .25f);
            }
        }
        if(MatchManager.Instance.CurrentState == MatchManager.MatchState.SetUp_Player || MatchManager.Instance.CurrentState == MatchManager.MatchState.SetUp_Enemy)
        {
            Arbitor.Instance.CheckPieces();
        }
        if (MatchManager.Instance.CurrentState == MatchManager.MatchState.Turn_Player || MatchManager.Instance.CurrentState == MatchManager.MatchState.Turn_Enemy)
        {
            MatchManager.Instance.SwitchTurn();
        }
    }
    public void ClearTile(Vector2 oldTile)
    {
        Tile temp = GetTileAt(oldTile);
        temp.SetPiece();
    }
    public void ClearBoard()
    {
        foreach (GameObject tile in tiles)
        {
            //if(piece exists)
            tile.GetComponent<Tile>().SetVacancy(false);
        }
        ShowVacantTiles(0.1f);
    }
    public void CheckDirections(Vector2 curPos)
    {
        Tile top = GetTileAt(new Vector2(curPos.x, curPos.y+1));
        Tile bottom = GetTileAt(new Vector2(curPos.x, curPos.y-1));
        Tile left = GetTileAt(new Vector2(curPos.x-1, curPos.y));
        Tile right = GetTileAt(new Vector2(curPos.x+1, curPos.y));

       //setvacany to true means they wont see the piece on it 
       //Vacany doesnt mean its a free move
       //- Dae Han
        if (top != null)
        {
            top.SetVacancy(true);
        }
        if (bottom != null)
            bottom.SetVacancy(true);
        if(left != null)
            left.SetVacancy(true);
        if(right != null)
            right.SetVacancy(true);

    }
   
    Tile GetTileAt(Vector2 Position)
    {
        foreach(GameObject tile in tiles)
        {
            Tile temp = tile.GetComponent<Tile>();
            if (temp.GridPosition == Position)
            {
                return temp;
            }
        }
        return null;
    }
    //Juice
    IEnumerator LoadInBoard()
    {
        yield return new WaitForSeconds(.1f);
        Debug.Log("Starting Load");
        // SpriteRenderer icons = FindObjectsOfType<>
        yield return new WaitForSeconds(1f);

        foreach (GameObject tile in tiles)
        {
            yield return new WaitForSeconds(.025f);
            tile.transform.DOScale(new Vector3(1.5f, 1, 0), .1f);
        }
        MatchManager.Instance.ChangeStates(MatchManager.MatchState.SetUp_Player);
    }
    IEnumerator LoadVacantTiles(float delay)
    {
        yield return new WaitForSeconds(delay);
        foreach (GameObject tile in tiles)
        {
            Tile tempTile = tile.GetComponent<Tile>();
            tempTile.ShowAvailable();
        }
    }

}
