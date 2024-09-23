using UnityEngine;

public class LevelGenerator:MonoBehaviour
{
    private Grid grid;
    [SerializeField] private Texture2D baseMap;
    [SerializeField] private Texture2D puzzleMap;

    [Header("通过颜色映射到预制体,分别是基底层的映射和拼图层的映射")]
    [SerializeField] private ColorToPrefabs[] baseColroMappings;
    [SerializeField] private ColorToPrefabs[] puzzleColroMappings;

    public void LevelInit(int level,int steps,Grid grid)  //构造函数
    {
        Debug.Log("LevelGenerator init 当前关卡为" + level + "，步数为" + steps);
        this.grid = grid;
        if(this.grid == null){
            Debug.LogError("未初始化网格地图");
        }
        if(baseMap == null){
            Debug.LogError("未设置基底层映射地图");
        }
        if(puzzleMap == null){
            Debug.LogError("未设置拼图层映射地图");
        }
    

        for(int x = 0; x<grid.Width; x++){
            for(int y = 0;y<grid.Height; y++){
                GeneraterBaseBrick(x,y);
                GeneraterPuzzleBrick(x,y);
            }
        }
    }

    private void GeneraterBaseBrick(int x,int y)
    {
        Color mapPixel = baseMap.GetPixel(x,y);
        if(mapPixel.a == 0){
            return;
        }

        //生成相应颜色对应的基底砖块
        foreach(ColorToPrefabs colorToPrefabs in baseColroMappings){
            if(colorToPrefabs.color.Equals(mapPixel)){
                //生成基底砖块
                Vector3 pos = new Vector3(x,y);
                grid.SetBase(pos,Instantiate(colorToPrefabs.brickPrefab));
                //Debug.Log(pos + "生成了" + colorToPrefabs.brickPrefab.name);
            }
        }
    }

    private void GeneraterPuzzleBrick(int x,int y)
    {
        Color mapPixel = puzzleMap.GetPixel(x,y);
        if(mapPixel.a == 0){
            return;
        }

        //生成相应颜色对应的拼图砖块
        foreach (ColorToPrefabs colorToPrefabs in puzzleColroMappings)
        {
            if(colorToPrefabs.color.Equals(mapPixel)){
                //生成砖块
                Vector3 pos = new Vector3(x,y);
                grid.SetPuzzle(pos,Instantiate(colorToPrefabs.brickPrefab));
                //Debug.Log(pos + "生成了" + colorToPrefabs.brickPrefab.name);
            }
        }
    }
}
