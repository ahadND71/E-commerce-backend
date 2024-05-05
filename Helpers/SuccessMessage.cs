namespace api.Helpers
{
  public class SuccessMessage<T>
  {
    public bool Success { get; set; } = true;
    public string? Message { get; set; }
    public T? Data { get; set; }
    public PaginationMeta? Meta { get; set; }

  }

  public class PaginationMeta
  {
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
  }
}
