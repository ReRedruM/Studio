using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DrawMap : MonoBehaviour
{
    public static void DrawMapStart(int mapSize, GameObject water, GameObject[] Islands, float islandSizeX, float islandSizeZ)
    {
        int type = 0;
        int i = 0;
        int j = 0;
        int spawn = 0;
        for (int z = -2; z < 3; ++z)
        {
            for (int x = -2; x < 3; ++x)
            {
                Map._Instances[x+2, z+2, 0] = Instantiate(water, new Vector3((x) * islandSizeX, 0, (z) * islandSizeZ), Quaternion.identity);

                spawn = 0;
                for (int k = 0; k < 3; ++k)
                {
                    for (int l = 0; l < 3; ++l)
                    {
                        j = x; i = z;
                        if (x < 0)
                        {
                            j = mapSize + x;
                        }
                        if (z < 0)
                        {
                            i = mapSize + z;
                        }
        
                        type = Map._map[j, i]._islands[spawn]._type;
                        if (type != 0)
                        {
                            Map._Instances[x + 2, z + 2, spawn+1] = Instantiate(Islands[type], new Vector3((((x) * islandSizeX) + ((l - 1) * (islandSizeX / 3))), 0, (((z) * islandSizeZ) + ((k - 1) * (islandSizeZ / 3)))), Quaternion.identity);
                        }
                        spawn++;

                    }
                }

            }
        }
    }

    public static void DrawMapUp(int mapSize, GameObject water, Vector3 currentWaterPiece, GameObject[] Islands, float islandSizeX, float islandSizeZ)
    {
        int type = 0;
        float tempZ = 0;
        float tempX = 0;
        int placeZ = 0;
        int placeX = 0;
        int placeZtemp = 0;
        int placeXtemp = 0;
        int spawn = 0;
        float drawPlaceZ = currentWaterPiece.z + (islandSizeZ * 3);
        float drawPlaceX = currentWaterPiece.x;

        tempZ = Mathf.Abs(drawPlaceZ) / (mapSize * islandSizeZ);
        placeZ = (int)(Mathf.Abs(drawPlaceZ) / islandSizeZ) - mapSize * (int)tempZ;

        tempX = Mathf.Abs(drawPlaceX) / (mapSize * islandSizeX);
        placeX = (int)(Mathf.Abs(drawPlaceX) / islandSizeX) - mapSize * (int)tempX;

        placeXtemp = placeX;
        placeZtemp = placeZ;

        if (placeZtemp > mapSize - 1) { placeZtemp = placeZtemp - mapSize; }

        for (int x = -2; x < 3; ++x)
        {
            for (int delIsland = 1; delIsland < 10; ++delIsland)
            {
                if(Map._Instances[x + 2, 0, delIsland] != null)
                {
                    Destroy(Map._Instances[x + 2, 0, delIsland]);
                }
            }
            Destroy(Map._Instances[x + 2, 0, 0]);

            for (int run = 0; run < 4; ++run){               
                Map._Instances[x + 2, run, 0] = Map._Instances[x + 2, run + 1, 0];
                for (int insIsland = 1; insIsland < 10; ++insIsland)
                {
                    Map._Instances[x + 2, run, insIsland] = Map._Instances[x + 2, run + 1, insIsland];
                }
            }            
            Map._Instances[x + 2, 4,0] = Instantiate(water, new Vector3(((x) * islandSizeX) + currentWaterPiece.x, 0, drawPlaceZ), Quaternion.identity);

            spawn = 0;

            placeXtemp = placeX + x;
            if (placeXtemp > mapSize - 1)
            {
                placeXtemp = placeXtemp - mapSize;
            }
            if (placeXtemp < 0)
            {
                placeXtemp = placeXtemp + mapSize;
            }

            for (int k = 0; k < 3; ++k)
            {
                for (int l = 0; l < 3; ++l)
                {
                    type = Map._map[placeXtemp, placeZtemp]._islands[spawn]._type;
                    if (type != 0)
                    {
                        Map._Instances[x + 2, 4, spawn+1] = Instantiate(Islands[type], new Vector3((x * islandSizeX + currentWaterPiece.x) + ((l - 1) * (islandSizeX / 3)), 0, (drawPlaceZ) + ((k - 1) * (islandSizeZ / 3))), Quaternion.identity);
                    }
                    spawn++;
                }
            }
        }
    }
    public static void DrawMapRight(int mapSize, GameObject water, Vector3 currentWaterPiece, GameObject[] Islands, float islandSizeX, float islandSizeZ)
    {
        int type = 0;
        float tempZ = 0;
        float tempX = 0;
        int placeZ = 0;
        int placeX = 0;
        int placeZtemp = 0;
        int placeXtemp = 0;
        int spawn = 0;
        float drawPlaceZ = currentWaterPiece.z;
        float drawPlaceX = currentWaterPiece.x + (islandSizeZ * 3);

        tempZ = Mathf.Abs(drawPlaceZ) / (mapSize * islandSizeZ);
        placeZ = (int)(Mathf.Abs(drawPlaceZ) / islandSizeZ) - mapSize * (int)tempZ;

        tempX = Mathf.Abs(drawPlaceX) / (mapSize * islandSizeX);
        placeX = (int)(Mathf.Abs(drawPlaceX) / islandSizeX) - mapSize * (int)tempX;

        placeXtemp = placeX;
        placeZtemp = placeZ;

        if (placeXtemp > mapSize - 1) { placeXtemp = placeXtemp - mapSize; }

        for (int z = -2; z < 3; ++z)
        {
            for (int delIsland = 1; delIsland < 10; ++delIsland)
            {
                if (Map._Instances[0, z+2, delIsland] != null)
                {
                    Destroy(Map._Instances[0, z+2, delIsland]);
                }
            }
            Destroy(Map._Instances[0, z+2, 0]);
            for (int run = 0; run < 4; ++run)
            {
                Map._Instances[run, z+2, 0] = Map._Instances[run+1, z+2, 0];
                for (int insIsland = 1; insIsland < 10; ++insIsland)
                {
                    Map._Instances[run, z+2, insIsland] = Map._Instances[run+1, z+2, insIsland];
                }
            }
            Map._Instances[4, z+2, 0] = Instantiate(water, new Vector3(drawPlaceX, 0, currentWaterPiece.z + (z * islandSizeZ)), Quaternion.identity);

            spawn = 0;

            placeZtemp = placeZ + z;
            if (placeZtemp > mapSize - 1)
            {
                placeZtemp = placeZtemp - mapSize;
            }
            if (placeZtemp < 0)
            {
                placeZtemp = placeZtemp + mapSize;
            }

            for (int k = 0; k < 3; ++k)
            {
                for (int l = 0; l < 3; ++l)
                {
                    type = Map._map[placeXtemp, placeZtemp]._islands[spawn]._type;
                    if (type != 0)
                    {
                        Map._Instances[4, z + 2, spawn+1] = Instantiate(Islands[type], new Vector3((drawPlaceX) + ((l - 1) * (islandSizeX / 3)), 0, (z * islandSizeZ + currentWaterPiece.z) + ((k - 1) * (islandSizeZ / 3))), Quaternion.identity);
                    }
                    spawn++;

                }
            }
        }
    }
    public static void DrawMapLeft(int mapSize, GameObject water, Vector3 currentWaterPiece, GameObject[] Islands, float islandSizeX, float islandSizeZ)
    {
        //Not yet dobe removal
        int type = 0;
        float tempZ = 0;
        float tempX = 0;
        int placeZ = 0;
        int placeX = 0;
        int placeZtemp = 0;
        int placeXtemp = 0;
        int spawn = 0;
        float drawPlaceZ = currentWaterPiece.z;
        float drawPlaceX = currentWaterPiece.x - (islandSizeZ * 3);

        tempZ = Mathf.Abs(drawPlaceZ) / (mapSize * islandSizeZ);
        placeZ = (int)(Mathf.Abs(drawPlaceZ) / islandSizeZ) - mapSize * (int)tempZ;

        tempX = Mathf.Abs(drawPlaceX) / (mapSize * islandSizeX);
        placeX = (int)(Mathf.Abs(drawPlaceX) / islandSizeX) - mapSize * (int)tempX;

        placeXtemp = mapSize - placeX;
        placeZtemp = placeZ;

        //Debug.Log(placeZtemp + " placeZtemp 1 ");
        if (placeXtemp > mapSize - 1) { placeXtemp = placeXtemp - mapSize; }

        for (int z = -2; z < 3; ++z)
        {
            for (int delIsland = 1; delIsland < 10; ++delIsland)
            {
                if (Map._Instances[4, z + 2, delIsland] != null)
                {
                    Destroy(Map._Instances[4, z + 2, delIsland]);
                }
            }
            Destroy(Map._Instances[4, z + 2, 0]);
            for (int run = 0; run < 4; ++run)
            {
                Map._Instances[4-run, z + 2, 0] = Map._Instances[3 - run, z + 2, 0];
                for (int insIsland = 1; insIsland < 10; ++insIsland)
                {
                    Map._Instances[4 - run, z + 2, insIsland] = Map._Instances[3 - run, z + 2, insIsland];
                }
            }
            Map._Instances[0, z + 2, 0] = Instantiate(water, new Vector3(drawPlaceX, 0, currentWaterPiece.z + (z * islandSizeZ)), Quaternion.identity);
            spawn = 0;

            placeZtemp = placeZ + z;
            if (placeZtemp > mapSize - 1)
            {
                placeZtemp = placeZtemp - mapSize;
            }
            if (placeZtemp < 0)
            {
                placeZtemp = placeZtemp + mapSize;
            }

            for (int k = 0; k < 3; ++k)
            {
                for (int l = 0; l < 3; ++l)
                {
                    //Debug.Log(placeXtemp + " placeXtemp 1 ");
                    type = Map._map[placeXtemp, placeZtemp]._islands[spawn]._type;
                    if (type != 0)
                    {
                        Map._Instances[0, z + 2, spawn+1] = Instantiate(Islands[type], new Vector3((drawPlaceX) + ((l - 1) * (islandSizeX / 3)), 0, (z * islandSizeZ + currentWaterPiece.z) + ((k - 1) * (islandSizeZ / 3))), Quaternion.identity);
                    }
                    spawn++;

                }
            }
        }
    }
    public static void DrawMapDown(int mapSize, GameObject water, Vector3 currentWaterPiece, GameObject[] Islands, float islandSizeX, float islandSizeZ)
    {
        //Not yet dobe removal
        int type = 0;
        float tempZ = 0;
        float tempX = 0;
        int placeZ = 0;
        int placeX = 0;
        int placeZtemp = 0;
        int placeXtemp = 0;
        int spawn = 0;
        float drawPlaceZ = currentWaterPiece.z - (islandSizeZ * 3);
        float drawPlaceX = currentWaterPiece.x;

        tempZ = Mathf.Abs(drawPlaceZ) / (mapSize * islandSizeZ);
        placeZ = (int)(Mathf.Abs(drawPlaceZ) / islandSizeZ) - mapSize * (int)tempZ;

        tempX = Mathf.Abs(drawPlaceX) / (mapSize * islandSizeX);
        placeX = (int)(Mathf.Abs(drawPlaceX) / islandSizeX) - mapSize * (int)tempX;

        placeXtemp = placeX;
        placeZtemp = mapSize - placeZ;

        if (placeZtemp > mapSize - 1) { placeZtemp = placeZtemp - mapSize; }

        for (int x = -2; x < 3; ++x)
        {
            for (int delIsland = 1; delIsland < 10; ++delIsland)
            {
                if (Map._Instances[x + 2, 4, delIsland] != null)
                {
                    Destroy(Map._Instances[x + 2, 4, delIsland]);
                }
            }
            Destroy(Map._Instances[x + 2, 4, 0]);

            for (int run = 0; run < 4; ++run)
            {
                Map._Instances[x + 2, 4-run, 0] = Map._Instances[x + 2, 3-run, 0];
                for (int insIsland = 1; insIsland < 10; ++insIsland)
                {
                    Map._Instances[x + 2, 4-run, insIsland] = Map._Instances[x + 2, 3-run, insIsland];
                }
            }
            Map._Instances[x + 2, 0, 0] = Instantiate(water, new Vector3(((x) * islandSizeX) + currentWaterPiece.x, 0, drawPlaceZ), Quaternion.identity);
            spawn = 0;

            placeXtemp = placeX + x;
            if (placeXtemp > mapSize - 1)
            {
                placeXtemp = placeXtemp - mapSize;
            }
            if (placeXtemp < 0)
            {
                placeXtemp = placeXtemp + mapSize;
            }

            for (int k = 0; k < 3; ++k)
            {
                for (int l = 0; l < 3; ++l)
                {
                    //Debug.Log(placeXtemp + "  " + placeZtemp);
                    type = Map._map[placeXtemp, placeZtemp]._islands[spawn]._type;
                    if (type != 0)
                    {
                        Map._Instances[x + 2, 0, spawn + 1] = Instantiate(Islands[type], new Vector3((x * islandSizeX + currentWaterPiece.x) + ((l - 1) * (islandSizeX / 3)), 0, (drawPlaceZ) + ((k - 1) * (islandSizeZ / 3))), Quaternion.identity);
                    }
                    spawn++;
                }
            }
        }
    }
}
