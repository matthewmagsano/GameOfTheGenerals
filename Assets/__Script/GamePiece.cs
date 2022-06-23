using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GamePiece : MonoBehaviour
{
   [SerializeField] Sprite sprite;
   [SerializeField] int power;
   [SerializeField] bool isFriendly;
    Rigidbody2D rb;
    Tile targetTile;

    Vector2 position;
    Vector2 previousPosition;
    Vector2 originalPosition;

    bool dragging;
    public bool canMove = true;

    public Vector3 PreviousPosition { get { return previousPosition; } }
    public Vector3 OriginalPosition { get { return originalPosition; } }

    public bool Friendly { get { return isFriendly;} }
    public int Power { get { return power; } }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        originalPosition = transform.position;
        previousPosition = originalPosition;
    }
    private void Update()
    {
        if (transform.position.z != 0)
            transform.position = new Vector3(transform.position.x, transform.position.y, 0f);

        if (!dragging) return;
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position = Vector2.Lerp(transform.position, mousePos, 10f);
    }


    private void FixedUpdate()
    {
        if (!dragging) return;
        rb.MovePosition(position);

        //transform.position = (Vector2)mousePos -_offset;
    }

    private void OnMouseDown()
    {
        if (canMove)
        {
            dragging = true;
            //Board.ShowAvailableMoves(vector2)
            if(MatchManager.Instance.CurrentState == MatchManager.MatchState.Turn_Enemy|| MatchManager.Instance.CurrentState == MatchManager.MatchState.Turn_Player)
            {
                Board.Instance.ClearBoard();
                //board so valid movements
                Board.Instance.CheckDirections(new Vector2(previousPosition.x / 1.5f, previousPosition.y));
                //Board.Instance.ShowVacantTiles(0.1f);
            }
        }
    }

    private void OnMouseUp()
    {
        if (canMove)
        {
            dragging = false;
            if (targetTile != null)
            {
                Board.Instance.MovePiece(this, targetTile);

            }
            else
                transform.DOMove(previousPosition, .25f);

            if (MatchManager.Instance.CurrentState == MatchManager.MatchState.Turn_Enemy || MatchManager.Instance.CurrentState == MatchManager.MatchState.Turn_Player)
            {
                Board.Instance.ClearBoard();
                Board.Instance.ShowVacantTiles(0.1f);
            }

        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Tile") && dragging)
        {
            targetTile = other.GetComponent<Tile>();
            
        }
    }
    public void SetPrevPosition(Vector2 newPosition)
    {
        previousPosition = newPosition;
    }
}
