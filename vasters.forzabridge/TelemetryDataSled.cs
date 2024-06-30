namespace Vasters.ForzaBridge
{
    public class TelemetryDataSled
    {
        public int IsRaceOn; // S32
        public uint TimestampMS; // U32
        public float EngineMaxRpm; // F32
        public float EngineIdleRpm; // F32
        public float CurrentEngineRpm; // F32
        public float AccelerationX; // F32
        public float AccelerationY; // F32
        public float AccelerationZ; // F32
        public float VelocityX; // F32
        public float VelocityY; // F32
        public float VelocityZ; // F32
        public float AngularVelocityX; // F32
        public float AngularVelocityY; // F32
        public float AngularVelocityZ; // F32
        public float Yaw; // F32
        public float Pitch; // F32
        public float Roll; // F32
        public float NormalizedSuspensionTravelFrontLeft; // F32
        public float NormalizedSuspensionTravelFrontRight; // F32
        public float NormalizedSuspensionTravelRearLeft; // F32
        public float NormalizedSuspensionTravelRearRight; // F32
        public float TireSlipRatioFrontLeft; // F32
        public float TireSlipRatioFrontRight; // F32
        public float TireSlipRatioRearLeft; // F32
        public float TireSlipRatioRearRight; // F32
        public float WheelRotationSpeedFrontLeft; // F32
        public float WheelRotationSpeedFrontRight; // F32
        public float WheelRotationSpeedRearLeft; // F32
        public float WheelRotationSpeedRearRight; // F32
        public int WheelOnRumbleStripFrontLeft; // S32
        public int WheelOnRumbleStripFrontRight; // S32
        public int WheelOnRumbleStripRearLeft; // S32
        public int WheelOnRumbleStripRearRight; // S32
        public float WheelInPuddleDepthFrontLeft; // F32
        public float WheelInPuddleDepthFrontRight; // F32
        public float WheelInPuddleDepthRearLeft; // F32
        public float WheelInPuddleDepthRearRight; // F32
        public float SurfaceRumbleFrontLeft; // F32
        public float SurfaceRumbleFrontRight; // F32
        public float SurfaceRumbleRearLeft; // F32
        public float SurfaceRumbleRearRight; // F32
        public float TireSlipAngleFrontLeft; // F32
        public float TireSlipAngleFrontRight; // F32
        public float TireSlipAngleRearLeft; // F32
        public float TireSlipAngleRearRight; // F32
        public float TireCombinedSlipFrontLeft; // F32
        public float TireCombinedSlipFrontRight; // F32
        public float TireCombinedSlipRearLeft; // F32
        public float TireCombinedSlipRearRight; // F32
        public float SuspensionTravelMetersFrontLeft; // F32
        public float SuspensionTravelMetersFrontRight; // F32
        public float SuspensionTravelMetersRearLeft; // F32
        public float SuspensionTravelMetersRearRight; // F32
        public int CarOrdinal; // S32
        public int CarClass; // S32
        public int CarPerformanceIndex; // S32
        public int DrivetrainType; // S32
        public int NumCylinders; // S32
    }
}
