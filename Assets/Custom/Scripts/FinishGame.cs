using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishGame : MonoBehaviour
{
    public JellyKara pl;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Finish")
            pl.GameFinish();
    }
}
