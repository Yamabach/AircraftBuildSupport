using System;
using System.Collections.Generic;
using Modding;
using Modding.Common;
using UnityEngine;
using Localisation;

namespace ABSspace
{
	public class Mod : ModEntryPoint
	{
		/// <summary>
		/// ���{�ꉻ����Ă��邩�ǂ���
		/// </summary>
		public static bool isJapanese = SingleInstance<LocalisationManager>.Instance.currLangName.Contains("���{��");
		public static GameObject mod;
		/// <summary>
		/// NoBounds�������[�h����Ă��邩
		/// </summary>
		public static bool Extended
		{
			get
			{
				return Mods.IsModLoaded(new Guid("7062baee-484e-4cdd-8750-b4baa7b964e5")) //NoBounds
				|| Mods.IsModLoaded(new Guid("bb7ba333-e72b-49a8-8c0d-011a3fcadaf3")) //EasyScale
				;
			}
		}
		public static bool Missile
        {
            get
            {
				return Mods.IsModLoaded(new Guid("90a17943-af2a-40ea-a51f-530553b9fcb0")) || Mods.IsModLoaded(new Guid("15259577-bd19-4397-8646-88d235cf8d24"));
			}
        }
		/// <summary>
		/// �u���b�N�̃��x��
		/// </summary>
		public static BlockGrouping.GroupLabel blockGroup;
		//public static EntityController.GroupLabel entityGroup; // �G���e�B�e�B�̃��x��

		public override void OnLoad()
		{
			mod = new GameObject("ABS Mod Controller");
			Log("Load");
			SingleInstance<CoLController.CoLGUI>.Instance.transform.parent = mod.transform;
			SingleInstance<BlockController.BlockSelector>.Instance.transform.parent = mod.transform;
			SingleInstance<EntityController.EntitySelector>.Instance.transform.parent = mod.transform;
			SingleInstance<BlockGrouping.BlockGroupingManager>.Instance.transform.parent = mod.transform;
			//SingleInstance<EntityController.EntityGroupingManager>.Instance.transform.parent = mod.transform;
			UnityEngine.Object.DontDestroyOnLoad(mod);
			//CheckExtended();

			// xml�ǂݍ���
			blockGroup = BlockGrouping.XMLDeserializer.Deserialize();
			//entityGroup = EntityController.XMLDeserializer.Deserialize();
		}

		/// <summary>
		/// PNG���e�@�ɃX�N���v�g�\��t��
		/// </summary>
		/// <param name="entityId"></param>
		/// <param name="prefab"></param>
		public override void OnEntityPrefabCreation(int entityId, GameObject prefab)
		{
			switch (entityId)
			{
				case 1:
					prefab.AddComponent<EntityController.PngProjectorScript>();
					break;
			}
		}

		public static void Log(string message)
		{
			Debug.Log("ABS Log : " + message);
		}
		public static void Warning(string message)
		{
			Debug.LogWarning("ABS Warning : " + message);
		}
		public static void Error(string message)
		{
			Debug.LogError("ABS Error : " + message);
		}
		/// <summary>
		/// �X�^�u�����擾����
		/// </summary>
		/// <returns></returns>
		public static BlockBehaviour GetStartingBlock() //�X�^�u�����Q�Ƃ���̂͂�����Ɗ�Ȃ������H�i�}���`�Ƃ��j
		{
			Machine machine = Machine.Active();
			if (machine == null)
            {
				return null;
            }
			List<BlockBehaviour> BlockList = machine.isSimulating ? machine.SimulationBlocks : machine.BuildingBlocks;
			foreach (BlockBehaviour BB in BlockList)
			{
				if (BB.BlockID == 0) //�X�^�u������������
				{
					return BB;
				}
			}
			return null; //������Ȃ�������
		}
	}
	/*
	mod�u���b�N��ǉ��������Ƃ��̒ǋL��������
	�EBlockSelector.ModBlockDict �� ModID-LocalID �� �}���������N���X����������
	�EModBlockExchangerScript �̐擪�� Guid ��ǉ�����
	�EModBlockExchangerScript �́u�����ł���u���b�N�Q�v���X�g�ɒǉ�����
	�EModBlockExchangerScript.ModBlockId.SetGroup �� else if �ȉ���ǉ�����
	�EModBlockExchangerScript.ModBlockId.SetDictionary �Ɏ����̐錾�Ə�������ǉ�����
	�EModBlocks ���O��ԂɃN���X��ǉ�����
	 */
}
