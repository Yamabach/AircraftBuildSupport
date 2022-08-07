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
		/// 日本語化されているかどうか
		/// </summary>
		public static bool isJapanese = SingleInstance<LocalisationManager>.Instance.currLangName.Contains("日本語");
		public static GameObject mod;
		/// <summary>
		/// NoBounds等がロードされているか
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
		/// ブロックのラベル
		/// </summary>
		public static BlockGrouping.GroupLabel blockGroup;
		//public static EntityController.GroupLabel entityGroup; // エンティティのラベル

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

			// xml読み込み
			blockGroup = BlockGrouping.XMLDeserializer.Deserialize();
			//entityGroup = EntityController.XMLDeserializer.Deserialize();
		}

		/// <summary>
		/// PNG投影機にスクリプト貼り付け
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
		/// スタブロを取得する
		/// </summary>
		/// <returns></returns>
		public static BlockBehaviour GetStartingBlock() //スタブロを参照するのはちょっと危ないかも？（マルチとか）
		{
			Machine machine = Machine.Active();
			if (machine == null)
            {
				return null;
            }
			List<BlockBehaviour> BlockList = machine.isSimulating ? machine.SimulationBlocks : machine.BuildingBlocks;
			foreach (BlockBehaviour BB in BlockList)
			{
				if (BB.BlockID == 0) //スタブロを見つけたら
				{
					return BB;
				}
			}
			return null; //見つからなかったら
		}
	}
	/*
	modブロックを追加したいときの追記部分メモ
	・BlockSelector.ModBlockDict に ModID-LocalID と 挿入したいクラスを書き込む
	・ModBlockExchangerScript の先頭に Guid を追加する
	・ModBlockExchangerScript の「交換できるブロック群」リストに追加する
	・ModBlockExchangerScript.ModBlockId.SetGroup に else if 以下を追加する
	・ModBlockExchangerScript.ModBlockId.SetDictionary に辞書の宣言と初期化を追加する
	・ModBlocks 名前空間にクラスを追加する
	 */
}
