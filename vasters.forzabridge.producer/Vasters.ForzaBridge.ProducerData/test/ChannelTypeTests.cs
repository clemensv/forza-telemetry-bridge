using System;
using NUnit.Framework;
using FluentAssertions;

namespace Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry
{
    /// <summary> Test class for ChannelType </summary>
    [TestFixture]
    public class ChannelTypeTests
    {
        /// <summary> Test ChannelType Enum </summary>
        [Test]
        public void Test_ChannelType_Enum()
        {
            var values = Enum.GetValues(typeof(ChannelType));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "EngineMaxRpm"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "EngineIdleRpm"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "CurrentEngineRpm"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "AccelerationX"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "AccelerationY"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "AccelerationZ"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "VelocityX"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "VelocityY"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "VelocityZ"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "AngularVelocityX"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "AngularVelocityY"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "AngularVelocityZ"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "Yaw"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "Pitch"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "Roll"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "NormalizedSuspensionTravelFrontLeft"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "NormalizedSuspensionTravelFrontRight"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "NormalizedSuspensionTravelRearLeft"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "NormalizedSuspensionTravelRearRight"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "TireSlipRatioFrontLeft"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "TireSlipRatioFrontRight"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "TireSlipRatioRearLeft"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "TireSlipRatioRearRight"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "WheelRotationSpeedFrontLeft"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "WheelRotationSpeedFrontRight"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "WheelRotationSpeedRearLeft"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "WheelRotationSpeedRearRight"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "WheelOnRumbleStripFrontLeft"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "WheelOnRumbleStripFrontRight"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "WheelOnRumbleStripRearLeft"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "WheelOnRumbleStripRearRight"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "WheelInPuddleDepthFrontLeft"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "WheelInPuddleDepthFrontRight"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "WheelInPuddleDepthRearLeft"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "WheelInPuddleDepthRearRight"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "SurfaceRumbleFrontLeft"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "SurfaceRumbleFrontRight"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "SurfaceRumbleRearLeft"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "SurfaceRumbleRearRight"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "TireSlipAngleFrontLeft"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "TireSlipAngleFrontRight"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "TireSlipAngleRearLeft"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "TireSlipAngleRearRight"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "TireCombinedSlipFrontLeft"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "TireCombinedSlipFrontRight"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "TireCombinedSlipRearLeft"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "TireCombinedSlipRearRight"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "SuspensionTravelMetersFrontLeft"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "SuspensionTravelMetersFrontRight"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "SuspensionTravelMetersRearLeft"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "SuspensionTravelMetersRearRight"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "PositionX"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "PositionY"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "PositionZ"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "Speed"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "Power"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "Torque"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "TireTempFrontLeft"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "TireTempFrontRight"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "TireTempRearLeft"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "TireTempRearRight"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "Boost"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "Fuel"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "DistanceTraveled"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "RacePosition"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "Accel"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "Brake"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "Clutch"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "HandBrake"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "Gear"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "Steer"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "NormalizedDrivingLine"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "NormalizedAIBrakeDifference"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "TireWearFrontLeft"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "TireWearFrontRight"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "TireWearRearLeft"));
            Assert.That(Enum.IsDefined(typeof(ChannelType), "TireWearRearRight"));
        }
    }
}