using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KModkit;

public class dontTouchAnything : MonoBehaviour
{
    public KMAudio Audio;
    public KMBombModule Module;
    public AudioSource Sounds;
    public KMBombInfo Bomb;
    public KMBossModule Boss;
    public TextMesh[] Text;
    public Color[] Colors;

    public KMSelectable Button;
    private int Stage;
    private int MaxStage;
    decimal TimeLeft = 999m;
    int FakeStrikes;
    int StrikeSound;
    public AudioClip[] SFX;

    static int moduleIdCounter = 1;
    int moduleId;
    private bool moduleSolved;
    private bool strikeBypass;
    private bool Downcount;
    private string[] Ignored = { "Don't Touch Anything" };

    void Awake()
    {
        moduleId = moduleIdCounter++;
        if (Boss.GetIgnoredModules(Module, Ignored) != null)
        {
            Ignored = Boss.GetIgnoredModules(Module, Ignored);
        }
        Button.OnInteract += delegate () { PressButton(); return false; };
    }

    void Start()
    {
        MaxStage = Bomb.GetSolvableModuleNames().Where(a => !Ignored.Contains(a)).Count();
        Debug.LogFormat("[Don't Touch Anything #{0}]: The button must be pushed at [ERROR: VALID TIME NOT FOUND]", moduleId);
    }
    void PressButton()
    {
        if (!moduleSolved)
        {
            if (strikeBypass == true)
            {
                Module.HandleStrike();
            }
            StopAllCoroutines();
            StartCoroutine(StrikeChecker());
        }
    }

    void Update()
    {
        if (Downcount == false)
        {
            if (Stage != MaxStage)
            {
                StartCoroutine(FakeStrikeChecker());
            }
        }
    }

    IEnumerator StrikeChecker()
    {
        StrikeSound = UnityEngine.Random.Range(0, 20);
        if (StrikeSound == 0)
        {
            Sounds.clip = SFX[4];
            Sounds.Play();
            strikeBypass = true;
            Text[0].text = "NO!";
            Text[0].characterSize = 0.35f;
            Text[1].text = "THAT";
            Text[1].color = Colors[1];
            Text[2].text = "WAS";
            Text[2].color = Colors[1];
            Text[3].text = "WRONG";
            Text[3].color = Colors[1];
            Debug.LogFormat("[Don't Touch Anything #{0}]: You failed to press the button at the correct time. The module is disappointed, and struck.", moduleId);
            for(int i=0; i<17; i++)
            {
                Text[0].color = Colors[0];
                yield return new WaitForSeconds(0.025f);
                Text[0].color = Colors[1];
                yield return new WaitForSeconds(0.025f);
            }
            Text[0].characterSize = 0.25f;
            Text[0].text = "NO";
            yield return new WaitForSeconds(0.25f);
            Text[0].characterSize = 0.15f;
            Text[0].text = "N";
            yield return new WaitForSeconds(0.25f);
            Module.HandleStrike();
            strikeBypass = false;
            Text[0].characterSize = 0.4f;
            Text[0].text = ".";
            Text[1].text = "";
            Text[2].text = "";
            Text[3].text = "";
            yield return new WaitForSeconds(0.025f);
            Text[0].characterSize = 0.35f;
            Text[0].text = "..";
            Text[0].text += System.Environment.NewLine + "..";
            yield return new WaitForSeconds(0.025f);
            Text[0].characterSize = 0.3f;
            Text[0].text = ". .";
            Text[0].text += System.Environment.NewLine + ".";
            Text[0].text += System.Environment.NewLine + ". .";
            yield return new WaitForSeconds(0.025f);
            Text[0].characterSize = 0.25f;
            Text[0].text = ".  .";
            Text[0].text += System.Environment.NewLine + "..";
            Text[0].text += System.Environment.NewLine + "..";
            Text[0].text += System.Environment.NewLine + ".  .";
            yield return new WaitForSeconds(0.025f);
            Text[0].text = "";
            yield return new WaitForSeconds(1.5f);
        }
        else
        {
            Sounds.clip = SFX[0];
            Sounds.Play();
            strikeBypass = true;
            Text[0].characterSize = 0.35f;
            Text[0].text = "NO!";
            Text[1].text = "THAT";
            Text[1].color = Colors[1];
            Text[2].text = "WAS";
            Text[2].color = Colors[1];
            Text[3].text = "WRONG";
            Text[3].color = Colors[1];
            Debug.LogFormat("[Don't Touch Anything #{0}]: You failed to press the button at the correct time. The module is disappointed, and struck.", moduleId);
            for (int i = 0; i < 17; i++)
            {
                Text[0].color = Colors[0];
                yield return new WaitForSeconds(0.025f);
                Text[0].color = Colors[1];
                yield return new WaitForSeconds(0.025f);
            }
            Module.HandleStrike();
            strikeBypass = false;
            yield return new WaitForSeconds(0.01f);
            Text[1].color = Colors[2];
            Text[2].color = Colors[2];
            Text[3].color = Colors[2];
            for (int i = 0; i < 35; i++)
            {
                Text[0].characterSize = Text[0].characterSize - 0.01f;
                yield return new WaitForSeconds(0.015f);
            }
            yield return new WaitForSeconds(0.285f);
        }
        StartCoroutine(Respawn());
    }
    IEnumerator TheCountdown()
    {
        Sounds.clip = SFX[3];
        Sounds.Play();
        Downcount = true;
        yield return new WaitForSeconds(2.4f);
        Text[1].text = "PUSH";
        Text[2].text = "IT";
        Text[3].text = "NOW";
        Text[0].characterSize = 0.35f;
        Text[0].text = TimeLeft.ToString();
        yield return new WaitForSeconds(0.1674f);
           while (TimeLeft > 0)
            {
            TimeLeft = TimeLeft - 1m;
            if (TimeLeft < 10m)
            {
                Text[0].text = "00" + TimeLeft.ToString();
            }
            else if (TimeLeft < 100m)
            {
                Text[0].text = "0" + TimeLeft.ToString();
            }
            else
                Text[0].text = TimeLeft.ToString();
            Text[0].color = Colors[0];
            yield return new WaitForSeconds(0.05f);
            Text[0].color = Colors[1];
            yield return new WaitForSeconds(0.1174f);
            TimeLeft = TimeLeft - 1m;
            if (TimeLeft < 10m)
            {
                Text[0].text = "00" + TimeLeft.ToString();
            }
            else if (TimeLeft < 100m)
            {
                Text[0].text = "0" + TimeLeft.ToString();
            }
            else
                Text[0].text = TimeLeft.ToString();
            yield return new WaitForSeconds(0.1674f);
            TimeLeft = TimeLeft - 1m;
            if (TimeLeft < 10m)
            {
                Text[0].text = "00" + TimeLeft.ToString();
            }
            else if (TimeLeft < 100m)
            {
                Text[0].text = "0" + TimeLeft.ToString();
            }
            else
                Text[0].text = TimeLeft.ToString();
            yield return new WaitForSeconds(0.1674f);
        }
        Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.Strike, Module.transform);
        Sounds.Stop();
        yield return new WaitForSeconds(2f);
        Text[0].text = "OH";
        Text[0].characterSize = 0.4f;
        Text[1].text = "SORRY,";
        Text[2].text = "FALSE";
        Text[3].text = "ALARM";
        yield return new WaitForSeconds(2.5f);
        Text[0].text = "";
        Text[1].text = "";
        Text[2].text = "";
        Text[3].text = "";
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(Respawn());
    }
    IEnumerator Respawn()
    {
        Sounds.clip = SFX[2];
        Sounds.Play();
        TimeLeft = 1000m;
        Downcount = false;
        Text[0].color = Colors[1];
        Text[1].color = Colors[1];
        Text[2].color = Colors[1];
        Text[3].color = Colors[1];
        Text[0].characterSize = 0.25f;
        Text[0].text = "";
        Text[1].text = "P";
        Text[2].text = "P";
        Text[3].text = "P";
        yield return new WaitForSeconds(0.07f);
        Text[1].text = "PU";
        Text[2].text = "PU";
        Text[3].text = "PU";
        yield return new WaitForSeconds(0.07f);
        Text[1].text = "PUS";
        Text[2].text = "PUS";
        Text[3].text = "PUS";
        yield return new WaitForSeconds(0.07f);
        Text[1].text = "PUSH";
        Text[2].text = "PUSH";
        Text[3].text = "PUSH";
        yield return new WaitForSeconds(0.07f);
        Text[1].text = "PUSH T";
        Text[2].text = "PUSH";
        Text[2].text += System.Environment.NewLine + "T";
        Text[3].text = "PUSH";
        Text[3].text += System.Environment.NewLine + "T";
        yield return new WaitForSeconds(0.07f);
        Text[1].text = "PUSH TH";
        Text[2].text = "PUSH";
        Text[2].text += System.Environment.NewLine + "TH";
        Text[3].text = "PUSH";
        Text[3].text += System.Environment.NewLine + "TH";
        yield return new WaitForSeconds(0.07f);
        Text[1].text = "PUSH THE";
        Text[2].text = "PUSH";
        Text[2].text += System.Environment.NewLine + "THE";
        Text[3].text = "PUSH";
        Text[3].text += System.Environment.NewLine + "THE";
        yield return new WaitForSeconds(0.07f);
        Text[1].text = "PUSH THE";
        Text[1].text += System.Environment.NewLine + "B";
        Text[2].text = "PUSH";
        Text[2].text += System.Environment.NewLine + "THE B";
        Text[3].text = "PUSH";
        Text[3].text += System.Environment.NewLine + "THE B";
        yield return new WaitForSeconds(0.07f);
        Text[1].text = "PUSH THE";
        Text[1].text += System.Environment.NewLine + "BU";
        Text[2].text = "PUSH";
        Text[2].text += System.Environment.NewLine + "THE BU";
        Text[3].text = "PUSH";
        Text[3].text += System.Environment.NewLine + "THE BU";
        yield return new WaitForSeconds(0.07f);
        Text[1].text = "PUSH THE";
        Text[1].text += System.Environment.NewLine + "BUT";
        Text[2].text = "PUSH";
        Text[2].text += System.Environment.NewLine + "THE BUT";
        Text[3].text = "PUSH";
        Text[3].text += System.Environment.NewLine + "THE BUT";
        yield return new WaitForSeconds(0.07f);
        Text[1].text = "PUSH THE";
        Text[1].text += System.Environment.NewLine + "BUTT";
        Text[2].text = "PUSH";
        Text[2].text += System.Environment.NewLine + "THE BUTT";
        Text[3].text = "PUSH";
        Text[3].text += System.Environment.NewLine + "THE BUTT";
        yield return new WaitForSeconds(0.07f);
        Text[1].text = "PUSH THE";
        Text[1].text += System.Environment.NewLine + "BUTTO";
        Text[2].text = "PUSH";
        Text[2].text += System.Environment.NewLine + "THE BUTTO";
        Text[3].text = "PUSH";
        Text[3].text += System.Environment.NewLine + "THE BUTTO";
        yield return new WaitForSeconds(0.07f);
        Text[1].text = "PUSH THE";
        Text[1].text += System.Environment.NewLine + "BUTTON";
        Text[2].text = "PUSH";
        Text[2].text += System.Environment.NewLine + "THE BUTTON";
        Text[3].text = "PUSH";
        Text[3].text += System.Environment.NewLine + "THE BUTTON";
        yield return new WaitForSeconds(0.07f);
        Text[0].text = "P";
        yield return new WaitForSeconds(0.07f);
        Text[0].text = "PU";
        yield return new WaitForSeconds(0.07f);
        Text[0].text = "PUS";
        yield return new WaitForSeconds(0.07f);
        Text[0].text = "PUSH";
        yield return new WaitForSeconds(0.07f);
        Text[0].text = "PUSH";
        Text[0].text += System.Environment.NewLine + "M";
        yield return new WaitForSeconds(0.07f);
        Text[0].text = "PUSH";
        Text[0].text += System.Environment.NewLine + "ME";
        yield return new WaitForSeconds(0.07f);
        Text[0].text = "PUSH";
        Text[0].text += System.Environment.NewLine + "ME!";
        yield return new WaitForSeconds(0.07f);
        Text[0].color = Colors[0];
        yield return new WaitForSeconds(0.07f);
        Text[0].color = Colors[1];
        Downcount = false;
    }
    IEnumerator FakeStrikeChecker()
    {
        if (Stage < Bomb.GetSolvedModuleNames().Where(a => !Ignored.Contains(a)).Count() && !moduleSolved)
        {
            Stage++;
                if (Stage == MaxStage)
                {
                    StopCoroutine(StrikeChecker());
                    moduleSolved = true;
                    Sounds.clip = SFX[1];
                    Sounds.Play();
                    Text[1].color = Colors[2];
                    Text[2].color = Colors[2];
                    Text[3].color = Colors[2];
                    Text[0].characterSize = 0.3f;
                    Text[0].text = "YOU";
                    Text[0].text += System.Environment.NewLine + "WIN";
                    Text[1].text = "I";
                    Text[2].text = "GIVE";
                    Text[3].text = "UP";
                for (int i = 0; i < 4; i++)
                {
                    Text[0].color = Colors[3];
                    yield return new WaitForSeconds(0.05f);
                    Text[0].color = Colors[1];
                    yield return new WaitForSeconds(0.05f);
                }
                    yield return new WaitForSeconds(0.40f);
                    Text[0].text = "YOU";
                    Text[0].text += System.Environment.NewLine + "WI";
                    yield return new WaitForSeconds(0.5f);
                    Text[0].text = "YOU";
                    Text[0].text += System.Environment.NewLine + "W";
                    yield return new WaitForSeconds(0.5f);
                    Text[0].text = "YOU";
                    Text[0].text += System.Environment.NewLine + "";
                    Text[2].text = "GIV";
                    yield return new WaitForSeconds(0.5f);
                    Text[0].text = "YO";
                    Text[0].text += System.Environment.NewLine + "";
                    Text[2].text = "GI";
                    yield return new WaitForSeconds(0.5f);
                    Text[0].text = "Y";
                    Text[0].text += System.Environment.NewLine + "";
                    Text[2].text = "G";
                    Text[3].text = "U";
                    yield return new WaitForSeconds(0.5f);
                    Text[0].text = "";
                    Text[1].text = "";
                    Text[2].text = "";
                    Text[3].text = "";
                    yield return new WaitForSeconds(0.45f);
                    if (Stage == MaxStage)
                    {
                        Module.HandlePass();
                        Debug.LogFormat("[Don't Touch Anything #{0}]: The module is bored. He solved himself just to get away from you not pressing him correctly.", moduleId);
                        Debug.LogFormat("[Don't Touch Anything #{0}]: Oh, yeah, and congratulations. You solved the module because you did NOTHING.", moduleId);
                    }
                }
        }
        FakeStrikes = UnityEngine.Random.Range(0, 100000);
        if (FakeStrikes == 0)
        {
            Audio.PlayGameSoundAtTransform(KMSoundOverride.SoundEffect.Strike, Module.transform);
            Debug.LogFormat("[Don't Touch Anything #{0}]: The button played a strike sound, hoping to get your attention.", moduleId);
        }
        else if (FakeStrikes == 1)
            StartCoroutine(TheCountdown());
    }
}