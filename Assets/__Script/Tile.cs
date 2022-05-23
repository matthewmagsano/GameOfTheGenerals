using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Tile : MonoBehaviour
{
    [SerializeField] BoxCollider2D boxCol;
    [SerializeField]  SpriteRenderer sprite;

    [SerializeField] Color defaultTile;
    [SerializeField] Color vacantTile;


    Vector2 gridPos;
    GamePiece piece;
    bool vacant=false;
    public bool Vacancy { get { return vacant; } }


    private void Awake()
    {
        if(sprite ==null)
            sprite = GetComponent<SpriteRenderer>();
        if(boxCol ==null)
            boxCol = GetComponent<BoxCollider2D>();
        boxCol.enabled = vacant;
    }
    public void SetGridPos(Vector2 newVector) {
        gridPos = newVector;
    }
    public  Vector2 GridPosition{ get { return gridPos; } }
    public void SetPiece(GamePiece newPiece)
    {
        piece = newPiece;
        vacant = false;
    }
    public void SetPiece()
    {
        piece = null;
        vacant = true;
    }

    public GamePiece GetPiece()
    {
        return piece;
    }
    public void ClearSpot()
    {
        piece = null;
    }
    public void ShowAvailable()
    {
        if (vacant)
            sprite.DOColor(defaultTile, .15f);
        else
            sprite.DOColor(vacantTile, .15f);

    }

    public void SetVacancy(bool toggle)
    {
        vacant = toggle;
        boxCol.enabled = vacant;

    }


}
