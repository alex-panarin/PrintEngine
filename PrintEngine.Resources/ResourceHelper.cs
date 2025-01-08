namespace PrintEngine.Resources
{
	public static class ResourceHelper
	{
		public static byte[]? GetImage(string name)
		{
			var obj = Images.ResourceManager.GetObject(name);
			if (obj == null)
				return null;

			return (byte[])obj;
		}

		public static byte[]? GetFont(string name)
		{
			var obj = Fonts.ResourceManager.GetObject(name);
			if (obj == null)
				return null;

			return (byte[])obj;
		}

		public static string GetMetadata(string metadataName) 
		{
			var metadata = Metadatas.ResourceManager.GetObject(metadataName);
			return $"{metadata}";
		}
	}
}