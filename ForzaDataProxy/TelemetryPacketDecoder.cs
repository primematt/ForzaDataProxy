using System;
using System.IO;
using System.Text.Json;

namespace ForzaDataProxy
{
    public class TelemetryPacketDecoder
    {
        public TelemetryPacketDecoder(byte[] packetData)
        {
            var dataReader = new BinaryReader(new MemoryStream(packetData));
            IsDriving = dataReader.ReadInt32() > 0;
            Timestamp = dataReader.ReadUInt32();

            EngineMaxRpm = dataReader.ReadSingle();
            EngineIdleRpm = dataReader.ReadSingle();
            EngineCurrentRpm = dataReader.ReadSingle();

            AccelerationX = dataReader.ReadSingle();
            AccelerationY = dataReader.ReadSingle();
            AccelerationZ = dataReader.ReadSingle();

            VelocityX = dataReader.ReadSingle();
            VelocityY = dataReader.ReadSingle();
            VelocityZ = dataReader.ReadSingle();

            AngularVelocityX = dataReader.ReadSingle();
            AngularVelocityY = dataReader.ReadSingle();
            AngularVelocityZ = dataReader.ReadSingle();

            Yaw = dataReader.ReadSingle();
            Pitch = dataReader.ReadSingle();
            Roll = dataReader.ReadSingle();

            NormalizedSuspensionTravelFrontLeft = dataReader.ReadSingle();
            NormalizedSuspensionTravelFrontRight = dataReader.ReadSingle();
            NormalizedSuspensionTravelRearLeft = dataReader.ReadSingle();
            NormalizedSuspensionTravelRearRight = dataReader.ReadSingle();

            TireSlipRatioFrontLeft = dataReader.ReadSingle();
            TireSlipRatioFrontRight = dataReader.ReadSingle();
            TireSlipRatioRearLeft = dataReader.ReadSingle();
            TireSlipRatioRearRight = dataReader.ReadSingle();

            WheelRotationSpeedFrontLeft = dataReader.ReadSingle();
            WheelRotationSpeedFrontRight = dataReader.ReadSingle();
            WheelRotationSpeedRearLeft = dataReader.ReadSingle();
            WheelRotationSpeedRearRight = dataReader.ReadSingle();

            WheelOnRumbleStripFrontLeft = dataReader.ReadInt32() > 0;
            WheelOnRumbleStripFrontRight = dataReader.ReadInt32() > 0;
            WheelOnRumbleStripRearLeft = dataReader.ReadInt32() > 0;
            WheelOnRumbleStripRearRight = dataReader.ReadInt32() > 0;

            WheelInPuddleDepthFrontLeft = dataReader.ReadSingle();
            WheelInPuddleDepthFrontRight = dataReader.ReadSingle();
            WheelInPuddleDepthRearLeft = dataReader.ReadSingle();
            WheelInPuddleDepthRearRight = dataReader.ReadSingle();

            SurfaceRumbleFrontLeft = dataReader.ReadSingle();
            SurfaceRumbleFrontRight = dataReader.ReadSingle();
            SurfaceRumbleRearLeft = dataReader.ReadSingle();
            SurfaceRumbleRearRight = dataReader.ReadSingle();

            TireSlipAngleFrontLeft = dataReader.ReadSingle();
            TireSlipAngleFrontRight = dataReader.ReadSingle();
            TireSlipAngleRearLeft = dataReader.ReadSingle();
            TireSlipAngleRearRight = dataReader.ReadSingle();

            TireCombinedSlipFrontLeft = dataReader.ReadSingle();
            TireCombinedSlipFrontRight = dataReader.ReadSingle();
            TireCombinedSlipRearLeft = dataReader.ReadSingle();
            TireCombinedSlipRearRight = dataReader.ReadSingle();

            SuspensionTravelMetersFrontLeft = dataReader.ReadSingle();
            SuspensionTravelMetersFrontRight = dataReader.ReadSingle();
            SuspensionTravelMetersRearLeft = dataReader.ReadSingle();
            SuspensionTravelMetersRearRight = dataReader.ReadSingle();

            CarOrdinal = dataReader.ReadInt32();
            CarClassRaw = dataReader.ReadInt32();
            CarPerformanceIndex = dataReader.ReadInt32();
            DrivetrainTypeRaw = dataReader.ReadInt32();
            NumCylinders = dataReader.ReadInt32();
            CarTypeRaw = dataReader.ReadInt32();

            Unknown1 = dataReader.ReadInt32();
            Unknown2 = dataReader.ReadInt32();

            PositionX = dataReader.ReadSingle();
            PositionY = dataReader.ReadSingle();
            PositionZ = dataReader.ReadSingle();

            Speed = dataReader.ReadSingle();
            Power = dataReader.ReadSingle();
            Torque = dataReader.ReadSingle();

            TireTempFrontLeft = dataReader.ReadSingle();
            TireTempFrontRight = dataReader.ReadSingle();
            TireTempRearLeft = dataReader.ReadSingle();
            TireTempRearRight = dataReader.ReadSingle();

            Boost = dataReader.ReadSingle();
            Fuel = dataReader.ReadSingle();
            DistanceTraveled = dataReader.ReadSingle();

            BestLap = dataReader.ReadSingle();
            LastLap = dataReader.ReadSingle();
            CurrentLap = dataReader.ReadSingle();
            CurrentRaceTime = dataReader.ReadSingle();
            LapNumber = dataReader.ReadUInt16();
            RacePosition = dataReader.ReadByte();
            Accel = dataReader.ReadByte();
            Brake = dataReader.ReadByte();
            Clutch = dataReader.ReadByte();
            HandBrake = dataReader.ReadByte();
            Gear = dataReader.ReadByte();
            Steer = dataReader.ReadSByte();

            NormalizedDrivingLine = dataReader.ReadSByte();
            NormalizedAIBrakeDifference = dataReader.ReadSByte();
        }

        public bool IsDriving { get; } // s32 IsRaceOn; // = 1 when race is on. = 0 when in menus/race stopped …
        public uint Timestamp { get; } // u32 TimestampMS; //Can overflow to 0 eventually

        public float EngineMaxRpm { get; } // f32 EngineMaxRpm;
        public float EngineIdleRpm { get; } // f32 EngineIdleRpm;
        public float EngineCurrentRpm { get; } // f32 CurrentEngineRpm;

        public float AccelerationX { get; } // f32 AccelerationX; //In the car’s local space; X = right, Y = up, Z = forward
        public float AccelerationY { get; } // f32 AccelerationY;
        public float AccelerationZ { get; } // f32 AccelerationZ;

        public float VelocityX { get; } // f32 VelocityX; //In the car’s local space; X = right, Y = up, Z = forward
        public float VelocityY { get; } // f32 VelocityY;
        public float VelocityZ { get; } // f32 VelocityZ;

        public float AngularVelocityX { get; } // f32 AngularVelocityX; //In the car’s local space; X = pitch, Y = yaw, Z = roll
        public float AngularVelocityY { get; } // f32 AngularVelocityY;
        public float AngularVelocityZ { get; } // f32 AngularVelocityZ;

        public float Yaw { get; } // f32 Yaw;
        public float Pitch { get; } // f32 Pitch;
        public float Roll { get; } // f32 Roll;

        public float NormalizedSuspensionTravelFrontLeft { get; } // f32 NormalizedSuspensionTravelFrontLeft; // Suspension travel normalized: 0.0f = max stretch; 1.0 = max compression
        public float NormalizedSuspensionTravelFrontRight { get; } // f32 NormalizedSuspensionTravelFrontRight;
        public float NormalizedSuspensionTravelRearLeft { get; } // f32 NormalizedSuspensionTravelRearLeft;
        public float NormalizedSuspensionTravelRearRight { get; } // f32 NormalizedSuspensionTravelRearRight;

        public float TireSlipRatioFrontLeft { get; } // f32 TireSlipRatioFrontLeft; // Tire normalized slip ratio, = 0 means 100% grip and |ratio| > 1.0 means loss of grip.
        public float TireSlipRatioFrontRight { get; } // f32 TireSlipRatioFrontRight;
        public float TireSlipRatioRearLeft { get; } // f32 TireSlipRatioRearLeft;
        public float TireSlipRatioRearRight { get; } // f32 TireSlipRatioRearRight;

        public float WheelRotationSpeedFrontLeft { get; } // f32 WheelRotationSpeedFrontLeft; // Wheel rotation speed radians/sec.
        public float WheelRotationSpeedFrontRight { get; } // f32 WheelRotationSpeedFrontRight;
        public float WheelRotationSpeedRearLeft { get; } // f32 WheelRotationSpeedRearLeft;
        public float WheelRotationSpeedRearRight { get; } // f32 WheelRotationSpeedRearRight;

        public bool WheelOnRumbleStripFrontLeft { get; } // s32 WheelOnRumbleStripFrontLeft; // = 1 when wheel is on rumble strip, = 0 when off.
        public bool WheelOnRumbleStripFrontRight { get; } // s32 WheelOnRumbleStripFrontRight;
        public bool WheelOnRumbleStripRearLeft { get; } // s32 WheelOnRumbleStripRearLeft;
        public bool WheelOnRumbleStripRearRight { get; } // s32 WheelOnRumbleStripRearRight;

        public float WheelInPuddleDepthFrontLeft { get; } // f32 WheelInPuddleDepthFrontLeft; // = from 0 to 1, where 1 is the deepest puddle
        public float WheelInPuddleDepthFrontRight { get; } // f32 WheelInPuddleDepthFrontRight;
        public float WheelInPuddleDepthRearLeft { get; } // f32 WheelInPuddleDepthRearLeft;
        public float WheelInPuddleDepthRearRight { get; } // f32 WheelInPuddleDepthRearRight;

        public float SurfaceRumbleFrontLeft { get; } // f32 SurfaceRumbleFrontLeft; // Non-dimensional surface rumble values passed to controller force feedback
        public float SurfaceRumbleFrontRight { get; } // f32 SurfaceRumbleFrontRight;
        public float SurfaceRumbleRearLeft { get; } // f32 SurfaceRumbleRearLeft;
        public float SurfaceRumbleRearRight { get; } // f32 SurfaceRumbleRearRight;

        public float TireSlipAngleFrontLeft { get; } // f32 TireSlipAngleFrontLeft; // Tire normalized slip angle, = 0 means 100% grip and |angle| > 1.0 means loss of grip.
        public float TireSlipAngleFrontRight { get; } // f32 TireSlipAngleFrontRight;
        public float TireSlipAngleRearLeft { get; } // f32 TireSlipAngleRearLeft;
        public float TireSlipAngleRearRight { get; } // f32 TireSlipAngleRearRight;

        public float TireCombinedSlipFrontLeft { get; } // f32 TireCombinedSlipFrontLeft; // Tire normalized combined slip, = 0 means 100% grip and |slip| > 1.0 means loss of grip.
        public float TireCombinedSlipFrontRight { get; } // f32 TireCombinedSlipFrontRight;
        public float TireCombinedSlipRearLeft { get; } // f32 TireCombinedSlipRearLeft;
        public float TireCombinedSlipRearRight { get; } // f32 TireCombinedSlipRearRight;

        public float SuspensionTravelMetersFrontLeft { get; } // f32 SuspensionTravelMetersFrontLeft; // Actual suspension travel in meters
        public float SuspensionTravelMetersFrontRight { get; } // f32 SuspensionTravelMetersFrontRight;
        public float SuspensionTravelMetersRearLeft { get; } // f32 SuspensionTravelMetersRearLeft;
        public float SuspensionTravelMetersRearRight { get; } // f32 SuspensionTravelMetersRearRight;

        public int CarOrdinal { get; } // s32 CarOrdinal; //Unique ID of the car make/model
        public int CarClassRaw { get; } // s32 CarClass; //Between 0 (D – worst cars) and 7 (X class – best cars) inclusive
        public int CarPerformanceIndex { get; } // s32 CarPerformanceIndex; //Between 100 (slowest car) and 999 (fastest car) inclusive
        public int DrivetrainTypeRaw { get; } // s32 DrivetrainType; //Corresponds to EDrivetrainType; 0 = FWD, 1 = RWD, 2 = AWD
        public int NumCylinders { get; } // s32 NumCylinders; //Number of cylinders in the engine

        public int CarTypeRaw { get; } // s32 CarType; //Category in FH5
        public int Unknown1 { get; }
        public int Unknown2 { get; }

        public float PositionX { get; } // f32 PositionX; //Position (meters)
        public float PositionY { get; } // f32 PositionY;
        public float PositionZ { get; } // f32 PositionZ;

        public float Speed { get; } // f32 Speed; // meters per second
        public float Power { get; } // f32 Power; // watts
        public float Torque { get; } // f32 Torque; // newton meter

        public float TireTempFrontLeft { get; } // f32 TireTempFrontLeft;
        public float TireTempFrontRight { get; } // f32 TireTempFrontRight;
        public float TireTempRearLeft { get; } // f32 TireTempRearLeft;
        public float TireTempRearRight { get; } // f32 TireTempRearRight;

        public float Boost { get; } // f32 Boost;
        public float Fuel { get; } // f32 Fuel;
        public float DistanceTraveled { get; } // f32 DistanceTraveled;
        public float BestLap { get; } // f32 BestLap;
        public float LastLap { get; } // f32 LastLap;
        public float CurrentLap { get; } // f32 CurrentLap;
        public float CurrentRaceTime { get; } // f32 CurrentRaceTime;
        public uint LapNumber { get; } // u16 LapNumber;
        public byte RacePosition { get; } // u8 RacePosition;
        public byte Accel { get; } // u8 Accel;
        public byte Brake { get; } // u8 Brake;
        public byte Clutch { get; } // u8 Clutch;
        public byte HandBrake { get; } // u8 HandBrake;
        public byte Gear { get; } // u8 Gear;
        public sbyte Steer { get; } // s8 Steer;
        public sbyte NormalizedDrivingLine { get; } // s8 NormalizedDrivingLine;
        public sbyte NormalizedAIBrakeDifference { get; } // s8 NormalizedAIBrakeDifference;

        public string CarClass {
            get
            {
                switch (CarClassRaw)
                {
                    case 0:
                        return "D";
                    case 1:
                        return "C";
                    case 2:
                        return "B";
                    case 3:
                        return "A";
                    case 4:
                        return "S1";
                    case 5:
                        return "S2";
                    case 6:
                        return "X";
                }

                return "-";
            }
        }

        public string DrivetrainType
        {
            get
            {
                switch (DrivetrainTypeRaw)
                {
                    case 0:
                        return "FWD";
                    case 1:
                        return "RWD";
                    case 2:
                        return "AWD";
                    default:
                        return "-";
                }
            }
        }

        public string CarType {
            get
            {
                switch (CarTypeRaw)
                {
                    case 11:
                        return "Modern Super Cars";
                    case 12:
                        return "Retro Super Cars";
                    case 13:
                        return "Hyper Cars";
                    case 14:
                        return "Retro Saloons";
                    case 16:
                        return "Vans & Utility";
                    case 17:
                        return "Retro Sports Cars";
                    case 18:
                        return "Modern Sports Cars";
                    case 19:
                        return "Super Saloons";
                    case 20:
                        return "Classic Racers";
                    case 21:
                        return "Cult Cars";
                    case 22:
                        return "Rare Classics";
                    case 25:
                        return "Super Hot Hatch";
                    case 29:
                        return "Rods & Customs";
                    case 30:
                        return "Retro Muscle";
                    case 31:
                        return "Modern Muscle";
                    case 32:
                        return "Retro Rally";
                    case 33:
                        return "Classic Rally";
                    case 34:
                        return "Rally Monsters";
                    case 35:
                        return "Modern Rally";
                    case 36:
                        return "GT Cars";
                    case 37:
                        return "Super GT";
                    case 38:
                        return "Extreme Offroad";
                    case 39:
                        return "Sports Utility Heroes";
                    case 40:
                        return "Offroad";
                    case 41:
                        return "Offroad Buggies";
                    case 42:
                        return "Classic Sports Cars";
                    case 43:
                        return "Track Toys";
                    case 44:
                        return "Vintage Racers";
                    case 45:
                        return "Trucks";
                    default:
                        return $"Unknown ({CarTypeRaw})";
                }
            }
        }

        public string toJson()
        {
            return JsonSerializer.Serialize(this, new JsonSerializerOptions() { AllowTrailingCommas = false });
        }
    }
}