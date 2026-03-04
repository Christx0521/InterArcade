using UnityEngine;

public class PAC_GhostScatter : PAC_GhostBehavior
{
    private void OnDisable()
    {
        ghost.chase.Enable();
    } 

    private void OnTriggerEnter2D(Collider2D other)
    {
        PAC_Node node = other.GetComponent<PAC_Node>();

        if (node == null || !enabled || ghost.frightened.enabled)
            return;

        if (node.availableDirections == null || node.availableDirections.Count == 0)
            return;

        int index = Random.Range(0, node.availableDirections.Count);

        if (node.availableDirections.Count > 1 &&
            node.availableDirections[index] == -ghost.movement.direction)
        {
            index = (index + 1) % node.availableDirections.Count;
        }

        ghost.movement.SetDirection(node.availableDirections[index]);
    }
    
}
