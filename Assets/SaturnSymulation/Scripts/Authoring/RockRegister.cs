using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public class RockRegister : MonoBehaviour
{
    [SerializeField] bool useHQC = false;

    public List<GameObject> rocks;

    public List<GameObject> HQC_rocks;

    //public List<>
    class Baker : Baker<RockRegister>
    {
        [System.Obsolete]
        public override void Bake(RockRegister authoring)
        {
            var Buffer = AddBuffer<RocksIBufferData>();

            if(authoring.useHQC)
                foreach (GameObject rock in authoring.HQC_rocks)
                {
                    Buffer.Add(new RocksIBufferData
                    {
                        rockPrefab = GetEntity(rock.gameObject),
                    });
                }
            else
                foreach (GameObject rock in authoring.rocks)
                {
                    Buffer.Add(new RocksIBufferData
                    {
                        rockPrefab = GetEntity(rock.gameObject),
                    });
                }
        }
    }

    public struct RocksIBufferData : IBufferElementData
    {
        public Entity rockPrefab;
    }
}
