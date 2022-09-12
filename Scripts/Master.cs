// #2022 Maciej "Avritas" Jaszczuk
// Github: https://github.com/SomeOfNothingArts
// Itch: https://some-of-nothing-arts.itch.io

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Its just a master script to hold master variables
/// </summary>
public class Master : MonoBehaviour
{
    public static Master instance { get; private set; }

    private void Awake()
    {
        if (instance == null || instance == this)
            instance = this;
        else
            Destroy(gameObject);
    }

    public MasterRadio Radio;
}