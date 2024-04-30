using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helpers
{
  public class ErrorMessage
  {
    public bool Success { get; set; } = false;
    public string? Message { get; set; }
  }
}