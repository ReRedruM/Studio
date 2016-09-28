using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public static class Map
{
    //public static List<MapPiece> map;
    public static MapPiece[,] _map;
    public static Object[,,] _Instances;


    public static void LoadMap(int mapSize)
    {
        _map = new MapPiece[mapSize, mapSize]; // Allocating map
        _Instances = new Object[5, 5, 10];
        /*
        for (int z = 0; z < 5; ++z)
        {
            for (int x = 0; x < 5; ++x)
            {
                _Instances[x, z]._islands = new List<Object>();
            }
        }
        */
    }

    public static void RandomGenerateMap(int mapSize, int islandTypes)
    {
        int id = 0;
        int type = 0;
        int count = 1;
        int countObject = 1;
        //Debug.Log(mapSize + " mapSize ");
        // Randomgenerates islands
        for (int z = 0; z < mapSize; z++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                id = 0;
                count = 1;
                //countObject = 1;
                Island[] islands = new Island[9];
                _map[x, z] = new MapPiece(islands, x, z);

                for (int islandZ = 0; islandZ < 3; islandZ++)
                {
                    for (int islandX = 0; islandX < 3; islandX++)
                    {
                        if (Random.Range(0, 5) == 1 && count < 4)
                        {
                            /*
                            type = Random.Range(1, islandTypes);
                            _map[x, z]._islands[id] = new Island(id, type, islandX, islandZ);
                            ++count;
                            */                         
                            if (Random.Range(0, 30) == 1 && countObject < 2)
                            {
                                _map[x, z]._islands[id] = new Island(id, 101, islandX, islandZ);
                                ++countObject;
                            }
                            else
                            {
                                type = Random.Range(1, islandTypes);
                                _map[x, z]._islands[id] = new Island(id, type, islandX, islandZ);
                                ++count;
                            }                

                        }
                        else
                        {
                            _map[x, z]._islands[id] = new Island(id, 0, islandX, islandZ);
                        }
                        ++id;

                    }
                }
            }
        }


        _map[0, 0]._islands[4]._type = 0;
        _map[2, 2]._islands[4]._type = 101; //Temp object loaction

        /*
        foreach (MapPiece pice in Map._map)
        {
            Debug.Log(pice._x + " " + pice._z + " x and y of water ");
            for (int o = 0; o < 9; ++o)
            {
                Debug.Log(pice._x + " " + pice._z + " x and y of water " + pice._islands[o]._x + " " + pice._islands[o]._z + " x and y of island " + pice._islands[o]._type);
            }
        }
        */
        
    }
}
