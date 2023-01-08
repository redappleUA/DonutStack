using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DonutStick : MonoBehaviour
{
    public int DonutCount { get; private set; }
    public Color[] Colors => colors;
    private readonly Color[] colors = { Color.red, Color.green, Color.blue, Color.white };
    public List<GameObject> spawnedDonuts { get; private set; } = new();
    public bool isSpawned { get; set; } = false;

    private static int lastValue = 0;

    private void Start()
    {
        DonutCount = UniqueRandom(1, 4);
    }

    /// <summary>
    /// Random.Range without duplicate
    /// </summary>
    /// <param name="min">min range</param>
    /// <param name="max">max range</param>
    /// <returns></returns>
    int UniqueRandom(int min, int max)
    {
        int val = Random.Range(min, max);
        while (lastValue == val)
        {
            val = Random.Range(min, max);
        }
        lastValue = val;
        return val;
    }
}
