using System;
using Modding;
using UnityEngine;

namespace ABSspace
{
	/// <summary>
	/// ABSで追加されるリバースブロックに関する名前空間
	/// </summary>
	namespace AdditionalBlockController
	{
		/// <summary>
		/// リバースブロックにアタッチするスクリプト
		/// </summary>
		public abstract class ReversedBlockScript : BlockScript
		{
			/// <summary>
			/// ブロックの長さ
			/// </summary>
			public float blockLength;
			/// <summary>
			/// 変換先
			/// </summary>
			public BlockType ChangeTo;
			public BlockBehaviour BB;

			public void Awake()
			{
				BB = GetComponent<BlockBehaviour>();
			}
			public override void SafeAwake()
			{
				SetLengthAndBlockType();
			}
			public void Start()
			{
				if (!IsSimulating)
				{
					// リバースブロックの情報を記録
					Machine machine = VisualController.Block.ParentMachine;
					Vector3 lastPosition = BB.transform.position;
					Vector3 forward = BB.transform.forward;
					Vector3 up = BB.transform.up;

					// リバースブロックを消去
					machine.UndoSystem.Undo();

					machine.isLoadingInfo = true;

					// 変換
					BlockBehaviour newBB;

					if (!machine.AddBlockGlobal(lastPosition + forward * blockLength, Quaternion.LookRotation(-forward, up), ChangeTo, false, out newBB))
					{
						Mod.Log("Failed to place Reversed block!");
					}
					else
					{
						machine.UndoSystem.AddAction(new UndoActionAdd(machine, BlockInfo.FromBlockBehaviour(newBB)));
					}
					machine.isLoadingInfo = false;
				}
			}
			/// <summary>
			/// ブロックの長さとブロックの種類を指定
			/// </summary>
			public abstract void SetLengthAndBlockType();
		}
		public class ReversedGrabber : ReversedBlockScript
		{
			public override void SetLengthAndBlockType()
			{
				blockLength = 1;
				ChangeTo = BlockType.Grabber;
			}
		}
		public class ReversedHinge : ReversedBlockScript
		{
			public override void SetLengthAndBlockType()
			{
				blockLength = 1;
				ChangeTo = BlockType.Hinge;
			}
		}
		public class ReversedDoubleWoodenBlock : ReversedBlockScript
		{
			public override void SetLengthAndBlockType()
			{
				blockLength = 2;
				ChangeTo = BlockType.DoubleWoodenBlock;
			}
		}
		public class ReversedScalingBlock : ReversedBlockScript
		{
			public override void SetLengthAndBlockType()
			{
				blockLength = 1;
				ChangeTo = BlockType.ScalingBlock;
			}
		}
		public class ReversedSingleWoodenBlock : ReversedBlockScript
		{
			public override void SetLengthAndBlockType()
			{
				blockLength = 1;
				ChangeTo = BlockType.SingleWoodenBlock;
			}
		}
		public class ReversedSwivel : ReversedBlockScript
		{
			public override void SetLengthAndBlockType()
			{
				blockLength = 1;
				ChangeTo = BlockType.Swivel;
			}
		}
		public class ReversedSteeringHinge : ReversedBlockScript
		{
			public override void SetLengthAndBlockType()
			{
				blockLength = 1;
				ChangeTo = BlockType.SteeringHinge;
			}
		}
		public class ReversedWoodenPole : ReversedBlockScript
		{
			public override void SetLengthAndBlockType()
			{
				blockLength = 2;
				ChangeTo = BlockType.WoodenPole;
			}
		}
		public class ReversedBallJoint : ReversedBlockScript
		{
			public override void SetLengthAndBlockType()
			{
				blockLength = 1;
				ChangeTo = BlockType.BallJoint;
			}
		}
		public class ReversedLog : ReversedBlockScript
		{
			public override void SetLengthAndBlockType()
			{
				blockLength = 3;
				ChangeTo = BlockType.Log;
			}
		}
		public class ReversedMediumCogPowered : ReversedBlockScript
		{
			public override void SetLengthAndBlockType()
			{
				blockLength = 1;
				ChangeTo = BlockType.CogMediumPowered;
			}
		}
		public class ReversedMediumCogUnpowered : ReversedBlockScript
		{
			public override void SetLengthAndBlockType()
			{
				blockLength = 1;
				ChangeTo = BlockType.CogMediumUnpowered;
			}
		}
	}
}
