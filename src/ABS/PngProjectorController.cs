using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Modding;
using Modding.Levels;
using UnityEngine;

namespace ABSspace
{
	/// <summary>
	/// ABSで追加されるエンティティに関する名前空間
	/// </summary>
	namespace EntityController
	{
		/// <summary>
		/// png投影機
		/// </summary>
		public class PngProjectorScript : MonoBehaviour
		{
			public bool isJapanese = Mod.isJapanese;
			public bool hasImageFile = false;
			private GenericEntity GE;
			/// <summary>
			/// 画像フォルダを開くボタン
			/// </summary>
			private MToggle OpenDirectoryToggle;
			/// <summary>
			/// 画像を変更するボタン
			/// </summary>
			private MToggle ChangeToggle;
			/// <summary>
			/// 画像を選択するメニュー
			/// </summary>
			private MMenu ChangeMenu;
			/// <summary>
			/// x軸方向のオフセット
			/// </summary>
			private MSlider OffsetX;
			/// <summary>
			/// y軸方向のオフセット
			/// </summary>
			private MSlider OffsetY;
			/// <summary>
			/// z軸方向のオフセット
			/// </summary>
			private MSlider Offset;
			/// <summary>
			/// 画像ファイル名
			/// </summary>
			private List<string> items;

			private string FolderName
			{
				get
				{
					return "PngImages";
				}
			}
			private string path
			{
				get
				{
					return Application.dataPath + "/Mods/Data/AircraftBuildSupport_a7f2f9ae-e11f-41ff-a5dd-28ab14eaa6a2/" + FolderName;
				}
			}
			/// <summary>
			/// 画像のゲームオブジェクト
			/// </summary>
			private GameObject Screen;
			/// <summary>
			/// 読み込んだ画像データ
			/// </summary>
			private List<Sprite> Images;
			/// <summary>
			/// スプライト描画コンポーネント
			/// </summary>
			private SpriteRenderer SR;

			public void Awake()
			//レベルロード時に限り、画像の大きさがエンティティのスケール倍になる不具合
			{
				//ModConsole.Log("PngProjector : load");
				if (Game.IsSimulating)
				{
					return;
				}
				items = new List<string>();
				GE = GetComponent<GenericEntity>();
				
				//メモ
				//指定したフォルダの中のpngファイルの名前をitemsリストに格納
				//そのリストでMMenuを作る
				//トグルを押すとその画像をテクスチャにする
				//ファイルを開くトグルや、リロードするトグルも作る

				Screen = new GameObject("pngScreen");
				//Screen.transform.position = GE.transform.position;
				Screen.transform.parent = GE.transform; //ここでスケールも変更されている
				Screen.transform.localScale = Vector3.one; // GE.transform.localScale; //スケール修正
				Screen.gameObject.SetActive(true);
				Screen.gameObject.layer = GE.gameObject.layer;
				SR = Screen.AddComponent<SpriteRenderer>();
				SR.enabled = true;
				Images = new List<Sprite>();

				if (!Modding.ModIO.ExistsDirectory(FolderName, true)) //無ければ、ディレクトリを生成
				{
					ModConsole.Log("ABS.PngProjectorScript : Created PngImages folder in " + Application.dataPath + "/Mods/Data/AircraftBuildSupport_a7f2f9ae-e11f-41ff-a5dd-28ab14eaa6a2/");
					Modding.ModIO.CreateDirectory(FolderName, true);
					//サムネの画像を追加したい
				}
				string[] Content = Modding.ModIO.GetFiles(FolderName, true);
				if (hasImageFile = Content.Length > 0 || Content != null)
				{
					foreach (string file in Content)
					{
						Images.Add(Util.CreateSpriteFromBytes(Modding.ModIO.ReadAllBytes(file, true))); //
						items.Add(Path.GetFileNameWithoutExtension(file));
					}
				}
				if (Images == null)
				{
					Debug.LogError("ABS.PngProjectorScript : Images is null!");
					hasImageFile = false;
				}
				else if (Images.Count == 0)
				{
					Debug.LogWarning("ABS.PngProjectorScript : Images.Count == 0!");
					hasImageFile = false;
				}
				else
				{
					ChangeToggle = GE.AddToggle(isJapanese ? "画像を適用" : "Apply Image", "Apply Image", false);
					ChangeMenu = GE.AddMenu("Change Menu", 0, items);
					OffsetX = GE.AddSlider(isJapanese ? "Xオフセット" : "Offset X", "OffsetX", 0f, -100.0f, 100.0f);
					OffsetY = GE.AddSlider(isJapanese ? "Yオフセット" : "Offset Y", "OffsetY", 0f, -100.0f, 100.0f);
					Offset = GE.AddSlider(isJapanese ? "Zオフセット" : "Offset Z", "Offset", 1.5f, 1.0f, 100f);
					ChangeImage(ChangeMenu.Value); //特定の画像にチェンジする
				}
				OpenDirectoryToggle = GE.AddToggle(isJapanese ? "画像フォルダを開く" : "Open Data Folder", "Open Data Folder", false);
			}
			public void Update()
			{
				if (!Game.IsSimulating) //シミュ中でない時
				{
					if (ChangeToggle != null)
					{
						if (ChangeToggle.IsActive)
						{
							ChangeToggle.IsActive = false;
							ChangeImage(ChangeMenu.Value);
						}
					}
					if (OpenDirectoryToggle.IsActive)
					{
						OpenDirectoryToggle.IsActive = false;
						ModConsole.Log("ABS : Open file < " + path + " >");
						Modding.ModIO.OpenFolderInFileBrowser(FolderName, true);
					}
					if (hasImageFile)
					{
						Screen.transform.localPosition = new Vector3(
							OffsetX.Value, //SR.bounds.size.x / GE.transform.localScale.x / 2, 
							OffsetY.Value, //-SR.bounds.size.y / GE.transform.localScale.y / 2, 
							Offset.Value
							);
						//原点は投影機から見て右下
						if (Offset.Value < Offset.Min)
						{
							Offset.Value = Offset.Min;
						}
						if (Offset.Value > Offset.Max)
						{
							Offset.Value = Offset.Max;
						}
					}
					
				}
			}
			/// <summary>
			/// 投影する画像を変更する
			/// </summary>
			/// <param name="index"></param>
			public void ChangeImage(int index = 0)
			{
				SR.sprite = Images[index];
				SR.flipX = true;
				Screen.transform.localRotation = Quaternion.Euler(0, 0, 0); //rotationが何かおかしい？
				Screen.transform.localScale = Vector3.one; //スケールは変えない（エンティティのスケールをそのまま使う）
				//Screen.transform.localPosition = new Vector3(SR.bounds.size.x/2, -SR.bounds.size.y/2, Offset.Value);
				//回転させると位置ズレが起こる

				//Screen.transform.localPosition = GE.transform.position;
				//一度ポジションを確認する
				//ポジションをここで変えてもUpdateで変わるから意味ない
			}
		}
		
		/// <summary>
		/// 便利関数置き場
		/// </summary>
		public static class Util
		{
			/// <summary>
			/// byte列から画像を生成
			/// </summary>
			/// <param name="bytes"></param>
			/// <returns></returns>
			public static Sprite CreateSpriteFromBytes(byte[] bytes)
			{
				//横サイズの判定
				int pos = 16;
				int width = 0;
				for (int i=0; i<4; i++)
				{
					width = width * 256 + bytes[pos++];
				}
				//縦サイズの判定
				int height = 0;
				for (int j=0; j<4; j++)
				{
					height = height * 256 + bytes[pos++];
				}
				//byteからTexture2D作成
				Texture2D texture = new Texture2D(width, height);
				texture.LoadImage(bytes);

				return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
			}
		}
	}
}
