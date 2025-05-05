using System.Collections.Generic;
using UnityEngine;
using WorldGeneration;

public class MeadowBiome : Biome {
    public MeadowBiome(System.Random seedRandom) : base(seedRandom) {
        objectPrefabs = new List<GameObject>();

        for (int i = 1; i <= 3; i++) {
            GameObject tree = Resources.Load<GameObject>($"Prefabs/Trees/Tree{i}");
            if (tree != null) objectPrefabs.Add(tree);
        }

        for (int i = 1; i <= 3; i++) {
            GameObject rock = Resources.Load<GameObject>($"Prefabs/Rocks/Rock{i}");
            if (rock != null) objectPrefabs.Add(rock);
        }

        for (int i = 1; i <= 1; i++) {
            GameObject branching = Resources.Load<GameObject>($"Prefabs/Branching/Branching{i}");
            if (branching != null) objectPrefabs.Add(branching);
        }

        minObjectsPerChunk = 8;
        maxObjectsPerChunk = 25;
    }

    private Vector3 IsoPosition(int x, int y) {
        // Pour des tuiles à l'échelle 1x1, les facteurs doivent être ajustés
        // Sachant que les tuiles sont deux fois plus petites, on divise par 2 l'espacement
        float isoX = (x - y) * 0.25f;
        float isoY = (x + y) * 0.125f;
        return new Vector3(isoX, isoY, 0f);
    }

    public override void GenerateObjects(Chunk chunk, int startX, int startY, int endX, int endY, List<Vector2Int> occupiedPositions) {
        // Utilisation de random pour générer le nombre d'objets
        int total = random.Next(minObjectsPerChunk, maxObjectsPerChunk);

        for (int i = 0; i < total; i++) {
            // Utilisation de random pour les positions x et y
            int x = random.Next(startX, endX);
            int y = random.Next(startY, endY);
            Vector2Int pos = new Vector2Int(x, y);

            if (IsTooCloseToOtherObjects(pos, occupiedPositions, 1f)) continue;

            Vector3 worldPos = IsoPosition(x, y);
            worldPos.y += 0.5f; // Ajustement Y pour alignement des bases
            GameObject prefab = GetRandomPrefab(objectPrefabs);
            if (prefab == null) continue;

            // Récupérer le PrefabInfo associé au prefab
            PrefabInfo prefabInfo = prefab.GetComponent<PrefabInfo>();

            // Assigner la position locale dans le chunk
            GameObject instance = Object.Instantiate(prefab, worldPos, Quaternion.identity, chunk.chunkTransform);

            TileData data = new TileData(pos, TileType.Ground) {
                prefabName = prefabInfo.prefabName,
                gameIdentifier = prefabInfo.gameIdentifier,
                isDestroyed = false,
                currentHealth = prefabInfo.isDestructible ? prefabInfo.baseHealth : 0 // Si destructible, on met la santé de base
            };
            chunk.tiles[pos] = data;

            // Permutation aléatoire de l'axe X
            MaybeFlipX(instance);

            SpriteRenderer sr = instance.GetComponent<SpriteRenderer>();
            if (sr != null)
                sr.sortingOrder = -(x + y);

            occupiedPositions.Add(pos);
        }
    }

    private void MaybeFlipX(GameObject obj) {
        // Utilisation de random pour déterminer si on flippe l'objet
        if (random.NextDouble() < 0.5) {
            Vector3 scale = obj.transform.localScale;
            scale.x *= -1f;
            obj.transform.localScale = scale;
        }
    }

    private bool IsTooCloseToOtherObjects(Vector2Int pos, List<Vector2Int> occupiedPositions, float minDistance) {
        foreach (var occupiedPos in occupiedPositions) {
            if (Vector2Int.Distance(pos, occupiedPos) < minDistance)
                return true;
        }
        return false;
    }

    private GameObject GetRandomPrefab(List<GameObject> prefabs) {
        if (prefabs == null || prefabs.Count == 0) return null;
        return prefabs[random.Next(prefabs.Count)];
    }

    public override TileType GetTileTypeAt(Vector2Int worldPos) {
        return TileType.Ground;
    }

    // Placement du sol
    public override void GenerateTileVisuals(Chunk chunk) {
        // Charger tous les types de sol
        GameObject[] groundPrefabs = new GameObject[3];
        for (int i = 0; i < 3; i++) {
            groundPrefabs[i] = Resources.Load<GameObject>($"Prefabs/Ground/Ground{i + 1}");
            if (groundPrefabs[i] == null) {
                Debug.LogWarning($"Le prefab Ground{i + 1} est manquant !");
                return;
            }
        }

        foreach (var pair in chunk.tiles) {
            Vector2Int localPos = pair.Key;
            TileData tile = pair.Value;

            if (tile.type == TileType.Ground) {
                Vector2Int worldPos = new(
                    chunk.chunkPosition.x * chunk.size + localPos.x,
                    chunk.chunkPosition.y * chunk.size + localPos.y
                );

                Vector3 isoPos = IsoPosition(worldPos.x, worldPos.y);
                isoPos.y -= 0.1875f; // Ajustement Y pour alignement des bases (0.75/4 pour la nouvelle échelle)

                GameObject selectedPrefab = groundPrefabs[random.Next(groundPrefabs.Length)];
                GameObject instance = Object.Instantiate(selectedPrefab, isoPos, Quaternion.identity, chunk.chunkTransform);

                SpriteRenderer sr = instance.GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.sortingOrder = -(worldPos.x + worldPos.y);
            }
        }
    }
}
