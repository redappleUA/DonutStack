using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonutSpawner : MonoBehaviour
{
    [SerializeField] GameObject donutPrefab;
    [SerializeField] List<Transform> spawnPosition = new();

    private DonutStick donutStick;

    void Start()
    {
        donutStick = GetComponent<DonutStick>();

        SpawnDonuts(); 
    }

    void SpawnDonuts()
    {

        for(int i = 0; i < donutStick.DonutCount; i++)
        {
            var donutGO = Instantiate(donutPrefab, spawnPosition[i]);
            donutGO.GetComponent<MeshRenderer>().material.color = donutStick.Colors[Random.Range(0, donutStick.Colors.Length)];
            donutStick.spawnedDonuts.Add(donutGO);
        }
    }
}
