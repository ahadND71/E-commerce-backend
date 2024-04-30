using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Helpers 
{
  public class SlugGenerator
  {
    public static string GenerateSlug(string name){
      return name.ToLower().Replace(" ","-");
    }
  }
}