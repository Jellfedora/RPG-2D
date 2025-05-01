using System.Collections.Generic;
using UnityEngine;
using WorldGeneration;
using System.Linq;
public class MeadowBiome : Biome
{
    public MeadowBiome()
    {
        Name = "Meadow";

        Debug.Log("Création du biome : " + Name);

        // Chargement des préfabriqués depuis les bons dossiers Resources
        objectPrefabs = new List<GameObject>();

        // Chargement des arbres
        for (int i = 1; i <= 3; i++)
        {
            GameObject tree = Resources.Load<GameObject>($"Prefabs/Trees/Tree{i}");
            if (tree != null) objectPrefabs.Add(tree);
        }

        // Chargement des rochers
        for (int i = 1; i <= 3; i++)
        {
            GameObject rock = Resources.Load<GameObject>($"Prefabs/Rocks/Rock{i}");
            if (rock != null) objectPrefabs.Add(rock);
        }

        minObjectsPerChunk = 30;
        maxObjectsPerChunk = 50;

        Debug.Log($"Nombre de prefabs chargés pour la Prairie : {objectPrefabs.Count}");
    }

    // Définir les règles pour la génération des objets dans les prairies
    public override void GenerateObjects(Chunk chunk, int startX, int startY, int endX, int endY, List<Vector2Int> occupiedPositions)
    {
        int total = Random.Range(minObjectsPerChunk, maxObjectsPerChunk);

        for (int i = 0; i < total; i++)
        {
            int x = Random.Range(startX, endX);
            int y = Random.Range(startY, endY);
            Vector2Int pos = new Vector2Int(x, y);

            if (IsTooCloseToOtherObjects(pos, occupiedPositions, 1f)) continue;

            Vector3 worldPos = new Vector3(x + 0.5f, y + 0.5f, 0f);
            GameObject prefab = GetRandomPrefab(objectPrefabs);

            if (prefab == null) continue;

            GameObject instance = Object.Instantiate(prefab, worldPos, Quaternion.identity, chunk.chunkTransform);
            Debug.Log($"Objet instancié : {prefab.name} à {worldPos}");
            MaybeFlipX(instance); // ➤ Ajout de l'inversion horizontale aléatoire si tu veux

            // Optionnel : changer le sortingOrder pour que les objets s'empilent correctement
            SpriteRenderer sr = instance.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sortingOrder = -y;

            occupiedPositions.Add(pos);
        }
    }

    // Inversion horizontale aléatoire
    private void MaybeFlipX(GameObject obj)
    {
        if (Random.value < 0.5f)
        {
            Vector3 scale = obj.transform.localScale;
            scale.x *= -1f;
            obj.transform.localScale = scale;
        }
    }

    // Vérifier si une position est trop proche d'autres objets
    private bool IsTooCloseToOtherObjects(Vector2Int pos, List<Vector2Int> occupiedPositions, float minDistance)
    {
        foreach (var occupiedPos in occupiedPositions)
        {
            if (Vector2Int.Distance(pos, occupiedPos) < minDistance)
                return true;
        }
        return false;
    }

    // Récupérer un préfab aléatoire
    private GameObject GetRandomPrefab(List<GameObject> prefabs)
    {
        if (prefabs == null || prefabs.Count == 0) return null;
        return prefabs[Random.Range(0, prefabs.Count)];
    }

    public override TileType GetTileTypeAt(Vector2Int worldPos)
    {
        // Exemple simple : tout est de l'herbe avec quelques tuiles d'eau aléatoires
        //float noise = Mathf.PerlinNoise(worldPos.x * 0.1f, worldPos.y * 0.1f);

        //if (noise > 0.7f)
            //return TileType.Water;
        //else
        return TileType.Grass;
    }

    // Générer les visuels des tuiles d'herbe parmis plusieurs prefabs
    public override void GenerateTileVisuals(Chunk chunk)
    {
        // Charger les 5 prefabs d'herbe
        GameObject[] grassPrefabs = new GameObject[5];
        for (int i = 1; i <= 5; i++)
        {
            grassPrefabs[i - 1] = Resources.Load<GameObject>($"Prefabs/Grass/Grass{i}");
        }

        // Vérifier que les 5 prefabs ont bien été chargés
        if (grassPrefabs.Any(g => g == null))
        {
            Debug.LogWarning("Certains prefabs d'herbe sont manquants !");
            return;
        }

        foreach (var pair in chunk.tiles)
        {
            Vector2Int localPos = pair.Key;
            TileData tile = pair.Value;

            if (tile.type == TileType.Grass)
            {
                Vector2Int worldPos = new(
                    chunk.chunkPosition.x * chunk.size + localPos.x,
                    chunk.chunkPosition.y * chunk.size + localPos.y
                );

                // Choisir un prefab d'herbe avec des probabilités
                GameObject grassPrefab = GetRandomGrassPrefab(grassPrefabs);

                // Positionner l'herbe
                Vector3 position = new(worldPos.x + 0.5f, worldPos.y + 0.5f, 1f);
                GameObject instance = Object.Instantiate(grassPrefab, position, Quaternion.identity, chunk.chunkTransform);

                // Gérer le tri des objets en fonction de leur position pour les superpositions
                SpriteRenderer sr = instance.GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.sortingOrder = -worldPos.y;
            }
        }
    }

    // Fonction pour choisir un prefab d'herbe en fonction des probabilités
    private GameObject GetRandomGrassPrefab(GameObject[] grassPrefabs)
    {
        // Poids pour chaque prefab (0 = jamais choisi, 1 = fréquence normale, >1 = plus fréquent)
        float[] weights = new float[] { 1f, 1f, 1f, 0.1f, 0.1f }; // grass4 et grass5 ont moins de poids

        // Calculer un index basé sur les poids
        float totalWeight = weights.Sum();
        float randomValue = Random.value * totalWeight;

        float cumulativeWeight = 0f;
        for (int i = 0; i < grassPrefabs.Length; i++)
        {
            cumulativeWeight += weights[i];
            if (randomValue <= cumulativeWeight)
            {
                return grassPrefabs[i];
            }
        }

        // Retourner un prefab par défaut (en cas d'erreur)
        return grassPrefabs[0];
    }
}
