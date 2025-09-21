namespace EventBus.Messages.Common
{
    public class BaseIntegrationEvent
    {
        public string CorrelationId { get; set; }

        public DateTime CreatedDate { get; set; }

        public BaseIntegrationEvent()
        {
            CorrelationId = Guid.NewGuid().ToString();
            CreatedDate = DateTime.UtcNow;
        }

        public BaseIntegrationEvent(Guid corelationId, DateTime creationDate)
        {
            CorrelationId = corelationId.ToString();
            CreatedDate = creationDate;
        }
    }
}