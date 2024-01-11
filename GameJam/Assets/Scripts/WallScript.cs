using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallScript : MonoBehaviour
{
    public float speed = 10.0f;

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-speed * Time.deltaTime, 0, 0);

        if (transform.position.x <= -18)
        {
            transform.position = new Vector3(transform.position.x + 2 * 27, transform.position.y, transform.position.z);
        }
    }
}