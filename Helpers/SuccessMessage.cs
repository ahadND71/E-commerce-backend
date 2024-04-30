namespace api.Helpers
{
  public class SuccessMessage<T>
  {
    public bool Success { get; set; } = true;
    public string? Message { get; set; }
    public T? Data { get; set; }
  }
}