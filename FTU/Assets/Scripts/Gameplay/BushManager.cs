using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BushManager : MonoBehaviour
{
    public static BushManager Instance()
    {
        return _singleton;
    }

    private static BushManager _singleton;

    public List<GameObject>[] entitiesInBush;

    // Start is called before the first frame update
    void Start()
    {
        if (!_singleton)
        {
            _singleton = this;
            entitiesInBush = new List<GameObject>[7];
            for (int i = 0; i < entitiesInBush.Length; i++)
            {
                entitiesInBush[i] = new List<GameObject>();
            }
        }
    }

    public void AddEntityToBush(int idBush, GameObject go)
    {
        entitiesInBush[idBush].Add(go);
        if (entitiesInBush[idBush] == null) return;
        if (entitiesInBush[idBush].Select(x => x.GetComponent<IDamageable>().team.Code).Distinct().Count() > 1)
        {
            foreach (var entity in entitiesInBush[idBush])
            {
                entity.layer = LayerMask.NameToLayer("Default");
                entity.transform.SetLayerRecursively(LayerMask.NameToLayer("Default"));
            }
        }
    }

    public void RemoveEntityToBush(int idBush, GameObject go)
    {
        entitiesInBush[idBush].Remove(go);
        if (!entitiesInBush[idBush].Any())
        {
            return;
        }
        
        if (entitiesInBush[idBush].Select(x => x.GetComponent<IDamageable>().team.Code).Distinct().Count() == 1)
        {
            var team = entitiesInBush[idBush].Select(x => x.GetComponent<IDamageable>().team.Code).Distinct().First();
            var layer = team == 0 ? "InvisibleDominion" : "InvisibleVeritas";
            foreach (var entity in entitiesInBush[idBush])
            {
                entity.layer = LayerMask.NameToLayer(layer);
                entity.transform.SetLayerRecursively(LayerMask.NameToLayer(layer));
            }
        }
    }
}
