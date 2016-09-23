using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour
{
    public GameObject _water;
    public GameObject[] _islands;
    public GameObject _boat;
    public Vector3 _currentTile;
    int _islandTypes;
    public int _size;
    int _split;
    float _pieceX, _pieceZ;

    void Start()
    {
        _boat = GameObject.FindWithTag("Player");
        _currentTile = new Vector3(0,0,0);
/*
        if (_size < 5) { _size = 5; }

        if (_size % 2 == 0){ _split = (_size / 2) - 1;} //Is even
        else { _split = (_size / 2); } //Is odd      
*/          
        _islands = Resources.LoadAll<GameObject>("IslandPrefaps");

        _pieceX = _water.GetComponent<Renderer>().bounds.size.x;
        _pieceZ = _water.GetComponent<Renderer>().bounds.size.z;

        _islandTypes = _islands.Length;

        Map.LoadMap(_size);
        Map.RandomGenerateMap(_size, _islandTypes);
        DrawMap.DrawMapStart(_size, _water, _islands, _pieceX, _pieceZ);


        //Instantiate(_water, new Vector3(0, 0, 0), Quaternion.identity);



    }
    void FixedUpdate()
    {
        /*

        if (Input.GetKeyDown(KeyCode.K)){
            for (int i = 0; i < 5; ++i)
            {
                Map._Instances[i, 0, 0] = Instantiate(_water, new Vector3(i+1 * 20, 0, 0), Quaternion.identity);
            }

        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            for (int i = 0; i < 5; ++i)
            {
                Destroy(Map._Instances[i, 0, 0]);
            }

        }
        */
        
        if (_boat.transform.position.z > (_currentTile.z + _pieceZ/2 +1))
        {
            DrawMap.DrawMapUp(_size, _water, _currentTile, _islands, _pieceX, _pieceZ);
            _currentTile.z = _currentTile.z + _pieceZ;
        }
        if (_boat.transform.position.x > (_currentTile.x + _pieceX/2 + 1))
        {
            DrawMap.DrawMapRight(_size, _water, _currentTile, _islands, _pieceX, _pieceZ);
            _currentTile.x = _currentTile.x + _pieceX;
        }
        if (_boat.transform.position.x < (_currentTile.x - _pieceX / 2 - 1))
        {
            DrawMap.DrawMapLeft(_size, _water, _currentTile, _islands, _pieceX, _pieceZ);
            _currentTile.x = _currentTile.x - _pieceX;
        }
        if (_boat.transform.position.z < (_currentTile.z - _pieceZ / 2 - 1))
        {
            DrawMap.DrawMapDown(_size, _water, _currentTile, _islands, _pieceX, _pieceZ);
            _currentTile.z = _currentTile.z - _pieceZ;
        }
        
    }

}
