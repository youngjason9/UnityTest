using System.IO;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TestClass
{
    public string name;
    public int age;
    public PuzzleType[] puzzleType;
    public BaseType[] baseType;
    public Vector3[] position;


    public void PrintInfo()
    {
        Debug.Log(name + " " + age + "拼图类型： " + puzzleType + "基底类型： " + baseType);
        foreach (var puzzle in puzzleType)
        {
            Debug.Log(puzzle);
        }
        foreach (var baseType in baseType)
        {
            Debug.Log(baseType);
        }
        foreach (var pos in position)
        {
            Debug.Log(pos);
        }
    }
}

public class SaveOrLoadTest : MonoBehaviour
{
    [SerializeField]private bool testSave;
    [SerializeField]private string fileName;
    [SerializeField]private TextAsset jsonFile;

    private string filePath;

    private void Start()
    {
        if(testSave)
        {
            fileName += ".json";
            filePath = Path.Combine(Application.dataPath, "Data/LevelData/", fileName);
            string directoryPath = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            

            TestClass testClass = new TestClass();
            testClass.name = "John";
            testClass.age = 25;
            BaseType[] testBaseBrick = new BaseType[4];
            testClass.baseType = testBaseBrick;

            testClass.puzzleType = new PuzzleType[2];
            testClass.position = new Vector3[2];

            testClass.baseType[0] = BaseType.FiexedRotaBase;
            testClass.puzzleType[0] = PuzzleType.normalPuzzle;
            testClass.position[0] = new Vector3(0, 0, 1);

            Save(testClass);
        }
        else{
            TestClass testClass = Load(jsonFile);
            testClass.PrintInfo();
        }      
    }

    private void Save(TestClass testClass)
    {
        string json = JsonUtility.ToJson(testClass);
        File.WriteAllText(filePath, json);
        Debug.Log("Saved in" + filePath);
    }

    private TestClass Load(TextAsset jsonFile)
    {      
        if(jsonFile!= null)
        {
            string json = jsonFile.text;
            TestClass testClass = JsonUtility.FromJson<TestClass>(json);
            Debug.Log("成功读取" +testClass.name + " " + testClass.age);
            Debug.Log("Loaded");
            return testClass;
        }
        else{
            Debug.LogError("File not found at " + jsonFile);
            return null;
        }
    }
}
