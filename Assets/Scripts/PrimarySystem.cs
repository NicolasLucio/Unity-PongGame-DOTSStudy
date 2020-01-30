using System.Collections;
using TMPro;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using UnityEngine;

public class PrimarySystem : MonoBehaviour
{
    public static PrimarySystem main;

    public GameObject ballPrefab;

    public float xBound = 9f;
    public float yBound = 4f;
    public float ballSpeed = 3f;
    public float respawnDelay = 2f;
    public int[] playerScores;

    public TextMeshProUGUI mainText;
    public TextMeshProUGUI[] playerTexts;

    Entity ballEntityPrefab;
    EntityManager manager;

    WaitForSeconds oneSecond;
    WaitForSeconds delay;

    GameObjectConversionSettings settings;

    private void Awake()
    {
        Cursor.visible = false;
        if (main != null && main != this)
        {
            Destroy(gameObject);
            return;
        }
        main = this;
        playerScores = new int[2];

        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null);
        //settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, new BlobAssetStore());
        ballEntityPrefab = GameObjectConversionUtility.ConvertGameObjectHierarchy(ballPrefab, settings);        

        oneSecond = new WaitForSeconds(1f);
        delay = new WaitForSeconds(respawnDelay);

        StartCoroutine(CountdownAndSpawnBall());
    }

    public void PlayerScored(int playerID)
    {
        playerScores[playerID]++;
        for (int i = 0; i < playerScores.Length && i < playerTexts.Length; i++)
        {
            playerTexts[i].text = playerScores[i].ToString();
        }

        StartCoroutine(CountdownAndSpawnBall());
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {            
            Application.Quit();            
        }        
    }

    IEnumerator CountdownAndSpawnBall()
    {
        mainText.text = "Get Ready";
        yield return delay;

        mainText.text = "3";
        yield return oneSecond;

        mainText.text = "2";
        yield return oneSecond;

        mainText.text = "1";
        yield return oneSecond;

        mainText.text = "";

        SpawnBall();
    }

    void SpawnBall()
    {
        Entity ball = manager.Instantiate(ballEntityPrefab);

        Vector3 dir = new Vector3(UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1, UnityEngine.Random.Range(-0.5f, 0.5f), 0f).normalized;
        Vector3 speed = dir * ballSpeed;

        PhysicsVelocity velocity = new PhysicsVelocity()
        {
            Linear = speed,
            Angular = float3.zero
        };

        manager.AddComponentData(ball, velocity);    
    }
}
