using System;
using Modding;
using UnityEngine;

namespace ABSspace
{
	namespace AdditionalBlockController
	{
		public abstract class ReversedBlockScript : BlockScript
		{
			public float blockLength;
			public BlockType ChangeTo;
			public BlockBehaviour BB;

			private void Awake()
			{
				BB = GetComponent<BlockBehaviour>();
				//ModConsole.Log("Reversed Block Script : Awake"); //対称設置で呼ばれる
			}
			public override void SafeAwake()
			{
				SetLengthAndBlockType();
				//ModConsole.Log("Reversed Block Script : SafeAwake"); //対称設置で呼ばれる
			}
			private void Start()
			{
				if (!IsSimulating)
				{
					//ModConsole.Log("Reversed Block Script : Start"); //対称設置で呼ばれてない

					//Awakeで呼び出してみると、元の位置と対称の位置のmodブロックは両方とも消えず、
					//対称の位置にリバースブロックが設置された
					//Undoしてみると、modブロックは消えて、対称の位置にあるリバースブロックのみ残った
					//対称設置をオフにした場合、同じ場所にmod/リバースの両ブロックが設置された

					Machine machine = VisualController.Block.ParentMachine;
					Vector3 lastPosition = BB.transform.position;
					Quaternion lastRotation = BB.transform.rotation;
					//machine.UndoSystem.Undo();
					machine.RemoveBlock(BB);
					machine.isLoadingInfo = true;
					BlockBehaviour newBB;
					if (!machine.AddBlockGlobal(lastPosition + lastRotation * Vector3.forward * blockLength, lastRotation, ChangeTo, false, out newBB))
					{
						ModConsole.Log("ABS : Failed to place Reversed block!");
					}
					else
					{
						machine.RotateBlock(newBB.Guid, lastRotation * Quaternion.AngleAxis(180, Vector3.right));
						machine.UndoSystem.AddAction(new UndoActionAdd(machine, BlockInfo.FromBlockBehaviour(newBB)));
					}
					machine.isLoadingInfo = false;
				}
			}
			public virtual void SetLengthAndBlockType() { }
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
