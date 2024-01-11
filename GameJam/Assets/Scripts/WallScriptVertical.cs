using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScriptVertical : MonoBehaviour
{
    public float speed = 10.0f;
   

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-speed * Time.deltaTime, 0, 0);

        if (transform.position.x <= -3)
        {
            float newYPos = Random.Range(0, 9);
            transform.position = new Vector3(transform.position.x + 2 * 18, newYPos, transform.position.z);
        }
    }
}