using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;   // 어드레서블을 사용하기 위한 네임스페이스
using UnityEngine.ResourceManagement.AsyncOperations;

public class AddressableManager : MonoBehaviour
{
    // Addressable로 관리되는 Capsule, Cube, Sphere 프리팹에 대한 참조
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

    // Addressables 초기화 코루틴
    private IEnumerator InitAddressable()
    {
        var init = Addressables.InitializeAsync(); // 비동기 초기화
        yield return init; // 초기화 완료될 때까지 대기
    }

    // 버튼 클릭 시 오브젝트를 Addressables에서 인스턴스화
    public void ButtonSpawnObject() 
    {
        // Capsule 오브젝트 인스턴스화
        capsuleObj.InstantiateAsync().Completed += (obj) =>
        {
            gameObjs.Add(obj.Result); // 인스턴스화된 오브젝트를 리스트에 저장
        };

        // Cube 오브젝트 인스턴스화
        cubeObj.InstantiateAsync().Completed += (obj) =>
        {
            gameObjs.Add(obj.Result);
        };

        // Sphere 오브젝트 인스턴스화
        sphereObj.InstantiateAsync().Completed += (obj) =>
        {
            gameObjs.Add(obj.Result);
        };
    }

    // 버튼 클릭 시 인스턴스화된 오브젝트 해제
    public void ButtonReleaseObject()
    {
        // 생성된 오브젝트가 없으면 리턴
        if (gameObjs.Count == 0)
        {
            return;
        }

        // 리스트를 역순으로 순회하며 Addressables.ReleaseInstance로 해제
        // 역순으로 제거: 뒤에서부터 제거하므로, 리스트 크기가 줄어도 인덱스가 꼬이지 않는다.
        for (int i = gameObjs.Count - 1; i >= 0; i--)
        {
            Addressables.ReleaseInstance(gameObjs[i]);
            gameObjs.RemoveAt(i); // 리스트에서 제거
        }
    }
}

// 코드로 어드레서블 로드하기
public class LoadExample : MonoBehaviour
{
    public string prefabAddress = "MyPreFab";
    // 클래스 멤버 변수로 handle 저장
    private AsyncOperationHandle<GameObject> handle;

    void Start()
    {
        // 비동기로 로드
        handle = Addressables.LoadAssetAsync<GameObject>(prefabAddress);
        handle.Completed += OnPrefabLoaded;
    }

    private void OnPrefabLoaded(AsyncOperationHandle<GameObject> completedHandle)
    {
        if (completedHandle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject prefab = completedHandle.Result;
            Instantiate(prefab); // 인스턴스화
        }
        else
        {
            Debug.LogError("Failed to load addressable prefab.");
        }
    }

    private void OnRelese()
    {
        // 메모리에서 해제 (사용 후)
        Addressables.Release(handle);
    }

    // 객체가 파괴될 때 Relese 진행
    private void OnDestroy()
    {
        OnRelese();
    }
}