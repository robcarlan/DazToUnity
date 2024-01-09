using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Daz3D
{
	public class DTUConverter : Editor {
		// Read a DTU file into memory
		public static DTU ParseDTUFile(string path)
		{

			var dtu = new DTU();
			dtu.DTUPath = path;
			dtu.UseSharedMaterialDir = false;
			dtu.UseSharedTextureDir = false;

			if (!System.IO.File.Exists(path))
			{
				Debug.LogError("DTU File: " + path + " does not exist");
				return dtu;
			}

			var text = System.IO.File.ReadAllText(path);

			if(text.Length<=0)
			{
				Debug.LogError("DTU File: " + path + " is empty");
				return dtu;
			}
			//text = CleanJSON(text);
			//var dtu = JsonUtility.FromJson<DTU>(text);


			var root = SimpleJSON.JSON.Parse(text);

			dtu.AssetID = root["Asset Id"].Value;
			dtu.AssetName = root["Asset Name"].Value;
			dtu.AssetType = root["Asset Type"].Value;
			dtu.ProductName = root["Product Name"].Value;
			dtu.ProductComponentName = root["Product Component Name"].Value;
			dtu.FBXFile = root["FBX File"].Value;
			dtu.ImportFolder = root["Import Folder"].Value;
			dtu.Materials = new List<DTUMaterial>();

			var materials = root["Materials"].AsArray;

			foreach(var matKVP in materials)
			{
				var mat = matKVP.Value;
				var dtuMat = new DTUMaterial();

				dtuMat.Version = mat["Version"].AsFloat;
				dtuMat.ProductName = dtu.ProductName;
				dtuMat.ProductComponentName = dtu.ProductComponentName;
				dtuMat.AssetName = mat["Asset Name"].Value;
				dtuMat.MaterialName = mat["Material Name"].Value;
				dtuMat.MaterialType = mat["Material Type"].Value;
				dtuMat.Value = mat["Value"].Value;
				dtuMat.Properties = new List<DTUMaterialProperty>();

				var properties = mat["Properties"];
				foreach(var propKVP in properties)
				{
					var prop = propKVP.Value;
					var dtuMatProp = new DTUMaterialProperty();

					//since this property was found, mark it
					dtuMatProp.Exists = true;

					dtuMatProp.Name = prop["Name"].Value;
					dtuMatProp.Texture = prop["Texture"].Value;
					var v = new DTUValue();

					var propDataType = prop["Data Type"].Value;
					if(propDataType == "Double")
					{
						v.Type = DTUValue.DataType.Double;
						v.AsDouble = prop["Value"].AsDouble;

					} else if(propDataType == "Integer")
					{
						v.Type = DTUValue.DataType.Integer;
						v.AsInteger = prop["Value"].AsInt;
					} else if(propDataType == "Float")
					{
						v.Type = DTUValue.DataType.Float;
						v.AsDouble = prop["Value"].AsFloat;
					} else if(propDataType == "String")
					{
						v.Type = DTUValue.DataType.String;
						v.AsString = prop["Value"].Value;
					} else if(propDataType == "Color")
					{
						v.Type = DTUValue.DataType.Color;
						var tmpStr = prop["Value"].Value;
						Color color;
						if(!ColorUtility.TryParseHtmlString(tmpStr,out color))
						{
							Debug.LogError("Failed to parse color hex code: " + tmpStr);
							throw new System.Exception("Invalid color hex code");
						}
						v.AsColor = color;
					} else if(propDataType == "Texture")
					{
						v.Type = DTUValue.DataType.Texture;

						//these values will be hex colors
						var tmpStr = prop["Value"].Value;
						Color color;
						if(!ColorUtility.TryParseHtmlString(tmpStr,out color))
						{
							Debug.LogError("Failed to parse color hex code: " + tmpStr);
							throw new System.Exception("Invalid color hex code");
						}
						v.AsColor = color;
					}

					else
					{
						Debug.LogError("Type: " + propDataType + " is not supported");
						throw new System.Exception("Unsupported type");
					}

					dtuMatProp.Value = v;

					dtuMat.Properties.Add(dtuMatProp);
				}

				dtu.Materials.Add(dtuMat);
			}

			return dtu;
		}


		/// <summary>
		/// Strips spaces from the json text in preparation for the JsonUtility (which doesn't handle spaces in keys)
		/// This won't appropriately handle the special Value/Data Type in the Properties array, but if you don't need that this cleaner may help you
		/// </summary>
		/// <param name="jsonRaw"></param>
		/// <returns></returns>
		protected static string CleanJSON(string jsonText)
		{
			//Converts something like "Asset Name" :  => "AssetName"
			// basically its... find something starting with whitespace, then a " then any space anywhere up to the next quote, but only the first occurance on the line
			// then only replace it with the first capture and third capture group, skipping the 2nd capture group (the space)
			var result = Regex.Replace(jsonText,"^(\\s+\"[^\"]+)([\\s]+)([^\"]+\"\\s*)","$1$3",RegexOptions.Multiline);
			return result;

		}

		/// <summary>
		/// Parses the DTU file and converts all materials and textures if dirty, will place next to DTU file
		/// </summary>
		[MenuItem("Daz3D/Extract materials from selected DTU", false, 102)]
		[MenuItem("Assets/Daz3D/Extract materials", false, 102)]
		public static void MenuItemConvert()
		{
			var path = Daz3DInstance.getSelectedDTUAssetPath();
			var dtu = ParseDTUFile(path);

			Debug.Log("DTU: " + dtu.AssetName + " contains: " + dtu.Materials.Count + " materials");

			foreach(var dtuMat in dtu.Materials)
			{
				dtu.ConvertToUnity(dtuMat, true);
			}
		}
		
		[MenuItem("Daz3D/Extract Hair materials", false, 102)]
		public static void HairShaderConvert()
		{
			var path = Daz3DInstance.getSelectedDTUAssetPath();
			var dtu = ParseDTUFile(path);

			Debug.Log("DTU: " + dtu.AssetName + " contains: " + dtu.Materials.Count + " materials");

			foreach(var dtuMat in dtu.Materials)
			{
				if (dtu.IsDTUMaterialHair(dtuMat))
				{
					dtu.ConvertToUnity(dtuMat, true);
				}
			}
		}
		
		[MenuItem("Daz3D/Extract Skin materials", false, 102)]
		public static void SkinShaderConvert()
		{
			var path = Daz3DInstance.getSelectedDTUAssetPath();
			var dtu = ParseDTUFile(path);

			Debug.Log("DTU: " + dtu.AssetName + " contains: " + dtu.Materials.Count + " materials");

			foreach(var dtuMat in dtu.Materials)
			{
				if (dtu.IsDTUMaterialSkin(dtuMat))
				{
					dtu.ConvertToUnity(dtuMat, true);
				}
			}
		}
	}
}