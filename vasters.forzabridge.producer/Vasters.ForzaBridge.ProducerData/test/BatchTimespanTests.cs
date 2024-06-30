using System;
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;


namespace Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry
{


    
    /// <summary> Test class for BatchTimespan </summary> 
    [TestFixture]
    public class BatchTimespanTests
    {
        private BatchTimespan _instance;

        /// <summary> Constructor </summary>
        public BatchTimespanTests()
        {
            _instance = CreateInstance();
        }

        /// <summary> Create instance of BatchTimespan </summary>
        public BatchTimespan CreateInstance()
        {
            var instance = new BatchTimespan();
            instance.StartTS = 42L;
            instance.EndTS = 42L;
            return instance;
        }
        /// <summary> Testing property StartTS  </summary>
        [Test]
        public void TestStartTSProperty()
        {
            var testValue = 42L;
            _instance.StartTS = testValue;
            _instance.StartTS.Should().Be(testValue);
        }
        /// <summary> Testing property EndTS  </summary>
        [Test]
        public void TestEndTSProperty()
        {
            var testValue = 42L;
            _instance.EndTS = testValue;
            _instance.EndTS.Should().Be(testValue);
        }
        /// <summary> Testing Avro serializer </summary>
        [Test]
        public void Test_ToByteArray_FromData()
        {
            var mediaType = "application/vnd.apache.avro+avro";
            var bytes = _instance.ToByteArray(mediaType);
            var newInstance = BatchTimespan.FromData(bytes, mediaType);
            _instance.Should().BeEquivalentTo(newInstance);
        }
    }


}
