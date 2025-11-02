using UnityEngine;
using GLTFast; // 네임스페이스 추가

public class GltfLoader : MonoBehaviour
{
    // 로드할 glTF 파일의 경로 또는 URL
    public string gltfUrl = "https://raw.githubusercontent.com/KhronosGroup/glTF-Sample-Models/master/2.0/Box/glTF/Box.gltf";

    async void Start()
    {
        var gltf = new GltfImport();
        // glTF 파일 로딩을 시도합니다. 비동기 작업이므로 await를 사용합니다.
        var success = await gltf.Load(gltfUrl);

        if (success)
        {
            // 로딩이 성공하면 씬에 모델을 인스턴스화합니다.
            gltf.InstantiateSceneAsync(transform);
            Debug.Log("glTF 모델 로드 성공!");
        }
        else
        {
            Debug.LogError("glTF 모델 로드 실패!");
        }
    }
}