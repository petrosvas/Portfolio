using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowMoves : MonoBehaviour
{
    public int PressedId { get; set; }
    public bool IsPressed { get; set; } = false;
    public GameObject TileToMove { get; set; }
}
