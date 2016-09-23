using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NewWaterCollider : MonoBehaviour {
    public Transform Water;
    
    // Use this for initialization
    void OnCollisionEnter(Collision collision)
    {
        //new Vector3() currentSlice = (transform.position.x, 0, (transform.position.z + 10));
        if (collision.gameObject.tag == "Player")
        {

            Instantiate(Water, new Vector3(transform.position.x, 0, (transform.position.z + 10)), Quaternion.identity);
            Instantiate(Water, new Vector3(transform.position.x+10, 0, (transform.position.z + 10)), Quaternion.identity);
            Instantiate(Water, new Vector3(transform.position.x-10, 0, (transform.position.z + 10)), Quaternion.identity);
            /*
            Debug.Log(Map.map[0].transform.position.x);
            //Debug.Log("There Was a File");
            //Instantiate(Water, new Vector3(0, 0, gameObject.transform.position.z + 10), Quaternion.identity);
            foreach (GameObject mapSlice in Map.map)
            {
                if (mapSlice != null)
                {
                    float x = mapSlice.transform.position.x;
                    if (x == 10)//!= transform.position.x && mapSlice.transform.position.z != (transform.position.z + 10))// transform.position.x && map_slice.transform.position.x != transform.position.z+10)
                    {
                        Instantiate(Water, new Vector3(transform.position.x, 0, (transform.position.z + 10)), Quaternion.identity);
                    }
                }

            }
            */
        }
        
        

    }
}
