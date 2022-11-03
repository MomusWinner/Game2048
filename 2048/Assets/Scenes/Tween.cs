using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tween : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Image image = GetComponent<Image>();

        transform.DOMove(new Vector2(277, 602), 1);

        image.DOColor(Color.green, 1);

        transform.DOMove(new Vector2(250, 500), 1);
    }

    void Update()
    {
        
    }
}
