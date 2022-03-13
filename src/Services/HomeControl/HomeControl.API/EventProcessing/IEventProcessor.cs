namespace HomeControl.API.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}
