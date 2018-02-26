namespace ProductsApp.Dashboard.Util {
  public static class NullExtensions {
    public static bool IsNull<T> (this T obj) where T : class {
      return obj == null;
      }

    public static bool IsNull<T> (this T? obj) where T : struct {
      return !obj.HasValue;
      }
    }
  }
