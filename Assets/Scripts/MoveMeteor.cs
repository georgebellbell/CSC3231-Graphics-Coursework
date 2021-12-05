using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// class for moving meteor and causing some dynamic changes to the terrain
public class MoveMeteor : MonoBehaviour
{
    [SerializeField] Transform target;
    [SerializeField] float force;
    [SerializeField] GameObject explosion1, explosion2;
    [SerializeField] GameObject mainRockTerrain;

    Rigidbody rb;
    Vector3 direction;
    GameObject[] rocks;

    // When meteor is activated, force is added in a certain direction to collide with rock wall
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        direction = -(transform.position - target.transform.position);
        rb.AddForce(direction.normalized * force);

        rocks = GameObject.FindGameObjectsWithTag("Rock");
    }
   
    // As soon as it hits terrain, the meteor is destroyed
    private void OnCollisionEnter(Collision collision)
    {
        // Once the meteor hits the main rock wall, explosion is activated and main rock is destroyed
        if (collision.gameObject.CompareTag("Rock"))
        {
            explosion1.SetActive(true);
            //GetComponent<SphereCollider>().isTrigger = false;
            StartCoroutine(DestroyWall(mainRockTerrain));


            // then each of the smaller rocks have physics applied to them, in addition to the force from the meteor
            foreach (GameObject rock in rocks)
            {
                rock.GetComponent<Rigidbody>().useGravity = true;
                rock.GetComponent<Rigidbody>().AddExplosionForce(500f, collision.gameObject.transform.position, 50);
            }


        }


        if (collision.gameObject.CompareTag("Terrain"))
        {
            explosion2.SetActive(true);
            gameObject.SetActive(false);
        }

        
    }

    IEnumerator DestroyWall(GameObject wall)
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(wall);
        
    }
  
}

    

