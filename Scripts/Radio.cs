// #2022 Maciej "Avritas" Jaszczuk
// Github: https://github.com/SomeOfNothingArts
// Itch: https://some-of-nothing-arts.itch.io

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Radio : MonoBehaviour
{
    public bool RadioChangingInProgress;

    public GameObject FrequencySlider;
    public GameObject VolumeSlider;

    private void Update()
    {
        if (RadioChangingInProgress)
        {
            ChangeRadioVariables();
        }
    }

    /// <summary>
    /// Changich radio volume and frequency
    /// </summary>
    void ChangeRadioVariables()
    {
        Master.instance.Radio.Volume = VolumeSlider.GetComponent<Slider>().value;
        Master.instance.Radio.Frequency = 90 + 30 * FrequencySlider.GetComponent<Slider>().value;

        Master.instance.Radio.ChangeRadio();
    }

    /// <summary>
    ///  activeting changing radio variables in update function if slider of volume or frequency is holded/moved
    /// </summary>
    public void SwitchProgress(bool t)
    {
        RadioChangingInProgress = t;
    }
}
