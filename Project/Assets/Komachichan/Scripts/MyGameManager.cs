using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGameManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameObject.Find("Player").GetComponent<Player>().AddMovingCard(7);
        GameObject.Find("Player").GetComponent<Player>().SetCurrentSquare(GameObject.Find("Start").GetComponent<SquareBase>());
        GameObject.Find("Player").GetComponent<PlayerController>().Move();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
