namespace PrintEngine.Core.Interfaces
{
    public interface IResourceService
	{
	}

    public interface IResourceService<TImage, TFont>
		: IResourceService
		where TImage : class
		where TFont : class
	{
		TImage GetImage(string key);
		TFont GetFont(string key);
	}
}
