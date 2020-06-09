namespace RPG.Saving
{
    public interface ISaveable
    {
        object CaptureState();

        /**
         * We make sure this function always called after Awake(), but before Start().
         * 
         * So in Start() we can rely on a state that was restored.
         * In the RestoreState() we can be happy that everything in Awake() has already been called.
         */
        void RestoreState(object state);
    }
}