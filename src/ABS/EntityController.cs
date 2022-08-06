using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Modding;
using Modding.Levels;
using Modding.Serialization;
using UnityEngine;

namespace ABSspace
{
    namespace EntityController
    {
        public class EntitySelector : SingleInstance<EntitySelector>
        {
            public override string Name
            {
                get
                {
                    return "Entity Selector";
                }
            }

            public void Awake()
            {
                //Events.OnEntityPlaced += new Action<Entity>(AddScript);
            }
            public void AddScript(Entity entity)
            {
                /*
                if (entity.GameObject.GetComponent<EntityGrouping>() == null)
                {
                    entity.GameObject.AddComponent<EntityGrouping>();
                }
                */
            }
        }
        /*
        public class EntityGrouping : MonoBehaviour
        {
            GenericEntity entity;

            // エンティティのグループ化系
            public MMenu GroupMenu;
            public int groupLabel
            {
                get
                {
                    return GroupMenu.Value;
                }
            }

            void Awake()
            {
                Mod.Log("EntityGrouping awake");
                entity = GetComponent<GenericEntity>();
                GroupMenu = entity.AddMenu("Group", 0, Mod.entityGroup.labels, true);
            }
        }
        */

        // ブロックの選択を拡張し、あるタグのブロックを選択すると、そのタグ全てのブロックを選択するようにする
        /*
        public class EntityGroupingManager : SingleInstance<EntityGroupingManager>
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
                    SelectEntities(GetBlockList());
                    isGrouping = false;
                }
            }

            // 選択するブロックを選ぶ
            private List<LevelEntity> GetBlockList()
            {
                //Mod.Log("GetBlockList has been invoked");
                // 現在選択中のブロック
                List<LevelEntity> current = LevelEditor.Instance.selectionController.LevelSelection;
                // これから選択するブロック
                List<LevelEntity> result = new List<LevelEntity>();

                // 同じラベルのブロックをすべて選択
                List<int> group = new List<int>();
                foreach (LevelEntity entity in current)
                {
                    EntityGrouping component = entity.GetComponent<EntityGrouping>();
                    if (component == null)
                    {
                        Mod.LogError("EntityGrouping not found! 1");
                        continue;
                    }
                    if (!group.Contains(component.groupLabel))
                    {
                        group.Add(component.groupLabel);
                    }
                }
                foreach (Entity entity in Level.GetCurrentLevel().Selection)
                {
                    EntityGrouping component2 = entity.GameObject.GetComponent<EntityGrouping>();
                    if (component2 == null)
                    {
                        Mod.LogError("EntityGrouping not found! 2");
                        continue;
                    }
                    if (group.Contains(component2.groupLabel))
                    {
                        result.Add(entity.InternalObject);
                    }
                }

                return result;
            }

            // ブロックを選択する
            private void SelectEntities(List<LevelEntity> list)
            {
                Mod.Log("SelectEntities");
                // 移動、回転、スケール、対称以外なら何もしない
                if (!toolList.Contains(StatMaster.Mode.LevelEditor.selectedTool))
                {
                    Mod.LogWarning("selectedTool not appropriate");
                    //return;
                }
                LevelEntity lastBlock = LevelEditor.Instance.selectionController.LastEntity;

                //AdvancedBlockEditor.Instance.Select(tool, Machine.Active(), GetStartingBlock().Guid, true, 0, 1f);

                // 対象のブロックをすべて選択
                LevelEditor.Instance.selectionController.Select(list, true, true);

                // こうすると最後に選択したブロックを変更できる（Undoには引っかからない）
                if (lastBlock != null)
                {
                    LevelEditor.Instance.selectionController.Deselect(lastBlock, false, true);
                    LevelEditor.Instance.selectionController.Select(lastBlock, true, false, true);
                }
            }
        }
        public class XMLDeserializer
        {
            public static readonly string FileName = "name.xml";
            private static readonly string ResourcesPath = "Resources/EntityGroupName/";
            private static readonly string DataPath = Application.dataPath + "/Mods/Data/AircraftBuildSupport_a7f2f9ae-e11f-41ff-a5dd-28ab14eaa6a2/EntityGroupName/";

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
    */
    }
}