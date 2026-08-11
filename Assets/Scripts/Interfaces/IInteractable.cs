namespace Interfaces
{
    public interface IInteractable
    {
        string InteractionHint { get; }
    
        void Interact(Player.Player player);
    } 
}