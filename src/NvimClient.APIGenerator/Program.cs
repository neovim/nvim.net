namespace NvimClient.APIGenerator
{
  internal static class Program
  {
    public static void Main() =>
      NvimAPIGenerator.GenerateCSharpFile(
        "../../../../NvimClient.API/NvimAPI.generated.cs");
  }
}
