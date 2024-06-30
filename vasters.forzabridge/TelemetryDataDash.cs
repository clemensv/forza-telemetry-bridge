namespace Vasters.ForzaBridge
{
    public class TelemetryDataDash : TelemetryDataSled
    {
        public float PositionX; // F32
        public float PositionY; // F32
        public float PositionZ; // F32
        public float Speed; // F32
        public float Power; // F32
        public float Torque; // F32
        public float TireTempFrontLeft; // F32
        public float TireTempFrontRight; // F32
        public float TireTempRearLeft; // F32
        public float TireTempRearRight; // F32
        public float Boost; // F32
        public float Fuel; // F32
        public float DistanceTraveled; // F32
        public float BestLap; // F32
        public float LastLap; // F32
        public float CurrentLap; // F32
        public float CurrentRaceTime; // F32
        public ushort LapNumber; // U16
        public byte RacePosition; // U8
        public byte Accel; // U8
        public byte Brake; // U8
        public byte Clutch; // U8
        public byte HandBrake; // U8
        public byte Gear; // U8
        public sbyte Steer; // S8
        public sbyte NormalizedDrivingLine; // S8
        public sbyte NormalizedAIBrakeDifference; // S8
        public float TireWearFrontLeft; // F32
        public float TireWearFrontRight; // F32
        public float TireWearRearLeft; // F32
        public float TireWearRearRight; // F32
        public int TrackOrdinal; // S32
    }
}
