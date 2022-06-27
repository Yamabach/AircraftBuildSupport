using System;
using System.Collections.Generic;
using Modding;
using Modding.Blocks;
using Modding.Serialization;
using UnityEngine;
using ABSspace.AdditionalBlockController;
using ABSspace.BlockController;
using ABSspace.CoLController;
using System.Xml.Serialization;

namespace ABSspace
{
    namespace BlockGrouping
    {
        // ブロックの選択を拡張し、あるタグのブロックを選択すると、そのタグ全てのブロックを選択するようにする
        public class BlockGroupingManager : SingleInstance<BlockGroupingManager>
        {
            // 選択を有効化するツールのリスト
            private readonly List<StatMaster.Tool> toolList = new List<StatMaster.Tool>
            {
                StatMaster.Tool.Translate,
                StatMaster.Tool.Rotate,
                StatMaster.Tool.Scale,
                StatMaster.Tool.Mirror
            };
            public override string Name
            {
                get { return "Block Grouping Manager"; }
            }
            public static bool isGrouping = false; // グループ化させる

            public void Update()
            {
                isGrouping = Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.B); // Alt + B
                if (isGrouping)
                {
                    SelectBlocks(GetBlockList());
                    isGrouping = false;
                }
            }

            // 選択するブロックを選ぶ
            private List<BlockBehaviour> GetBlockList()
            {
                //Mod.Log("GetBlockList has been invoked");
                // 現在選択中のブロック
                List<BlockBehaviour> current = AdvancedBlockEditor.Instance.selectionController.MachineSelection;
                // これから選択するブロック
                List<BlockBehaviour> result = new List<BlockBehaviour>();

                #region 同じ種類のフロックをすべて選択
                /*
                // 試しに同じ種類のブロックを選択するようにしてみる
                // currentに含まれるブロックの種類を抽出
                List<int> IDs = new List<int>();
                foreach (BlockBehaviour block in current)
                {
                    if (!IDs.Contains(block.BlockID))
                    {
                        IDs.Add(block.BlockID);
                    }
                }
                // IDsに含まれるIDを持つブロックがあれば、それをresultに追加
                foreach (BlockBehaviour block in Machine.Active().BuildingBlocks)
                {
                    if (IDs.Contains(block.BlockID))
                    {
                        result.Add(block);
                    }
                }
                */
                #endregion
                // 同じラベルのブロックをすべて選択
                List<int> group = new List<int>();
                foreach (BlockBehaviour block in current)
                {
                    BlockExchangerScript component = block.GetComponent<BlockExchangerScript>();
                    if (component == null)
                    {
                        //Mod.LogError("BlockExchagnerScript not found! 1");
                        continue;
                    }
                    if (!group.Contains(component.groupLabel))
                    {
                        group.Add(component.groupLabel);
                    }
                }
                foreach (BlockBehaviour block in Machine.Active().BuildingBlocks)
                {
                    BlockExchangerScript component2 = block.GetComponent<BlockExchangerScript>();
                    if (component2 == null)
                    {
                        //Mod.LogError("BlockExchagnerScript not found! 2"); // 毎回出るが問題無し（最初のスタブロが原因か）
                        continue;
                    }
                    if (group.Contains(component2.groupLabel))
                    {
                        result.Add(block);
                    }
                }

                return result;
            }

            // ブロックを選択する
            private void SelectBlocks(List<BlockBehaviour> list)
            {
                // 移動、回転、スケール、対称以外なら何もしない
                if (!toolList.Contains(StatMaster.Mode.selectedTool))
                {
                    return;
                }
                BlockBehaviour lastBlock = AdvancedBlockEditor.Instance.selectionController.LastBlock;

                //AdvancedBlockEditor.Instance.Select(tool, Machine.Active(), GetStartingBlock().Guid, true, 0, 1f);

                // 対象のブロックをすべて選択
                AdvancedBlockEditor.Instance.selectionController.Select(list, true, true);

                // こうすると最後に選択したブロックを変更できる（Undoには引っかからない）
                if (lastBlock != null)
                {
                    AdvancedBlockEditor.Instance.selectionController.Deselect(lastBlock, false, true);
                    AdvancedBlockEditor.Instance.selectionController.Select(lastBlock, true, false, true);
                }
            }
        }
        public class XMLDeserializer
        {
            public static readonly string FileName = "name.xml";
            private static readonly string ResourcesPath = "Resources/BlockGroupName/";
            private static readonly string DataPath = Application.dataPath + "/Mods/Data/AircraftBuildSupport_a7f2f9ae-e11f-41ff-a5dd-28ab14eaa6a2/BlockGroupName/";

            public static GroupLabel Deserialize()
            {
                if (Modding.ModIO.ExistsDirectory(DataPath, true))
                {
                    Mod.Log("Loaded " + FileName + " from data folder");
                    return Modding.ModIO.DeserializeXml<GroupLabel>(DataPath + FileName, true);
                }
                Mod.Log("Loaded " + FileName + " from resources folder");
                return Modding.ModIO.DeserializeXml<GroupLabel>(ResourcesPath + FileName);
            }
        }

        [XmlRoot("block-group")]
        public class GroupLabel : Element
        {
            [XmlArray("group")]
            [XmlArrayItem("label")]
            public List<string> labels { get; set; }
        }
    }
}