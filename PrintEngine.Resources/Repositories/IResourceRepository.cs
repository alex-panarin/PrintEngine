namespace PrintEngine.Resources.Repositories
{
	public interface IResourceRepository
	{
		byte[]? GetFontBytes(string name);
		byte[]? GetImageBytes(string name);
	}
}
