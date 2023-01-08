using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DonutAlgorithm : MonoBehaviour
{
    [SerializeField] List<GameObject> spawnPositions = new List<GameObject>();

    private GameObject fwdDonut;
    private GameObject backDonut;
    private GameObject leftDonut;
    private GameObject rightDonut;

    private DonutStick donutsOrder;

    //For delay between donut moves
    private static bool moveIsRunning { get; set; } = false;

    private void Awake()
    {
        donutsOrder = GetComponent<DonutStick>();   
    }

    private void Update()
    {
        Algorithm();
    }

    void Algorithm()
    {
        if(FullStickCheck()) Destroy(gameObject);

        var donuts = donutsOrder.spawnedDonuts;
        var nearbyDonuts = GetNearbyDonuts();
        Debug.Log("Donuts " + donuts.Count);

        for (int i = 0; i < donuts.Count; i++)
        {
            //If stick have 2 the same last donuts & nearby sticks have 1 same donut
            if (donuts.Count >= 2 && donuts[^1].GetComponent<MeshRenderer>().material.color ==
                           donuts[^2].GetComponent<MeshRenderer>().material.color)
            {
                List<GameObject> sticksWithOneDonut = new();

                foreach (var nearbydonut in nearbyDonuts)
                {
                    if (nearbydonut.GetComponent<DonutStick>().spawnedDonuts.Count > 0)
                    {
                        if (IsEmptyStick(nearbydonut) || IsEmptyStick(gameObject)) continue;

                        //If this stick have 2 same donut
                        if (nearbydonut.GetComponent<DonutStick>().spawnedDonuts.Count == 1 &&
                            nearbydonut.GetComponent<DonutStick>().spawnedDonuts[^1].GetComponent<MeshRenderer>().material.color ==
                                 donuts[^1].GetComponent<MeshRenderer>().material.color &&
                                 nearbydonut.GetComponent<DonutStick>().spawnedDonuts[^1].GetComponent<MeshRenderer>().material.color ==
                                    donuts[^2].GetComponent<MeshRenderer>().material.color)
                        {
                            if (!sticksWithOneDonut.Contains(nearbydonut))
                            {
                                sticksWithOneDonut.Add(nearbydonut);
                                nearbydonut.GetComponent<DonutStick>().spawnedDonuts.RemoveAt(nearbydonut.GetComponent<DonutStick>().spawnedDonuts.Count - 1);
                            }
                        }
                        else
                            continue;
                    }     
                }

                if (sticksWithOneDonut.Count != 0)
                {
                    int indexOfStickToMove = Random.Range(0, sticksWithOneDonut.Count);

                    //if (!moveIsRunning)
                    {
                        Debug.LogWarning("2 Donuts and 1 nearby");
                        //moveIsRunning = true;
                        StartCoroutine(MoveDonut(gameObject, sticksWithOneDonut[indexOfStickToMove]));
                    }
                    //if (!moveIsRunning)
                    {
                        Debug.LogWarning("2 Donuts and 1 nearby");
                        //moveIsRunning = true;
                        StartCoroutine(MoveDonut(gameObject, sticksWithOneDonut[indexOfStickToMove]));
                    }
                    sticksWithOneDonut.Clear();
                }      
            }

            //If nearby sticks have 2 same donut
            {
                List<GameObject> sticksWithTwoDonut = new();

                foreach (var nearbydonut in nearbyDonuts)
                {

                    if (nearbydonut.GetComponent<DonutStick>().spawnedDonuts.Count > 0)
                    {
                        if (IsEmptyStick(nearbydonut) || IsEmptyStick(gameObject)) continue;

                        if (nearbydonut.GetComponent<DonutStick>().spawnedDonuts.Count >= 2 && donutsOrder.spawnedDonuts.Count == 1)
                        {
                            //If stick have 2 same donut
                            if (nearbydonut.GetComponent<DonutStick>().spawnedDonuts[^1].GetComponent<MeshRenderer>().material.color ==
                                 donuts[^1].GetComponent<MeshRenderer>().material.color &&
                            nearbydonut.GetComponent<DonutStick>().spawnedDonuts[^2].GetComponent<MeshRenderer>().material.color ==
                                 donuts[^1].GetComponent<MeshRenderer>().material.color)
                            {
                                if (!sticksWithTwoDonut.Contains(nearbydonut))
                                {
                                    sticksWithTwoDonut.Add(nearbydonut);
                                    nearbydonut.GetComponent<DonutStick>().spawnedDonuts.RemoveAt(nearbydonut.GetComponent<DonutStick>().spawnedDonuts.Count - 1);
                                }
                            }
                            else
                                continue;
                        }      
                    }
                }
                //move just 2 last donut
                if (sticksWithTwoDonut.Count != 0)
                {
                    int indexOfStickToMove = Random.Range(0, sticksWithTwoDonut.Count);
                    //if (!moveIsRunning)
                    {
                        Debug.LogWarning("2 nearby Donuts");
                        //moveIsRunning = true;
                        StartCoroutine(MoveDonut(sticksWithTwoDonut[indexOfStickToMove], gameObject));
                        StartCoroutine(MoveDonut(sticksWithTwoDonut[indexOfStickToMove], gameObject));
                        sticksWithTwoDonut.Clear();    
                    }
                }      
            }

            //If nearby sticks have the same last donut
            {
                List<GameObject> sticksWithSameLastDonutToMove = new();
                List<GameObject> sticksWithSameLastDonutHearMove = new();

                foreach (var nearbydonut in nearbyDonuts)
                {
                    if(nearbydonut.GetComponent<DonutStick>().spawnedDonuts.Count > 0)
                    {
                        if (IsEmptyStick(nearbydonut) || IsEmptyStick(gameObject)) continue;

                        if (nearbydonut.GetComponent<DonutStick>().spawnedDonuts[^1].GetComponent<MeshRenderer>().material.color ==
                                    donuts[^1].GetComponent<MeshRenderer>().material.color)
                        {
                            if (nearbydonut.GetComponent<DonutStick>().spawnedDonuts.Count <= 3 && donutsOrder.spawnedDonuts.Count < 3)
                            {
                                if (!sticksWithSameLastDonutToMove.Contains(nearbydonut))
                                {
                                    sticksWithSameLastDonutToMove.Add(nearbydonut);
                                    nearbydonut.GetComponent<DonutStick>().spawnedDonuts.RemoveAt(nearbydonut.GetComponent<DonutStick>().spawnedDonuts.Count - 1);
                                }
                            }
                            else if (nearbydonut.GetComponent<DonutStick>().spawnedDonuts.Count < 3 && donutsOrder.spawnedDonuts.Count <= 3)
                            {
                                if (!sticksWithSameLastDonutHearMove.Contains(nearbydonut))
                                {
                                    sticksWithSameLastDonutHearMove.Add(nearbydonut);
                                    nearbydonut.GetComponent<DonutStick>().spawnedDonuts.RemoveAt(nearbydonut.GetComponent<DonutStick>().spawnedDonuts.Count - 1);
                                }
                            }  
                        }
                        else
                            continue;
                    }   
                }
                //move just 1 last donut
                if (sticksWithSameLastDonutToMove.Count > 0)
                {
                    //if (!moveIsRunning)
                    {
                        Debug.LogWarning("Last Donut");
                        //moveIsRunning = true;
                        StartCoroutine(MoveDonut(gameObject, sticksWithSameLastDonutToMove[Random.Range(0, sticksWithSameLastDonutToMove.Count)]));
                        sticksWithSameLastDonutToMove.Clear();
                    }
                }
                else if (sticksWithSameLastDonutHearMove.Count > 0)
                {
                    //if (!moveIsRunning)
                    {
                        Debug.LogWarning("Last Donut");
                        //moveIsRunning = true;
                        StartCoroutine(MoveDonut(gameObject, sticksWithSameLastDonutHearMove[Random.Range(0, sticksWithSameLastDonutHearMove.Count)]));
                        sticksWithSameLastDonutToMove.Clear();
                    }
                }
            }
        }
    }

    //Dont work
    /// <summary>
    /// Check if stick 3 donuts & all donuts have same color
    /// </summary>
    /// <returns>True if stick is full, else - false</returns>
    bool FullStickCheck()
    {
        if (IsEmptyStick(gameObject)) return true;

        var donuts = GetComponent<DonutStick>().spawnedDonuts;

        if (donuts.Count > 2)
        {
            var color = donuts[^1].GetComponent<MeshRenderer>().material.color;

            foreach (var donut in donuts)
            {
                if (color == donut.GetComponent<MeshRenderer>().material.color)
                {
                    continue;
                }
                else return false;
            }
            
            Debug.LogWarning("DESTROYED");
            return true;
        }
        else return false;
    }

    //Get nearby donuts
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("DonutStick"))
        {
            if (.5f <= transform.position.z - other.transform.position.z)
                fwdDonut = other.gameObject;
            else if (.5f <= other.transform.position.z - transform.position.z)
                backDonut = other.gameObject;
            else if (.5f <= transform.position.x - other.transform.position.x)
                leftDonut = other.gameObject;
            else if (.5f <= other.transform.position.x - transform.position.x)
                rightDonut = other.gameObject;
        }
    }

    List<GameObject> GetNearbyDonuts()
    {
        List<GameObject> nearbyDonuts = new List<GameObject>();

        if(fwdDonut != null) nearbyDonuts.Add(fwdDonut);
        if(backDonut != null) nearbyDonuts.Add(backDonut);
        if(leftDonut != null) nearbyDonuts.Add(leftDonut);
        if(rightDonut != null) nearbyDonuts.Add(rightDonut);

        return nearbyDonuts;
    }

    /// <summary>
    /// Move donut in another donut stick
    /// </summary>
    /// <param name="targetDonut">Target donut stick to move last donut in this donut stick</param>
    /// <returns></returns>
    IEnumerator MoveDonut(GameObject moveDonut, GameObject targetDonut)
    {
        GameObject donut;
        Transform targetPosition = null;

        yield return new WaitUntil(() => GetComponent<Rigidbody>().velocity.magnitude == 0);

        //Get the last donut
        if (moveDonut.GetComponent<DonutStick>().spawnedDonuts.Count > 0)
            donut = moveDonut.GetComponent<DonutStick>().spawnedDonuts[^1];
        else
            yield break;

        //Get target spawn position for the last donut
        foreach (Transform child in targetDonut.transform)
        {
            if (child.name != "Stick")
            {
                if (child.childCount == 0)
                {
                    targetPosition = child;
                    break;
                }
            }
        }
        //Move donut
        if (donut != null && targetPosition != null)
        {
            donut.transform.SetParent(targetPosition, false);
            moveDonut.GetComponent<DonutStick>().spawnedDonuts.RemoveAt(moveDonut.GetComponent<DonutStick>().spawnedDonuts.Count - 1);
            targetDonut.GetComponent<DonutStick>().spawnedDonuts.Add(donut);

            if (FullStickCheck()) Destroy(gameObject);
            moveIsRunning = false;
            yield return new WaitForSeconds(1f);
        }
        else
        {
            moveIsRunning = false;
        }

        //Check if stick is empty
        if(!IsEmptyStick(moveDonut)) yield break;

        var row = GetComponent<StickMove>().row.GetComponent<Row>();
        row.donutStickCount--;
        Destroy(gameObject);
    }

    bool IsEmptyStick(GameObject target)
    {
        foreach (Transform child in target.transform)
        {
            if (child.name != "Stick")
            {
                if (child.childCount != 0)
                    return false;
            }
        }
        return true;
    }
}