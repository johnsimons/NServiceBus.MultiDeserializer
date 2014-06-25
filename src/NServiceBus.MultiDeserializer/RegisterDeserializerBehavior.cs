using NServiceBus.Pipeline;

namespace NServiceBus.MultiDeserializer
{
    public class RegisterDeserializerBehavior : INeedInitialization
    {
        public void Init(Configure config)
        {
            config.Pipeline.Replace(WellKnownBehavior.DeserializeMessages, typeof(MultiDeserializer), "Deserializes messages based on the content type header");
        }
    }
}