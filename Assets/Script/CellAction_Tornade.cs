using UnityEngine;

// Équivalent de BPC_Action_Tornade : téléporte le pion sur une case au hasard.
public class CellAction_Tornade : CellAction
{
    public override void Execute(Player player)
    {
        base.Execute(player); // = Parent: Execute

        // Random.Range (int) exclut le max : (0, Count) donne déjà 0 à Count-1,
        // donc pas besoin du "- 1" qu'on avait dans Unreal.
        int destination = Random.Range(0, player.Board.GetCellCount());

        Debug.Log("Tornade ! -> case " + destination);
        player.PlaceOnCell(destination);
    }
}
