using UnityEngine;

// Équivalent de BPC_Action_Portail : la case "laisse un mot" au player
// avec 2 destinations, et le player attend la touche 1 ou 2.
public class CellAction_Portail : CellAction
{
    // Équivalent de DestinationA / DestinationB (Instance Editable).
    // Numéros de case dans le tableau Cells du Board (commence à 0).
    [SerializeField] private int destinationA;
    [SerializeField] private int destinationB;

    public override void Execute(Player player)
    {
        base.Execute(player); // = Parent: Execute

        player.PortalA = destinationA;
        player.PortalB = destinationB;
        player.IsChoosing = true;

        Debug.Log("Portail. Appuie sur 1 (case " + destinationA + ") ou 2 (case " + destinationB + ").");
    }
}
