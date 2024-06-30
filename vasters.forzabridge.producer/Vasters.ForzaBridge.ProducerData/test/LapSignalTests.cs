using System;
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;


namespace Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry
{


    
    /// <summary> Test class for LapSignal </summary> 
    [TestFixture]
    public class LapSignalTests
    {
        private LapSignal _instance;

        /// <summary> Constructor </summary>
        public LapSignalTests()
        {
            _instance = CreateInstance();
        }

        /// <summary> Create instance of LapSignal </summary>
        public LapSignal CreateInstance()
        {
            var instance = new LapSignal();
            instance.LapId = "test_string";
            instance.CarId = "test_string";
            instance.SessionId = "test_string";
            instance.Timespan = new global::Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry.LapTimespan();
            return instance;
        }
        /// <summary> Testing property LapId  </summary>
        [Test]
        public void TestLapIdProperty()
        {
            var testValue = "test_string";
            _instance.LapId = testValue;
            _instance.LapId.Should().Be(testValue);
        }
        /// <summary> Testing property CarId  </summary>
        [Test]
        public void TestCarIdProperty()
        {
            var testValue = "test_string";
            _instance.CarId = testValue;
            _instance.CarId.Should().Be(testValue);
        }
        /// <summary> Testing property SessionId  </summary>
        [Test]
        public void TestSessionIdProperty()
        {
            var testValue = "test_string";
            _instance.SessionId = testValue;
            _instance.SessionId.Should().Be(testValue);
        }
        /// <summary> Testing property Timespan  </summary>
        [Test]
        public void TestTimespanProperty()
        {
            var testValue = new global::Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry.LapTimespan();
            _instance.Timespan = testValue;
            _instance.Timespan.Should().BeEquivalentTo(testValue);
        }
        /// <summary> Testing Avro serializer </summary>
        [Test]
        public void Test_ToByteArray_FromData()
        {
            var mediaType = "application/vnd.apache.avro+avro";
            var bytes = _instance.ToByteArray(mediaType);
            var newInstance = LapSignal.FromData(bytes, mediaType);
            _instance.Should().BeEquivalentTo(newInstance);
        }
    }


}
