using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using NServiceBus.MessageInterfaces;
using NServiceBus.Pipeline;
using NServiceBus.Pipeline.Contexts;
using NServiceBus.Serialization;
using NServiceBus.Serializers.Binary;
using NServiceBus.Serializers.Json;
using NServiceBus.Serializers.XML;
using NServiceBus.Unicast.Messages;

namespace NServiceBus.MultiDeserializer
{
    public class MultiDeserializer : IBehavior<IncomingContext>
    {
        private readonly IMessageSerializer _defaultSerializer;
        private readonly LogicalMessageFactory _logicalMessageFactory;
        private readonly MessageMetadataRegistry _metadataRegistry;

        private readonly Dictionary<string, IMessageSerializer> serializers =
            new Dictionary<string, IMessageSerializer>();

        public MultiDeserializer(Configure config, Conventions conventions, IMessageSerializer defaultSerializer,
            IMessageMapper mapper,
            LogicalMessageFactory logicalMessageFactory, MessageMetadataRegistry metadataRegistry)
        {
            _defaultSerializer = defaultSerializer;
            _logicalMessageFactory = logicalMessageFactory;
            _metadataRegistry = metadataRegistry;

            var json = new JsonMessageSerializer(mapper);
            serializers.Add(json.ContentType, json);

            var bson = new BsonMessageSerializer(mapper);
            serializers.Add(bson.ContentType, bson);

            var binary = new BinaryMessageSerializer();
            serializers.Add(binary.ContentType, binary);

            var xml = new XmlMessageSerializer(mapper, conventions);
            List<Type> messageTypes = config.TypesToScan.Where(conventions.IsMessageType).ToList();

            xml.Initialize(messageTypes);
            serializers.Add(xml.ContentType, xml);

            if (!serializers.ContainsKey(_defaultSerializer.ContentType))
            {
                serializers.Add(_defaultSerializer.ContentType, _defaultSerializer);
            }
        }

        public void Invoke(IncomingContext context, Action next)
        {
            TransportMessage transportMessage = context.PhysicalMessage;

            //if (IsControlMessage(transportMessage))
            //{
            //    next();
            //    return;
            //}

            string contentType = _defaultSerializer.ContentType;

            if (context.PhysicalMessage.Headers.ContainsKey(Headers.ContentType))
            {
                contentType = context.PhysicalMessage.Headers[Headers.ContentType];
            }
            try
            {
                context.LogicalMessages = Extract(transportMessage, contentType);
            }
            catch (Exception exception)
            {
                throw new SerializationException(
                    string.Format(
                        "An error occurred while attempting to extract logical messages from transport message {0}",
                        transportMessage), exception);
            }

            next();
        }

        private List<LogicalMessage> Extract(TransportMessage physicalMessage, string contentType)
        {
            if (physicalMessage.Body == null || physicalMessage.Body.Length == 0)
            {
                return new List<LogicalMessage>();
            }

            string messageTypeIdentifier;
            var messageMetadata = new List<MessageMetadata>();

            if (physicalMessage.Headers.TryGetValue(Headers.EnclosedMessageTypes, out messageTypeIdentifier))
            {
                foreach (string messageTypeString in messageTypeIdentifier.Split(';'))
                {
                    MessageMetadata metadata = _metadataRegistry.GetMessageMetadata(messageTypeString);
                    if (metadata == null)
                    {
                        continue;
                    }
                    messageMetadata.Add(metadata);
                }
            }

            using (var stream = new MemoryStream(physicalMessage.Body))
            {
                List<Type> messageTypesToDeserialize = messageMetadata.Select(metadata => metadata.MessageType).ToList();

                return serializers[contentType].Deserialize(stream, messageTypesToDeserialize)
                    .Select(x => _logicalMessageFactory.Create(x.GetType(), x, physicalMessage.Headers))
                    .ToList();
            }
        }

        private static bool IsControlMessage(TransportMessage transportMessage)
        {
            return transportMessage.Headers != null &&
                   transportMessage.Headers.ContainsKey(Headers.ControlMessageHeader);
        }
    }
}