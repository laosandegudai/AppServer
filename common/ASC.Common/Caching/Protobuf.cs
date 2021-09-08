using System;

using Confluent.Kafka;

using Google.Protobuf;
using Google.Protobuf.Collections;

namespace ASC.Common.Caching
{
    public class ProtobufSerializer<T> : ISerializer<T> where T : IMessage<T>, new()
    {
        public byte[] Serialize(T data, SerializationContext context)
        {
            return data.ToByteArray();
        }
    }

    public class ProtobufDeserializer<T> : IDeserializer<T> where T : IMessage<T>, new()
    {
        private readonly MessageParser<T> parser;

        public ProtobufDeserializer()
        {
            parser = new MessageParser<T>(() => new T());
        }

        public T Deserialize(ReadOnlySpan<byte> data, bool isNull, SerializationContext context)
        {
            return parser.ParseFrom(data.ToArray());
        }
    }

    public static class GuidExtension
    {
        public static ByteString ToByteString(this Guid id)
        {
            return ByteString.CopyFrom(id.ToByteArray());
        }
        public static Guid FromByteString(this ByteString id)
        {
            if (id.Length == 0) return new Guid("00000000-0000-0000-0000-000000000000");

            return new Guid(id.ToByteArray());
        }
    }

    public static class RepeatedFieldExtension
    {
        public static int RemoveAll<T>(this RepeatedField<T> repeatedField, Predicate<T> match)
        {
            int counter = 0;

            foreach (var field in repeatedField)
            {
                if (match.Invoke(field))
                {
                    repeatedField.Remove(field);
                    counter++;
                }
            }

            return counter;
        }
    }
}
