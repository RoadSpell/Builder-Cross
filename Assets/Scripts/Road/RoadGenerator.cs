using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEditor;
using Random = UnityEngine.Random;
using RoadType = ObstacleData.RoadType;

[ExecuteAlways]
public class RoadGenerator : MonoBehaviour
{
    public static RoadGenerator Instance { get; set; }

    [SerializeField, CanBeNull] private Transform lastRoadTransform;
    [SerializeField] private GameObject collectibleRoad;
    [SerializeField] private GameObject emptyRoad;
    [SerializeField] private GameObject turretRoad;
    [SerializeField] private GameObject bearTrapRoad;
    [SerializeField] private GameObject stickyLiquidRoad;
    [SerializeField] private GameObject spinningBladeRoad;
    [SerializeField] private GameObject mineRoad;
    [SerializeField] private GameObject spikeRoad;
    [SerializeField] private GameObject attackerRoad;
    [SerializeField] private GameObject barrierRoad;
    [SerializeField] private GameObject zombieRoad;
    

    private Dictionary<RoadType, GameObject> roadTypeDict = new();

    private Vector3 _newRoadPosition;



    [Header("Level Generation")]
    [SerializeField, Min(1)]
    private int collectibleAmount;

    [SerializeField, Min(1)] private int collectibleInterval;

    private void Awake() //Singleton
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void OnEnable()
    {
       
        
        roadTypeDict[RoadType.Collectible] = collectibleRoad;
        roadTypeDict[RoadType.Empty] = emptyRoad;
        roadTypeDict[RoadType.Turret] = turretRoad;
        roadTypeDict[RoadType.BearTrap] = bearTrapRoad;
        roadTypeDict[RoadType.StickyLiquid] = stickyLiquidRoad;
        roadTypeDict[RoadType.SpinningBlades] = spinningBladeRoad;
        roadTypeDict[RoadType.Mine] = mineRoad;
        roadTypeDict[RoadType.Spikes] = spikeRoad;
        roadTypeDict[RoadType.Attacker] = attackerRoad;
        roadTypeDict[RoadType.Barrier] = barrierRoad;
        roadTypeDict[RoadType.Zombie] = zombieRoad;
    }
    private void OnDisable()
    {
        
       
    }

    private void CreateRoad(GameObject road)
    {
        GameObject createdRoad = Instantiate(road);    /*PrefabUtility.InstantiatePrefab(road) as GameObject;*/
        _newRoadPosition = new Vector3(lastRoadTransform.position.x, lastRoadTransform.position.y,
            lastRoadTransform.position.z + emptyRoad.GetComponent<BoxCollider>().size.z);

        if (createdRoad != null)
        {
            createdRoad.transform.position = _newRoadPosition;
            createdRoad.transform.parent = this.transform;

            lastRoadTransform = createdRoad.transform;
        }
    }

    void AddRoadPrefab(RoadType roadType)
    {
        if (lastRoadTransform == null)
        {
            lastRoadTransform = GameObject.FindWithTag("Player").GetComponent<Transform>();
            
        }

        //Create a dictionary with enum keys and values 

        CreateRoad(roadTypeDict[roadType]);

    }

    [ContextMenu("Add Collectible Road")]
    void AddCollectibleRoad()
    {
        AddRoadPrefab(ObstacleData.RoadType.Collectible);
    }
    
  
    [ContextMenu("Add Empty Road")]
    void AddEmptyRoad()
    {
        AddRoadPrefab(ObstacleData.RoadType.Empty);
    }

    [ContextMenu("Add Turret Road")]
    void AddTurretRoad()
    {
        AddRoadPrefab(ObstacleData.RoadType.Turret);
    }

    [ContextMenu("Add Bear Trap Road")]
    void AddBearTrapRoad()
    {
        AddRoadPrefab(RoadType.BearTrap);
    }

    [ContextMenu("Add Sticky Road")]
    void AddStickyRoad()
    {
        AddRoadPrefab(ObstacleData.RoadType.StickyLiquid);
    }

    [ContextMenu("Add Spinning Blade Road")]
    void AddSpinningBladeRoad()
    {
        AddRoadPrefab(ObstacleData.RoadType.SpinningBlades);
    }

    [ContextMenu("Add Mine Road")]
    void AddMineRoad()
    {
        AddRoadPrefab(ObstacleData.RoadType.Mine);
    }

    [ContextMenu("Add Spike Road")]
    void AddSpikeRoad()
    {
        AddRoadPrefab(ObstacleData.RoadType.Spikes);
    }

    [ContextMenu("Add Attacker Road")]
    void AddAttackerRoad()
    {
        AddRoadPrefab(ObstacleData.RoadType.Attacker);
    }

    [ContextMenu("Add Barrier Road")]
    void AddBarrierTrapRoad()
    {
        AddRoadPrefab(ObstacleData.RoadType.Barrier);
    }

    [ContextMenu("Add Zombie Road")]
    void AddZombieRoad()
    {
        AddRoadPrefab(ObstacleData.RoadType.Zombie);
    }

    [ContextMenu("Generate A Level")]
    private void GenerateLevel()
    {
        //roadAmount = collectibleAmount * collectibleInterval + collectibleAmount;

        int roadTypeCount = Enum.GetNames(typeof(ObstacleData.RoadType)).Length; //GetNames is 10x faster than GetValues

        for (int i = 0; i < collectibleAmount; i++)
        {
            for (int j = 0; j < collectibleInterval; j++)
            {
                AddRoadPrefab((ObstacleData.RoadType)Random.Range(1, roadTypeCount));
            }

            AddRoadPrefab(ObstacleData.RoadType.Collectible);
        }
    }

    private int CalculateWeight()
    {
        int totalWeight = 0;
        foreach (var pair in ObstacleData.obstacleRarity)
        {
            totalWeight += pair.Value;
        }

        return totalWeight;
    }

    [ContextMenu("Generate Weighted Level")]
    private void GenerateWeightedLevel()
    {
        ObstacleData.InitializeObstacleDictionary();
        //int roadTypeCount = Enum.GetNames(typeof(ObstacleData.RoadType)).Length;

        int totalWeight = CalculateWeight();

        for (int i = 0; i < collectibleAmount; i++)
        {
            for (int j = 0; j < collectibleInterval; j++)
            {
                int randomNumber = Random.Range(0, totalWeight + 1);
                int pointer = 0; //To go through the pie chart. This sums the values

                for (int k = 0; k < ObstacleData.obstacleRarity.Count; k++)
                {
                    if (randomNumber <= ObstacleData.obstacleRarity.ElementAt(k).Value + pointer)
                    {
                        AddRoadPrefab((ObstacleData.RoadType)k);
                        ObstacleData.ReduceChance((ObstacleData.RoadType)k);
                        totalWeight = CalculateWeight();
                        break;
                    }

                    pointer += ObstacleData.obstacleRarity.ElementAt(k).Value; //Summing action
                }
            }

            AddRoadPrefab(ObstacleData.RoadType.Collectible);
        }
    }

    [ContextMenu("Clear Levels")]
    private void ClearAllLevels()
    {
        foreach (var child in gameObject.GetComponentsInChildren<Transform>())
        {
            if (child != null &&
                child.gameObject != this.gameObject) //WILL destroy itself if you dont check if child is itself
                DestroyImmediate(child.gameObject);
        }

        ObstacleData.ResetObstacleDictionary();
    }
}