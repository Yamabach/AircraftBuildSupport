using System;
using System.Collections.Generic;
using Modding;
using Modding.Blocks;
using UnityEngine;
using ABSspace.BlockGrouping;
using spaar.ModLoader.UI;

namespace ABSspace
{
	/// <summary>
	/// 空力中心の表示機能に関する名前空間
	/// </summary>
	namespace CoLController
	{
		/// <summary>
		/// GUI
		/// </summary>
		public class CoLGUI : SingleInstance<CoLGUI>
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
			/// <summary>
			/// GUIを非表示にするかどうか
			/// </summary>
			private bool hide = false;
			/// <summary>
			/// GUIのトグル用
			/// </summary>
			public bool ShowAxis = false;
			/// <summary>
			/// シミュ中に軸を表示するかどうか
			/// </summary>
			public bool ShowAxisInSimulating = false;
			/// <summary>
			/// サーフェスの空気抵抗を考慮するかどうか
			/// </summary>
			public static bool ConsiderBuildSurface = false;
			/// <summary>
			/// 詳細なトランスフォームを表示するかどうか
			/// </summary>
			public bool ShowTransform = false;
			/// <summary>
			/// 真の重心表示
			/// </summary>
			public bool ShowCoM = false;
			/// <summary>
			/// シミュ中に重心を表示
			/// </summary>
			public bool ShowCoMInSimulating = false;
			/// <summary>
			/// 現在のスタブロの位置
			/// </summary>
			public Vector3 position;
			/// <summary>
			/// 1つ前のスタブロの位置
			/// </summary>
			public Vector3 prevPosition;
			/// <summary>
			/// マシンの速さ（m/s）
			/// </summary>
			public float velocity = 0f;
			/// <summary>
			/// マシンの速さの単位を切り替えるボタン用
			/// </summary>
			public bool VelocityUnitChange = false;
			/// <summary>
			/// 現在のマシンの速さの単位のindex
			/// </summary>
			public int CurrentUnit = 0;
			/// <summary>
			/// ブロックの角速度の単位を切り替えるボタン用
			/// </summary>
			public bool AngularVelocityUnitChange = false;
			/// <summary>
			/// 現在のブロックの角速度の単位のindex
			/// </summary>
			public int AngularVelocityCurrentUnit = 0;
			/// <summary>
			/// 現在カーソル上にあるブロック
			/// </summary>
			public BlockController.BlockExchangerScript picked;
			/// <summary>
			/// 単位の名称
			/// </summary>
			public readonly string[] UnitName = new string[]
			{
				"m/s", 
				"km/h", 
				"kt", 
				"Mach",
			};
			/// <summary>
			/// 単位ごとの倍率
			/// </summary>
			public readonly float[] Coefficient = new float[]
			{
				1f,
				3.6f,
				1.9438f,
				0.00293866987f,
			};
			/// <summary>
			/// 単位の名称
			/// </summary>
			public readonly string[] AngularVelocityUnitName = new string[]
			{
				"deg/s",
				"RPS",
				"RPM",
			};
			/// <summary>
			/// 単位ごとの倍率
			/// </summary>
			public readonly float[] AngularVelocityCoefficient = new float[]
			{
				1f,
				0.0027777777778f,
				0.00004629629f,
			};
			/// <summary>
			/// スタブロ
			/// </summary>
			public BlockBehaviour StartingBlock;
			/// <summary>
			/// スタブロの前方の方向を取得するためのスクリプト
			/// </summary>
			public BlockController.VanillaBlocks.StartingBlockScript startingBlockScript;
			/// <summary>
			/// GUIのウィンドウID
			/// </summary>
			private int windowId;
			/// <summary>
			/// 実際に空力中心を表示するかどうか
			/// </summary>
			public bool EnableAxis
			{
				internal set { }
				get
				{
					return ShowAxis && !StatMaster.isMainMenu && (!Game.IsSimulating || ShowAxisInSimulating);
				}
			}
			/// <summary>
			/// 実際に重心を表示するかどうか
			/// </summary>
			public bool EnableCoM
			{
				internal set { }
				get
				{
					return ShowCoM && !StatMaster.isMainMenu && (!Game.IsSimulating || ShowCoMInSimulating);
				}
			}
			/// <summary>
			/// 日本語表示化
			/// </summary>
			public bool isJapanese;
			/// <summary>
			/// 空力中心軸（ピッチ）
			/// </summary>
			public PAxis PAxis { private set; get; }
			/// <summary>
			/// 空力中心軸（ヨー）
			/// </summary>
			public YAxis YAxis { private set; get; }
			/// <summary>
			/// 空力中心軸（ロール）
			/// </summary>
			public RAxis RAxis { private set; get; }
			/// <summary>
			/// 真の重心
			/// </summary>
			public MassAxis MAxis { private set; get; }
			/// <summary>
			/// GUIをドラッグするかどうか
			/// </summary>
			public bool canDrag = true;
			/// <summary>
			/// シミュ中にUIを消すかどうか
			/// </summary>
			public bool DisappearInSim = false;

			/// <summary>
			/// プロペラの空力表示を行うかどうか
			/// </summary>
			public bool ShowLiftVectors = false;
			/// <summary>
			/// プロペラの空力表示をグローバル化するかどうか（NO USE）
			/// </summary>
			public bool IsGlobal = false;

			public void Awake()
			{
				// コンポーネント初期化
				PAxis = GetComponent<PAxis>() ?? gameObject.AddComponent<PAxis>();
				YAxis = GetComponent<YAxis>() ?? gameObject.AddComponent<YAxis>();
				RAxis = GetComponent<RAxis>() ?? gameObject.AddComponent<RAxis>();
				MAxis = GetComponent<MassAxis>() ?? gameObject.AddComponent<MassAxis>();

				isJapanese = Mod.isJapanese;
				windowId = ModUtility.GetWindowId();
			}
			public void Start()
            {
				// 詳細なコマンド設定
				// Config.xmlか何かで制御してあげたほうが良さそう
				ModConsole.RegisterCommand("abs-drag-window", new CommandHandler(CanDragWindow), 
					"Turn on/off whether you can drag GUI window. If the value is true or non-zero, it turns on. If it's false or zero, it turns off.");
				ModConsole.RegisterCommand("abs-disappear-in-sim", new CommandHandler(DisappearsInSimulation),
					"GUI dissapears in simulation. If the value is true or non-zero, it turns on. If it's false or zero, it turns off.");
			}
			public void Update()
			{
				// GUIを隠す
				if (!StatMaster.isMainMenu && Input.GetKeyDown(KeyCode.Tab))
				{
					hide = !hide;
				}
				// 速さ表示の単位を変える
				if (VelocityUnitChange)
				{
					CurrentUnit++;
					if (CurrentUnit >= UnitName.Length)
					{
						CurrentUnit = 0;
					}
				}
				if (AngularVelocityUnitChange)
                {
					AngularVelocityCurrentUnit++;
					if (AngularVelocityCurrentUnit >= AngularVelocityUnitName.Length)
                    {
						AngularVelocityCurrentUnit = 0;
                    }
                }
			}
			public void FixedUpdate()
			{
				// 各軸の有効化を調整
				PAxis.axis.enabled = EnableAxis;
				YAxis.axis.enabled = EnableAxis;
				RAxis.axis.enabled = EnableAxis;
				for (int i=0; i<3; i++)
				{
					MAxis.Axis[i].enabled = EnableCoM;
				}

				// メニューなら何もしない
				if (StatMaster.isMainMenu || StatMaster.inMenu)
                {
					return;
				}

				// スタブロを取得
				StartingBlock = Mod.GetStartingBlock();
				//if (StartingBlock == null) return;

				// Rigidbodyによらない速度計測
				position = Mod.GetStartingBlock().transform.position;
				if (position == null || prevPosition == null)
				{
					velocity = 0f;
				}
                else
				{
					velocity = (position - prevPosition).magnitude / Time.fixedDeltaTime;
				}
				prevPosition = position;

				// 回転
				if (startingBlockScript != null)
                {
					Instance.transform.rotation = startingBlockScript.frontObject.transform.rotation;
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
					windowRect.height = 200 + labelHeight * ((ShowAxis ? 1 : 0f) + (ShowCoM ? 1 : 0f) + (ShowTransform ? 5.5f : 0f) + (Mod.Missile ? 1 : 0f));
					//windowRect = GUILayout.Window(windowId, windowRect, AxisMapper, "ABS 1.3.0 Lennon"); //Lennonは1.3のコードネーム
					//windowRect = GUILayout.Window(windowId, windowRect, AxisMapper, "ABS 1.5.0 McCartney"); //McCarteneyは1.4のコードネーム
					//windowRect = GUILayout.Window(windowId, windowRect, AxisMapper, "ABS 1.6.1 Harrison"); //Harrisonは1.6のコードネーム
					//windowRect = GUILayout.Window(windowId, windowRect, AxisMapper, "ABS 1.6.2 Harrison" + (Mod.Extended ? " Extended" : "")); //Harrisonは1.6のコードネーム
					//windowRect = GUILayout.Window(windowId, windowRect, AxisMapper, $"ABS {Mods.GetVersion(new Guid("a7f2f9ae-e11f-41ff-a5dd-28ab14eaa6a2"))} Starr {(Mod.Extended ? " Extended" : "")}"); //Starrは1.7のコードネーム
					windowRect = GUILayout.Window(windowId, windowRect, AxisMapper, 
						$"ABS {Mods.GetVersion(new Guid("a7f2f9ae-e11f-41ff-a5dd-28ab14eaa6a2"))} Preston{(Mod.Extended ? " Extended" : "")}"); //Prestonは2.0.0のコードネーム
				}
			}
			/// <summary>
			/// GUIの中身
			/// </summary>
			/// <param name="windowId"></param>
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

                #region mass
                GUILayout.BeginHorizontal();
				GUILayout.Label(isJapanese ? "マシン重量" : "Machine total mass");
				GUILayout.Label(Machine.Active().Mass.ToString());
				GUILayout.EndHorizontal();
                #endregion

                #region velocity
                GUILayout.BeginHorizontal();
				GUILayout.Label(isJapanese ? "スタブロ速さ" : "Speed");
				GUILayout.FlexibleSpace();
				GUILayout.Label(Game.IsSimulating ? ((float)Math.Round(velocity * Coefficient[CurrentUnit], 1)).ToString() : "0.0");
				VelocityUnitChange = GUILayout.Button(UnitName[CurrentUnit]);
				GUILayout.EndHorizontal();
                #endregion

                #region propellers
                GUILayout.BeginHorizontal();
				GUILayout.Label(isJapanese ? "プロペラ枚数" : "Propellers");
				GUILayout.FlexibleSpace();
				GUILayout.Label(NumOfPropellers().ToString()); //長短プロペラの枚数
				GUILayout.EndHorizontal();
				#endregion

				// transform information
				ToggleIndent(isJapanese ? "トランスフォームを表示" : "Show Transform", 10f, ref ShowTransform, delegate
				{
					#region 選択中のブロック
					GUILayout.BeginHorizontal();
					GUILayout.Label(isJapanese ? "選択中のブロック" : "Picked Block");
					GUILayout.FlexibleSpace();
					GUILayout.Label(picked == null ? "-" : picked.name);
					GUILayout.EndHorizontal();
					#endregion

					#region position
					GUILayout.BeginHorizontal();
					GUILayout.Label(isJapanese ? "位置" : "Position");
					GUILayout.FlexibleSpace();
					GUILayout.Label(picked != null ? picked.gameObject.transform.localPosition.ToString() : "(-, -, -)");
					GUILayout.EndHorizontal();
					#endregion

					#region rotation
					GUILayout.BeginHorizontal();
					GUILayout.Label(isJapanese ? "姿勢" : "Rotation");
					GUILayout.FlexibleSpace();
					GUILayout.Label(picked != null ? picked.gameObject.transform.localRotation.eulerAngles.ToString() : "(-, -, -)");
					GUILayout.EndHorizontal();
					#endregion

					#region scale
					GUILayout.BeginHorizontal();
					GUILayout.Label(isJapanese ? "縮尺" : "Local Scale");
					GUILayout.FlexibleSpace();
					GUILayout.Label(picked != null ? picked.gameObject.transform.localScale.ToString() : "(-, -, -)");
					GUILayout.EndHorizontal();
					#endregion

					#region angular velocity
					GUILayout.BeginHorizontal();
					GUILayout.Label(isJapanese ? "角速度" : "Angular Velocity");
					GUILayout.FlexibleSpace();
					GUILayout.Label(picked != null ? ((float)Math.Round(picked.anglularVelocity * AngularVelocityCoefficient[AngularVelocityCurrentUnit], 1)).ToString() : "0.0");
					AngularVelocityUnitChange = GUILayout.Button(AngularVelocityUnitName[AngularVelocityCurrentUnit]);
					GUILayout.EndHorizontal();
					#endregion
				});

				// missile cost
				if (Mod.Missile)
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
			/// <summary>
			/// ACMより移植 インデントを付ける
			/// </summary>
			/// <param name="text"></param>
			/// <param name="width"></param>
			/// <param name="flag"></param>
			/// <param name="func"></param>
			private void ToggleIndent(string text, float width, ref bool flag, Action func)
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
			/// <summary>
			/// ミサイルコスト表
			/// </summary>
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
			/// <summary>
			/// 誘導ミサイルのコストを計算する
			/// </summary>
			/// <returns></returns>
			public int GMissileCost()
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
			/// <summary>
			/// 長短プロペラ枚数を算出
			/// </summary>
			/// <returns></returns>
			public int NumOfPropellers()
			{
				int ret = 0;
				Machine machine = Machine.Active();
				foreach (BlockBehaviour BB in machine.BuildingBlocks)
				{
					if (BB.BlockID == (int)BlockType.Propeller || BB.BlockID == (int)BlockType.SmallPropeller || BB.BlockID == 52)
					{
						ret++;
					}
				}
				return ret;
			}
			/// <summary>
			/// ドラッグ可能かどうかを切り替えるコマンド
			/// </summary>
			/// <param name="args"></param>
			private void CanDragWindow(params string[] args)
            {
				if (args.Length > 2)
                {
					Mod.Error("The number of arguments exceeds the usage!");
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
			/// <summary>
			/// シミュ中に消えるかどうかを切り替えるコマンド
			/// </summary>
			/// <param name="args"></param>
			private void DisappearsInSimulation(params string[] args)
            {
				if (args.Length > 2)
				{
					Mod.Error("The number of arguments exceeds the usage!");
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
		/// <summary>
		/// 空力中心軸表示（継承用のひな型）
		/// </summary>
		public abstract class AbstractAiroAxis : MonoBehaviour
		{
			public LineRenderer axis;
			/// <summary>
			/// 線分の長さの半分
			/// </summary>
			protected float LengthOfAxis = 20f;
			/// <summary>
			/// 端点の位置の初期値
			/// </summary>
			public Vector3 EndsVector { internal set; get; } = Vector3.zero;
			/// <summary>
			/// 自分のマシン
			/// </summary>
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
			/// <summary>
			/// シミュ中のスタブロ
			/// </summary>
			public BlockBehaviour StartingBlockSimulation;
			/// <summary>
			/// ビルド中のスタブロ
			/// </summary>
			public BlockBehaviour StartingBlockBuild;

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
				Initialize();
			}
			public virtual void FixedUpdate()
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
			/// <summary>
			/// 軸を初期化する
			/// </summary>
			public void Initialize()
            {
				// 初期化
				AxisController = new GameObject("AeroAxis");
				AxisController.transform.parent = CoLGUI.Instance.transform; //StatMaster.isMP ? (machine as ServerMachine).player.buildZone.transform : machine.transform;
				axis = AxisController.gameObject.GetComponent<LineRenderer>() ?? AxisController.gameObject.AddComponent<LineRenderer>();
				axis.SetWidth(0.1f, 0.1f);
				axis.GetComponent<Renderer>().material.color = Color.white;
				axis.material = new Material(Shader.Find("Sprites/Default"));
			}
			/// <summary>
			/// スタブロを取得する
			/// </summary>
			/// <returns></returns>
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
			/// <summary>
			/// 線分の中心の点を決めて、それをもとに端点を指定する
			/// </summary>
			public void SetAxisEnds()
			{
				numerator = Vector3.zero;
				denominator = 0f;
				foreach (BlockBehaviour current in BlockList)
				{
					component = current.GetComponent<AxialDrag>();
					if (component != null) // Rigidbodyがあり、かつAxialDragがnullでないとき、則ち空力系ブロックの時
					{
						numerator += current.GetCenter() * Math.Abs(CalculateRotation(current));
						denominator += Math.Abs(CalculateRotation(current));
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
				AxisController.transform.position = numerator;
				axis.SetPosition(0, numerator + EndsVector);
				axis.SetPosition(1, numerator - EndsVector);
			}
			/// <summary>
			/// 抵抗の強さ（ベクトル）を回転させて射影を得る
			/// </summary>
			/// <param name="current"></param>
			/// <returns></returns>
			public abstract float CalculateRotation(BlockBehaviour current);
			/// <summary>
			/// サーフェスの抵抗の強さ（ベクトル）を回転させて射影を得る
			/// わからん
			/// </summary>
			/// <param name="node"></param>
			/// <param name="surface"></param>
			/// <returns></returns>
			public abstract float CalculateRotation(BuildNodeBlock node, BuildSurface surface); //ここがわからない
		}
		/// <summary>
		/// ピッチ軸
		/// </summary>
		public class PAxis : AbstractAiroAxis
		{
			public void Awake()
			{
				EndsVector = LengthOfAxis * Vector3.right;
			}
			public override void Start()
			{
				base.Start();
				axis.GetComponent<Renderer>().material.color = Color.red;
			}
			public override void FixedUpdate()
            {
				base.FixedUpdate();
				LengthOfAxis = 20 + machine.Size.x / 2f;
				EndsVector = LengthOfAxis * AxisController.transform.right;
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
		/// <summary>
		/// ヨー軸
		/// </summary>
		public class YAxis : AbstractAiroAxis
		{
			public void Awake()
			{
				EndsVector = LengthOfAxis * Vector3.up;
			}
			public override void Start()
			{
				base.Start();
				axis.GetComponent<Renderer>().material.color = Color.green;
			}
			public override void FixedUpdate()
			{
				base.FixedUpdate();
				LengthOfAxis = 20 + machine.Size.y / 2f;
				EndsVector = LengthOfAxis * AxisController.transform.up;
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
		/// <summary>
		/// ロール軸
		/// </summary>
		public class RAxis : AbstractAiroAxis
		{
			public void Awake()
			{
				EndsVector = LengthOfAxis * Vector3.forward;
			}
			public override void Start()
			{
				base.Start();
				axis.GetComponent<Renderer>().material.color = Color.blue;
			}
			public override void FixedUpdate()
			{
				base.FixedUpdate();
				LengthOfAxis = 20 + machine.Size.z / 2f;
				EndsVector = LengthOfAxis * AxisController.transform.forward;
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
		/// <summary>
		/// 重心軸
		/// </summary>
		public class MassAxis : MonoBehaviour
		{
			public LineRenderer[] Axis = new LineRenderer[3];
			/// <summary>
			/// 線分の長さの半分
			/// </summary>
			private float LengthOfAxisP = 20f;
			private float LengthOfAxisY = 20f;
			private float LengthOfAxisR = 20f;
			/// <summary>
			/// x, y, zの順
			/// </summary>
			public Vector3[] EndsVector
			{
				internal set; get;
			}
			public Machine machine
            {
                get
                {
					return Machine.Active();
                }
            }
			/// <summary>
			/// lineRenderer保持用のゲームオブジェクト
			/// </summary>
			public GameObject[] AxisController = new GameObject[3];
			protected Vector3 numerator;
			protected float denominator;
			/// <summary>
			/// シミュ中のスタブロ
			/// </summary>
			public BlockBehaviour StartingBlockSimulation;
			/// <summary>
			/// ビルド中のスタブロ
			/// </summary>
			public BlockBehaviour StartingBlockBuild;
			/// <summary>
			/// 軸を表示するかどうか
			/// </summary>
			public bool ShowAxis = false;

			/// <summary>
			/// シミュ中とビルド中でブロックが切り替わる対策
			/// </summary>
			private List<BlockBehaviour> BlockList
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
				Initialize();
			}
			public void FixedUpdate()
			{
				if (machine == null)
				{
					return;
				}

				// 軸の長さを指定
				LengthOfAxisP = 20f + machine.Size.x / 2f;
				LengthOfAxisY = 20f + machine.Size.y / 2f;
				LengthOfAxisR = 20f + machine.Size.z / 2f;

				// 軸の方向を指定
				EndsVector[0] = LengthOfAxisP * AxisController[0].transform.right;
				EndsVector[1] = LengthOfAxisP * AxisController[1].transform.up;
				EndsVector[2] = LengthOfAxisP * AxisController[2].transform.forward;

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
			/// <summary>
			/// 3軸を初期化する
			/// </summary>
			public void Initialize()
            {
				EndsVector = new Vector3[3] { Vector3.zero, Vector3.zero, Vector3.zero };

				// 3軸の初期化
				for (int i = 0; i < 3; i++)
				{
					AxisController[i] = new GameObject("Axis Controller");
					AxisController[i].transform.parent = CoLGUI.Instance.transform; //StatMaster.isMP ? (machine as ServerMachine).player.buildZone.transform : machine.transform;

					Axis[i] = AxisController[i].GetComponent<LineRenderer>() ?? AxisController[i].AddComponent<LineRenderer>();
					Axis[i].SetWidth(0.1f, 0.1f);
					Axis[i].GetComponent<Renderer>().material.color = Color.white;
					Axis[i].material = new Material(Shader.Find("Sprites/Default"));

					EndsVector[i] = Vector3.zero;
				}
			}
			/// <summary>
			/// スタブロを取得する
			/// </summary>
			/// <returns></returns>
			public BlockBehaviour GetStartingBlock() 
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
			/// <summary>
			/// 線分の中心の点を決めて、それをもとに端点を指定する
			/// </summary>
			public void SetAxisEnds()
			{
				numerator = Vector3.zero;
				denominator = 0f;

				for (int i = 0; i < BlockList.Count; i++)
				{
					BlockBehaviour current = BlockList[i];
					float mass = current.isSimulating ? current.BuildingBlock.Rigidbody.mass : current.Rigidbody.mass;
					if (current.BlockID != (int)BlockType.Pin && current.BlockID != (int)BlockType.CameraBlock && current.Prefab.Type != BlockType.BuildNode && current.Prefab.Type != BlockType.BuildEdge)
					{
						if (current.Prefab.Type == BlockType.Brace || current.Prefab.Type == BlockType.RopeWinch || current.Prefab.Type == BlockType.Spring)
						{
							GenericDraggedBlock GDB = current as GenericDraggedBlock;
							numerator += (GDB.startPoint.position + GDB.endPoint.position) / 2 * mass;
							denominator += mass;
						}
						else
						{
							numerator += current.GetCenter() * mass;
							denominator += mass;
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
					AxisController[i].transform.position = numerator;
					Axis[i].SetPosition(0, numerator + EndsVector[i]);
					Axis[i].SetPosition(1, numerator - EndsVector[i]);
				}
				
			}
		}
	}
}