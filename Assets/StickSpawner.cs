using UnityEngine;

public class StickSpawner : MonoBehaviour
{
    [SerializeField] Transform spawnPosition;
    [SerializeField] GameObject donutStickPrefab;

    private GameObject donutStick;

    private void Start()
    {
        SpawnStick();
    }
    private void FixedUpdate()
    {
        SpawnStick();
    }

    void SpawnStick()
    {
        if(donutStick == null)
            donutStick = Instantiate(donutStickPrefab, spawnPosition.position, Quaternion.Euler(-19, 0, 0));
        
        else if(donutStick.GetComponent<DonutStick>().isSpawned)
            donutStick = Instantiate(donutStickPrefab, spawnPosition.position, Quaternion.Euler(-19, 0, 0));
    }
}
