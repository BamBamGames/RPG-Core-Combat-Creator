namespace RPG.Control
{
    public interface IRaycastable
    {
        bool HandleRaycast(PlayerController callingController);
        PlayerController.CursorType GetCursorType();
    }
}