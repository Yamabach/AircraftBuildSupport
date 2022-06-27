using System;
using System.Collections.Generic;
using Modding;
using Modding.Blocks;
using UnityEngine;
using ABSspace.BlockGrouping;
using spaar.ModLoader.UI;

namespace ABSspace
{
	namespace CoLController
	{
		public class CoLGUI : SingleInstance<CoLGUI> //重心表示でブレースの回転がおかしくなるバグがある
		{
			Rect windowRect = new Rect(0, 80, 200, 200);
			int fontSize = 16;
			float labelHeight = 18f;
			public override string Name
            {
                get
                {
					return "CoL GUI";
                }
            }
			private bool hide = false;
			public bool ShowAxis = false; //GUIのトグル用
			public bool ShowAxisInSimulating = false; //シミュ中に軸を表示するかどうか
			public static bool ConsiderBuildSurface = false; //サーフェスの空気抵抗を考慮するかどうか
			public bool ShowTransform = false; //詳細なトランスフォームを表示するかどうか
			public bool ShowCoM = false; //真の重心表示
			public bool ShowCoMInSimulating = false; //シミュ中に重心を表示
			public float velocity = 0f; //マシンの速さ（m/s）
			public bool VelocityUnitChange = false;
			public int CurrentUnit = 0;
			public static BlockBehaviour picked;
			public static readonly string[] UnitName = new string[]
			{
				"m/s", 
				"km/h", 
				"kt", 
				"Mach",
			};
			public static readonly float[] Coefficient = new float[]
			{
				1f,
				3.6f,
				1.9438f,
				0.00293866987f,
			};
			public BlockBehaviour StartingBlock;
			private int windowId;
			public bool ShowMissileCost = false;
			public bool EnableAxis
			{
				internal set { }
				get
				{
					return ShowAxis && !StatMaster.isMainMenu && (!Game.IsSimulating || ShowAxisInSimulating);
				}
			}
			public bool EnableCoM
			{
				internal set { }
				get
				{
					return ShowCoM && !StatMaster.isMainMenu && (!Game.IsSimulating || ShowCoMInSimulating);
				}
			}
			public bool isJapanese;
			public PAxis PAxis { private set; get; }
			public YAxis YAxis { private set; get; }
			public RAxis RAxis { private set; get; }
			public MassAxis MAxis { private set; get; }
			public bool canDrag = true; // GUIをドラッグするかどうか
			public bool DisappearInSim = false; // シミュ中にUIを消すかどうか

			//プロペラの空力表示系
			public bool ShowLiftVectors = false;
			public bool IsGlobal = false;

			public void Awake()
			{
				PAxis = gameObject.GetComponent<PAxis>() ?? gameObject.AddComponent<PAxis>();
				YAxis = gameObject.GetComponent<YAxis>() ?? gameObject.AddComponent<YAxis>();
				RAxis = gameObject.GetComponent<RAxis>() ?? gameObject.AddComponent<RAxis>();
				MAxis = gameObject.GetComponent<MassAxis>() ?? gameObject.AddComponent<MassAxis>();

				isJapanese = Mod.isJapanese;
				windowId = ModUtility.GetWindowId();
			}
			public void Start()
            {
				ModConsole.RegisterCommand("abs-drag-window", new CommandHandler(CanDragWindow), 
					"Turn on/off whether you can drag GUI window. If the value is true or non-zero, it turns on. If it's false or zero, it turns off.");
				ModConsole.RegisterCommand("abs-disappear-in-sim", new CommandHandler(DisappearsInSimulation),
					"GUI dissapears in simulation. If the value is true or non-zero, it turns on. If it's false or zero, it turns off.");
			}
			public void Update()
			{
				if (!StatMaster.isMainMenu && Input.GetKeyDown(KeyCode.Tab))
				{
					hide = !hide;
				}
				if (VelocityUnitChange)
				{
					CurrentUnit++;
					if (CurrentUnit >= UnitName.Length)
					{
						CurrentUnit = 0;
					}
				}
			}
			public void FixedUpdate()
			{
				PAxis.axis.enabled = EnableAxis;
				YAxis.axis.enabled = EnableAxis;
				RAxis.axis.enabled = EnableAxis;
				for (int i=0; i<3; i++)
				{
					MAxis.Axis[i].enabled = EnableCoM;
				}
				StartingBlock = Mod.GetStartingBlock();

				//速度計測関係
				if (((StatMaster.isMP && StatMaster.isHosting) || !StatMaster.isMP) && StartingBlock != null) //SP時または鯖主時
				{
					velocity = StartingBlock.Rigidbody.velocity.magnitude;
				}
				else if ((StatMaster.isMP && StatMaster.isClient && StartingBlock != null)) // クライアント時
                {
					velocity = StartingBlock.Rigidbody.velocity.magnitude; // ここがnull get_velocityができない
                }
                else
                {
					velocity = 0f;
                }
			}
			
			public void OnGUI()
			{
				if (Game.IsSimulating && DisappearInSim)
                {
					return;
                }
				GUI.skin = ModGUI.Skin;
				GUI.skin.label.fontSize = fontSize;
				if (!StatMaster.isMainMenu && !StatMaster.inMenu && !hide && Machine.Active() != null)
				{
					windowRect.height = 200 + labelHeight * ((ShowAxis ? 1 : 0f) + (ShowCoM ? 1 : 0f) + (ShowTransform ? 4 : 0f) + (ShowMissileCost ? 1 : 0f));
					//windowRect = GUILayout.Window(windowId, windowRect, AxisMapper, "ABS 1.3.0 Lennon"); //Lennonは1.3のコードネーム
					//windowRect = GUILayout.Window(windowId, windowRect, AxisMapper, "ABS 1.5.0 McCartney"); //McCarteneyは1.4のコードネーム
					//windowRect = GUILayout.Window(windowId, windowRect, AxisMapper, "ABS 1.6.1 Harrison"); //Harrisonは1.6のコードネーム
					//windowRect = GUILayout.Window(windowId, windowRect, AxisMapper, "ABS 1.6.2 Harrison" + (Mod.Extended ? " Extended" : "")); //Harrisonは1.6のコードネーム
					windowRect = GUILayout.Window(windowId, windowRect, AxisMapper, "ABS " + Mods.GetVersion(new Guid("a7f2f9ae-e11f-41ff-a5dd-28ab14eaa6a2")).ToString() + " Starr" + (Mod.Extended ? " Extended" : "")); //Starrは1.7のコードネーム
					//windowRect = GUILayout.Window(windowId, windowRect, AxisMapper, 
					//	"ABS " + Mods.GetVersion(new Guid("a7f2f9ae-e11f-41ff-a5dd-28ab14eaa6a2")).ToString() + " Preston" + (Mod.Extended ? " Extended" : "")); //Prestonは1.13のコードネーム
				}
			}
			public void AxisMapper(int windowId)
			{
				ToggleIndent(isJapanese ? "空力中心軸を表示" : "Show lift resistance axis", 10f, ref ShowAxis, delegate
				{
					GUILayout.BeginHorizontal();
					GUILayout.Label(isJapanese ? "シミュ中も軸を表示" : "Show axis during simulation");
					GUILayout.FlexibleSpace();
					ShowAxisInSimulating = GUILayout.Toggle(ShowAxisInSimulating, "    ");
					GUILayout.EndHorizontal();
				});
				ToggleIndent(isJapanese ? "重心を表示" : "Show center of mass", 10f, ref ShowCoM, delegate
				{
					GUILayout.BeginHorizontal();
					GUILayout.Label(isJapanese ? "シミュ中も重心を表示" : "Show CoM during simulation");
					GUILayout.FlexibleSpace();
					ShowCoMInSimulating = GUILayout.Toggle(ShowCoMInSimulating, "    ");
					GUILayout.EndHorizontal();
				});
				GUILayout.BeginHorizontal();
				GUILayout.Label(isJapanese ? "プロペラの空気抵抗ベクトルを表示" : "Show the Propeller's lift resistance vector");
				GUILayout.FlexibleSpace();
				ShowLiftVectors = GUILayout.Toggle(ShowLiftVectors, "    ");
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				{
					GUILayout.Label(isJapanese ? "マシン重量" : "Machine total mass");
					GUILayout.Label(Machine.Active().Mass.ToString());
				}
				GUILayout.EndHorizontal();
				
				GUILayout.BeginHorizontal();
				{
					GUILayout.Label(isJapanese ? "スタブロ速さ" : "Speed");
					GUILayout.FlexibleSpace();
					GUILayout.Label(Game.IsSimulating ? ((float)Math.Round(velocity * Coefficient[CurrentUnit], 1)).ToString() : "0.0");
					VelocityUnitChange = GUILayout.Button(UnitName[CurrentUnit]);
				}
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				{
					GUILayout.Label(isJapanese ? "プロペラ枚数" : "Propellers");
					GUILayout.FlexibleSpace();
					GUILayout.Label(NumOfPropellers().ToString()); //長短プロペラの枚数
				}
				GUILayout.EndHorizontal();

				ToggleIndent(isJapanese ? "トランスフォームを表示" : "Show Transform", 10f, ref ShowTransform, delegate
				{
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label(isJapanese ? "選択中のブロック" : "Picked Block");
						GUILayout.FlexibleSpace();
						GUILayout.Label(picked == null ? "-" : picked.name);
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label(isJapanese ? "位置" : "Position"); GUILayout.FlexibleSpace(); GUILayout.Label(picked != null ? picked.gameObject.transform.position.ToString() : "(-, -, -)");
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label(isJapanese ? "姿勢" : "Rotation"); GUILayout.FlexibleSpace(); GUILayout.Label(picked != null ? picked.gameObject.transform.rotation.eulerAngles.ToString() : "(-, -, -)");
					}
					GUILayout.EndHorizontal();
					GUILayout.BeginHorizontal();
					{
						GUILayout.Label(isJapanese ? "縮尺" : "Local Scale"); GUILayout.FlexibleSpace(); GUILayout.Label(picked != null ? picked.gameObject.transform.localScale.ToString() : "(-, -, -)");
					}
					GUILayout.EndHorizontal();
				});

				ShowMissileCost =
					Mods.IsModLoaded(new Guid("90a17943-af2a-40ea-a51f-530553b9fcb0")) //誘導ミサイルmodがロードされている場合
					|| Mods.IsModLoaded(new Guid("15259577-bd19-4397-8646-88d235cf8d24")); //LaserWeapons
				if (ShowMissileCost)
				{
					GUILayout.BeginHorizontal();
					GUILayout.Label(isJapanese ? "誘導ミサイルコスト" : "Missile Cost");
					GUILayout.FlexibleSpace();
					GUILayout.Label(GMissileCost().ToString()); //誘導ミサイルのコスト
					GUILayout.EndHorizontal();
				}
				
				if (canDrag)
				{
					GUI.DragWindow();
				}
			}
			private void ToggleIndent(string text, float width, ref bool flag, Action func) //ACMより移植 インデントを付ける
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(text);
				GUILayout.FlexibleSpace();
				flag = GUILayout.Toggle(flag, "    ", new GUILayoutOption[0]);
				GUILayout.EndHorizontal();
				if (flag)
				{
					GUILayout.BeginHorizontal(new GUILayoutOption[0]);
					GUILayout.Label("", new GUILayoutOption[]
					{
						GUILayout.Width(width)
					});
					GUILayout.BeginVertical(new GUILayoutOption[0]);
					func();
					GUILayout.EndVertical();
					GUILayout.EndHorizontal();
				}
			}
			
			public Dictionary<string, int> MissileCostDict = new Dictionary<string, int>
			{
				//誘導ミサイル
				{
					"90a17943-af2a-40ea-a51f-530553b9fcb0-10", //ITANO
					2
				},

				{
					"90a17943-af2a-40ea-a51f-530553b9fcb0-11", //HEXAPOD
					3
				},
				{
					"90a17943-af2a-40ea-a51f-530553b9fcb0-12", //TWINPOD
					1
				},
				{
					"90a17943-af2a-40ea-a51f-530553b9fcb0-13", //RAPIER
					2
				},
				{
					"15259577-bd19-4397-8646-88d235cf8d24-20", //Distoray3
					2
				},
				{
					"15259577-bd19-4397-8646-88d235cf8d24-25", //Distoray3v
					2
				},
			};
			public int GMissileCost() //誘導ミサイルのコスト
			{
				int cost = 0;
				Machine machine = Machine.Active();
				List<BlockBehaviour> BlockList = machine.BuildingBlocks;
				foreach (BlockBehaviour BB in BlockList)
				{
					if (MissileCostDict.ContainsKey(BB.name.ToString()))
					{
						cost += MissileCostDict[BB.name.ToString()];
					}
				}
				return cost;
			}
			public int NumOfPropellers() //長短プロペラ枚数の表示
			{
				int ret = 0;
				Machine machine = Machine.Active();
				foreach (BlockBehaviour BB in machine.BuildingBlocks)
				{
					if (BB.BlockID == (int)BlockType.Propeller || BB.BlockID == (int)BlockType.SmallPropeller)
					{
						ret++;
					}
				}
				return ret;
			}
			private void CanDragWindow(params string[] args)
            {
				if (args.Length > 2)
                {
					Mod.LogError("The number of arguments exceeds the usage!");
					Debug.Log("Usage : abs-drag-window <bool>");
					return;
                }
				else if (args.Length == 1)
                {
					bool b_value;
					if (bool.TryParse(args[0], out b_value))
                    {
						canDrag = b_value;
						Mod.Log(string.Format("Drag Window = {0}", canDrag.ToString()));
						return;
                    }
					int i_value;
					if (int.TryParse(args[0], out i_value))
                    {
						canDrag = i_value != 0;
						Mod.Log(string.Format("Drag Window = {0}", canDrag.ToString()));
						return;
                    }
				}
				else if (args.Length == 0)
                {
					canDrag = !canDrag;
					Mod.Log(string.Format("Drag Window = {0}", canDrag.ToString()));
					return;
                }
                else
				{
					Debug.Log("Usage : abs-drag-window <bool>");
				}
			}
			private void DisappearsInSimulation(params string[] args)
            {
				if (args.Length > 2)
				{
					Mod.LogError("The number of arguments exceeds the usage!");
					Debug.Log("Usage : abs-drag-window <bool>");
					return;
				}
				else if (args.Length == 1)
				{
					bool b_value;
					if (bool.TryParse(args[0], out b_value))
					{
						DisappearInSim = b_value;
						Mod.Log(string.Format("Disappears in Simulation = {0}", DisappearInSim.ToString()));
						return;
					}
					int i_value;
					if (int.TryParse(args[0], out i_value))
					{
						DisappearInSim = i_value != 0;
						Mod.Log(string.Format("Disappears in Simulation = {0}", DisappearInSim.ToString()));
						return;
					}
				}
				else if (args.Length == 0)
				{
					DisappearInSim = !DisappearInSim;
					Mod.Log(string.Format("Disappears in Simulation = {0}", DisappearInSim.ToString()));
					return;
				}
                else
				{
					Debug.Log("Usage : abs-drag-window <bool>");
				}
			}
		}
		public abstract class AbstractAiroAxis : MonoBehaviour //継承用のひな型
		{
			public LineRenderer axis;
			public const float LengthOfAxis = 20; //線分の長さの半分
			public Vector3 ProtoEndsVector { internal set; get; } = Vector3.zero;
			public Vector3 EndsVector
			{
				get
				{
					if (StartingBlockBuild == null || StartingBlockSimulation == null)
					{
						return ProtoEndsVector;
					}
					//シミュ中とビルド中のスタブロの角度の差を取り、その差の角度でProtoEndsVectorを回転させる
					return (StartingBlockSimulation.transform.rotation * Quaternion.Inverse(StartingBlockBuild.transform.rotation)) * ProtoEndsVector;
					
					//return machine.transform.localRotation * ProtoEndsVector;
				}
			}
			public Machine machine
            {
                get
                {
					return Machine.Active();
                }
            }
			public GameObject AxisController;
			protected Vector3 numerator;
			protected float denominator;
			protected AxialDrag component;
			public BlockBehaviour StartingBlockSimulation; //シミュ中のスタブロ
			public BlockBehaviour StartingBlockBuild; //ビルド中のスタブロ

			public bool ShowAxis = false;

			private List<BlockBehaviour> BlockList //シミュ中とビルド中でブロックが切り替わる対策
			{
				get
				{
					if (!StatMaster.isMainMenu && Game.IsSimulating)
					{
						return machine.SimulationBlocks;
					}
					else
					{
						return machine.BuildingBlocks;
					}
				}
			}

			public virtual void Start()
			{
				AxisController = new GameObject("AeroAxis");
				AxisController.transform.parent = SingleInstance<CoLGUI>.Instance.transform;
				axis = AxisController.gameObject.GetComponent<LineRenderer>() ?? AxisController.gameObject.AddComponent<LineRenderer>();
				axis.SetWidth(0.1f, 0.1f);
				axis.GetComponent<Renderer>().material.color = Color.white;
			}
			public void FixedUpdate()
			{
				if (machine == null)
                {
					return;
                }
				if (!StatMaster.isMainMenu && axis.enabled)
				{
					SetAxisEnds();
				}
				if (Game.IsSimulating)
				{
					
					StartingBlockSimulation = GetStartingBlock();
				}
				else
				{
					if (StartingBlockBuild == null)
					{
						StartingBlockBuild = GetStartingBlock();
					}
				}
			}
			
			public BlockBehaviour GetStartingBlock() //スタブロを参照するのはちょっと危ないかも？（マルチとか）
			{
				foreach(BlockBehaviour BB in BlockList)
				{
					if (BB.BlockID == 0) //スタブロを見つけたら
					{
						return BB;
					}
				}
				return null; //見つからなかったら
			}
			
			public void SetAxisEnds() //線分の中心の点を決めて、それをもとに端点を指定する
			{
				numerator = Vector3.zero;
				denominator = 0f;
				foreach (BlockBehaviour current in BlockList)
				{
					component = current.GetComponent<AxialDrag>();
					if (!current.noRigidbody && component != null) //Rigidbodyがあり、かつAxialDragがnullでないとき、則ち空力系ブロックの時
					{
						numerator += current.GetCenter() * Math.Abs(CalculateRotation(current));
						//numerator += current.Rigidbody.worldCenterOfMass * Math.Abs(CalculateRotation(current));
						denominator += Math.Abs(CalculateRotation(current));
					}
					/*
					else if (current.BlockID == (int)BlockType.BuildSurface && CoLGUI.ConsiderBuildSurface) //サーフェスの時
					{
						BuildSurface BS = (BuildSurface)current; //サーフェスのノードで空気抵抗が発生する。どの方向に発生するかがわからない
						if (BS.currentType.hasAerodynamics)
						{
							foreach(BuildNodeBlock node in BS.nodes)
							{
								numerator += node.GetCenter() * Math.Abs(CalculateRotation(node, BS));
								denominator += Math.Abs(CalculateRotation(node, BS));
							}
						}
					}
					*/
				}
				if (denominator != 0f)
				{
					numerator /= denominator;
				}
				else
				{
					numerator = machine.GetBounds().center;
				}
				axis.SetPosition(0, numerator + EndsVector);
				axis.SetPosition(1, numerator - EndsVector);
			}
			public abstract float CalculateRotation(BlockBehaviour current); //抵抗の強さ（ベクトル）を回転させて射影を得る
			public abstract float CalculateRotation(BuildNodeBlock node, BuildSurface surface); //ここがわからない
		}
		public class PAxis : AbstractAiroAxis
		{
			public void Awake()
			{
				//ModConsole.Log("PAxis is awaken.");
				ProtoEndsVector = new Vector3(LengthOfAxis, 0, 0);
			}
			public override void Start()
			{
				base.Start();
				axis.GetComponent<Renderer>().material.color = Color.red;
			}
			public override float CalculateRotation(BlockBehaviour current)
			{
				return (current.transform.rotation * current.GetComponent<AxialDrag>().AxisDrag).y;
			}
			public override float CalculateRotation(BuildNodeBlock node, BuildSurface surface)
			{
				return surface.currentType.dragMultiplier * (node.transform.rotation * Vector3.up).y;
			}
		}
		public class YAxis : AbstractAiroAxis
		{
			public void Awake()
			{
				//ModConsole.Log("YAxis is awaken.");
				ProtoEndsVector = new Vector3(0, LengthOfAxis, 0);
			}
			public override void Start()
			{
				base.Start();
				axis.GetComponent<Renderer>().material.color = Color.green;
			}
			public override float CalculateRotation(BlockBehaviour current)
			{
				return (current.transform.rotation * current.GetComponent<AxialDrag>().AxisDrag).x;
			}
			public override float CalculateRotation(BuildNodeBlock node, BuildSurface surface)
			{
				return surface.currentType.dragMultiplier * (node.transform.rotation * Vector3.up).x;
			}
		}
		public class RAxis : AbstractAiroAxis
		{
			public void Awake()
			{
				//ModConsole.Log("RAxis is awaken.");
				ProtoEndsVector = new Vector3(0, 0, LengthOfAxis);
			}
			public override void Start()
			{
				base.Start();
				axis.GetComponent<Renderer>().material.color = Color.blue;
			}
			public override float CalculateRotation(BlockBehaviour current)
			{
				return (current.transform.rotation * current.GetComponent<AxialDrag>().AxisDrag).z;
			}
			public override float CalculateRotation(BuildNodeBlock node, BuildSurface surface)
			{
				return surface.currentType.dragMultiplier * (node.transform.rotation * Vector3.up).y;
			}
		}
		public class MassAxis : MonoBehaviour
		{
			public LineRenderer[] Axis = new LineRenderer[3];

			public const float LengthOfAxis = 20; //線分の長さの半分
			public Vector3[] ProtoEndsVector //x, y, zの順
			{
				internal set { }
				get { return new Vector3[3] { new Vector3(LengthOfAxis, 0, 0), new Vector3(0, LengthOfAxis, 0), new Vector3(0, 0, LengthOfAxis) }; }
			}
			public Vector3 EndsVector(int i) //関数化させる
			{
				if (StartingBlockBuild == null || StartingBlockSimulation == null)
				{
					return ProtoEndsVector[i];
				}
				//シミュ中とビルド中のスタブロの角度の差を取り、その差の角度でProtoEndsVectorを回転させる
				return (StartingBlockSimulation.transform.rotation * Quaternion.Inverse(StartingBlockBuild.transform.rotation)) * ProtoEndsVector[i];
			}
			public Machine machine
            {
                get
                {
					return Machine.Active();
                }
            }
			public GameObject[] AxisController = new GameObject[3];
			protected Vector3 numerator;
			protected float denominator;
			protected AxialDrag component;
			public BlockBehaviour StartingBlockSimulation; //シミュ中のスタブロ
			public BlockBehaviour StartingBlockBuild; //ビルド中のスタブロ

			public bool ShowAxis = false;

			private List<BlockBehaviour> BlockList //シミュ中とビルド中でブロックが切り替わる対策
			{
				get
				{
					if (!StatMaster.isMainMenu && Game.IsSimulating)
					{
						return machine.SimulationBlocks;
					}
					else
					{
						return machine.BuildingBlocks;
					}
				}
			}

			public void Start()
			{
				for (int i = 0; i < 3; i++)
				{
					AxisController[i] = new GameObject("Axis Controller");
					AxisController[i].transform.parent = SingleInstance<CoLGUI>.Instance.transform;

					Axis[i] = AxisController[i].gameObject.GetComponent<LineRenderer>() ?? AxisController[i].gameObject.AddComponent<LineRenderer>();
					Axis[i].SetWidth(0.1f, 0.1f);
					Axis[i].GetComponent<Renderer>().material.color = Color.white;
				}
			}
			public void FixedUpdate()
			{
				if (machine == null)
                {
					return;
                }
				if (!StatMaster.isMainMenu && Axis[0].enabled)
				{
					SetAxisEnds();
				}
				if (Game.IsSimulating)
				{

					StartingBlockSimulation = GetStartingBlock();
				}
				else
				{
					if (StartingBlockBuild == null)
					{
						StartingBlockBuild = GetStartingBlock();
					}
				}
			}

			public BlockBehaviour GetStartingBlock() //スタブロを参照するのはちょっと危ないかも？（マルチとか）
			{
				foreach (BlockBehaviour BB in BlockList)
				{
					if (BB.BlockID == 0) //スタブロを見つけたら
					{
						return BB;
					}
				}
				return null; //見つからなかったら
			}

			public void SetAxisEnds() //線分の中心の点を決めて、それをもとに端点を指定する
			{
				numerator = Vector3.zero;
				denominator = 0f;

				for (int i = 0; i < BlockList.Count; i++)
				{
					BlockBehaviour current = BlockList[i];
					if (!current.noRigidbody && current.BlockID != (int)BlockType.Pin && current.BlockID != (int)BlockType.CameraBlock)
					{
						if (current.Prefab.Type == BlockType.BuildNode || current.Prefab.Type == BlockType.BuildEdge)
						{
							continue;
						}
						if (current.Prefab.Type == BlockType.Brace || current.Prefab.Type == BlockType.RopeWinch || current.Prefab.Type == BlockType.Spring)
						{
							GenericDraggedBlock GDB = current as GenericDraggedBlock;
							numerator += (GDB.startPoint.position + GDB.endPoint.position) / 2 * current.Rigidbody.mass;
							denominator += current.Rigidbody.mass;
						}
						else
						{
							numerator += current.GetCenter() * current.Rigidbody.mass;
							denominator += current.Rigidbody.mass;
						}
					}
				}
				if (denominator != 0f)
				{
					numerator /= denominator;
				}
				else
				{
					numerator = machine.GetBounds().center;
				}
				for (int i=0; i<3; i++)
				{
					Axis[i].SetPosition(0, numerator + EndsVector(i));
					Axis[i].SetPosition(1, numerator - EndsVector(i));
				}
				
			}
		}
	}
}