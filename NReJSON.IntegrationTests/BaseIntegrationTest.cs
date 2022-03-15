using System;
using StackExchange.Redis;

namespace NReJSON.IntegrationTests
{
    public abstract class BaseIntegrationTest : IDisposable
    {
        private static readonly ISerializerProxy _serializer = new TestJsonSerializer();
        private readonly ConnectionMultiplexer _muxer;
        protected readonly IDatabase _db;

        protected BaseIntegrationTest()
        {
            _muxer = ConnectionMultiplexer.Connect("localhost:6381");
            _db = _muxer.GetDatabase(0);

            NReJSONSerializer.SerializerProxy = _serializer;
        }

        public void Dispose()
        {
            _muxer.Dispose();
        }
    }
}
