namespace api.Helpers
{
  public class SlugGenerator
  {
    public static string GenerateSlug(string name)
    {
      return name.ToLower().Replace(" ", "-");
    }
  }
}