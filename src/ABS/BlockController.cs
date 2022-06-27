using System;
using System.Collections.Generic;
using Modding;
using Modding.Blocks;
using UnityEngine;
using ABSspace.BlockController.VanillaBlocks;
using ABSspace.BlockController.ModBlocks;

	/*
	modブロックを追加したいときの追記部分メモ
	・BlockSelector.ModBlockDict に ModID-LocalID と 挿入したいクラスを書き込む
	・ModBlockExchangerScript の先頭に Guid を追加する
	・ModBlockExchangerScript の「交換できるブロック群」リストに追加する
	・ModBlockExchangerScript.ModBlockId.SetGroup に else if 以下を追加する
	・ModBlockExchangerScript.ModBlockId.SetDictionary に辞書の宣言と初期化を追加する
	・ModBlocks 名前空間にクラスを追加する
	 */

namespace ABSspace
{
	namespace BlockController
	{
		public class BlockSelector : SingleInstance<BlockSelector>
		{
			public override string Name { get; }
			internal PlayerMachineInfo PMI;
			public Dictionary<int, Type> BlockDict = new Dictionary<int, Type>
			{
				//木製ブロック系
				{
					15,
					typeof(SingleWoodenBlockScript)
				},
				{
					1,
					typeof(WoodenBlockScript)
				},
				{
					63,
					typeof(LogBlockScript)
				},
				{
					41,
					typeof(WoodenPoleScript)
				},

				//ヒンジ系
				{
					19,
					typeof(SwivelScript)
				},
				{
					44,
					typeof(BallJointScript)
				},
				{
					5,
					typeof(HingeScript)
				},
				{
					28,
					typeof(SteeringHingeScript)
				},
				{
					12,
					typeof(ScalingBlockScript)
				},

				//ホイール系
				{
					2,
					typeof(MotorWheelScript)
				},
				{
					40,
					typeof(UnpoweredWheelScript)
				},
				{
					38,
					typeof(UnpoweredMediumCogScript)
				},
				{
					39,
					typeof(PoweredMediumCogScript)
				},
				{
					17,
					typeof(CircularSawScript)
				},
				{
					46,
					typeof(LargeWheelScript)
				},
				{
					60,
					typeof(UnpoweredLargeWheelScript)
				},
				{
					51,
					typeof(UnpoweredLargeCogScript)
				},

				//プロペラ系
				{
					26,
					typeof(PropellerScript)
				},
				{
					55,
					typeof(SmallPropellerScript)
				},
				{
					52,
					typeof(Propeller52Script)
				},
				{
					3,
					typeof(MetalBladeScript)
				},
				{
					20,
					typeof(SpikeScript)
				},

				//装甲系
				{
					10,
					typeof(WoodenPanelScript)
				},
				{
					32,
					typeof(ArmorPlateLargeScript)
				},
				{
					24,
					typeof(ArmorPlateSmallScript)
				},
				{
					29,
					typeof(ArmorPlateRoundScript)
				},

				//武装系
				{
					11,
					typeof(CannonScript)
				},
				{
					53,
					typeof(ShrapnelCannonScript)
				},
				{
					61,
					typeof(CrossbowScript)
				},
				{
					62,
					typeof(VacuumScript)
				},
				{
					56,
					typeof(WaterCannonScript)
				},

				//球体系
				{
					23,
					typeof(BombScript)
				},
				{
					31,
					typeof(FlameBallScript)
				},
				{
					36,
					typeof(BoulderScript)
				},

                // ロジック系
                {
					(int)BlockType.Sensor, typeof(SensorScript)
                },
				{
					(int)BlockType.Timer, typeof(TimerScript)
				},
                {
					(int)BlockType.Altimeter, typeof(AltimeterScript)
                },
                {
					(int)BlockType.LogicGate, typeof(LogicGateScript)
                },
                {
					(int)BlockType.Anglometer, typeof(AnglometerScript)
                },
				{
					(int)BlockType.Speedometer, typeof(SpeedometerScript)
				},

                // 風船系
                {
					(int)BlockType.Balloon, typeof(BalloonScript)
                },
                {
					(int)BlockType.SqrBalloon, typeof(SqrBalloonScript)
                },
			//武装なんかも追加したい（ACM製ブロックを含む）
			};
			public Dictionary<string, Type> ModBlockDict = new Dictionary<string, Type>
			{
				#region オトカム砲
				{
                    "1f5c7130-1eaa-47bc-ad1f-66586f68c9d6-1",
					typeof(SimpleMachinegunScript)
				},
				{
					"1f5c7130-1eaa-47bc-ad1f-66586f68c9d6-2",
					typeof(SimpleMachinegunEXScript)
				},
				{
					"1f5c7130-1eaa-47bc-ad1f-66586f68c9d6-3",
					typeof(SimpleMachinegunAPScript)
				},
				{
					"1f5c7130-1eaa-47bc-ad1f-66586f68c9d6-4",
					typeof(AutoCannonScript)
				},
				{
					"1f5c7130-1eaa-47bc-ad1f-66586f68c9d6-5",
					typeof(FlakCannonScript)
				},
				#endregion

				#region スケピン砲(ver1)
				{
                    "72a20968-e89a-486a-a797-460e65a7df9e-1",
					typeof(BBv1_001hScript)
				},
				{
					"72a20968-e89a-486a-a797-460e65a7df9e-3",
					typeof(BBv1_001vScript)
				},
				{
					"72a20968-e89a-486a-a797-460e65a7df9e-2",
					typeof(BBv1_002hScript)
				},
				{
					"72a20968-e89a-486a-a797-460e65a7df9e-4",
					typeof(BBv1_002vScript)
				},
				#endregion

				#region スケピン砲(ver2)
				{
                    "0e746ba8-7cbe-4afd-95b6-a9f67e814f7e-10",
					typeof(BBv2_001hScript)
				},
				{
					"0e746ba8-7cbe-4afd-95b6-a9f67e814f7e-11",
					typeof(BBv2_001vScript)
				},
				{
					"0e746ba8-7cbe-4afd-95b6-a9f67e814f7e-15",
					typeof(BBv2_002hScript)
				},
				{
					"0e746ba8-7cbe-4afd-95b6-a9f67e814f7e-16",
					typeof(BBv2_002vScript)
				},
				#endregion

				#region 無誘導ミサイル
				{
                    "d67339bc-a869-4f8d-8943-166a523d86f5-10",
					typeof(Missile001Script)
				},
				#endregion

				#region 誘導ミサイル
				{
                    "90a17943-af2a-40ea-a51f-530553b9fcb0-10",
					typeof(ITANOScript)
				},
				{
					"90a17943-af2a-40ea-a51f-530553b9fcb0-11",
					typeof(HexapodScript)
				},
				{
					"90a17943-af2a-40ea-a51f-530553b9fcb0-12",
					typeof(TwinpodScript)
				},
				{
					"90a17943-af2a-40ea-a51f-530553b9fcb0-13",
					typeof(RapierScript)
				},
				{
					"90a17943-af2a-40ea-a51f-530553b9fcb0-20",
					typeof(ChaffLauncherScript)
				},
				#endregion

				#region 船
				{
                    "0c99ef6c-f627-4b4f-a5c8-ccdddc08b8f5-1",
					typeof(ScrewScript)
				},
				{
					"0c99ef6c-f627-4b4f-a5c8-ccdddc08b8f5-2",
					typeof(BouyScript)
				},
				{
					"0c99ef6c-f627-4b4f-a5c8-ccdddc08b8f5-3",
					typeof(Beam7Script)
				},
				{
					"0c99ef6c-f627-4b4f-a5c8-ccdddc08b8f5-4",
					typeof(Beam5Script)
				},
				{
					"0c99ef6c-f627-4b4f-a5c8-ccdddc08b8f5-5",
					typeof(Plate5x5Script)
				},
				{
					"0c99ef6c-f627-4b4f-a5c8-ccdddc08b8f5-6",
					typeof(Plate3x5Script)
				},
				{
					"0c99ef6c-f627-4b4f-a5c8-ccdddc08b8f5-7",
					typeof(Plate3x3Script)
				},
				#endregion

				#region 板
				{
                    "7b20ba2d-d1d5-4e1c-89ee-f585d1acbcd0-1",
					typeof(PlateScript)
				},
				{
					"7b20ba2d-d1d5-4e1c-89ee-f585d1acbcd0-2",
					typeof(HalfPlateScript)
				},
				{
					"7b20ba2d-d1d5-4e1c-89ee-f585d1acbcd0-3",
					typeof(BeamScript)
				},
				{
					"7b20ba2d-d1d5-4e1c-89ee-f585d1acbcd0-4",
					typeof(ThinPlateScript)
				},
				{
					"7b20ba2d-d1d5-4e1c-89ee-f585d1acbcd0-5",
					typeof(HalfPlateScript)
				},
				{
					"7b20ba2d-d1d5-4e1c-89ee-f585d1acbcd0-6",
					typeof(DoubleBeamScript)
				},
				{
					"7b20ba2d-d1d5-4e1c-89ee-f585d1acbcd0-7",
					typeof(ThinHalfPlateScript)
				},
				{
					"7b20ba2d-d1d5-4e1c-89ee-f585d1acbcd0-8",
					typeof(SmallPlateScript)
				},
				{
					"7b20ba2d-d1d5-4e1c-89ee-f585d1acbcd0-9",
					typeof(CoreCubeScript)
				},
				#endregion

				#region CU
				{
					"f47836d2-4356-4ef5-9e69-cc1bcda5e0fc-1",
					typeof(ThrusterScript)
				},
				#endregion

				#region レーザー
				{
                    "15259577-bd19-4397-8646-88d235cf8d24-10",
					typeof(Laser001Script)
				},
				{
					"15259577-bd19-4397-8646-88d235cf8d24-11",
					typeof(Laser002Script)
				},
				{
					"15259577-bd19-4397-8646-88d235cf8d24-15",
					typeof(Laser001vScript)
				},
				{
					"15259577-bd19-4397-8646-88d235cf8d24-16",
					typeof(Laser002vScript)
				},
				{
					"15259577-bd19-4397-8646-88d235cf8d24-20",
					typeof(Distoray3Script)
				},
				{
					"15259577-bd19-4397-8646-88d235cf8d24-25",
					typeof(Distoray3vScript)
				},
				{
					"15259577-bd19-4397-8646-88d235cf8d24-40",
					typeof(LightChaffScript)
				},
				#endregion

				#region E砲
				{
                    "50e63b55-b976-4009-82ab-66f989218122-1",
					typeof(L44APFSDS_120mmScript)
				},
				{
					"50e63b55-b976-4009-82ab-66f989218122-3",
					typeof(AntennaScript)
				},
				{
					"50e63b55-b976-4009-82ab-66f989218122-4",
					typeof(L44HEATFS_120mmScript)
				},
				{
					"50e63b55-b976-4009-82ab-66f989218122-5",
					typeof(L74AntiAir_88mmScript)
				},
				{
					"50e63b55-b976-4009-82ab-66f989218122-6",
					typeof(L74APCR_88mmScript)
				},
				{
					"50e63b55-b976-4009-82ab-66f989218122-7",
					typeof(L46HE_45mmScript)
				},
				{
					"50e63b55-b976-4009-82ab-66f989218122-8",
					typeof(L46AP_45mmScript)
				},
				{
					"50e63b55-b976-4009-82ab-66f989218122-9",
					typeof(L23HE_152mmScript)
				},
				{
					"50e63b55-b976-4009-82ab-66f989218122-10",
					typeof(MedievalCannonScript)
				},
				{
					"50e63b55-b976-4009-82ab-66f989218122-11",
					typeof(L65HE_20mmScript)
				},
				{
					"50e63b55-b976-4009-82ab-66f989218122-12",
					typeof(L48HE_75mmScript)
				},
				{
					"50e63b55-b976-4009-82ab-66f989218122-13",
					typeof(L48AP_75mmScript)
				},
				{
					"50e63b55-b976-4009-82ab-66f989218122-14",
					typeof(RedBigFlagScript)
				},
				{
					"50e63b55-b976-4009-82ab-66f989218122-15",
					typeof(BlueBigFlagScript)
				},
				{
					"50e63b55-b976-4009-82ab-66f989218122-16",
					typeof(RedSmallFlagScript)
				},
				{
					"50e63b55-b976-4009-82ab-66f989218122-17",
					typeof(BlueSmallFlagScript)
				},
				{
					"50e63b55-b976-4009-82ab-66f989218122-18",
					typeof(L24HEAT_75mmScript)
				},
				{
					"50e63b55-b976-4009-82ab-66f989218122-19",
					typeof(MachineGunHE_127mmScript)
				},
				{
					"50e63b55-b976-4009-82ab-66f989218122-20",
					typeof(MachineGunT_127mmScript)
				},
				#endregion

				#region アナログコントロール
				{
                    "6a56a26c-5f7d-4dc5-b73d-7161060a8656-10",
					typeof(AnalogHingeScript)
				},
                {
					"6a56a26c-5f7d-4dc5-b73d-7161060a8656-20",
					typeof(Motor01Script)
				},
                {
					"6a56a26c-5f7d-4dc5-b73d-7161060a8656-40",
					typeof(AnalogWheel_BaseScript)
				},
                {
					"6a56a26c-5f7d-4dc5-b73d-7161060a8656-140",
					typeof(LargeWheel_Tire01Script)
				},
                {
					"6a56a26c-5f7d-4dc5-b73d-7161060a8656-141",
					typeof(LargeWheel_Tire02Script)
				},
                {
					"6a56a26c-5f7d-4dc5-b73d-7161060a8656-142",
					typeof(LargeWheel_Tire03Script)
				},
				#endregion

				#region 構造材
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-1",
					typeof(NoriRodL3Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-2",
					typeof(NoriRodL5Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-3",
					typeof(NoriRodL8Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-4",
					typeof(NoriRodL12Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-5",
					typeof(NoriRodL17Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-6",
					typeof(NoriRodL23Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-7",
					typeof(NoriRodL30Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-11",
					typeof(NoriBoardS3Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-12",
					typeof(NoriBoardS5Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-13",
					typeof(NoriBoardS8Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-14",
					typeof(NoriBoardS12Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-15",
					typeof(NoriBoardS17Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-16",
					typeof(NoriBoardS23Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-17",
					typeof(NoriBoardS30Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-21",
					typeof(NoriBoardO3x5Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-22",
					typeof(NoriBoardO3x8Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-23",
					typeof(NoriBoardO5x8Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-24",
					typeof(NoriBoardO3x12Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-25",
					typeof(NoriBoardO3x17Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-26",
					typeof(NoriBoardO3x23Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-27",
					typeof(NoriBoardO3x5Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-32",
					typeof(NoriBoardO5x12Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-33",
					typeof(NoriBoardO5x17Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-34",
					typeof(NoriBoardO5x23Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-35",
					typeof(NoriBoardO5x30Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-41",
					typeof(NoriBoardO8x12Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-42",
					typeof(NoriBoardO8x17Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-43",
					typeof(NoriBoardO8x23Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-44",
					typeof(NoriBoardO8x30Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-51",
					typeof(NoriBoardO12x17Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-52",
					typeof(NoriBoardO12x23Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-53",
					typeof(NoriBoardO12x30Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-61",
					typeof(NoriBoardO17x23Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-62",
					typeof(NoriBoardO17x30Script)
				},
				{
					"68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d-71",
					typeof(NoriBoardO23x30Script)
				},
                #endregion

                #region ブースター
                {
					"d1dffba4-cfa1-49c3-95a3-0a3094bbc517-10",
					typeof(Booster01Script)
				},
                {
					"d1dffba4-cfa1-49c3-95a3-0a3094bbc517-11",
					typeof(Booster02Script)
				},
                {
					"d1dffba4-cfa1-49c3-95a3-0a3094bbc517-12",
					typeof(Booster03Script)
				},
                {
					"d1dffba4-cfa1-49c3-95a3-0a3094bbc517-13",
					typeof(Booster04Script)
				},
                {
					"d1dffba4-cfa1-49c3-95a3-0a3094bbc517-20",
					typeof(Intake01Script)
				},
                {
					"d1dffba4-cfa1-49c3-95a3-0a3094bbc517-30",
					typeof(Tank01Script)
				},
                {
					"d1dffba4-cfa1-49c3-95a3-0a3094bbc517-31",
					typeof(Tank02Script)
				},
				#endregion
			};

			public void Awake()
			{
				//ModConsole.Log("BlockSelector is awaken.");
				Events.OnBlockInit += new Action<Block>(AddScript);
			}
			public void AddScript(Block block)
			{
				BlockBehaviour internalObject = block.BuildingBlock.InternalObject;

				if (BlockDict.ContainsKey(internalObject.BlockID))
				{
					Type type = BlockDict[internalObject.BlockID];
					try
					{
						if (internalObject.GetComponent(type) == null)
						{
							internalObject.gameObject.AddComponent(type);
						}
					}
					catch
					{
						ModConsole.Log("ABS : AddScript Error.");
					}
					return;
				}
				if (ModBlockDict.ContainsKey(internalObject.name.ToString()))
				{
					Type type = ModBlockDict[internalObject.name.ToString()];
					try
					{
						if (internalObject.GetComponent(type) == null)
						{
							internalObject.gameObject.AddComponent(type);
						}
					}
					catch
					{
						ModConsole.Log("ABS : AddSctipt to mod block Error.");
					}
					return;
				}
				//特定のブロック出なかった場合
				if (internalObject.GetComponent(typeof(OtherBlockScript)) == null)
				{
					internalObject.gameObject.AddComponent<OtherBlockScript>();
				}
			}
		}
		public abstract class AbstractBlockScript : MonoBehaviour //ブロック基本
		{
			[Obsolete]
			public Action<XDataHolder> BlockDataLoadEvent;
			[Obsolete]
			public Action<XDataHolder> BlockDataSaveEvent;
			public Action BlockPropertiseChangedEvent;
			public bool isFirstFrame;
			public BlockBehaviour BB { internal set; get; }
			public bool CombatUtilities { set; get; }

			private void Awake()
			{
				BB = GetComponent<BlockBehaviour>();
				SafeAwake();
				ChangedProperties();
				try
				{
					BlockPropertiseChangedEvent();
				}
				catch
				{

				}
				DisplayInMapper(CombatUtilities);
			}
			private void Update()
			{
				if (BB.isSimulating)
				{
					if (isFirstFrame)
					{
						isFirstFrame = false;
						if (CombatUtilities)
						{
							OnSimulateStart();
						}
						if (!StatMaster.isClient)
						{
							ChangeParameter();
						}
					}
					if (CombatUtilities)
					{
						if (StatMaster.isHosting)
						{
							SimulateUpdateHost();
						}
						if (StatMaster.isClient)
						{
							SimulateUpdateClient();
						}
						SimulateUpdateAlways();
					}
				}
				else
				{
					if (CombatUtilities)
					{
						BuildingUpdate();
					}
					isFirstFrame = true;
				}
			}
			private void FixedUpdate()
			{
				if (CombatUtilities && BB.isSimulating && !isFirstFrame)
				{
					SimulateFixedUpdateAlways();
				}
			}
			private void LastUpdate()
			{
				if (CombatUtilities && BB.isSimulating && !isFirstFrame)
				{
					SimulateLateUpdateAlways();
				}
			}

			[Obsolete]
			private void SaveConfiguration(PlayerMachineInfo pmi)
			{
				ConsoleController.ShowMessage("On save en");
				if (pmi != null)
				{
					foreach (Modding.Blocks.BlockInfo current in pmi.Blocks)
					{
						if (current.Guid == BB.Guid)
						{
							XDataHolder data = current.Data;
							try
							{
								BlockDataSaveEvent(data);
							}
							catch
							{

							}
							this.SaveConfiguration(data);
							break;
						}
					}
				}
			}
			[Obsolete]
			private void LoadConfiguration()
			{
				ConsoleController.ShowMessage("On load en");
				if (SingleInstance<BlockSelector>.Instance.PMI != null)
				{
					foreach (Modding.Blocks.BlockInfo current in SingleInstance<BlockSelector>.Instance.PMI.Blocks)
					{
						if (current.Guid == BB.Guid)
						{
							XDataHolder data = current.Data;
							try
							{
								BlockDataLoadEvent(data);
							}
							catch { }
							LoadConfiguration(data);
							break;
						}
					}
				}
			}
			[Obsolete]
			public virtual void SaveConfiguration(XDataHolder BlockData) { }
			[Obsolete]
			public virtual void LoadConfiguration(XDataHolder BlockData) { }
			public virtual void SafeAwake() { }
			public virtual void OnSimulateStart() { }
			public virtual void SimulateUpdateHost() { }
			public virtual void SimulateUpdateClient() { }
			public virtual void SimulateUpdateAlways() { }
			public virtual void SimulateFixedUpdateAlways() { }
			public virtual void SimulateLateUpdateAlways() { }
			public virtual void BuildingUpdate() { }
			public virtual void DisplayInMapper(bool value) { }
			public virtual void ChangedProperties() { }
			public virtual void ChangeParameter() { }
			public static void SwitchMatalHardness(int Hardness, ConfigurableJoint CJ)
			{
				if (Hardness != 1)
				{
					if (Hardness != 2)
					{
						CJ.projectionMode = JointProjectionMode.None;
					}
					else
					{
						CJ.projectionMode = JointProjectionMode.PositionAndRotation;
						CJ.projectionAngle = 0f;
					}
				}
				else
				{
					CJ.projectionMode = JointProjectionMode.PositionAndRotation;
					CJ.projectionAngle = 0.5f;
				}
			}
			public static void SwitchWoodHardness(int Hardness, ConfigurableJoint CJ)
			{
				switch (Hardness)
				{
					case 0:
						CJ.projectionMode = JointProjectionMode.PositionAndRotation;
						CJ.projectionAngle = 10f;
						CJ.projectionDistance = 5f;
						return;
					case 2:
						CJ.projectionMode = JointProjectionMode.PositionAndRotation;
						CJ.projectionAngle = 5f;
						CJ.projectionDistance = 2.5f;
						return;
					case 3:
						CJ.projectionMode = JointProjectionMode.PositionAndRotation;
						CJ.projectionAngle = 0f;
						CJ.projectionDistance = 0f;
						return;
					default:
						CJ.projectionMode = JointProjectionMode.None;
						CJ.projectionDistance = 0f;
						CJ.projectionAngle = 0f;
						return;
				}
			}
			public void OnMouseOver()
			{
				if (Input.GetKeyDown(KeyCode.G) && !Game.IsSimulating && !StatMaster.isMainMenu)
				{
					ModifyBlock01();
				}
				if (Input.GetKeyDown(KeyCode.T) && !Game.IsSimulating && !StatMaster.isMainMenu)
				{
					ModifyBlock02();
				}
				if (Input.GetKeyDown(KeyCode.H) && !Game.IsSimulating && !StatMaster.isMainMenu)
				{
					ModifyBlock03();
				}
				if (Input.GetKeyDown(KeyCode.Y) && !Game.IsSimulating && !StatMaster.isMainMenu)
				{
					ModifyBlock04();
				}
				CoLController.CoLGUI.picked = BB; // トランスフォーム表示用
			}

			//ショートカット系
			public virtual void ModifyBlock01() //Gキーが押されたときに呼び出し
			{
				//ModConsole.Log("ABC.ModifyBlock01 : called  Selected block is " + BB.name);
			}
			public virtual void ModifyBlock02() //Tキーが押されたときに呼び出し
			{
				//ModConsole.Log("ABS.ModifyBlock02 : called  Selected block is " + BB.name);
			}
			public virtual void ModifyBlock03() { }
			public virtual void ModifyBlock04() { }
			public AbstractBlockScript()
			{
				CombatUtilities = true;
				isFirstFrame = true;
			}

		}
		public abstract class BlockExchangerScript : AbstractBlockScript //プロペラ等に追加するコンポーネントの元
		{
			public bool isJapanese = Mod.isJapanese;

			//ブロックデータ
			public Block block; //既存のブロック //BlockBehaviourはBBで取得できる
			public Block newBlock;
			public BlockBehaviour newBB;
			public PlayerMachine PM;
			public Machine machine;
			public Transform InitTransform;

			//新ブロック交換系
			public MMenu ExchangeMenu;
			public MToggle ExchangeToggle;
			public List<BlockType> ExchangeList;
			public List<string> ExchangeListMenu;

			public string displayName //"ブロック交換"
			{
				get
				{
					if (isModBlock)
					{
						return isJapanese ? "バニラブロックへ変換" : "Change To Vanilla Block";
					}
					return isJapanese ? "ブロック交換" : "Exchange";
				}
			}

			public bool isModBlock = false;

			//ペラ・ウッドパネル交換系
			public bool isWoodenPanel = false;
			public bool isPropeller = false;
			public MToggle PPToggle;
			public string PPToggleLabel
			{
				get
				{
					if (isPropeller)
					{
						return isJapanese ? "ウッドパネルに変更" : "Exchange to Wooden Panel";
					}
					if (isWoodenPanel)
					{
						return isJapanese ? "短プロペラに変更" : "Exchange to Propeller";
					}
					return "";
				}
			}

			// ブロックのグループ化系
			public MMenu GroupMenu;
			public int groupLabel
            {
                get
                {
					return GroupMenu.Value;
                }
            }

			public override void SafeAwake()
			{
				//情報の初期化
				ExchangeList = new List<BlockType>();
				ExchangeListMenu = new List<string>();
				block = Block.From(BB);
				PM = block.Machine;
				//machine = PM.InternalObject;
				machine = BB.ParentMachine;
				InitTransform = BB.transform;
				Mod.CheckExtended(); //再チェック

				//modブロック交換系
				ModBlockSafeAwake();

				//ブロック交換系
				SetBlockList();
				if (ExchangeList.Count > 0)
                {
					ExchangeMenu = BB.AddMenu("ExchangeMenu", 0, ExchangeListMenu, false);
					ExchangeToggle = BB.AddToggle(displayName, "Exchange", false);
				}

				//プロペラ・パネル交換系
				if (isPropeller || isWoodenPanel)
				{
					PPToggle = BB.AddToggle(PPToggleLabel, PPToggleLabel, false);
				}

				CanShortenBlockSafeAwake();
				PropellerSafeAwake();

				//ブロックのグループ化系
				GroupMenu = BB.AddMenu("Group", 0, Mod.blockGroup.labels, true);
			}

			public override void BuildingUpdate()
			{
				if (ExchangeList.Count > 0)
				{
					if (ExchangeToggle.IsActive)
					{
						ExchangeToggle.IsActive = false;
						machine.UndoSystem.Undo();
						ChangeBlock(ExchangeList[ExchangeMenu.Value]);
					}
				}
				ModBlockBuildingUpdate();
				
				if (isPropeller || isWoodenPanel) //短ペラ・ウッドパネル交換系
				{
					if (PPToggle.IsActive)
					{
						//プロペラとウッドパネルを交換する
						PPToggle.IsActive = false;
						//machine.UndoSystem.Undo();
						Vector3 lastPosition = BB.transform.position;
						Quaternion lastRotation = BB.transform.rotation;
						Quaternion nextRotation = BB.transform.localRotation * Quaternion.AngleAxis(90 * (isPropeller ? 1 : -1), Vector3.left);
						if (machine.AddBlock(new BlockInfo
						{
							ID = isPropeller ? BlockType.WoodenPanel : BlockType.SmallPropeller,
							Position = BB.transform.localPosition,
							Rotation = nextRotation,
							Scale = BB.transform.localScale,
							Flipped = BB.Flipped,
							Skin = BB.VisualController.selectedSkin,
							BlockData = new XDataHolder()
						}, out newBB)) //条件文でブロックを追加
						{
							machine.RemoveBlock(BB);
							newBB.VisualController.PlaceFromBlock(newBB);
							//machine.UndoSystem.AddAction(new UndoActionExchange(machine, BlockInfo.FromBlockBehaviour(BB), BlockInfo.FromBlockBehaviour(newBB)));
							//これはうまくいくので、たぶんUndoActionExchangeが壊れている
							machine.UndoSystem.AddActions(
								new List<UndoAction> 
								{
									new UndoActionAdd(machine, BlockInfo.FromBlockBehaviour(newBB)),
									new UndoActionRemove(machine, BlockInfo.FromBlockBehaviour(BB))
								}
								);
						}
					}
				}
				CanShortenBlockBuildingUpdate();
				PropellerBuildingUpdate();
			}
			public override void SimulateFixedUpdateAlways()
			{
				PropellerSimulationUpdate();
			}

			//ブロック交換系
			public abstract void SetBlockList(); //ExchangeListとExchangeListMenu
			public void ChangeBlock(BlockType BlockID) //指定したブロックと交換する
			{
				if (machine.AddBlock(new BlockInfo
					{
						ID = BlockID,
						Position = BB.transform.localPosition,
						Rotation = BB.transform.localRotation,
						Scale = BB.transform.localScale,
						Flipped = BB.Flipped,
						Skin = BB.VisualController.selectedSkin,
						BlockData = new XDataHolder()
					}, out newBB))
				{
					newBB.VisualController.PlaceFromBlock(newBB);
					//Mod.Log(string.Format("From {0}, {1}", BB.VisualController.selectedSkin.prefab.name, BB.VisualController.selectedSkin.pack.name));
					//Mod.Log(string.Format("To {0}, {1}", newBB.VisualController.selectedSkin.prefab.name, newBB.VisualController.selectedSkin.pack.name));
					//newBB.VisualController.selectedSkin = FindSkin(BB.VisualController.selectedSkin.pack, newBB);
					machine.RemoveBlock(BB); //ブロック長さ変更の時にぬるぽ
				}
				machine.UndoSystem.AddActions(
					new List<UndoAction>
						{
							new UndoActionAdd(machine, BlockInfo.FromBlockBehaviour(newBB)),
							new UndoActionRemove(machine, BlockInfo.FromBlockBehaviour(BB))
						}
					);
			}
			public class UndoActionExchange : UndoAction //AddとRemoveを組み合わせた形 //スケールやスキンも引き継ぐ //ステータスをコピペする
			{
				BlockInfo newInfo;
				Guid newGuid;

				public UndoActionExchange(Machine m, BlockInfo preInfo, BlockInfo newInfo)
				{
					Debug.Log("ABS : UndoActionExchange : called");
					info = preInfo;
					this.newInfo = newInfo;
					guid = preInfo.Guid;
					newGuid = newInfo.Guid;
					changesCount = true;
					machine = m;
				}
				public override bool Redo()
				{
					//ModConsole.Log("Redo");
					BlockBehaviour block;
					BlockBehaviour blockBehaviour;
					if (machine.AddBlock(newInfo, out blockBehaviour))
					{
						blockBehaviour.VisualController.PlaceFromBlockInfo(newInfo);
					}
					if (machine.GetBlock(guid, out block))
					{
						/*
						BlockMapper.Open(block).Copy();
						BlockMapper.Close();
						BlockMapper.Open(blockBehaviour).Paste();
						BlockMapper.Close();
						*/
						machine.RemoveBlock(block);
					}
					return true;
				}
				public override bool Undo()
				{
					//ModConsole.Log("Undo");
					BlockBehaviour blockBehaviour;
					BlockBehaviour block;
					if (machine.AddBlock(info, out blockBehaviour))
					{
						blockBehaviour.VisualController.PlaceFromBlockInfo(info);
					}
					if (machine.GetBlock(newGuid, out block))
					{
						//いったんコピペをやめる //ここが間違い!?
						/*
						BlockMapper.Open(block).Copy();
						BlockMapper.Close();
						BlockMapper.Open(blockBehaviour).Paste();
						BlockMapper.Close();
						*/
						machine.RemoveBlock(block);
					}
					return true;
				}
			}
			public BlockSkinLoader.SkinPack.Skin FindSkin(BlockSkinLoader.SkinPack pack, BlockBehaviour bb) // packの中のbbに対応するskinを返す
            {
				foreach (BlockSkinLoader.SkinPack.Skin skin in pack.skins)
                {
					if (skin.prefab == bb.Prefab)
                    {
						return skin;
                    }
                }
				return null;
            }

			//modブロック交換系
			public virtual void ModBlockSafeAwake() { }
			public virtual void ModBlockBuildingUpdate() { }

			//ブロック短縮系
			public virtual void CanShortenBlockSafeAwake() { }
			public virtual void CanShortenBlockBuildingUpdate() { }

			//プロペラの空力をベクトル表示
			public virtual void PropellerSafeAwake() { }
			public virtual void PropellerBuildingUpdate() { }
			public virtual void PropellerSimulationUpdate() { }
		}
		
		namespace VanillaBlocks
		{
			public abstract class AbstractShortenableBlockScript : BlockExchangerScript
			{
				//ブロック短縮化系
				public bool isShort;
				public ShorteningBlock ShortenBB;
				public MToggle SToggle;
				public string SToggleLabel
				{
					get
					{
						return isJapanese ? "長さ変更（G）" : "Change Length (G)";
					}
				}

				public override void SetBlockList()
				{
					
				}
				public override void CanShortenBlockSafeAwake()
				{
					//ブロック短縮化系
					
					SToggle = BB.AddToggle(SToggleLabel, SToggleLabel, false);
					
				}
				public override void CanShortenBlockBuildingUpdate()
				{
					if (SToggle.IsActive)
					{
						SToggle.IsActive = false;
						machine.UndoSystem.Undo();
						Shorten();
					}
				}
				public void Shorten()
				{
					if (!BB.gameObject.transform.FindChild("Joint").gameObject.activeSelf) //短い時
					{
						if (machine.AddBlock(new BlockInfo
						{
							ID = BB.Prefab.Type,
							Position = BB.transform.localPosition,
							Rotation = BB.transform.localRotation,
							Scale = BB.transform.localScale,
							Flipped = BB.Flipped,
							Skin = BB.VisualController.selectedSkin,
							BlockData = new XDataHolder()
						}, out newBB))
						{
							ShortenBB = (ShorteningBlock)newBB;
							newBB.VisualController.PlaceFromBlock(ShortenBB);
							machine.RemoveBlock(BB);
						}
						machine.UndoSystem.AddAction(new UndoActionChangeLength(machine, BlockInfo.FromBlockBehaviour(BB), BlockInfo.FromBlockBehaviour(ShortenBB)));
					}
					else
					{
						if (machine.AddBlock(new BlockInfo
						{
							ID = BB.Prefab.Type,
							Position = BB.transform.localPosition,
							Rotation = BB.transform.localRotation,
							Scale = BB.transform.localScale,
							Flipped = BB.Flipped,
							Skin = BB.VisualController.selectedSkin,
							BlockData = new XDataHolder()
						}, out newBB))
						{
							ShortenBB = (ShorteningBlock)newBB;
							newBB.VisualController.PlaceFromBlock(ShortenBB);
							ShortenBB.UpdateLength(ShortenBB.startingLength - 1, true);
							machine.RemoveBlock(BB);
						}
						machine.UndoSystem.AddAction(new UndoActionChangeLength(machine, BlockInfo.FromBlockBehaviour(BB), BlockInfo.FromBlockBehaviour(ShortenBB)));
					}
				}
				public class UndoActionChangeLength : UndoAction //AddとRemoveを組み合わせた形 //スケールやスキンも引き継ぐ //ステータスをコピペする
				{
					BlockInfo newInfo;
					Guid newGuid;

					public UndoActionChangeLength(Machine m, BlockInfo preInfo, BlockInfo newInfo)
					{
						info = preInfo;
						this.newInfo = newInfo;
						guid = info.Guid;
						newGuid = this.newInfo.Guid;
						changesCount = true;
						machine = m;
					}
					public override bool Redo()
					{
						//ModConsole.Log("Redo");
						BlockBehaviour block;
						BlockBehaviour blockBehaviour;
						if (machine.AddBlock(newInfo, out blockBehaviour))
						{
							blockBehaviour.VisualController.PlaceFromBlockInfo(newInfo);

						}
						if (machine.GetBlock(guid, out block))
						{
							machine.RemoveBlock(block);
						}
						return true;
					}
					public override bool Undo()
					{
						//ModConsole.Log("Undo");
						BlockBehaviour blockBehaviour;
						BlockBehaviour block;
						if (machine.AddBlock(info, out blockBehaviour))
						{
							blockBehaviour.VisualController.PlaceFromBlockInfo(info);

						}
						if (machine.GetBlock(newGuid, out block))
						{
							machine.RemoveBlock(block);
						}
						return true;
					}
				}
				public override void ModifyBlock01()
				{
					//machine.UndoSystem.Undo();
					Shorten();
				}
			}
			public class SingleWoodenBlockScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.DoubleWoodenBlock,
						BlockType.Log,
						BlockType.WoodenPole,
					};
					ExchangeListMenu = new List<string>
					{
						"Double Wooden Block",
						"Log",
						"Wooden Pole",
					};
				}
			}
			public class WoodenBlockScript : AbstractShortenableBlockScript
			{
				public WoodenBlockScript()
				{
					isShort = false;
				}
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.Log,
						BlockType.WoodenPole,
						BlockType.SingleWoodenBlock,
					};

					ExchangeListMenu = new List<string>
					{
						"Log",
						"Wooden Pole",
						"Single Wooden Block",
					};
				}
			}
			public class LogBlockScript : AbstractShortenableBlockScript
			{
				public LogBlockScript()
				{
					isShort = false;
				}
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.WoodenPole,
						BlockType.SingleWoodenBlock,
						BlockType.DoubleWoodenBlock,
					};
					ExchangeListMenu = new List<string>
					{
						"Wooden Pole",
						"Single Wooden Block",
						"Double Wooden Block",
					};
				}
			}
			public class WoodenPoleScript : AbstractShortenableBlockScript
			{
				public WoodenPoleScript()
				{
					isShort = false;
				}
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.SingleWoodenBlock,
						BlockType.DoubleWoodenBlock,
						BlockType.Log,
					};
					ExchangeListMenu = new List<string>
					{
						"Single Wooden Block",
						"Double Wooden Block",
						"Log",
					};
				}
			}
			public class SwivelScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					if (Mod.Extended)
					{
						ExchangeList = new List<BlockType>
						{
							BlockType.BallJoint,
							BlockType.Hinge,
							BlockType.SteeringHinge,
							BlockType.ScalingBlock,
						};
						ExchangeListMenu = new List<string>
						{
							"Ball Joint",
							"Hinge",
							"Steering Hinge",
							"Scaling Block",
						};
					}
					else
					{
						ExchangeList = new List<BlockType>
						{
							BlockType.BallJoint,
							BlockType.Hinge,
							BlockType.SteeringHinge,
						};
						ExchangeListMenu = new List<string>
						{
							"Ball Joint",
							"Hinge",
							"Steering Hinge",
						};
					}
				}
			}
			public class BallJointScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					
					if (Mod.Extended)
					{
						ExchangeList = new List<BlockType>
						{
							BlockType.Hinge,
							BlockType.SteeringHinge,
							BlockType.ScalingBlock,
							BlockType.Swivel,
						};
						ExchangeListMenu = new List<string>
						{
							"Hinge",
							"Steering Hinge",
							"Scaling Block",
							"Swivel",
						};
					}
					else
					{
						ExchangeList = new List<BlockType>
						{
							BlockType.Hinge,
							BlockType.SteeringHinge,
							BlockType.Swivel,
						};
						ExchangeListMenu = new List<string>
						{
							"Hinge",
							"Steering Hinge",
							"Swivel",
						};
					}
				}
			}
			public class HingeScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					if (Mod.Extended)
					{
						ExchangeList = new List<BlockType>
						{
							BlockType.SteeringHinge,
							BlockType.ScalingBlock,
							BlockType.Swivel,
							BlockType.BallJoint,
						};
						ExchangeListMenu = new List<string>
						{
							"Steering Hinge",
							"Scaling Block",
							"Swivel",
							"Ball Joint",
						};
					}
					else
					{
						ExchangeList = new List<BlockType>
						{
							BlockType.SteeringHinge,
							BlockType.Swivel,
							BlockType.BallJoint,
						};
						ExchangeListMenu = new List<string>
						{
							"Steering Hinge",
							"Swivel",
							"Ball Joint",
						};
					}
				}

			}
			public class SteeringHingeScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					if (Mod.Extended)
					{
						ExchangeList = new List<BlockType>
						{
							BlockType.ScalingBlock,
							BlockType.Swivel,
							BlockType.BallJoint,
							BlockType.Hinge,
						};
						ExchangeListMenu = new List<string>
						{
							"Scaling Block",
							"Swivel",
							"BallJoint",
							"Hinge",
						};
					}
					else
					{
						ExchangeList = new List<BlockType>
						{
							BlockType.Swivel,
							BlockType.BallJoint,
							BlockType.Hinge,
						};
						ExchangeListMenu = new List<string>
						{
							"Swivel",
							"BallJoint",
							"Hinge",
						};
					}
				}
			}
			public class ScalingBlockScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.Swivel,
						BlockType.BallJoint,
						BlockType.Hinge,
						BlockType.SteeringHinge,
					};
					ExchangeListMenu = new List<string>
					{
						"Swivel",
						"BallJoint",
						"Hinge",
						"Steering Hinge",
					};
				}
			}
			public class MotorWheelScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.WheelUnpowered,
						BlockType.CogMediumPowered,
						BlockType.CogMediumUnpowered,
						BlockType.CircularSaw,
					};
					ExchangeListMenu = new List<string>
					{
						"Unpowered Wheel",
						"Powered Cog",
						"Unpowered Cog",
						"Circular Saw",
					};
				}
			}
			public class UnpoweredWheelScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.CogMediumPowered,
						BlockType.CogMediumUnpowered,
						BlockType.CircularSaw,
						BlockType.Wheel
					};
					ExchangeListMenu = new List<string>
					{
						"Powered Cog",
						"Unpowered Cog",
						"Circular Saw",
						"Powered Wheel",
					};
				}
			}
			public class PoweredMediumCogScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.CogMediumUnpowered,
						BlockType.CircularSaw,
						BlockType.Wheel,
						BlockType.WheelUnpowered,
					};
					ExchangeListMenu = new List<string>
					{
						"Unpowered Cog",
						"Circular Saw",
						"Powered Wheel",
						"Unpowered Wheel",
					};
				}
			}
			public class UnpoweredMediumCogScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.CircularSaw,
						BlockType.Wheel,
						BlockType.WheelUnpowered,
						BlockType.CogMediumPowered,
					};
					ExchangeListMenu = new List<string>
					{
						"Circular Saw",
						"Powered Wheel",
						"Unpowered Wheel",
						"Powered Cog",
					};
				}
			}
			public class CircularSawScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.Wheel,
						BlockType.WheelUnpowered,
						BlockType.CogMediumPowered,
						BlockType.CogMediumUnpowered,
					};
					ExchangeListMenu = new List<string>
					{
						"Powered Wheel",
						"Unpowered Wheel",
						"Powered Cog",
						"Unpowered Cog",
					};
				}
			}
			public class LargeWheelScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.LargeWheelUnpowered,
						BlockType.CogLargeUnpowered,
					};
					ExchangeListMenu = new List<string>
					{
						"Unpowered Large Wheel",
						"Unpowered Large Cog",
					};
				}
			}
			public class UnpoweredLargeWheelScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.CogLargeUnpowered,
						BlockType.LargeWheel,
					};
					ExchangeListMenu = new List<string>
					{
						"Unpowered Large Cog",
						"Powered Large Wheel",
					};
				}
			}
			public class UnpoweredLargeCogScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.LargeWheel,
						BlockType.LargeWheelUnpowered,
					};
					ExchangeListMenu = new List<string>
					{
						"Powered Large Wheel",
						"Unpowered Large Wheel",
					};
				}
			}
			public abstract class AbstractPropellerScript : BlockExchangerScript
			{
				public bool isFirst = true;
				public GameObject AxisParent;
				public LineRenderer axis;
				public float angleFlat = 23.06876f; //水平化するための角度 //短プロペラでは22.845
				public Vector3 EndsVector
				{
					get
					{
						if (StartingBlockBuild == null || StartingBlockSimulation == null)
						{
							return component.AxisDrag;
						}
						//シミュ中とビルド中のスタブロの角度の差を取り、その差の角度でProtoEndsVectorを回転させる
						return (StartingBlockSimulation.transform.rotation * Quaternion.Inverse(StartingBlockBuild.transform.rotation)) * component.AxisDrag;

						//return machine.transform.localRotation * ProtoEndsVector;
					}
				}
				protected AxialDrag component;
				public BlockBehaviour StartingBlockSimulation; //シミュ中のスタブロ
				public BlockBehaviour StartingBlockBuild; //ビルド中のスタブロ

				public BlockBehaviour GetStartingBlock() //スタブロを参照するのはちょっと危ないかも？（マルチとか）
				{
					foreach (BlockBehaviour BB in machine.isSimulating ? machine.SimulationBlocks : machine.BuildingBlocks)
					{
						if (BB.BlockID == 0) //スタブロを見つけたら
						{
							return BB;
						}
					}
					return null; //見つからなかったら
				}

				public AbstractPropellerScript()
				{
					isPropeller = true;
					isWoodenPanel = false;
				}
				public override void PropellerSafeAwake()
				{
					if (!BB.isSimulating)
					{
						axis = new LineRenderer();
					}
					AxisParent = new GameObject("Aerodynamic Arrow");
					AxisParent.transform.parent = BB.transform;
					machine = Machine.Active();
					component = GetComponent<AxialDrag>();
					if (component == null)
					{
						Mod.LogError("AxialDrag is null!");
					}
					if (!isFirst)
					{
						return;
					}
					axis = AxisParent.GetComponent<LineRenderer>() ?? AxisParent.gameObject.AddComponent<LineRenderer>();
					axis.SetWidth(0.1f, 0f);
					axis.material = new Material(Shader.Find("Sprites/Default"));
					axis.GetComponent<Renderer>().material.color = Color.cyan;
					isFirst = false;
				}
				public override void PropellerBuildingUpdate() // ここでぬるぽ
				{
					if (machine == null)
					{
						machine = block.Machine.InternalObject;
					}
					//GetCenterはビルド時しか効かないのでは
					//BB.hogehoge = ローカル
					axis.enabled = (CoLController.CoLGUI.Instance.ShowLiftVectors);
					if (axis.enabled)
					{
						Vector3 center = BB.GetCenter();
						if (CoLController.CoLGUI.Instance.IsGlobal)
						{
							axis.SetPositions(new Vector3[2] {
								center,
								center + new Vector3(0, 0, 100 * component.AxisDrag.y * (machine.Rotation * BB.transform.rotation * Vector3.up).z * (BB.Flipped ? -1 : 1))
							});
						}
						else
						{
							axis.SetPositions(new Vector3[2] {
								center,
								center + 100 * component.AxisDrag[1] * (machine.Rotation * BB.transform.rotation * Vector3.up)
							});
						}
					}
				}
				public override void PropellerSimulationUpdate()
				{
					if (machine == null)
					{
						machine = block.Machine.InternalObject;
					}
					if (axis == null)
                    {
						Mod.LogError("axis is null");
                    }
					axis.enabled = CoLController.CoLGUI.Instance.ShowLiftVectors;
					if (axis.enabled)
                    {
						Vector3 center = BB.GetCenter();
						/* // ここは関係ない
						if (CoLController.CoLGUI.IsGlobal)
						{
							axis.SetPositions(new Vector3[2] {
								center,
								center + new Vector3(0, 0, 0.1f * component.AxisDrag.y * (machine.Rotation * BB.transform.rotation * Vector3.up).z * (BB.Flipped ? -1 : 1))
							});
						}
						*/
						axis.SetPositions(new Vector3[2] {
							center,
							//center + 100 * component.AxisDrag[1] * (machine.Rotation * BB.transform.rotation * Vector3.up)
							center + 0.1f * (BB.transform.rotation * PropellerForce())
						});
					}
				}
				public Vector3 PropellerForce() // プロペラにかかる揚力（抵抗）を求める
                {
					Vector3 currentVelocity = component.Rigidbody.velocity;
					Vector3 a = component.upTransform.InverseTransformDirection(currentVelocity);
					Vector3 xyz = Vector3.Scale(-a, component.AxisDrag);
					float currentVeclocitySqr = Mathf.Min(currentVelocity.sqrMagnitude, 100 ^ 2);
					return xyz * currentVeclocitySqr;
                }
				public override void SetBlockList()
				{
					
				}
				public override void ModifyBlock01()
				{
					//machine.UndoSystem.Undo(); //これはトグルを元に戻すことと同じ
					float deg = angleFlat * (BB.Flipped ? 1 : -1);
					Vector3 lastPosition = BB.transform.localPosition;
					Quaternion lastRotation = BB.transform.localRotation;
					machine.RotateBlock(BB.Guid, lastRotation * Quaternion.AngleAxis(deg, Vector3.forward));
					machine.UndoSystem.AddAction(
						new UndoActionRotate(machine, BB.Guid, lastPosition, lastPosition, BB.transform.localRotation, lastRotation)
						);
				}
				public override void ModifyBlock02()
				{
					float deg = angleFlat/2 * (BB.Flipped ? 1 : -1);
					Vector3 lastPosition = BB.transform.localPosition;
					Quaternion lastRotation = BB.transform.localRotation;
					machine.RotateBlock(BB.Guid, lastRotation * Quaternion.AngleAxis(deg, Vector3.forward));
					machine.UndoSystem.AddAction(
						new UndoActionRotate(machine, BB.Guid, lastPosition, lastPosition, BB.transform.localRotation, lastRotation)
						);
				}
				public override void ModifyBlock03()
				{
					float deg = -angleFlat * (BB.Flipped ? 1 : -1);
					Vector3 lastPosition = BB.transform.localPosition;
					Quaternion lastRotation = BB.transform.localRotation;
					machine.RotateBlock(BB.Guid, lastRotation * Quaternion.AngleAxis(deg, Vector3.forward));
					machine.UndoSystem.AddAction(
						new UndoActionRotate(machine, BB.Guid, lastPosition, lastPosition, BB.transform.localRotation, lastRotation)
						);
				}
				public override void ModifyBlock04()
				{
					float deg = -angleFlat/2 * (BB.Flipped ? 1 : -1);
					Vector3 lastPosition = BB.transform.localPosition;
					Quaternion lastRotation = BB.transform.localRotation;
					machine.RotateBlock(BB.Guid, lastRotation * Quaternion.AngleAxis(deg, Vector3.forward));
					machine.UndoSystem.AddAction(
						new UndoActionRotate(machine, BB.Guid, lastPosition, lastPosition, BB.transform.localRotation, lastRotation)
						);
				}

			}
			public class SmallPropellerScript : AbstractPropellerScript
			{
				public SmallPropellerScript()
				{
					isPropeller = true;
					isWoodenPanel = false;
					angleFlat = 22.845f;
				}
				public override void SetBlockList()
				{
					if (Mod.Extended) //52プロペラを追加
					{
						ExchangeList = new List<BlockType>
						{
							BlockType.Propeller,
							(BlockType)52,
							BlockType.MetalBlade,
							BlockType.Spike,
						};
						ExchangeListMenu = new List<string>
						{
							"Long Propeller",
							"52 Propeller",
							"Metal Blade",
							"Spike",
						};
					}
					else
					{
						ExchangeList = new List<BlockType>
					{
						BlockType.Propeller,
						BlockType.MetalBlade,
						BlockType.Spike,
					};
						ExchangeListMenu = new List<string>
					{
						"Long Propeller",
						"Metal Blade",
						"Spike",
					};
					}
				}
			}
			public class PropellerScript : AbstractPropellerScript
			{
				public PropellerScript()
				{
					isPropeller = true;
					isWoodenPanel = false;
					angleFlat = 23.06876f;
				}
				public override void SetBlockList()
				{
					if (Mod.Extended) //52プロペラを追加
					{
						ExchangeList = new List<BlockType>
						{
							(BlockType)52,
							BlockType.MetalBlade,
							BlockType.Spike,
							BlockType.SmallPropeller,
						};
						ExchangeListMenu = new List<string>
						{
							"52 Propeller",
							"Metal Blade",
							"Spike",
							"Small Propeller",
						};
					}
					else
					{
						ExchangeList = new List<BlockType>
						{
							BlockType.MetalBlade,
							BlockType.Spike,
							BlockType.SmallPropeller,
						};
						ExchangeListMenu = new List<string>
						{
							"Metal Blade",
							"Spike",
							"Small Propeller",
						};
					}
				}
			}
			public class Propeller52Script : AbstractPropellerScript
			{
				public Propeller52Script()
				{
					isPropeller = true;
					isWoodenPanel = false;
					angleFlat = 22.845f;
				}
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.MetalBlade,
						BlockType.Spike,
						BlockType.SmallPropeller,
						BlockType.Propeller,
					};
					ExchangeListMenu = new List<string>
					{
						"Metal Blade",
						"Spike",
						"Small Propeller",
						"Long Propeller",
					};
				}
			}
			public class MetalBladeScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					if (Mod.Extended)
					{
						ExchangeList = new List<BlockType>
						{
							BlockType.Spike,
							BlockType.SmallPropeller,
							BlockType.Propeller,
							(BlockType)52,
						};
						ExchangeListMenu = new List<string>
						{
							"Spike",
							"Small Propeller",
							"Long Propeller",
							"52 Propeller",
						};
					}
					else
					{
						ExchangeList = new List<BlockType>
						{
							BlockType.Spike,
							BlockType.SmallPropeller,
							BlockType.Propeller,
						};
						ExchangeListMenu = new List<string>
						{
							"Spike",
							"Small Propeller",
							"Long Propeller",
						};
					}
				}
			}
			public class SpikeScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					if (Mod.Extended)
					{
						ExchangeList = new List<BlockType>
						{
							BlockType.SmallPropeller,
							BlockType.Propeller,
							(BlockType)52,
							BlockType.MetalBlade,
						};
						ExchangeListMenu = new List<string>
						{
							"Small Propeller",
							"Long Propeller",
							"52 Propeller",
							"Metal Blade",
						};
					}
					else
					{
						ExchangeList = new List<BlockType>
						{
							BlockType.SmallPropeller,
							BlockType.Propeller,
							BlockType.MetalBlade,
						};
						ExchangeListMenu = new List<string>
						{
							"Small Propeller",
							"Long Propeller",
							"Metal Blade",
						};
					}
				}
			}
			public class WoodenPanelScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.ArmorPlateLarge,
						BlockType.ArmorPlateSmall,
						BlockType.ArmorPlateRound,
					};
					ExchangeListMenu = new List<string>
					{
						"Large Armor Plate",
						"Small Armor Plate",
						"Round Armor Plate",
					};
				}
				public WoodenPanelScript()
				{
					isWoodenPanel = true;
					isPropeller = false;
				}
			}
			public class ArmorPlateLargeScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
			{
				BlockType.ArmorPlateSmall,
				BlockType.ArmorPlateRound,
				BlockType.WoodenPanel,
			};
					ExchangeListMenu = new List<string>
			{
				"Small Armor Plate",
				"Round Armor Plate",
				"Wooden Panel",
			};
				}
			}
			public class ArmorPlateSmallScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
			{
				BlockType.ArmorPlateRound,
				BlockType.WoodenPanel,
				BlockType.ArmorPlateLarge,
			};
					ExchangeListMenu = new List<string>
			{
				"Round Armor Plate",
				"Wooden Panel",
				"Large Armor Plate",
			};
				}
			}
			public class ArmorPlateRoundScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.WoodenPanel,
						BlockType.ArmorPlateLarge,
						BlockType.ArmorPlateSmall,
					};
					ExchangeListMenu = new List<string>
					{
						"Wooden Panel",
						"Large Armor Plate",
						"Small Armor Plate",
					};
				}
			}
			public class CannonScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.ShrapnelCannon,
						BlockType.Crossbow,
						BlockType.Vacuum,
						BlockType.WaterCannon,
					};
					ExchangeListMenu = new List<string>
					{
						"Shrapnel Cannon",
						"Cross Bow",
						"Vacuum",
						"Water Cannon"
					};
				}
			}
			public class ShrapnelCannonScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.Crossbow,
						BlockType.Vacuum,
						BlockType.WaterCannon,
						BlockType.Cannon,
					};
					ExchangeListMenu = new List<string>
					{
						"Cross Bow",
						"Vacuum",
						"Water Cannon",
						"Cannon",
					};
				}
			}
			public class CrossbowScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.Vacuum,
						BlockType.WaterCannon,
						BlockType.Cannon,
						BlockType.ShrapnelCannon,
					};
					ExchangeListMenu = new List<string>
					{
						"Vacuum",
						"WaterCannon",
						"Cannon",
						"Shrapnel Cannon",
					};
				}
			}
			public class VacuumScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.WaterCannon,
						BlockType.Cannon,
						BlockType.ShrapnelCannon,
						BlockType.Crossbow,
					};
					ExchangeListMenu = new List<string>
					{
						"Water Cannon",
						"Cannon",
						"Shrapnel Cannon",
						"Cross Bow",
					};
				}
			}
			public class WaterCannonScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.Cannon,
						BlockType.ShrapnelCannon,
						BlockType.Crossbow,
						BlockType.Vacuum,
					};
					ExchangeListMenu = new List<string>
					{
						"Cannon",
						"Shrapnel Cannon",
						"Cross Bow",
						"Vacuum",
					};
				}
			}
			public class BombScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.FlameBall,
						BlockType.Boulder,
					};
					ExchangeListMenu = new List<string>
					{
						"Flame Ball",
						"Boulder",
					};
				}
			}
			public class FlameBallScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.Boulder,
						BlockType.Bomb,
					};
					ExchangeListMenu = new List<string>
					{
						"Boulder",
						"Bomb",
					};
				}
			}
			public class BoulderScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.Bomb,
						BlockType.FlameBall,
					};
					ExchangeListMenu = new List<string>
					{
						"Bomb",
						"Flame Ball",
					};
				}
			}
			public class SensorScript : BlockExchangerScript
            {
                public override void SetBlockList()
                {
					ExchangeList = new List<BlockType>
					{
						//BlockType.Sensor,
						BlockType.Timer,
						BlockType.Altimeter,
						BlockType.LogicGate,
						BlockType.Anglometer,
						BlockType.Speedometer,
					};
					ExchangeListMenu = new List<string>
					{
						//"Sensor",
						"Timer",
						"Altimeter",
						"Logicgate",
						"Anglometer",
						"Speedometer",
					};
				}
            }
			public class TimerScript : BlockExchangerScript
            {
                public override void SetBlockList()
                {
					ExchangeList = new List<BlockType>
					{
						//BlockType.Timer,
						BlockType.Altimeter,
						BlockType.LogicGate,
						BlockType.Anglometer,
						BlockType.Speedometer,
						BlockType.Sensor,
					};
					ExchangeListMenu = new List<string>
					{
						//"Timer",
						"Altimeter",
						"Logicgate",
						"Anglometer",
						"Speedometer",
						"Sensor",
					};
				}
			}
			public class AltimeterScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						//BlockType.Altimeter,
						BlockType.LogicGate,
						BlockType.Anglometer,
						BlockType.Speedometer,
						BlockType.Sensor,
						BlockType.Timer,
					};
					ExchangeListMenu = new List<string>
					{
						//"Altimeter",
						"Logicgate",
						"Anglometer",
						"Speedometer",
						"Sensor",
						"Timer",
					};
				}
			}
			public class LogicGateScript : BlockExchangerScript
            {
                public override void SetBlockList()
                {
					ExchangeList = new List<BlockType>
					{
						//BlockType.LogicGate,
						BlockType.Anglometer,
						BlockType.Speedometer,
						BlockType.Sensor,
						BlockType.Timer,
						BlockType.Altimeter,
					};
					ExchangeListMenu = new List<string>
					{
						//"Logicgate",
						"Anglometer",
						"Speedometer",
						"Sensor",
						"Timer",
						"Altimeter",
					};
                }
			}
			public class AnglometerScript : BlockExchangerScript
			{
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						//BlockType.Anglometer,
						BlockType.Speedometer,
						BlockType.Sensor,
						BlockType.Timer,
						BlockType.Altimeter,
						BlockType.LogicGate,
					};
					ExchangeListMenu = new List<string>
					{
						//"Anglometer",
						"Speedometer",
						"Sensor",
						"Timer",
						"Altimeter",
						"Logicgate",
					};
				}
			}
			public class SpeedometerScript : BlockExchangerScript
            {
                public override void SetBlockList()
                {
					ExchangeList = new List<BlockType>
					{
						//BlockType.Speedometer,
						BlockType.Sensor,
						BlockType.Timer,
						BlockType.Altimeter,
						BlockType.LogicGate,
						BlockType.Anglometer,
					};
					ExchangeListMenu = new List<string>
					{
						//"Speedometer",
						"Sensor",
						"Timer",
						"Altimeter",
						"Logicgate",
						"Anglometer",
					};
                }
            }
			public class BalloonScript : BlockExchangerScript
            {
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.SqrBalloon
					};
					ExchangeListMenu = new List<string>
					{
						"Hot Air Balloon"
					};
				}
			}
			public class SqrBalloonScript : BlockExchangerScript
            {
				public override void SetBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.Balloon
					};
					ExchangeListMenu = new List<string>
					{
						"Balloon"
					};
				}
			}
			public class OtherBlockScript : BlockExchangerScript
			{
                public override void SetBlockList()
                {
					ExchangeList = new List<BlockType>
					{

					};
					ExchangeListMenu = new List<string>
					{
						
					};
				}
            }
		}
		public abstract class ModBlockExchangerScript : BlockExchangerScript
		{
			//それぞれのmodのID
			public static Guid SimpleMachinegunGuid = new Guid("1f5c7130-1eaa-47bc-ad1f-66586f68c9d6");
			public static Guid BattleBullet1Guid = new Guid("72a20968-e89a-486a-a797-460e65a7df9e");
			public static Guid BattleBullet2Guid = new Guid("0e746ba8-7cbe-4afd-95b6-a9f67e814f7e");
			public static Guid BattleMissileGuid = new Guid("d67339bc-a869-4f8d-8943-166a523d86f5");
			public static Guid GuidedMissileGuid = new Guid("90a17943-af2a-40ea-a51f-530553b9fcb0");
			public static Guid CruiserGuid = new Guid("0c99ef6c-f627-4b4f-a5c8-ccdddc08b8f5");
			public static Guid ItaGuid = new Guid("7b20ba2d-d1d5-4e1c-89ee-f585d1acbcd0");
			public static Guid CUGuid = new Guid("f47836d2-4356-4ef5-9e69-cc1bcda5e0fc");
			public static Guid LaserWeaponsGuid = new Guid("15259577-bd19-4397-8646-88d235cf8d24");
			public static Guid EsTankCannonGuid = new Guid("50e63b55-b976-4009-82ab-66f989218122");
			public static Guid AnalogControllableBlocksGuid = new Guid("6a56a26c-5f7d-4dc5-b73d-7161060a8656");
			public static Guid ShipsBlocksGuid = new Guid("68ea4a46-26cd-4a8e-8189-ffcbd96aeb9d");
			public static Guid BoosterFuelGuid = new Guid("d1dffba4-cfa1-49c3-95a3-0a3094bbc517");

			//交換できるブロック群
			public static List<ModBlockId> HorizontalWeapon
			{
				get
				{
					List<ModBlockId> ret = new List<ModBlockId>();
					if (Mods.IsModLoaded(SimpleMachinegunGuid))
					{
						ret.Add(new ModBlockId(SimpleMachinegunGuid, 1));
						ret.Add(new ModBlockId(SimpleMachinegunGuid, 2));
						ret.Add(new ModBlockId(SimpleMachinegunGuid, 3));
					}
					
					if (Mods.IsModLoaded(BattleBullet1Guid))
					{
						ret.Add(new ModBlockId(BattleBullet1Guid, 1));
						ret.Add(new ModBlockId(BattleBullet1Guid, 2));
					}
					if (Mods.IsModLoaded(BattleBullet2Guid))
					{
						ret.Add(new ModBlockId(BattleBullet2Guid, 10));
						ret.Add(new ModBlockId(BattleBullet2Guid, 15));
					}
					if (Mods.IsModLoaded(LaserWeaponsGuid))
					{
						ret.Add(new ModBlockId(LaserWeaponsGuid, 10));
						ret.Add(new ModBlockId(LaserWeaponsGuid, 11));
					}
					
					return ret;
				}
			}
			public static List<ModBlockId> VerticalWeapon
			{
				get
				{
					List<ModBlockId> ret = new List<ModBlockId>();
					if (Mods.IsModLoaded(SimpleMachinegunGuid))
					{
						ret.Add(new ModBlockId(SimpleMachinegunGuid, 4));
						ret.Add(new ModBlockId(SimpleMachinegunGuid, 5));
					}
					
					if (Mods.IsModLoaded(BattleBullet1Guid))
					{
						ret.Add(new ModBlockId(BattleBullet1Guid, 3));
						ret.Add(new ModBlockId(BattleBullet1Guid, 4));
					}
					if (Mods.IsModLoaded(BattleBullet2Guid))
					{
						ret.Add(new ModBlockId(BattleBullet2Guid, 11));
						ret.Add(new ModBlockId(BattleBullet2Guid, 16));
					}
					if (Mods.IsModLoaded(LaserWeaponsGuid))
					{
						ret.Add(new ModBlockId(LaserWeaponsGuid, 15));
						ret.Add(new ModBlockId(LaserWeaponsGuid, 16));
					}

					return ret;
				}
			}
			public static List<ModBlockId> Missile
			{
				get
				{
					List<ModBlockId> ret = new List<ModBlockId>();
					if (Mods.IsModLoaded(BattleMissileGuid))
					{
						ret.Add(new ModBlockId(BattleMissileGuid, 10));
					}
					if (Mods.IsModLoaded(GuidedMissileGuid))
					{
						ret.Add(new ModBlockId(GuidedMissileGuid, 10));
						ret.Add(new ModBlockId(GuidedMissileGuid, 11));
						ret.Add(new ModBlockId(GuidedMissileGuid, 12));
						ret.Add(new ModBlockId(GuidedMissileGuid, 13));
					}
					if (Mods.IsModLoaded(LaserWeaponsGuid))
					{
						ret.Add(new ModBlockId(LaserWeaponsGuid, 20));
						ret.Add(new ModBlockId(LaserWeaponsGuid, 25));
					}
					return ret;
				}
			}
			public static List<ModBlockId> Cruiser // 棒と推進系
			{
				get
				{
					List<ModBlockId> ret = new List<ModBlockId>();
					if (Mods.IsModLoaded(CruiserGuid))
					{
						for (int i=1; i<=7; i++)
						{
							ret.Add(new ModBlockId(CruiserGuid, i));
						}
					}
					if (Mods.IsModLoaded(CUGuid))
					{
						ret.Add(new ModBlockId(CUGuid, 1));
					}
					if (Mods.IsModLoaded(ShipsBlocksGuid))
                    {
						for (int i=1; i<=7; i++)
                        {
							ret.Add(new ModBlockId(ShipsBlocksGuid, i));
						}
                    }
					return ret;
				}
			}
			public static List<ModBlockId> Ita // 板
			{
				get
				{
					List<ModBlockId> ret = new List<ModBlockId>();
					if (Mods.IsModLoaded(ItaGuid))
					{
						for (int i=1; i<9; i++)
						{
							ret.Add(new ModBlockId(ItaGuid, i));
						}
					}
					if (Mods.IsModLoaded(ShipsBlocksGuid))
                    {
						foreach (int i in new int[] 
						{
							11, 12, 13, 14, 15, 16, 17, 21, 22, 23, 24, 25, 26, 27, 32, 33, 34, 35, 41, 42, 43, 44, 51, 52, 52, 61, 62, 71
						})
                        {
							ret.Add(new ModBlockId(ShipsBlocksGuid, i));
                        }
                    }
					return ret;
				}
			}
			public static List<ModBlockId> Chaff
			{
				get
				{
					List<ModBlockId> ret = new List<ModBlockId>();
					if (Mods.IsModLoaded(GuidedMissileGuid))
					{
						ret.Add(new ModBlockId(GuidedMissileGuid, 20));
					}
					if (Mods.IsModLoaded(LaserWeaponsGuid))
					{
						ret.Add(new ModBlockId(LaserWeaponsGuid, 40));
					}
					return ret;
				}
			}
			public static List<ModBlockId> ECannon1 //120mm 152mm
			{
				get
				{
					List<ModBlockId> ret = new List<ModBlockId>();
					if (Mods.IsModLoaded(EsTankCannonGuid))
					{
						ret.Add(new ModBlockId(EsTankCannonGuid, 1));
						ret.Add(new ModBlockId(EsTankCannonGuid, 4));
						ret.Add(new ModBlockId(EsTankCannonGuid, 9));
						ret.Add(new ModBlockId(EsTankCannonGuid, 10)); //Medieval Cannon を仮置き
					}
					return ret;
				}
			}
			public static List<ModBlockId> ECannon2 //88mm
			{
				get
				{
					List<ModBlockId> ret = new List<ModBlockId>();
					if (Mods.IsModLoaded(EsTankCannonGuid))
					{
						ret.Add(new ModBlockId(EsTankCannonGuid, 5));
						ret.Add(new ModBlockId(EsTankCannonGuid, 6));
					}
					return ret;
				}
			}
			public static List<ModBlockId> ECannon3 //75mm
			{
				get
				{
					List<ModBlockId> ret = new List<ModBlockId>();
					if (Mods.IsModLoaded(EsTankCannonGuid))
					{
						ret.Add(new ModBlockId(EsTankCannonGuid, 12));
						ret.Add(new ModBlockId(EsTankCannonGuid, 13));
						ret.Add(new ModBlockId(EsTankCannonGuid, 18));
					}
					return ret;
				}
			}
			public static List<ModBlockId> ECannon4 //45mm
			{
				get
				{
					List<ModBlockId> ret = new List<ModBlockId>();
					if (Mods.IsModLoaded(EsTankCannonGuid))
					{
						ret.Add(new ModBlockId(EsTankCannonGuid, 7));
						ret.Add(new ModBlockId(EsTankCannonGuid, 8));
					}
					return ret;
				}
			}
			public static List<ModBlockId> ECannon5 //12.7mm 20mm
			{
				get
				{
					List<ModBlockId> ret = new List<ModBlockId>();
					if (Mods.IsModLoaded(EsTankCannonGuid))
					{
						ret.Add(new ModBlockId(EsTankCannonGuid, 11));
						ret.Add(new ModBlockId(EsTankCannonGuid, 19));
						ret.Add(new ModBlockId(EsTankCannonGuid, 20));
					}
					return ret;
				}
			}
			public static List<ModBlockId> ECannon6 //antenna Flag
			{
				get
				{
					List<ModBlockId> ret = new List<ModBlockId>();
					if (Mods.IsModLoaded(EsTankCannonGuid))
					{
						ret.Add(new ModBlockId(EsTankCannonGuid, 3));
						ret.Add(new ModBlockId(EsTankCannonGuid, 14));
						ret.Add(new ModBlockId(EsTankCannonGuid, 15));
						ret.Add(new ModBlockId(EsTankCannonGuid, 16));
						ret.Add(new ModBlockId(EsTankCannonGuid, 17));
					}
					return ret;
				}
			}
			public static List<ModBlockId> ModWheel // 車輪
            {
				get
				{
					List<ModBlockId> ret = new List<ModBlockId>();
					if (Mods.IsModLoaded(AnalogControllableBlocksGuid))
					{
						ret.Add(new ModBlockId(AnalogControllableBlocksGuid, 10));
						ret.Add(new ModBlockId(AnalogControllableBlocksGuid, 20));
						ret.Add(new ModBlockId(AnalogControllableBlocksGuid, 40));
						ret.Add(new ModBlockId(AnalogControllableBlocksGuid, 140));
						ret.Add(new ModBlockId(AnalogControllableBlocksGuid, 141));
						ret.Add(new ModBlockId(AnalogControllableBlocksGuid, 142));
					}
					return ret;
				}
			}
			public static List<ModBlockId> FuelGenerator // インテーク
            {
                get
                {
					List<ModBlockId> ret = new List<ModBlockId>();
					if (Mods.IsModLoaded(BoosterFuelGuid))
                    {
						ret.Add(new ModBlockId(BoosterFuelGuid, 20));
                    }
					return ret;
                }
            }
			public static List<ModBlockId> FuelTank // タンク
            {
                get
                {
					List<ModBlockId> ret = new List<ModBlockId>();
					if (Mods.IsModLoaded(BoosterFuelGuid))
                    {
						ret.Add(new ModBlockId(BoosterFuelGuid, 30));
						ret.Add(new ModBlockId(BoosterFuelGuid, 31));
                    }
					return ret;
                }
            }
			public static List<ModBlockId> Booster // ブースター（CUとBoosterAndFuel）
            {
                get
                {
					List<ModBlockId> ret = new List<ModBlockId>();
					if (Mods.IsModLoaded(CUGuid))
                    {
						ret.Add(new ModBlockId(CUGuid, 1));
                    }
					if (Mods.IsModLoaded(BoosterFuelGuid))
                    {
						ret.Add(new ModBlockId(BoosterFuelGuid, 10));
						ret.Add(new ModBlockId(BoosterFuelGuid, 11));
						ret.Add(new ModBlockId(BoosterFuelGuid, 12));
						ret.Add(new ModBlockId(BoosterFuelGuid, 13));
                    }
					return ret;
                }
            }
			
			public class ModBlockId //このmodブロックの情報
			{
				//modブロックのローカルIDと名前の辞書
				public Dictionary<int, string> SimpleMachinegunName;
				public Dictionary<int, string> BB1Name;
				public Dictionary<int, string> BB2Name;
				public Dictionary<int, string> BMissileName;
				public Dictionary<int, string> GMissileName;
				public Dictionary<int, string> CruiserName;
				public Dictionary<int, string> ItaName;
				public Dictionary<int, string> CUName;
				public Dictionary<int, string> LaserWeaponsName;
				public Dictionary<int, string> EsTankCannonName;
				public Dictionary<int, string> AnalogControllableBlocksName;
				public Dictionary<int, string> ShipsBlocksName;
				public Dictionary<int, string> BoosterFuelName;

				public Guid ModId;
				public int LocalId;
				public string Name;
				public List<ModBlockId> BlockGroup; //このブロックと交換可能なブロック群
				public List<string> NameGroup; //このブロックと交換可能なブロック群の名前一覧
				public ModBlockId(Guid modId, int localId)
				{
					ModId = modId;
					LocalId = localId;
					//ModConsole.Log("ModBlockId is constructed.");
					SetName();
				}
				public void SetName()
				{
					SetDictionary();
					if (ModId == SimpleMachinegunGuid) { Name = SimpleMachinegunName[LocalId]; }
					else if (ModId == BattleBullet1Guid) { Name = BB1Name[LocalId]; }
					else if (ModId == BattleBullet2Guid) { Name = BB2Name[LocalId]; }
					else if (ModId == BattleMissileGuid) { Name = BMissileName[LocalId]; }
					else if (ModId == GuidedMissileGuid) { Name = GMissileName[LocalId]; }
					else if (ModId == CruiserGuid) { Name = CruiserName[LocalId]; }
					else if (ModId == ItaGuid) { Name = ItaName[LocalId]; }
					else if (ModId == CUGuid) { Name = CUName[LocalId]; }
					else if (ModId == LaserWeaponsGuid) { Name = LaserWeaponsName[LocalId]; }
					else if (ModId == EsTankCannonGuid) { Name = EsTankCannonName[LocalId]; }
					else if (ModId == AnalogControllableBlocksGuid) { Name = AnalogControllableBlocksName[LocalId]; }
					else if (ModId == ShipsBlocksGuid) { Name = ShipsBlocksName[LocalId]; }
					else if (ModId == BoosterFuelGuid) { Name = BoosterFuelName[LocalId]; }
				}
				public void SetGroup() //ブロックの名前と交換できるブロック群を設定する
				{
					BlockGroup = new List<ModBlockId>();
					NameGroup = new List<string>();
					SetDictionary();
					if (ModId == SimpleMachinegunGuid)
					{
						switch (LocalId)
						{
							case 1:
							case 2:
							case 3:
								BlockGroup = HorizontalWeapon;
								break;
							case 4:
							case 5:
								BlockGroup = VerticalWeapon;
								break;
						}
					}
					else if (ModId == BattleBullet1Guid)
					{
						switch (LocalId)
						{
							case 1:
							case 2:
								BlockGroup = HorizontalWeapon;
								break;
							case 3:
							case 4:
								BlockGroup = VerticalWeapon;
								break;
						}
					}
					else if (ModId == BattleBullet2Guid)
					{
						switch (LocalId)
						{
							case 10:
							case 15:
								BlockGroup = HorizontalWeapon;
								break;
							case 11:
							case 16:
								BlockGroup = VerticalWeapon;
								break;
						}
					}
					else if (ModId == BattleMissileGuid)
					{
						switch (LocalId)
						{
							case 10:
								BlockGroup = Missile;
								break;
						}
					}
					else if (ModId == GuidedMissileGuid)
					{
						switch (LocalId)
						{
							case 10:
							case 11:
							case 12:
							case 13:
								BlockGroup = Missile;
								break;
							case 20:
								BlockGroup = Chaff;
								break;
						}
					}
					else if (ModId == CruiserGuid)
					{
						switch (LocalId)
						{
							case 1:
							case 2:
							case 3:
							case 4:
							case 5:
							case 6:
							case 7:
								BlockGroup = Cruiser;
								break;
						}
					}
					else if (ModId == ItaGuid)
					{
						switch (LocalId)
						{
							case 1:
							case 2:
							case 3:
							case 4:
							case 5:
							case 6:
							case 7:
							case 8:
							case 9:
								BlockGroup = Ita;
								break;
						}
					}
					else if (ModId == CUGuid)
					{
						switch (LocalId)
						{
							case 1:
								BlockGroup = Cruiser;
								break;
						}
					}
					else if (ModId == LaserWeaponsGuid)
					{
						switch (LocalId)
						{
							case 10:
							case 11:
								BlockGroup = HorizontalWeapon;
								break;
							case 15:
							case 16:
								BlockGroup = VerticalWeapon;
								break;
							case 20:
							case 25:
								BlockGroup = Missile;
								break;
							case 40:
								BlockGroup = Chaff;
								break;
						}
					}
					else if (ModId == EsTankCannonGuid)
					{
						switch (LocalId)
						{
							case 1:
							case 4:
							case 9:
							case 10:
								BlockGroup = ECannon1;
								break;
							case 5:
							case 6:
								BlockGroup = ECannon2;
								break;
							case 12:
							case 13:
							case 18:
								BlockGroup = ECannon3;
								break;
							case 7:
							case 8:
								BlockGroup = ECannon4;
								break;
							case 11:
							case 19:
							case 20:
								BlockGroup = ECannon5;
								break;
							case 3:
							case 14:
							case 15:
							case 16:
							case 17:
								BlockGroup = ECannon6;
								break;
						}
					}
					else if (ModId == AnalogControllableBlocksGuid)
                    {
                        switch (LocalId)
                        {
							case 10:
							case 20:
							case 40:
							case 140:
							case 141:
							case 142:
								BlockGroup = ModWheel;
								break;
                        }
                    }
					else if (ModId == ShipsBlocksGuid)
                    {
						switch (LocalId)
                        {
							case 1:
							case 2:
							case 3:
							case 4:
							case 5:
							case 6:
							case 7:
								BlockGroup = Cruiser;
								break;
							case 11:
							case 12:
							case 13:
							case 14:
							case 15:
							case 16:
							case 17:
							case 21:
							case 22:
							case 23:
							case 24:
							case 25:
							case 26:
							case 27:
							case 32:
							case 33:
							case 34:
							case 35:
							case 41:
							case 42:
							case 43:
							case 44:
							case 51:
							case 52:
							case 53:
							case 61:
							case 62:
							case 71:
								BlockGroup = Ita;
								break;
                        }
                    }
					else if (ModId == BoosterFuelGuid)
                    {
                        switch (LocalId)
                        {
							case 10:
							case 11:
							case 12:
							case 13:
								BlockGroup = Booster;
								break;
							case 20:
								BlockGroup = FuelGenerator;
								break;
							case 30:
							case 31:
								BlockGroup = FuelTank;
								break;
                        }
                    }
					//自身が交換可能リストに入っているハズなので、自分を消去する
					//同時に、辞書に従ってNameGroupにブロック名を追加していく
					int BlockGroupCount = BlockGroup.Count;
					int IndexToRemove = 0;
					for (int i=0; i < BlockGroupCount; i++)
					{
						if (ModId == BlockGroup[i].ModId && LocalId == BlockGroup[i].LocalId)
						{
							IndexToRemove = i;
							continue;
						}
						NameGroup.Add(BlockGroup[i].Name);
						//ModConsole.Log("Add : " + BlockGroup[i].Name);
					}
					BlockGroup.RemoveAt(IndexToRemove);
					if (BlockGroup.Count != NameGroup.Count)
					{
						Mod.LogWarning("BlockGroup.Count != NameGroup.Count");
					}
				}
				public void SetDictionary()
				{
					SimpleMachinegunName = new Dictionary<int, string>()
					{
						{1, "Normal"},
						{2, "EX" },
						{3, "AP" },
						{4, "Auto Cannon" },
						{5, "Flak Cannon" },
					};
					BB1Name = new Dictionary<int, string>()
					{
						{1, "BB 001 h" },
						{2, "BB 002 h" },
						{3, "BB 001 v" },
						{4, "BB 002 v" },
					};
					BB2Name = new Dictionary<int, string>()
					{
						{10, "BB 001A h" },
						{11, "BB 001A v" },
						{15, "BB 002A h" },
						{16, "BB 002A v" },
					};
					BMissileName = new Dictionary<int, string>()
					{
						{10, "Missile 001" },
						{20, "Chaff Launcher" },
					};
					GMissileName = new Dictionary<int, string>()
					{
						{10, "ITANO" },
						{11, "Hexapod" },
						{12, "Twinpod" },
						{13, "RAPIER" },
						{20, "Chaff Launcher" }
					};
					CruiserName = new Dictionary<int, string>()
					{
						{1, "Screw" },
						{2, "Bouy" },
						{3, "Beam 7x" },
						{4, "Beam 5x" },
						{5, "Plate 5x5" },
						{6, "Plate 3x5" },
						{7, "Plate 3x3" },
					};
					ItaName = new Dictionary<int, string>()
					{
						{ 1, "Plate" },
						{ 2, "Half Plate" },
						{ 3, "Beam" },
						{ 4, "Thin Plate" },
						{ 5, "Half Beam" },
						{ 6, "Double Beam" },
						{ 7, "Thin Half Plate" },
						{ 8, "Small Plate" },
						{ 9, "Core Cube" },
					};
					CUName = new Dictionary<int, string>()
					{
						{ 1, "Thruster" },
					};
					LaserWeaponsName = new Dictionary<int, string>()
					{
						{10, "Laser 001" },
						{11, "Laser 002" },
						{15, "Laser 001v" },
						{16, "Laser 002v" },
						{20, "Distoray3" },
						{25, "Distoray3v" },
						{40, "Light Chaff" }
					};
					EsTankCannonName = new Dictionary<int, string>()
					{
						{1, "120mm L44 APFSDS" },
						{2, "Smoke Granade Discharger" },
						{3, "Antenna" },
						{4, "120mm L44 HEATFS" },
						{5, "88mm L74 AntiAir" },
						{6, "88mm L74 ACPR" },
						{7, "45mm L46 HE" },
						{8, "45m L46 AP" },
						{9, "152mm L23 HE" },
						{10, "Medieval Cannon" },
						{11, "20mm L65 HE" },
						{12, "75mm L48 HE" },
						{13, "75mm L48 AP" },
						{14, "Red Big Flag" },
						{15, "Blud Big Flag" },
						{16, "Red Small Flag" },
						{17, "Blue Small Flag" },
						{18, "75mm L24 HEAT" },
						{19, "12.7mm MachineGun HE" },
						{20, "12.7mm MachineGun T" },
						{21, "Nightvision" },
					};
					AnalogControllableBlocksName = new Dictionary<int, string>()
					{
						{10, "Analog Hinge" },
						{20, "Motor 01" },
						{40, "Analog Wheel Base" },
						{140, "Large Wheel Tire 01" },
						{141, "Large Wheel Tire 02" },
						{142, "Large Wheel Tire 03" },
					};
					ShipsBlocksName = new Dictionary<int, string>()
					{
						{1, "Rod L 3" },
						{2, "Rod L 5" },
						{3, "Rod L 8" },
						{4, "Rod L 12" },
						{5, "Rod L 17" },
						{6, "Rod L 23" },
						{7, "Rod L 30" },
						{11, "Board S 3" },
						{12, "Board S 5" },
						{13, "Board S 8" },
						{14, "Board S 12" },
						{15, "Board S 17" },
						{16, "Board S 23" },
						{17, "Board S 30" },
						{21, "Board O 3x5" },
						{22, "Board O 3x8" },
						{23, "Board O 5x8" },
						{24, "Board O 3x12" },
						{25, "Board O 3x17" },
						{26, "Board O 3x23" },
						{27, "Board O 3x30" },
						{32, "Board O 5x12" },
						{33, "Board O 5x17" },
						{34, "Board O 5x23" },
						{35, "Board O 5x30" },
						{41, "Board O 8x12" },
						{42, "Board O 8x17" },
						{43, "Board O 8x23" },
						{44, "Board O 8x30" },
						{51, "Board O 12x17" },
						{52, "Board O 12x23" },
						{53, "Board O 12x30" },
						{61, "Board O 17x23" },
						{62, "Board O 17x30" },
						{71, "Board O 23x30" },
					};
					BoosterFuelName = new Dictionary<int, string>()
					{
						{10, "Bo-T26-2" },
						{11, "BoMini-T9-2" },
						{12, "RoBo-T32-3" },
						{13, "RoBoL-T80-3" },
						{20, "Intake-S8" },
						{30, "Tank-F15" },
						{31, "MoTank-F15" },
					};
				}
			}

			//このブロックのID情報
			public ModBlockId ThisModBlockId;

			//2個目の交換ボタン等
			public MMenu MExchangeMenu;
			public MToggle MExchangeToggle;
			
			//交換後のmodブロック
			public Block newMblock;
			public BlockBehaviour newMBB;

			public List<BlockType> Guns
			{
				get
				{
					return new List<BlockType>
					{
						BlockType.Cannon,
						BlockType.ShrapnelCannon,
						BlockType.Crossbow,
					};
				}
			}
			public List<BlockType> Plates
			{
				get
				{
					return new List<BlockType>
					{
						BlockType.WingPanel,
						BlockType.Wing,
					};
				}
			}
			public List<BlockType> Beams 
			{
				get
				{
					return new List<BlockType>
					{
					BlockType.Log,
					BlockType.WoodenPole,
					};
				} 
			}
			public List<BlockType> Wheels
            {
                get
                {
					return new List<BlockType>
					{
						BlockType.Wheel,
						BlockType.WheelUnpowered,
						BlockType.LargeWheel,
						BlockType.LargeWheelUnpowered
					};
                }
            }
			public List<BlockType> Intakes
            {
                get
                {
					return new List<BlockType>
					{
						BlockType.WoodenPanel,
						BlockType.ArmorPlateSmall,
						BlockType.ArmorPlateLarge,
						BlockType.ArmorPlateRound,
						BlockType.GripPad
					};
                }
            }
			public List<BlockType> Tanks
            {
                get
                {
					return new List<BlockType>
					{
						BlockType.SingleWoodenBlock,
						BlockType.DoubleWoodenBlock,
						BlockType.Log,
						BlockType.WoodenPole
					};
                }
            }
			public List<BlockType> Boosters
            {
                get
                {
					return new List<BlockType>
					{
						BlockType.Flamethrower,
					};
                }
            }
			public List<string> GunsName
			{
				get
				{
					return new List<string>
					{
						"Cannon",
						"Sharpnel Cannon",
						"Cross Bow",
					};
				}
			}
			public List<string> PlatesName
			{
				get
				{
					return new List<string>
					{
						"Wing Panel",
						"Wing",
					};
				}
			}
			public List<string> BeamsName
			{
				get
				{
					return new List<string>
					{
						"Log",
						"Wooden Pole",
					};
				}
			}
			public List<string> WheelsName
            {
                get
                {
					return new List<string>
					{
						"Wheel",
						"Wheel Unpowered",
						"Large Wheel",
						"Large Wheel Unpowered"
					};
                }
            }
			public List<string> IntakesName
            {
                get
                {
					return new List<string>
					{
						"Wooden Panel",
						"Armor Plate Small",
						"Armor Plate Large",
						"Armor Plate Round",
						"Grip Pad"
					};
                }
            }
			public List<string> TanksName
            {
                get
                {
					return new List<string>
					{
						"Single Wooden Block",
						"Double Wooden Block",
						"Log",
						"Wooden Pole"
					};
                }
            }
			public List<string> BoostersName
            {
                get
                {
					return new List<string>
					{
						"Flame Thrower"
					};
                }
            }
			public override void SetBlockList() //mod機銃が標準 //modブロックの種類によっては書き換わる
			{
				ExchangeList = Guns;
				ExchangeListMenu = GunsName;
				ChangeBlockList(); //標準から書き換えたい場合
			}
			public override void ModBlockSafeAwake() //SafeAwake()中に呼び出される //何度も呼び出されている!? //ここでフリーズする //Groupをコンストラクタで作ってるからだった
			{
				isModBlock = true;
				SetModBlockId();
				ThisModBlockId.SetGroup();

				if (ThisModBlockId.BlockGroup.Count > 0)
				{
					MExchangeMenu = BB.AddMenu("MExchangeMenu", 0, ThisModBlockId.NameGroup);
					MExchangeToggle = BB.AddToggle(isJapanese ? "mod製ブロック交換" : "Exchange (mod Block)", "Exchange (mod Block)", false);
				}			
			}
			public override void ModBlockBuildingUpdate() //Update()中に呼び出される
			{
				if (isModBlock && MExchangeMenu != null && MExchangeToggle != null)
				{
					if (MExchangeToggle.IsActive)
					{
						MExchangeToggle.IsActive = false;
						//ModConsole.Log("UndoSystem.Undo()");
						machine.UndoSystem.Undo();
						//AddBlockはUndoで消えない //AddBlockの中にUndoActionAddが含まれている

						//if (machine.AddBlock(BlockInfo.FromBlockBehaviour(newMBB)))
						newMblock = PM.AddBlock(
							ThisModBlockId.BlockGroup[MExchangeMenu.Value].ModId,
						    ThisModBlockId.BlockGroup[MExchangeMenu.Value].LocalId,
						    BB.transform.localPosition,
						    BB.transform.localRotation,
						    BB.Flipped);
						//UndoやRedoを2回使わないといけない仕様は、たぶんしょうがない
						if (newMblock != null) //条件文でブロックを追加 //たぶんこれがUndoに引っかかってる //PM系のメソッドだとUndoに引っかかる!?
						{
							//ModConsole.Log("TestLog01");
							newMBB = newMblock.InternalObject;
							newMBB.gameObject.transform.localScale = block.GameObject.transform.localScale; //スケールは揃える
							//ModConsole.Log("TestLog02");
							//machine.RemoveBlock(BB);
							PM.RemoveBlock(block);
							//newMBB.VisualController.PlaceFromBlock(newMBB);
							newMBB.VisualController.ReplaceSkin(BB.VisualController.selectedSkin);
							//これでスキンもそのまま移行できる
							//machine.UndoSystem.AddAction(new UndoActionModBlockExchange(machine, BB, newBB));

							newBB = newMblock.InternalObject;
							
							//if (newBB == null) { Debug.Log("ABS : newBB is null !"); } //出ない
							//if (BB == null) { Debug.Log("ABS : BB is null !"); } //かならず出る
							/*
							machine.UndoSystem.AddActions(new List<UndoAction>
							{
								//この辺でFromBlockBehaviourがぬるぽ
								new UndoActionRemove(machine, BlockInfo.FromBlockBehaviour(BB)),
								new UndoActionAdd(machine, BlockInfo.FromBlockBehaviour(newBB)),
							});
							*/
						}
						/*
						if (machine.AddBlock(BlockInfo.FromBlockBehaviour(newMBB), out blockBehaviour))
						{
							blockBehaviour.VisualController.PlaceFromBlock(newMBB);
						}
						if (machine.GetBlock(guid, out block))
						{
							machine.RemoveBlock(block);
						}
						public void ChangeBlock(BlockType BlockID) //指定したブロックと交換する
						{
							if (machine.AddBlock(new BlockInfo
							{
								ID = BlockID,
								Position = BB.transform.localPosition,
								Rotation = BB.transform.localRotation,
								Scale = BB.transform.localScale,
								Flipped = BB.Flipped,
								Skin = BB.VisualController.selectedSkin,
								BlockData = new XDataHolder()
							}, out newBB))
							{
								newBB.VisualController.PlaceFromBlock(newBB);
								machine.RemoveBlock(BB);
							}
							machine.UndoSystem.AddAction(new UndoActionExchange(machine, BlockInfo.FromBlockBehaviour(BB), BlockInfo.FromBlockBehaviour(newBB)));
						}
						*/
					}
				}
			}
			/*
			public class UndoActionModBlockExchange : UndoAction
			{
				BlockBehaviour BB;
				BlockBehaviour newMBB;
				Block b;
				Block newMB;

				Guid newGuid;

				public UndoActionModBlockExchange(Machine m, BlockBehaviour BB, BlockBehaviour newMBB)
				{
					Debug.Log("UndoActionModBlockExchange : call");
					this.BB = BB;
					this.newMBB = newMBB;
					b = Block.From(this.BB);
					newMB = Block.From(this.newMBB);
					changesCount = true;
					machine = m;

					guid = BB.Guid;
					newGuid = newMBB.Guid;

					Debug.Log("newMBB : " + this.newMBB.name);
				}
				public override bool Redo()
				{
					ModConsole.Log("Redo");
					BlockBehaviour block;
					BlockBehaviour blockBehaviour;
					if (machine.AddBlock(BlockInfo.FromBlockBehaviour(newMBB), out blockBehaviour))
					{
						blockBehaviour.VisualController.PlaceFromBlock(newMBB);
					}
					if (machine.GetBlock(guid, out block))
					{
						machine.RemoveBlock(block);
					}
					return true;
				}
				public override bool Undo() //何らかのnullがある
				{
					ModConsole.Log("Undo");
					BlockBehaviour blockBehaviour;
					BlockBehaviour block;
					if (machine.AddBlock(BlockInfo.FromBlockBehaviour(BB), out blockBehaviour))
					{
						blockBehaviour.VisualController.PlaceFromBlock(BB);
					}
					/*
					if (machine.GetBlock(newGuid, out block))
					{
						machine.RemoveBlock(block);
					}
					
					Debug.Log("元のブロック設置完了");
					machine.RemoveBlock(newMBB);
					Debug.Log("今のブロック消去完了");
					return true;
				}
			}
			*/
			/*
			 public class UndoActionExchange : UndoAction //AddとRemoveを組み合わせた形 //スケールやスキンも引き継ぐ //ステータスをコピペする
			{
				BlockInfo newInfo;
				Guid newGuid;

				public UndoActionExchange(Machine m, BlockInfo preInfo, BlockInfo newInfo)
				{
					Debug.Log("UndoActionExchange : called");
					info = preInfo;
					this.newInfo = newInfo;
					guid = preInfo.Guid;
					newGuid = newInfo.Guid;
					changesCount = true;
					machine = m;
				}
				public override bool Redo()
				{
					//ModConsole.Log("Redo");
					BlockBehaviour block;
					BlockBehaviour blockBehaviour;
					Debug.Log("Redo : start");
					if (machine.AddBlock(newInfo, out blockBehaviour))
					{
						blockBehaviour.VisualController.PlaceFromBlockInfo(newInfo);
					}
					Debug.Log("Redo : mid");
					if (machine.GetBlock(guid, out block))
					{
			machine.RemoveBlock(block);
					}
		Debug.Log("Redo : end");
					return true;
				}
	public override bool Undo()
	{
		//ModConsole.Log("Undo");
		BlockBehaviour blockBehaviour;
		BlockBehaviour block;
		Debug.Log("Undo : start");
		if (machine.AddBlock(info, out blockBehaviour))
		{
			blockBehaviour.VisualController.PlaceFromBlockInfo(info);
		}
		Debug.Log("Undo : mid");
		if (machine.GetBlock(newGuid, out block))
		{
			machine.RemoveBlock(block);
		}
		Debug.Log("Undo : end");
		return true;
	}
			 */

			public abstract void SetModBlockId(); //IdOfThisを初期化する //IdOfThis = new ModBlockId(hoge, hoge);
			public virtual void ChangeBlockList() { } //変換できるバニラブロックを書き換えたい場合にオーバーライドする
		}
		namespace ModBlocks
		{
			#region  オトカム砲
			public class SimpleMachinegunScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(SimpleMachinegunGuid, 1);
				}
			}
			public class SimpleMachinegunEXScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(SimpleMachinegunGuid, 2);
				}
			}
			public class SimpleMachinegunAPScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(SimpleMachinegunGuid, 3);
				}
			}
			public class AutoCannonScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(SimpleMachinegunGuid, 4);
				}
			}
			public class FlakCannonScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(SimpleMachinegunGuid, 5);
				}
			}
			#endregion

			#region スケピン砲ver1
			public class BBv1_001hScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(BattleBullet1Guid, 1);
				}
			}
			public class BBv1_001vScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(BattleBullet1Guid, 3);
				}
			}
			public class BBv1_002hScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(BattleBullet1Guid, 2);
				}
			}
			public class BBv1_002vScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(BattleBullet1Guid, 4);
				}
			}
			#endregion

			#region スケピン砲ver2
			public class BBv2_001hScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(BattleBullet2Guid, 10);
				}
			}
			public class BBv2_001vScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(BattleBullet2Guid, 11);
				}
			}
			public class BBv2_002hScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(BattleBullet2Guid, 15);
				}
			}
			public class BBv2_002vScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(BattleBullet2Guid, 16);
				}
			}
			#endregion

			#region Battle Missile
			public class Missile001Script : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(BattleMissileGuid, 10);
				}
			}
			#endregion

			#region Guided Missile
			public class ITANOScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(GuidedMissileGuid, 10);
				}
			}
			public class HexapodScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(GuidedMissileGuid, 11);
				}
			}
			public class TwinpodScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(GuidedMissileGuid, 12);
				}
			}
			public class RapierScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(GuidedMissileGuid, 13);
				}
			}
			public class ChaffLauncherScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(GuidedMissileGuid, 20);
				}
			}
			#endregion

			#region 船mod
			public class ScrewScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(CruiserGuid, 1);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.Wheel,
						BlockType.CogMediumPowered,
						BlockType.SpinningBlock,
						BlockType.Drill,
						BlockType.CircularSaw,
					};
						ExchangeListMenu = new List<string>
					{
						"Wheel",
						"Powered Cog",
						"Spinning Block",
						"Drill",
						"Circular Saw",
					};
				}
			}
			public class BouyScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(CruiserGuid, 2);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.Ballast,
						BlockType.Balloon,
					};
					ExchangeListMenu = new List<string>
					{
						"Ballast",
						"Balloon",
					};
				}
			}
			public class Beam7Script : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(CruiserGuid, 3);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Beams;
					ExchangeListMenu = BeamsName;
				}
			}
			public class Beam5Script : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(CruiserGuid, 4);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Beams;
					ExchangeListMenu = BeamsName;
				}
			}
			public class Plate5x5Script : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(CruiserGuid, 5);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class Plate3x5Script : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(CruiserGuid, 6);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class Plate3x3Script : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(CruiserGuid, 7);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			#endregion

			#region 板mod
			public class PlateScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(ItaGuid, 1);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class HalfPlateScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(ItaGuid, 2);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class BeamScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(ItaGuid, 3);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Beams;
					ExchangeListMenu = BeamsName;
				}
			}
			public class ThinPlateScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(ItaGuid, 4);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class HalfBeamScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(ItaGuid, 5);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Beams;
					ExchangeListMenu = BeamsName;
				}
			}
			public class DoubleBeamScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(ItaGuid, 6);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Beams;
					ExchangeListMenu = BeamsName;
				}
			}
			public class ThinHalfPlateScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(ItaGuid, 7);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class SmallPlateScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(ItaGuid, 8);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class CoreCubeScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(ItaGuid, 9);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = new List<BlockType>
					{
						BlockType.Ballast,
					};
					ExchangeListMenu = new List<string>
					{
						"Ballast",
					};
				}
			}
            #endregion

            #region CU
            public class ThrusterScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(CUGuid, 1);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Boosters;
					ExchangeListMenu = BoostersName;
				}
			}
			#endregion

			#region  LaserWeapons
			public class Laser001Script : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(LaserWeaponsGuid, 10);
				}
			}
			public class Laser002Script : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(LaserWeaponsGuid, 11);
				}
			}
			public class Laser001vScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(LaserWeaponsGuid, 15);
				}
			}
			public class Laser002vScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(LaserWeaponsGuid, 16);
				}
			}
			public class Distoray3Script : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(LaserWeaponsGuid, 20);
				}
			}
			public class Distoray3vScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(LaserWeaponsGuid, 25);
				}
			}
			public class LightChaffScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(LaserWeaponsGuid, 40);
				}
			}
			#endregion

			#region E砲
			public class L44APFSDS_120mmScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(EsTankCannonGuid, 1);
				}
			}
			public class AntennaScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(EsTankCannonGuid, 3);
				}
			}
			public class L44HEATFS_120mmScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(EsTankCannonGuid, 4);
				}
			}
			public class L74AntiAir_88mmScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(EsTankCannonGuid, 5);
				}
			}
			public class L74APCR_88mmScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(EsTankCannonGuid, 6);
				}
			}
			public class L46HE_45mmScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(EsTankCannonGuid, 7);
				}
			}
			public class L46AP_45mmScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(EsTankCannonGuid, 8);
				}
			}
			public class L23HE_152mmScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(EsTankCannonGuid, 9);
				}
			}
			public class MedievalCannonScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(EsTankCannonGuid, 10);
				}
			}
			public class L65HE_20mmScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(EsTankCannonGuid, 11);
				}
			}
			public class L48HE_75mmScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(EsTankCannonGuid, 12);
				}
			}
			public class L48AP_75mmScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(EsTankCannonGuid, 13);
				}
			}
			public class RedBigFlagScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(EsTankCannonGuid, 14);
				}
			}
			public class BlueBigFlagScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(EsTankCannonGuid, 15);
				}
			}
			public class RedSmallFlagScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(EsTankCannonGuid, 16);
				}
			}
			public class BlueSmallFlagScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(EsTankCannonGuid, 17);
				}
			}
			public class L24HEAT_75mmScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(EsTankCannonGuid, 18);
				}
			}
			public class MachineGunHE_127mmScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(EsTankCannonGuid, 19);
				}
			}
			public class MachineGunT_127mmScript : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(EsTankCannonGuid, 20);
				}
			}
			#endregion

			#region Analog Controllable Blocks
			public class AnalogHingeScript : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(AnalogControllableBlocksGuid, 10);
                }
				public override void ChangeBlockList()
				{
					ExchangeList = new List<BlockType> { BlockType.SteeringHinge };
					ExchangeListMenu = new List<string> { "Steering Hinge" };
				}
			}
			public class Motor01Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(AnalogControllableBlocksGuid, 20);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Wheels;
					ExchangeListMenu = WheelsName;
				}
			}
			public class AnalogWheel_BaseScript : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(AnalogControllableBlocksGuid, 40);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Wheels;
					ExchangeListMenu = WheelsName;
				}
			}
			public class LargeWheel_Tire01Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(AnalogControllableBlocksGuid, 140);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Wheels;
					ExchangeListMenu = WheelsName;
				}
			}
			public class LargeWheel_Tire02Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(AnalogControllableBlocksGuid, 141);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Wheels;
					ExchangeListMenu = WheelsName;
				}
			}
			public class LargeWheel_Tire03Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(AnalogControllableBlocksGuid, 142);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Wheels;
					ExchangeListMenu = WheelsName;
				}
			}
			#endregion

			#region 構造材
			public class NoriRodL3Script : ModBlockExchangerScript
            {
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 1);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Beams;
					ExchangeListMenu = BeamsName;
				}
			}
			public class NoriRodL5Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 2);
                }
				public override void ChangeBlockList()
				{
					ExchangeList = Beams;
					ExchangeListMenu = BeamsName;
				}
			}
			public class NoriRodL8Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 3);
                }
				public override void ChangeBlockList()
				{
					ExchangeList = Beams;
					ExchangeListMenu = BeamsName;
				}
			}
			public class NoriRodL12Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 4);
                }
				public override void ChangeBlockList()
				{
					ExchangeList = Beams;
					ExchangeListMenu = BeamsName;
				}
			}
			public class NoriRodL17Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 5);
                }
				public override void ChangeBlockList()
				{
					ExchangeList = Beams;
					ExchangeListMenu = BeamsName;
				}
			}
			public class NoriRodL23Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 6);
                }
				public override void ChangeBlockList()
				{
					ExchangeList = Beams;
					ExchangeListMenu = BeamsName;
				}
			}
			public class NoriRodL30Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 7);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Beams;
					ExchangeListMenu = BeamsName;
				}
			}
			public class NoriBoardS3Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 11);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardS5Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 12);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardS8Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 13);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardS12Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 14);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardS17Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 15);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardS23Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 16);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardS30Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 17);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardO3x5Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 21);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardO3x8Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 22);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardO5x8Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 23);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardO3x12Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 24);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardO3x17Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 25);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardO3x23Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 26);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardO3x30Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 27);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardO5x12Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 32);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardO5x17Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 33);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardO5x23Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 34);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardO5x30Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 35);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardO8x12Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 41);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardO8x17Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 42);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardO8x23Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 43);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardO8x30Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 44);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardO12x17Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 51);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardO12x23Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 52);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardO12x30Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 53);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardO17x23Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 61);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardO17x30Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 62);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			public class NoriBoardO23x30Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(ShipsBlocksGuid, 71);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Plates;
					ExchangeListMenu = PlatesName;
				}
			}
			#endregion

			#region Booster And Fuel
			public class Booster01Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(BoosterFuelGuid, 10);
                }
                public override void ChangeBlockList()
                {
					ExchangeList = Boosters;
					ExchangeListMenu = BoostersName;
                }
            }
			public class Booster02Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(BoosterFuelGuid, 11);
                }
                public override void ChangeBlockList()
                {
					ExchangeList = Boosters;
					ExchangeListMenu = BoostersName;
                }
            }
			public class Booster03Script : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(BoosterFuelGuid, 12);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Boosters;
					ExchangeListMenu = BoostersName;
				}
			}
			public class Booster04Script : ModBlockExchangerScript
			{
				public override void SetModBlockId()
				{
					ThisModBlockId = new ModBlockId(BoosterFuelGuid, 13);
				}
				public override void ChangeBlockList()
				{
					ExchangeList = Boosters;
					ExchangeListMenu = BoostersName;
				}
			}
			public class Intake01Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(BoosterFuelGuid, 20);
                }
                public override void ChangeBlockList()
                {
					ExchangeList = Intakes;
					ExchangeListMenu = IntakesName;
                }
            }
			public class Tank01Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(BoosterFuelGuid, 30);
                }
                public override void ChangeBlockList()
                {
					ExchangeList = Tanks;
					ExchangeListMenu = TanksName;
                }
            }
			public class Tank02Script : ModBlockExchangerScript
            {
                public override void SetModBlockId()
                {
					ThisModBlockId = new ModBlockId(BoosterFuelGuid, 31);
                }
                public override void ChangeBlockList()
                {
					ExchangeList = Tanks;
					ExchangeListMenu = TanksName;
                }
            }
			#endregion
		}
    }
}

