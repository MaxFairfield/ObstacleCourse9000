using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    public GameObject gameManager;
    private GameManager gm;

    void Start()
    {
        if (gameManager)
        {
            gm = gameManager.GetComponent<GameManager>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.GetComponent<PlayerController>())
        {
            gm.FinishGame();
        }
    }
}
