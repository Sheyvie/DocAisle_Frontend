namespace DocAisle_Auth.RabbitMQ
{
    public interface IMessagePublisherInterface
    {
        void PublishMessage(object message, string exchangeTypeName);
    }
}
