using System;
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;


namespace Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry
{


    
    /// <summary> Test class for LapTimespan </summary> 
    [TestFixture]
    public class LapTimespanTests
    {
        private LapTimespan _instance;

        /// <summary> Constructor </summary>
        public LapTimespanTests()
        {
            _instance = CreateInstance();
        }

        /// <summary> Create instance of LapTimespan </summary>
        public LapTimespan CreateInstance()
        {
            var instance = new LapTimespan();
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
            var newInstance = LapTimespan.FromData(bytes, mediaType);
            _instance.Should().BeEquivalentTo(newInstance);
        }
    }


}
