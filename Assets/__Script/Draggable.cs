using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    bool dragging;
    Vector2 _offset;
    Rigidbody2D rb;
    Vector2 position;
    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        if (!dragging) return;
        var mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        position = Vector2.Lerp(transform.position,mousePos,10f);
    }


    private void FixedUpdate()
    {
        if (!dragging) return;
        rb.MovePosition(position);

        //transform.position = (Vector2)mousePos -_offset;
    }

    private void OnMouseDown()
    {
        dragging = true;
       // _offset = GetMousePos() - (Vector2)transform.position;
    }
    private void OnMouseUp()
    {
        dragging = false;
    }
    Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }


}
