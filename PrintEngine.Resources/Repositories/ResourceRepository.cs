using System.Collections.Concurrent;

namespace PrintEngine.Resources.Repositories
{
	public class ResourceRepository
		: IResourceRepository
	{
		private readonly ConcurrentDictionary<string, byte[]?> _storage = new();
		public byte[]? GetFontBytes(string name)
		{
			return _storage.GetOrAdd(name, (k) => ResourceHelper.GetFont(k));
		}
		public byte[]? GetImageBytes(string name)
		{
			return _storage.GetOrAdd(name, (k) => ResourceHelper.GetImage(k));
		}
	}
}
