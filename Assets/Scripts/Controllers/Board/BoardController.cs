using System.Collections.Generic;
using UnityEngine;

public class BoardController : Singleton<BoardController>
{
    [SerializeField] private List<BlockBoard> blocks;

    private bool locked;

    private void Awake() {
        instance = this;
        SeActiveBlocks(false);
    }

    internal static void RestartGame(){
        instance.ResetBlock();
    }
    
    internal static void ClickBlock(BlockBoard block){
        if(instance.locked)
            return;

        int index = instance.blocks.IndexOf(block);
        print(index);
        GameLogicController.SendChoice(index);
        SeActiveBlocks(false);
        instance.locked = true;
    }

    internal static void PlayerTurn(){
        instance.locked = false;
        SeActiveBlocks(true);
        UIGame.WaitOpponent(false);
    }
    
    internal static void OpponentTurn(){
        instance.locked = true;
        SeActiveBlocks(false);
        UIGame.WaitOpponent(true);
    }

    internal static void ActiveMonster(int index, string type){
        instance.blocks[index].SetActivePrefab(type == "X" ? 1 : 0);
    }

    internal void ResetBlock(){
        foreach (BlockBoard item in blocks)
            item.ResetBlock();
    }

    internal static void SeActiveBlocks(bool value){
        foreach (BlockBoard item in instance.blocks)
            item.SetActive(value);
    }
}
