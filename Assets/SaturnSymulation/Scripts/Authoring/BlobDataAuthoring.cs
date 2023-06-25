using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;

/*
public class BlobDataAuthoring : MonoBehaviour
{
    public List<GameObject> rockPrefabs;
}
public class BlobDataBaker : Baker<BlobDataAuthoring>
{
    public override void Bake(BlobDataAuthoring authoring)
    {
        var builder = new BlobBuilder(Allocator.Temp);
        ref RockPrefabs prefabData = ref builder.ConstructRoot<RockPrefabs>();

        BlobBuilderArray<RockPrefab> arrayBuilder = builder.Allocate(ref prefabData.rockPrefabs, authoring.rockPrefabs.Count);

       // NativeList<Entity> prefabsArray = new NativeList<Entity>(Allocator.Temp);

        for (int i = 0; i < authoring.rockPrefabs.Count; i++)
        {
            arrayBuilder[i].rockPrefab = GetEntity(authoring.rockPrefabs[i]);
          //  prefabsArray.Add(GetEntity(authoring.rockPrefabs[i]));
        }

        var resoult = builder.CreateBlobAssetReference<RockPrefabs>(Allocator.Persistent);

        builder.Dispose();

        AddBlobAsset<RockPrefabs>(ref resoult, out var hash);
        var entity = GetEntity(TransformUsageFlags.None);
        AddComponent(entity, new BlobDataComponent() { Blob = resoult });
    }

}

struct BlobDataComponent : IComponentData
{
    public BlobAssetReference<RockPrefabs> Blob;
}

public struct RockPrefabs
{
    public BlobArray<RockPrefab> rockPrefabs;
}

public struct RockPrefab 
{
   public Entity rockPrefab;
}


*/