using System;
using System.IO;
using UnityEngine;
//Json 파일
//오브젝트의 정보를 "key" , "value"의 형태로 저장해서 전달해주는 포맷(format)[서식]

//웹문서 기준으로는 json vs xml

public class JsonExample : MonoBehaviour
{

    [Serializable]
    //Json -> class의 형태로 읽어서 적용
    public class ExampleData
    {
        public int idx;
        public string contents;
        public string desc;
        public string[] items;
    }

    [Serializable]
    public class Tip
    {
        public int idx;
        public string contents;
        public string desc;
    }

    [Serializable]
    public class TipData
    {
        public Tip[] tip;
    }

    public ExampleData data;
    public TipData tipData;

    [ContextMenu("팁 로드")]
    public void LoadTips()
    {
        //리소스 폴더 위치에서 로드하기
        var jsonFile = Resources.Load<TextAsset>("data");
        //json 파일은 Assets에서 TextAsset로 처리됩니다.

        tipData = JsonUtility.FromJson<TipData>(jsonFile.text);
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this); //유니티 에디터에서의 변경 사항 저장
#endif
    }


    [ContextMenu("데이터 세이브")]
    //클래스 데이터를 Json 파일에 저장합니다.
    void SaveData()
    {
        //JsonUtility는 유니티에서 제공해주는 Json 작업을 위해
        //사용하는 클래스입니다.
        string jsonData = JsonUtility.ToJson(data, true);
        //경로 설정
        //Application.dataPath (프로젝트의 폴더 내부)
        //Application.streamingAssetsPath(Assets + StreamingAssets)
        //Applictaion.persistentDataPath : 각 운영체제에서 허용하는 읽기/쓰기 가능한 폴더의 경로
        //주로 안드로이드같은 모바일 환경에서 쓰기를 진행해야하는 경우 자주 사용되는 경로

        string path = Path.Combine(Application.dataPath + "/Data", "data.json");

        File.WriteAllText(path, jsonData);

        Debug.Log("작성한 데이터가 저장됬습니다.");
    }
    [ContextMenu("데이터 로드")]
    //Json 파일을 이용해 클래스 쪽으로 로드합니다.
    void LoadData()
    {
        string path = Path.Combine(Application.dataPath + "/Data", "data.json");
        string jsonData = File.ReadAllText(path);
        data = JsonUtility.FromJson<ExampleData>(jsonData);
    }
}
