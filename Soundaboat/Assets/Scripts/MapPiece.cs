using UnityEngine;
using System.Collections;

public class MapPiece{

    public int _x, _z;
    public Island[] _islands;

    public MapPiece(Island[] island, int x, int z)
    {
        _islands = island;
        _x = x;
        _z = z;
    }
}
