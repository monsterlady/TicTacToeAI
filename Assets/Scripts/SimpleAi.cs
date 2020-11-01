using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class SimpleAI : MonoBehaviour
    {
        [Header("Component References")]
        public GameStateController gameController;                       // Reference to the gamecontroller
        private Text[] board;                                            // Reference to Text


        void Start()
        {   
            board = gameController.TileList;
        }

         private int Score(string result)
         {
             switch (result)
             {
                 case "X":
                      return -1;
                 case "O":
                      return 1;
                 //DRAW
                 default:
                     return 0;
             }
         }
         
         private int MaxMinAlgorithm(Text[] currentboard,int depth,bool isMax)
         {
             String result = gameController.CheckWinner(currentboard);
             //Debug.Log("深度: " + depth + " 结果: " + result);
             //如果不是下一轮就返回score
             if (!result.Equals("TURN"))
             {
                 return Score(result);
             }

             if (isMax)
             {
                 int bestScore = int.MinValue;
                 for (int i = 0; i < currentboard.Length; i++)
                 {
                     //空位
                     if (currentboard[i].text.Equals(""))
                     {
                         currentboard[i].text = "O";
                         int tempscore = MaxMinAlgorithm(currentboard,depth + 1, false);
                         currentboard[i].text = "";
                         bestScore = Math.Max(bestScore, tempscore);
                     }
                 }
                 return bestScore;
             }
             else
             {
                 int bestScore = int.MaxValue;
                 for (int i = 0; i < currentboard.Length; i++)
                 {
                     //空位
                     if (currentboard[i].text.Equals(""))
                     {
                         currentboard[i].text = "X";
                         int tempscore = MaxMinAlgorithm(currentboard, depth + 1,true);
                         currentboard[i].text = "";
                         bestScore = Math.Min(bestScore, tempscore);
                     }
                 }
                 return bestScore;
             }
             
         }

         public void BestMove()
         {
             //TODO 结合算法
             int bestScore = int.MinValue;
             string bestTileName = "";

             for (int i = 0; i < board.Length; i++)
             {
                 //空位置
                 if (board[i].text.Equals(""))
                 {
                     // Test
                     // bestTileName = GetTileByIndex(i);
                     // break;
                      board[i].text = "O";
                      int tempScore = MaxMinAlgorithm(board,  0,false);
                      //Debug.Log(tempScore + " 分数");
                      board[i].text = "";
                      if (tempScore > bestScore)
                      {
                          bestScore = tempScore;
                          bestTileName = GetTileByIndex(i);
                      }
                 }
             }
             
            //选择完毕之后
            GameObject bestChoice = GameObject.Find(bestTileName);
            //模拟点击事件
            bestChoice.GetComponent<TileController>().UpdateTile();
        }

        public String GetTileByIndex(int index)
        {
            String objectName = "";
            switch(index){
                case 0 :
                    objectName = "Tile_1";
                    break;
                case 1 :
                    objectName = "Tile_2";
                    break;
                case 2 :
                    objectName = "Tile_3";
                    break;
                case 3 :
                    objectName = "Tile_4";
                    break;
                case 4 :
                    objectName = "Tile_5";
                    break;
                case 5 :
                    objectName = "Tile_6";
                    break;
                case 6 :
                    objectName = "Tile_7";
                    break;
                case 7 :
                    objectName = "Tile_8";
                    break;
                case 8 :
                    objectName = "Tile_9";
                    break;
                default:
                    objectName = "";
                    break;
            }
            return objectName;
        }
    }
