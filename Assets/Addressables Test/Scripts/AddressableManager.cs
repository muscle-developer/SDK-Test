using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;   // 어드레서블을 사용하기 위한 네임스페이스

public class AddressableManager : MonoBehaviour
{
    [SerializeField]
    private AssetReferenceGameObject capsuleObj;
    [SerializeField]
    private AssetReferenceGameObject cubeObj;
    [SerializeField]
    private AssetReferenceGameObject sphereObj;

    // 생성된 오브젝트들을 저장하는 리스트
    private List<GameObject> gameObjs = new List<GameObject>();

    void Start()
    {
        StartCoroutine(InitAddressable());
    }

    private IEnumerator InitAddressable()
    {
        var init = Addressables.InitializeAsync();
        yield return init;
    }

    // Load
    public void ButtonSpawnObject() 
    {
        capsuleObj.InstantiateAsync().Completed += (obj) =>
        {
            gameObjs.Add(obj.Result);
        };

        cubeObj.InstantiateAsync().Completed += (obj) =>
        {
            gameObjs.Add(obj.Result);
        };

        sphereObj.InstantiateAsync().Completed += (obj) =>
        {
            gameObjs.Add(obj.Result);
        };
    }
    
    // Release
    public void ButtonReleaseObject()
    {
        if (gameObjs.Count == 0)
        {
            return;
        }

        for (int i = gameObjs.Count - 1; i >= 0; i--)
        {
            Addressables.ReleaseInstance(gameObjs[i]);
            gameObjs.RemoveAt(i);
        }
    }
}
