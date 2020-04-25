using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    public float vievDst = 300f;
    public Transform player;

    public static Vector2 vievPos;
    int chunkSize;
    public int chunksVisible;

    Dictionary<Vector2, GameObject> chunkDict = new Dictionary<Vector2, GameObject>();

    public GameObject chunkPrefab;

    private void Start()
    {
        chunkSize = 32;
        chunksVisible = Mathf.RoundToInt(vievDst / chunkSize);
    }

    private void Update()
    {
        vievPos = new Vector2(player.position.x, player.position.z);
        UpdateChunks();
    }

    void UpdateChunks()
    {
        int currentChunkX = Mathf.RoundToInt(vievPos.x / chunkSize);
        int currentChunkY = Mathf.RoundToInt(vievPos.y / chunkSize);

        for(int yOffset = -chunksVisible; yOffset <= chunksVisible; yOffset++)
        {
            for (int xOffset = -chunksVisible; xOffset <= chunksVisible; xOffset++)
            {
                Vector2 viewedChunk = new Vector2(currentChunkX + xOffset, currentChunkY + yOffset);

                if(chunkDict.ContainsKey(viewedChunk))
                {
                    Debug.Log("contains");
                }
                else
                {
                    chunkDict.Add(viewedChunk, Instantiate(chunkPrefab, new Vector3(viewedChunk.x * chunkSize, 0, viewedChunk.y * chunkSize), Quaternion.identity, transform));
                }
            }
        }
    }
}
