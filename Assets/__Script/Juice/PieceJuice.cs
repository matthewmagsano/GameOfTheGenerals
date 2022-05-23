using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class PieceJuice : MonoBehaviour
{
    [SerializeField] GameObject[] pieces;



    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LoadInPieces());   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LoadInPieces()
    {
        foreach (GameObject piece in pieces)
        {
            piece.transform.localScale = Vector3.zero;
        }

        yield return new WaitForSeconds(.5f);

        foreach (GameObject piece in pieces)
        {
            yield return new WaitForSeconds(.1f);

            piece.transform.DOScale(new Vector3(1.2f, 0.8f, 0), .1f);
        }

    }
}
