///-----------------------------------------------------------------
///   Class:          GameStateController
///   Description:    Handles the current state of the game and whos turn it is
///   Author:         VueCode
///   GitHub:         https://github.com/ivuecode/
///-----------------------------------------------------------------

using System;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class GameStateController : MonoBehaviour
{
    [Header("TitleBar References")]
    public Image playerXIcon;                                        // Reference to the playerX icon
    public Image playerOIcon;                                        // Reference to the playerO icon
    public InputField player1InputField;                             // Reference to P1 input field
    public InputField player2InputField;                             // Refernece to P2 input field
    public Text winnerText;                                          // Displays the winners name

    [Header("Misc References")]
    public GameObject endGameState;                                  // Game footer container + winner text

    [Header("Asset References")]
    public Sprite tilePlayerO;                                       // Sprite reference to O tile
    public Sprite tilePlayerX;                                       // Sprite reference to X tile
    public Sprite tileEmpty;                                         // Sprite reference to empty tile
    public Text[] tileList;                                          // Gets a list of all the tiles in the scene

    [Header("GameState Settings")]
    public Color inactivePlayerColor;                                // Color to display for the inactive player icon
    public Color activePlayerColor;                                  // Color to display for the active player icon
    public string whoPlaysFirst;                                     // Who plays first (X : 0) {NOTE! no checks are made to ensure this is either X or O}

    [Header("Game AI")] public GameObject simpleAI;
    
    [Header("Private Variables")]
    private string playerTurn;                                       // Internal tracking whos turn is it
    private string player1Name;                                      // Player1 display name
    private string player2Name;                                      // Player2 display name
    private int moveCount;                                           // Internal move counter
    



    /// <summary>
    /// Start is called on the first active frame
    /// </summary>
    private void Start()
    {
        // Set the internal tracker of whos turn is first and setup UI icon feedback for whos turn it is
        playerTurn = whoPlaysFirst;
        if (playerTurn == "X") playerOIcon.color = inactivePlayerColor;
        else playerXIcon.color = inactivePlayerColor;
        
        //Adds a listener to the name input fields and invokes a method when the value changes. This is a callback.
        player1InputField.onValueChanged.AddListener(delegate { OnPlayer1NameChanged(); });
        player2InputField.onValueChanged.AddListener(delegate { OnPlayer2NameChanged(); });

        // Set the default values to what tthe inputField text is
        player1Name = player1InputField.text;
        player2Name = player2InputField.text;
    }

    /// <summary>
    /// Called at the end of every turn to check for win conditions
    /// Hardcoded all possible win conditions (8)
    /// We just take position of tiles and check the neighbours (within a row)
    /// 
    /// Tiles are numbered 0..8 from left to right, row by row, example:
    /// [0][1][2]
    /// [3][4][5]
    /// [6][7][8]
    /// </summary>
    public void EndTurn()
    {
        String result = CheckWinner(tileList);
        if (result.Equals("TURN"))
        {
            ChangeTurn();
        }
        else
        {
            GameOver(result);
        }

    }
    public string CheckWinner(Text[] tempBoard)
    {
        String result = "TURN";
        if (Equal3(tempBoard[0].text,tempBoard[1].text,tempBoard[2].text))
        {
            result = tempBoard[0].text;
        }
        else if (Equal3(tempBoard[3].text,tempBoard[4].text,tempBoard[5].text))
        {
            result = tempBoard[3].text;
        }
        else if (Equal3(tempBoard[6].text,tempBoard[7].text,tempBoard[8].text))
        {
            result = tempBoard[6].text;
        }
        else if (Equal3(tempBoard[0].text,tempBoard[3].text,tempBoard[6].text))
        {
            result = tempBoard[0].text;
        }
        else if (Equal3(tempBoard[1].text,tempBoard[4].text,tempBoard[7].text))
        {
            result = tempBoard[1].text;
        }
        else if (Equal3(tempBoard[2].text,tempBoard[5].text,tempBoard[8].text))
        {
            result = tempBoard[2].text;
        }
        else if (Equal3(tempBoard[0].text,tempBoard[4].text,tempBoard[8].text))
        {
            result = tempBoard[0].text;
        }
        else if (Equal3(tempBoard[2].text,tempBoard[4].text,tempBoard[6].text))
        {
            result = tempBoard[2].text;
        }
        else if (IsDraw(tempBoard))
        {
            result = "D";
        }

        return result;
    }
        
    private bool Equal3(string a, string b, string c)
    {
        return a.Equals(b) && b.Equals(c) && !a.Equals("");
    }

    private bool IsDraw(Text[] tempBoard)
    {
        bool b = true;
        foreach (Text each in tempBoard)
        {
            if (each.text.Equals(""))
            {
                b = false;
                break;
            }
        }
        return b;
    }

    // public string CheckWinner(Text[] board)
    // {
    //     String result = "";
    //     if (board[0].text == playerTurn && board[1].text == playerTurn && board[2].text == playerTurn)
    //     {
    //         result = playerTurn;
    //     }
    //     else if (board[3].text == playerTurn && board[4].text == playerTurn && board[5].text == playerTurn)
    //     {
    //         result = playerTurn;
    //     }
    //     else if (board[6].text == playerTurn && board[7].text == playerTurn && board[8].text == playerTurn)
    //     {
    //         result = playerTurn;
    //     }
    //     else if (board[0].text == playerTurn && board[3].text == playerTurn && board[6].text == playerTurn)
    //     {
    //         result = playerTurn;
    //     }
    //     else if (board[1].text == playerTurn && board[4].text == playerTurn && board[7].text == playerTurn)
    //     {
    //         result = playerTurn;
    //     }
    //     else if (board[2].text == playerTurn && board[5].text == playerTurn && board[8].text == playerTurn)
    //     {
    //         result = playerTurn;
    //     }
    //     else if (board[0].text == playerTurn && board[4].text == playerTurn && board[8].text == playerTurn)
    //     {
    //         result = playerTurn;
    //     }
    //     else if (board[2].text == playerTurn && board[4].text == playerTurn && board[6].text == playerTurn)
    //     {
    //         result = playerTurn;
    //     }
    //     else if (moveCount >= 9)
    //     {
    //         result = "D";
    //     }
    //     return result;
    // }

    /// <summary>
    /// Changes the internal tracker for whos turn it is
    /// </summary>
    public void ChangeTurn()
    {
        // This is called a Ternary operator which evaluates "X" and results in "O" or "X" based on truths
        // We then just change some ui feedback like colors.
        playerTurn = (playerTurn == "X") ? "O" : "X";
        if (playerTurn == "X")
        {
            playerXIcon.color = activePlayerColor;
            playerOIcon.color = inactivePlayerColor;
        }
        else
        {
            //轮到AI
            playerXIcon.color = inactivePlayerColor;
            playerOIcon.color = activePlayerColor;
            //选择最好的一步
            simpleAI.GetComponent<SimpleAI>().BestMove();
        }
    }

    /// <summary>
    /// Called when the game has found a win condition or draw
    /// </summary>
    /// <param name="winningPlayer">X O D</param>
    private void GameOver(string winningPlayer)
    {
        switch (winningPlayer)
        {
            case "D":
                winnerText.text = "DRAW";
                break;
            case "X":
                winnerText.text = player1Name;
                break;
            case "O":
                winnerText.text = player2Name;
                break;
        }
        Debug.Log(winnerText.text.ToString());
        endGameState.SetActive(true);
        ToggleButtonState(false);
    }

    /// <summary>
    /// Restarts the game state
    /// </summary>
    public void RestartGame()
    {
        // Reset some gamestate properties
        moveCount = 0;
        playerTurn = whoPlaysFirst;
        ToggleButtonState(true);
        endGameState.SetActive(false);

        // Loop though all tiles and reset them
        for (int i = 0; i < tileList.Length; i++)
        {
            tileList[i].GetComponentInParent<TileController>().ResetTile();
        }
    }

    /// <summary>
    /// Enables or disables all the buttons
    /// </summary>
    private void ToggleButtonState(bool state)
    {
        for (int i = 0; i < tileList.Length; i++)
        {
            tileList[i].GetComponentInParent<Button>().interactable = state;
        }
    }

    /// <summary>
    /// Returns the current players turn (X / O)
    /// </summary>
    public string GetPlayersTurn()
    {
        return playerTurn;
    }

    /// <summary>
    /// Retruns the display sprite (X / 0)
    /// </summary>
    public Sprite GetPlayerSprite()
    {
        if (playerTurn == "X") return tilePlayerX;
        else return tilePlayerO;
    }

    /// <summary>
    /// Callback for when the P1_textfield is updated. We just update the string for Player1
    /// </summary>
    public void OnPlayer1NameChanged()
    {
        player1Name = player1InputField.text;
    }

    /// <summary>
    /// Callback for when the P2_textfield is updated. We just update the string for Player2
    /// </summary>
    public void OnPlayer2NameChanged()
    {
        player2Name = player2InputField.text;
    }

    public Text[] TileList => tileList;
    
}
