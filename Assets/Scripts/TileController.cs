///-----------------------------------------------------------------
///   Class:          TileController
///   Description:    Updates information relative to this tile
///   Author:         VueCode
///   GitHub:         https://github.com/ivuecode/
///-----------------------------------------------------------------

using System;
using UnityEngine;
using UnityEngine.UI;

public class TileController : MonoBehaviour
{
    [Header("Component References")]
    public GameStateController gameController;                       // Reference to the gamecontroller
    public Button interactiveButton;                                 // Reference to this button
    public Text internalText;                                        // Reference to this Text
    
    /// <summary>
    /// Called everytime we press the button, we update the state of this tile.
    /// The internal tracking for whos position (the text component) and disable the button
    /// </summary>
    public void UpdateTile()
    {
        //updating bufffer
        internalText.text = gameController.GetPlayersTurn();
        //updating image
        interactiveButton.image.sprite = gameController.GetPlayerSprite();
        //updating status
        interactiveButton.interactable = false;
        //check if the game is over
        gameController.EndTurn();
    }

    /// <summary>
    /// Resets the tile properties
    /// - text component
    /// - buttton image
    /// </summary>
    public void ResetTile()
    {
        internalText.text = "";
        interactiveButton.image.sprite = gameController.tileEmpty;
    }
}