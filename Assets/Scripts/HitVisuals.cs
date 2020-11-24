using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitVisuals : MonoBehaviour
{
    [SerializeField] GameObject ThisVisual;
    [SerializeField] GameObject ThatVisual;
    private float timer = 0.2f;
    private bool exitTimer;

    void OnTriggerEnter2D(Collider2D other)
    {
        timer = 0.2f;
        if (ThatVisual.activeInHierarchy)
        {
            print("other is on");
            return;
        }
        else { ThisVisual.SetActive(true); }
       
        
    }

    private void Update()
    {
        if (exitTimer)
        {
            timer -= Time.deltaTime;
            if (timer < 0)
            {
                ThisVisual.SetActive(false);
                timer = 0.2f;
                exitTimer = false;
            }
        }
    }



    void OnTriggerExit2D(Collider2D other)
    {
        exitTimer = true;
    }

}
