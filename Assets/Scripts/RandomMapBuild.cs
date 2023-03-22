using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMapBuild : MonoBehaviour
{
    public GameObject floor;
    public GameObject hightFloor;
    public float sizeNumber;
    private GameObject block;

    // Start is called before the first frame update
    void Start()
    {
        for (float x = -5; x <= 5; x++)
        {
            for (float y = 5; y >= -5; y--)
            {
                if (x <= -sizeNumber || x >= sizeNumber || y <= -sizeNumber || y >= sizeNumber)
                    block = Instantiate(hightFloor, transform);
                else
                    block = Instantiate(floor, transform);
                block.transform.position = new Vector3(x, y);
            }
        }
    }

}
