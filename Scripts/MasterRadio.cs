// #2022 Maciej "Avritas" Jaszczuk
// Github: https://github.com/SomeOfNothingArts
// Itch: https://some-of-nothing-arts.itch.io

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// This is key radio script
/// It can take as many radio stations as you want
/// 
/// Radio Name - just a name
/// Frequency - on which frequency radio is set - signal clearance depends on how far radio frequency is from radio station frequency
/// Song - list of songs which radio stations can play
/// 
/// CurrentSongId and NextSongIn can be left empty
/// </summary>
[Serializable]
struct RadioStation
{
    public string RadioName;
    [Range(90f,120f)]
    public float RadioFrequency;
    public AudioClip[] Song;

    public int CurrentSongId;
    public float NextSongIn;
}

public class MasterRadio : MonoBehaviour
{
    public bool RadioIsOn;

    /// <summary>
    /// Volume 0 - muted, 1 - full
    /// Frequency is ranged between 90 and 120 but feel free to play with it
    /// AS - Is radio audio source
    /// NoiseSource - is audio source which play noise clip
    /// </summary>
    [Range(0f, 1f)]
    public float Volume = 0;
    [Range(90f, 120f)]
    public float Frequency = 90;

    public AudioSource AS;
    public AudioSource NoiseSource;

    [SerializeField]
    RadioStation[] Station;

    /// <summary>
    /// number of seconds after start of scene - every song change occures when is greater then time when song started playing + its length
    /// </summary>
    public float TimePassed;

    private void Start()
    {
        InvokeRepeating("Second", 0, 1f);
    }

    public void TurnRadio()
    {
        if (!RadioIsOn)
        {
            RadioIsOn = true;
            AS.mute = false;
            NoiseSource.mute = false;

            ChangeRadio();
        }
        else
        {
            RadioIsOn = false;
            AS.mute = true;
            NoiseSource.mute = true;
        }
}


    /// <summary>
    /// Adding second to every radio song to be able to switching in progress of playing same songs on diffrent stages
    /// </summary>
private void Second()
    {
        TimePassed += 1;

        for (int a = 0; a < Station.Length; a++)
        {
            if (TimePassed >= Station[a].NextSongIn)
            {
                PlayNextSong(a);
            }
        }
    }

    /// <summary>
    /// Changing radio and its clerance
    /// </summary>
    public void ChangeRadio()
    {
        int closestId = ClosestRadioId(Frequency);
        float signalClearance = SignalClearance(closestId, Frequency);

        AS.volume = (signalClearance * Volume) * 1;
        NoiseSource.volume = (Mathf.Abs(signalClearance - 1) * Volume) * 1;

        if (AS.clip != Station[closestId].Song[Station[closestId].CurrentSongId])
        {
            AS.clip = Station[closestId].Song[Station[closestId].CurrentSongId];
            AS.PlayDelayed(0);
            AS.time = AS.clip.length - Mathf.Abs(Station[closestId].NextSongIn - TimePassed);
        }
    }

    /// <summary>
    /// Calculeting which radio station is closes one (you cant catch two stations in same time - you can surely play with code and make it possible)
    /// </summary>
    int ClosestRadioId(float frequency)
    {
        int id = 0;
        float distance = 100;

        for (int a = 0; a < Station.Length; a++)
        {
            float newDistance = Mathf.Abs(Station[a].RadioFrequency - frequency);
            if (newDistance < distance)
            {
                distance = newDistance;
                id = a;
            }
        }

        return id;
    }

    /// <summary>
    /// Calculeting signal clearanc for closest radfio station
    /// </summary>
    float SignalClearance(int radioId, float targetFrequency)
    {
        float clerance;
        float frequency = Station[radioId].RadioFrequency;

        if (Mathf.Abs(frequency - targetFrequency) < 2)
        {
            clerance = 1 - (Mathf.Abs(frequency - targetFrequency) / 2);
        }
        else
        {
            clerance = 0;
        }

        return clerance;
    }

    /// <summary>
    /// Changing song in radio which song has ended
    /// </summary>
    void PlayNextSong(int station)
    {
        bool shouldChangeSong = false;
        if (AS.clip == Station[station].Song[Station[station].CurrentSongId])
        {
            shouldChangeSong = true;
        }

        Station[station].CurrentSongId = UnityEngine.Random.Range(0, Station[station].Song.Length);

        Station[station].NextSongIn = TimePassed + Station[station].Song[Station[station].CurrentSongId].length;

        if (shouldChangeSong)
        {
            AS.clip = Station[station].Song[Station[station].CurrentSongId];
            AS.PlayDelayed(0);
        }

    }
}