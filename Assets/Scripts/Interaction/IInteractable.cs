namespace EscapeCampus.Interaction
{
    public interface IInteractable
    {
        string InteractionPrompt { get; }
        void Interact();
    }
}
