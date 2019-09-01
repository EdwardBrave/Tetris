using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Self;
    public MapGrid grid;
    public PlayerController controller;
    public GameObject figureObj;
    public Text scoreCount;
    public GameObject mainScreen;
    public Text mainScoreCount;
    public float gameSpeed = 1F;
    public float deltaSpeed = 1F;
    public float exeloration = 0F; 
    public bool isGameRunning = false;

    public GameManager()
    {
        Self = this;
    }

    [HideInInspector]
    public Figure figure;
    public void StartGame()
    {
        mainScreen.SetActive(false);
        grid.Start();
        isGameRunning = true;
        deltaSpeed = gameSpeed;
        
        LoadFigure();
        StartCoroutine(nameof(FigureStep));
        StartCoroutine(nameof(Accelerate));
        
    }

    void LoadFigure()
    {
        figure = Instantiate(figureObj).GetComponent<Figure>();
        figure.SetRandomType();
        if (!figure.Generate(new Vector2Int(3, 0), grid))
        {
            isGameRunning = false;
            Debug.Log("GameOver!!!");
            mainScoreCount.text = "Your score: \n" +(grid.spawnCounter + grid.removeCounter*2).ToString();
            StopCoroutine(nameof(FigureStep));
            StopCoroutine(nameof(Accelerate));
            mainScreen.SetActive(true);
            return;
        }
        figure.SetRandomColor(new []
        {
            MaterialGen.Pallet.Black,
            MaterialGen.Pallet.Brown,
            MaterialGen.Pallet.BlueGrey,
            MaterialGen.Pallet.Grey,
            MaterialGen.Pallet.White
        });
        scoreCount.text = (grid.spawnCounter + grid.removeCounter*2).ToString();
    }
    
    public void FlourIsReached()
    {
        MapCheck();
        LoadFigure();
    }

    void MapCheck() 
    {
        for (int y = grid.Data.Count - 1; y >= 0; y--)
        {
            bool rowIsFull = true;
            for (int x = grid.Data[y].Count - 1; x >= 0; x--)
            {
                if (grid.Data[y][x] == null)
                {
                    rowIsFull = false;
                    break;
                }
            }
            if (rowIsFull)
                grid.RemoveRow(y++);
        }
    }

    IEnumerator Accelerate()
    {
        while (isGameRunning)
        {
            yield return new WaitForSeconds(1);
            deltaSpeed *= exeloration;
        }
    }
    IEnumerator FigureStep()
    {
        while (isGameRunning)
        {
            
            yield return new WaitForSeconds(1/deltaSpeed);
            controller.onClick(PlayerController.DOWN);
        }
    }
}
