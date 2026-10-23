using UnityEngine;

// Équivalent de BP_Cell.
public class Cell : MonoBehaviour
{
    // ActivateCell : Get Components by Class (CellAction) -> For Each -> Execute(Player).
    public void ActivateCell(Player player)
    {
        CellAction[] actions = GetComponents<CellAction>();
        foreach (CellAction action in actions)
        {
            action.Execute(player);
        }
    }
}
