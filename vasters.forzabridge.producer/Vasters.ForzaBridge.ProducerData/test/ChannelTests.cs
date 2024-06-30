using System;
using System.Collections.Generic;
using NUnit.Framework;
using FluentAssertions;


namespace Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry
{


    
    /// <summary> Test class for Channel </summary> 
    [TestFixture]
    public class ChannelTests
    {
        private Channel _instance;

        /// <summary> Constructor </summary>
        public ChannelTests()
        {
            _instance = CreateInstance();
        }

        /// <summary> Create instance of Channel </summary>
        public Channel CreateInstance()
        {
            var instance = new Channel();
            instance.ChannelId = new global::Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry.ChannelType();
            instance.CarId = "test_string";
            instance.SessionId = "test_string";
            instance.LapId = "test_string";
            instance.SampleCount = 42L;
            instance.Frequency = 42L;
            instance.Timespan = new global::Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry.BatchTimespan();
            instance.Data = new List<double>();
            return instance;
        }
        /// <summary> Testing property ChannelId  </summary>
        [Test]
        public void TestChannelIdProperty()
        {
            var testValue = new global::Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry.ChannelType();
            _instance.ChannelId = testValue;
            _instance.ChannelId.Should().Be(testValue);
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
        /// <summary> Testing property LapId  </summary>
        [Test]
        public void TestLapIdProperty()
        {
            var testValue = "test_string";
            _instance.LapId = testValue;
            _instance.LapId.Should().Be(testValue);
        }
        /// <summary> Testing property SampleCount  </summary>
        [Test]
        public void TestSampleCountProperty()
        {
            var testValue = 42L;
            _instance.SampleCount = testValue;
            _instance.SampleCount.Should().Be(testValue);
        }
        /// <summary> Testing property Frequency  </summary>
        [Test]
        public void TestFrequencyProperty()
        {
            var testValue = 42L;
            _instance.Frequency = testValue;
            _instance.Frequency.Should().Be(testValue);
        }
        /// <summary> Testing property Timespan  </summary>
        [Test]
        public void TestTimespanProperty()
        {
            var testValue = new global::Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry.BatchTimespan();
            _instance.Timespan = testValue;
            _instance.Timespan.Should().BeEquivalentTo(testValue);
        }
        /// <summary> Testing property Data  </summary>
        [Test]
        public void TestDataProperty()
        {
            var testValue = new List<double>();
            _instance.Data = testValue;
            _instance.Data.Should().AllBeEquivalentTo(testValue);
        }
        /// <summary> Testing Avro serializer </summary>
        [Test]
        public void Test_ToByteArray_FromData()
        {
            var mediaType = "application/vnd.apache.avro+avro";
            var bytes = _instance.ToByteArray(mediaType);
            var newInstance = Channel.FromData(bytes, mediaType);
            _instance.Should().BeEquivalentTo(newInstance);
        }
    }


}
