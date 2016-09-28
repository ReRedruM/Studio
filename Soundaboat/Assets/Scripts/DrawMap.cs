using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class DrawMap : MonoBehaviour
{
    public static void DrawMapStart(int mapSize, GameObject water, GameObject[] Islands, float islandSizeX, float islandSizeZ, GameObject objective)
    {
        int type = 0;
        int i = 0;
        int j = 0;
        int spawn = 0;

        //Instantiate(objective, new Vector3((((2) * islandSizeX) + ((0) * (islandSizeX / 3))), 0, (((2) * islandSizeZ) + ((0) * (islandSizeZ / 3)))), Quaternion.identity);

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
                            if(type == 101)
                            {
                                Map._Instances[x + 2, z + 2, spawn + 1] = Instantiate(objective, new Vector3((((x) * islandSizeX) + ((l - 1) * (islandSizeX / 3))), 0, (((z) * islandSizeZ) + ((k - 1) * (islandSizeZ / 3)))), Quaternion.identity);

                            }
                            else
                            {
                                Map._Instances[x + 2, z + 2, spawn + 1] = Instantiate(Islands[type], new Vector3((((x) * islandSizeX) + ((l - 1) * (islandSizeX / 3))), 0, (((z) * islandSizeZ) + ((k - 1) * (islandSizeZ / 3)))), Quaternion.identity);

                            }
                        }
                        spawn++;

                       

                    }
                }

            }
            
        }
    }

    public static void DrawMapUp(int mapSize, GameObject water, Vector3 currentWaterPiece, int mapX, int mapZ, GameObject[] Islands, float islandSizeX, float islandSizeZ, GameObject objective)
    {
        int type = 0;
        int tileX;
        int tileZ;
        int spawn = 0;
        float drawPlaceZ = currentWaterPiece.z + (islandSizeZ * 3);
        //float drawPlaceX = currentWaterPiece.x;

        for (int x = -2; x < 3; ++x)
        {
            tileX = mapX;
            tileZ = mapZ + 3;
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

                    if (Map._Instances[x + 2, run + 1, insIsland] != null)
                    {
                        Map._Instances[x + 2, run, insIsland] = Map._Instances[x + 2, run + 1, insIsland];
                    }
                    else
                    {
                        Map._Instances[x + 2, run, insIsland] = null;
                    }
                    if (run == 3)
                    {
                        Map._Instances[x + 2, run + 1, insIsland] = null;
                    }
                }
            }     
                
            Map._Instances[x + 2, 4,0] = Instantiate(water, new Vector3(((x) * islandSizeX) + currentWaterPiece.x, 0, drawPlaceZ), Quaternion.identity);

            spawn = 0;

            if (tileZ >= mapSize)
            {
                tileZ = tileZ - mapSize;
            }
            if (tileZ < 0)
            {
                tileZ = tileZ + mapSize;
            }
            tileX = tileX + x;
            if (tileX >= mapSize)
            {
                tileX = tileX - mapSize;
            }
            if (tileX < 0)
            {
                tileX = tileX + mapSize;
            }
            //Debug.Log(tileX + "   " + tileZ + " x and Y Up ");

            for (int k = 0; k < 3; ++k)
            {
                for (int l = 0; l < 3; ++l)
                {
                    type = Map._map[tileX, tileZ]._islands[spawn]._type;
                    if (type != 0)
                    {
                        if (type == 101)
                        {
                            Map._Instances[x + 2, 4, spawn + 1] = Instantiate(objective, new Vector3((x * islandSizeX + currentWaterPiece.x) + ((l - 1) * (islandSizeX / 3)), 0, (drawPlaceZ) + ((k - 1) * (islandSizeZ / 3))), Quaternion.identity);
                        }
                        else
                        {
                            Map._Instances[x + 2, 4, spawn + 1] = Instantiate(Islands[type], new Vector3((x * islandSizeX + currentWaterPiece.x) + ((l - 1) * (islandSizeX / 3)), 0, (drawPlaceZ) + ((k - 1) * (islandSizeZ / 3))), Quaternion.identity);
                        }
                    }
                    spawn++;
                }
            }
        }
    }
    public static void DrawMapRight(int mapSize, GameObject water, Vector3 currentWaterPiece, int mapX, int mapZ, GameObject[] Islands, float islandSizeX, float islandSizeZ, GameObject objective)
    {
        int type = 0;
        int tileX;
        int tileZ;
        int spawn = 0;
        
        //float drawPlaceZ = currentWaterPiece.z;
        float drawPlaceX = currentWaterPiece.x + (islandSizeX * 3);

        for (int z = -2; z < 3; ++z)
        {
            tileX = mapX + 3;
            tileZ = mapZ;
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
                    if (Map._Instances[run + 1, z + 2, insIsland] != null)
                    {
                        Map._Instances[run, z + 2, insIsland] = Map._Instances[run + 1, z + 2, insIsland];
                    }
                    else
                    {
                        Map._Instances[run, z + 2, insIsland] = null;
                    }
                    if (run == 3)
                    {
                        Map._Instances[run + 1, z + 2, insIsland] = null;
                    }

                }
            }
            
            Map._Instances[4, z+2, 0] = Instantiate(water, new Vector3(drawPlaceX, 0, currentWaterPiece.z + (z * islandSizeZ)), Quaternion.identity);

            spawn = 0;

            tileZ = tileZ + z;

            if (tileZ >= mapSize)
            {
                tileZ = tileZ - mapSize;
            }
            if (tileZ < 0)
            {
                tileZ = tileZ + mapSize;
            }
            if (tileX >= mapSize)
            {
                tileX = tileX - mapSize;
            }
            if (tileX < 0)
            {
                tileX = tileX + mapSize;
            }
            for (int k = 0; k < 3; ++k)
            {
                for (int l = 0; l < 3; ++l)
                {
                    type = Map._map[tileX, tileZ]._islands[spawn]._type;
                    if (type != 0)
                    {
                        if (type == 101)
                        {
                            Map._Instances[4, z + 2, spawn + 1] = Instantiate(objective, new Vector3((drawPlaceX) + ((l - 1) * (islandSizeX / 3)), 0, (z * islandSizeZ + currentWaterPiece.z) + ((k - 1) * (islandSizeZ / 3))), Quaternion.identity);
                        }
                        else
                        {
                            Map._Instances[4, z + 2, spawn + 1] = Instantiate(Islands[type], new Vector3((drawPlaceX) + ((l - 1) * (islandSizeX / 3)), 0, (z * islandSizeZ + currentWaterPiece.z) + ((k - 1) * (islandSizeZ / 3))), Quaternion.identity);
                        }
                    }
                    spawn++;

                }
            }
        }
    }
    public static void DrawMapLeft(int mapSize, GameObject water, Vector3 currentWaterPiece, int mapX, int mapZ, GameObject[] Islands, float islandSizeX, float islandSizeZ, GameObject objective)
    {
        int type = 0;
        int tileX;
        int tileZ;
        int spawn = 0;


        //float drawPlaceZ = currentWaterPiece.z;
        float drawPlaceX = currentWaterPiece.x - (islandSizeX * 3);
        
        for (int z = -2; z < 3; ++z)
        {
            tileX = mapX - 3;
            tileZ = mapZ;
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
                    if (Map._Instances[3 - run, z + 2, insIsland] != null)
                    {
                        Map._Instances[4 - run, z + 2, insIsland] = Map._Instances[3 - run, z + 2, insIsland];
                    }
                    else
                    {
                        Map._Instances[4 - run, z + 2, insIsland] = null;
                    }
                    if (run == 3)
                    {
                        Map._Instances[3 - run, z + 2, insIsland] = null;
                    }

                }
            }
            
            Map._Instances[0, z + 2, 0] = Instantiate(water, new Vector3(drawPlaceX, 0, currentWaterPiece.z + (z * islandSizeZ)), Quaternion.identity);
            spawn = 0;
            tileZ = tileZ + z;
            if (tileZ >= mapSize)
            {
                tileZ = tileZ - mapSize;
            }
            if (tileZ < 0)
            {
                tileZ = tileZ + mapSize;
            }

            if (tileX >= mapSize)
            {
                tileX = tileX - mapSize;
            }
            if (tileX < 0)
            {
                tileX = tileX + mapSize;
            }

            for (int k = 0; k < 3; ++k)
            {
                for (int l = 0; l < 3; ++l)
                {
                    type = Map._map[tileX, tileZ]._islands[spawn]._type;
                    if (type != 0)
                    {
                        if (type == 101)
                        {
                            Map._Instances[0, z + 2, spawn + 1] = Instantiate(objective, new Vector3((drawPlaceX) + ((l - 1) * (islandSizeX / 3)), 0, (z * islandSizeZ + currentWaterPiece.z) + ((k - 1) * (islandSizeZ / 3))), Quaternion.identity);
                        }
                        else
                        {
                            Map._Instances[0, z + 2, spawn + 1] = Instantiate(Islands[type], new Vector3((drawPlaceX) + ((l - 1) * (islandSizeX / 3)), 0, (z * islandSizeZ + currentWaterPiece.z) + ((k - 1) * (islandSizeZ / 3))), Quaternion.identity);
                        }
                    }
                    spawn++;

                }
            }
        }
    }
    public static void DrawMapDown(int mapSize, GameObject water, Vector3 currentWaterPiece, int mapX, int mapZ, GameObject[] Islands, float islandSizeX, float islandSizeZ, GameObject objective)
    {
        int type = 0;
        int tileX;
        int tileZ;
        int spawn = 0;
        float drawPlaceZ = currentWaterPiece.z - (islandSizeZ * 3);
        //float drawPlaceX = currentWaterPiece.x;


        for (int x = -2; x < 3; ++x)
        {
            tileX = mapX;
            tileZ = mapZ - 3;
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
                    if (Map._Instances[x + 2, 3 - run, insIsland] != null)
                    {
                        Map._Instances[x + 2, 4 - run, insIsland] = Map._Instances[x + 2, 3 - run, insIsland];
                    }
                    else
                    {
                        Map._Instances[x + 2, 4 - run, insIsland] = null;
                    }
                    if (run == 3)
                    {
                        Map._Instances[x + 2, 3 - run, insIsland] = null;
                    }
                }
            }
            Map._Instances[x + 2, 0, 0] = Instantiate(water, new Vector3(((x) * islandSizeX) + currentWaterPiece.x, 0, drawPlaceZ), Quaternion.identity);
            spawn = 0;
            tileX = tileX + x;
            if (tileZ >= mapSize)
            {
                tileZ = tileZ - mapSize;
            }
            if (tileZ < 0)
            {
                tileZ = tileZ + mapSize;
            }

            if (tileX >= mapSize)
            {
                tileX = tileX - mapSize;
            }
            if (tileX < 0)
            {
                tileX = tileX + mapSize;
            }
            //Debug.Log(tileX + "   " + tileZ + " x and Y  down");
            for (int k = 0; k < 3; ++k)
            {
                for (int l = 0; l < 3; ++l)
                {
                    type = Map._map[tileX, tileZ]._islands[spawn]._type;
                    if (type != 0)
                    {
                        if (type == 101)
                        {
                            Map._Instances[x + 2, 0, spawn + 1] = Instantiate(objective, new Vector3((x * islandSizeX + currentWaterPiece.x) + ((l - 1) * (islandSizeX / 3)), 0, (drawPlaceZ) + ((k - 1) * (islandSizeZ / 3))), Quaternion.identity);
                        }
                        else
                        {
                            Map._Instances[x + 2, 0, spawn + 1] = Instantiate(Islands[type], new Vector3((x * islandSizeX + currentWaterPiece.x) + ((l - 1) * (islandSizeX / 3)), 0, (drawPlaceZ) + ((k - 1) * (islandSizeZ / 3))), Quaternion.identity);
                        }
                    }
                    spawn++;
                }
            }
        }
    }
}
