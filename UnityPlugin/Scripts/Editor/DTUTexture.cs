using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Daz3D
{
       
	public struct DTUMaterial
	{
		public float Version;
		public string ProductName;
		public string ProductComponentName;
		public string AssetName;
		public string MaterialName;
		public string MaterialType;
		public string Value;

		public List<DTUMaterialProperty> Properties;

		private Dictionary<string,DTUMaterialProperty> _map;

		public Dictionary<string,DTUMaterialProperty> Map
		{
			get
			{
				if(_map == null || _map.Count == 0)
				{
					_map = new Dictionary<string, DTUMaterialProperty>();
					foreach(var prop in Properties)
					{
						_map[prop.Name] = prop;
					}
				}

				return _map;
			}
		}

		public bool HasProperty(string key)
        {
			return Map.ContainsKey(key);
        }

		public DTUMaterialProperty Get(string key)
		{
			if(Map.ContainsKey(key))
			{
				return Map[key];
			}
			return new DTUMaterialProperty();
		}

		// DB (2021-05-14): new override which returns defaultValue if property does not exist
		public DTUMaterialProperty Get(string key, DTUValue defaultValue)
		{
			if (Map.ContainsKey(key))
			{
				return Map[key];
			}

			DTUMaterialProperty newProp = new DTUMaterialProperty();
			newProp.Value = defaultValue;
			return newProp;

		}

	}

	public struct DTUMaterialProperty
	{
		public string Name;
		public DTUValue Value;
		public string Texture;
		public bool TextureExists() { return !string.IsNullOrEmpty(Texture); }

		/// <summary>
		/// True if this property was found in the DTU
		/// </summary>
		public bool Exists;

		public Color Color
		{
			get {
				return Value.AsColor;
			}
		}

		public float ColorStrength
		{
			get {
				return Daz3D.Utilities.GetStrengthFromColor(Color);
			}
		}

		public float Float
		{
			get {
				return (float)Value.AsDouble;
			}
		}

		public bool Boolean
		{
			get {
				return Value.AsDouble > 0.5;
			}
		}
	}

	public struct DTUValue
	{
		public enum DataType {
			Integer,
			Float,
			Double,
			Color,
			String,
			Texture,
		};

		public DataType Type;

		public int AsInteger;
		public float AsFloat;
		public double AsDouble;
		public Color AsColor;
		public string AsString;

		public override string ToString()
		{
			switch(Type)
			{
				case DataType.Integer:
					return "int:"+AsInteger.ToString();
				case DataType.Float:
					return "float:"+AsFloat.ToString();
				case DataType.Double:
					return "double:"+AsDouble.ToString();
				case DataType.Color:
					return "color:"+AsColor.ToString();
				case DataType.String:
					return AsString;
				default:
					throw new System.Exception("Unsupported type");
			}
		}

		public DTUValue(double value)
        {
			AsDouble = value;
			Type = DataType.Double;
			AsFloat = (float)value;
			AsInteger = (int) value;
			AsColor = new Color((float) value, (float) value, (float) value);
			AsString = "";
        }

	}
}