using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [Header("Visual")]
    [SerializeField] private Food foodPrefab;
    [SerializeField] private Food foodPrefab_2;

    [Header("Spawn Setup")]

    [SerializeField] private float spawnBoundXFrom;
    [SerializeField] private float spawnBoundXTo;

    [SerializeField] private float spawnBoundZFrom;
    [SerializeField] private float spawnBoundZTo;

    [SerializeField] private float spawnHeightFrom;
    [SerializeField] private float spawnHeightTo;

    [Range(0, 1)]
    [SerializeField] private float _minSpeed;
    public float minSpeed { get { return _minSpeed; } }
    [Range(0, 1)]
    [SerializeField] private float _maxSpeed;
    public float maxSpeed { get { return _maxSpeed; } }

    [SerializeField] public float decayHeight;

    public Food[] foodBuffer { get; set; }
    private int currIndex;
    private int bufferSize = 200;

    public static List <Food> allFoods = new List <Food> ();

    void Start()
    {
        currIndex = 0;
        foodBuffer = new Food [bufferSize];
    }

    void Update()
    {
        for (int i = 0; i < allFoods.Count; i++)
        {
            if (allFoods[i].transform.position.y < decayHeight)
            {
                Destroy(allFoods[i].gameObject);
                allFoods.RemoveAt(i);
            }
            else
            {
                allFoods[i].MoveUnit();
            }
        }
    }

    public Vector3[] getAllFoodsPositions ()
    {
        Vector3 [] positions = new Vector3 [allFoods.Count];
        for (int i = 0; i < allFoods.Count; i++)
        {
            positions[i] = allFoods[i].transform.position;
        }
        return positions;
    }

    private void incrementIndex ()
    {
        this.currIndex++;
        if (this.currIndex == bufferSize)
        {
            this.currIndex = 0;
        }
    }

    public void EatFood (int index)
    {
        Destroy(allFoods[index].gameObject);
        allFoods.RemoveAt(index);
    }

    public void SpawnFood()
    {
        Food food = gameObject.AddComponent<Food>() as Food;
        allFoods.Add(food);

        int index = allFoods.Count - 1;

        var spawnPosition = new Vector3(Random.Range(spawnBoundXFrom, spawnBoundXTo), Random.Range(spawnHeightFrom, spawnHeightTo), Random.Range(spawnBoundZFrom, spawnBoundZTo));
        var rotation = Quaternion.Euler(0, UnityEngine.Random.Range(0, 360), 0);
        float rand = Random.Range(-1.0f, 1.0f);
        if (rand <= 0.0f)
        {
            allFoods[index] = Instantiate(foodPrefab, spawnPosition, rotation);
        }
        else
        {
            allFoods[index] = Instantiate(foodPrefab_2, spawnPosition, rotation);
        }
        allFoods[index].AssignFallingSpeed(UnityEngine.Random.Range(minSpeed, maxSpeed));
    }
}
