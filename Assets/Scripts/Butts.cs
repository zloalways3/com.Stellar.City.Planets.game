using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Butts : MonoBehaviour
{
    public Game game;
    public int color;
    public int pos;


    public void OnButtonClick()
    {
        game.Button(pos);
        
    }
}
