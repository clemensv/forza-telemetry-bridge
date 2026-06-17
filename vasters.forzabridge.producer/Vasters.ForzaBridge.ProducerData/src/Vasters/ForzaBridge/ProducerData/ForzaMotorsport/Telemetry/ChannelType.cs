#pragma warning disable CS8618
#pragma warning disable CS8603

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Vasters.ForzaBridge.ProducerData.ForzaMotorsport.Telemetry
{
    #pragma warning disable 1591
    
    /// <summary>
    /// ChannelType
    /// </summary>
    public enum ChannelType
    {
        EngineMaxRpm,
        EngineIdleRpm,
        CurrentEngineRpm,
        AccelerationX,
        AccelerationY,
        AccelerationZ,
        VelocityX,
        VelocityY,
        VelocityZ,
        AngularVelocityX,
        AngularVelocityY,
        AngularVelocityZ,
        Yaw,
        Pitch,
        Roll,
        NormalizedSuspensionTravelFrontLeft,
        NormalizedSuspensionTravelFrontRight,
        NormalizedSuspensionTravelRearLeft,
        NormalizedSuspensionTravelRearRight,
        TireSlipRatioFrontLeft,
        TireSlipRatioFrontRight,
        TireSlipRatioRearLeft,
        TireSlipRatioRearRight,
        WheelRotationSpeedFrontLeft,
        WheelRotationSpeedFrontRight,
        WheelRotationSpeedRearLeft,
        WheelRotationSpeedRearRight,
        WheelOnRumbleStripFrontLeft,
        WheelOnRumbleStripFrontRight,
        WheelOnRumbleStripRearLeft,
        WheelOnRumbleStripRearRight,
        WheelInPuddleDepthFrontLeft,
        WheelInPuddleDepthFrontRight,
        WheelInPuddleDepthRearLeft,
        WheelInPuddleDepthRearRight,
        SurfaceRumbleFrontLeft,
        SurfaceRumbleFrontRight,
        SurfaceRumbleRearLeft,
        SurfaceRumbleRearRight,
        TireSlipAngleFrontLeft,
        TireSlipAngleFrontRight,
        TireSlipAngleRearLeft,
        TireSlipAngleRearRight,
        TireCombinedSlipFrontLeft,
        TireCombinedSlipFrontRight,
        TireCombinedSlipRearLeft,
        TireCombinedSlipRearRight,
        SuspensionTravelMetersFrontLeft,
        SuspensionTravelMetersFrontRight,
        SuspensionTravelMetersRearLeft,
        SuspensionTravelMetersRearRight,
        PositionX,
        PositionY,
        PositionZ,
        Speed,
        Power,
        Torque,
        TireTempFrontLeft,
        TireTempFrontRight,
        TireTempRearLeft,
        TireTempRearRight,
        Boost,
        Fuel,
        DistanceTraveled,
        BestLap,
        LastLap,
        CurrentLap,
        CurrentRaceTime,
        LapNumber,
        RacePosition,
        Accel,
        Brake,
        Clutch,
        HandBrake,
        Gear,
        Steer,
        NormalizedDrivingLine,
        NormalizedAIBrakeDifference,
        TireWearFrontLeft,
        TireWearFrontRight,
        TireWearRearLeft,
        TireWearRearRight,
        TrackOrdinal
    }
}