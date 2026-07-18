using System;
using System.Collections;
using System.Collections.Generic;
using NvimClient.NvimMsgpack;
using NvimClient.NvimMsgpack.Models;
using Xunit;

namespace NvimClient.Test
{
  public class NvimTypesMapTests
  {
    [Theory]
    [InlineData(typeof(bool), true)]
    [InlineData(typeof(int), false)]
    [InlineData(typeof(long), true)]
    [InlineData(typeof(object[]), true)]
    [InlineData(typeof(long[]), true)]
    [InlineData(typeof(int[]), false)]
    [InlineData(typeof(IDictionary), true)]
    [InlineData(typeof(IDictionary<object, object>), true)]
    [InlineData(typeof(IDictionary<long, string>), true)]
    [InlineData(typeof(IDictionary<string, DateTime>), false)]
    [InlineData(typeof(IDictionary<Random, long>), false)]
    public void TypeValidityMatchesExpected(Type type, bool shouldBeValid)
    {
      Assert.Equal(shouldBeValid, NvimTypesMap.IsValidType(type));
    }

    [Theory]
    [InlineData("Boolean", "bool")]
    [InlineData("Array", "object[]")]
    [InlineData("Dict", "IDictionary")]
    [InlineData("DictAs(get_mode)", "IDictionary")]
    [InlineData("DictOf(Integer)", "IDictionary")]
    [InlineData("Dict(win_config)", "IDictionary")]
    [InlineData("Dictionary", "IDictionary")]
    [InlineData("ArrayOf(Float)", "double[]")]
    [InlineData("ArrayOf(Integer, 2)", "long[]")]
    [InlineData("ArrayOf(Buffer)", "NvimBuffer[]")]
    [InlineData(
      "ArrayOf(DictionaryOf(String, String))",
      "IDictionary<string, string>[]"
    )]
    [InlineData(
      "DictionaryOf(Integer, ArrayOf(String))",
      "IDictionary<long, string[]>"
    )]
    public void NvimTypeMapsToExpectedCSharpType(
      string nvimType,
      string csharpType
    )
    {
      Assert.Equal(csharpType, NvimTypesMap.GetCSharpType(nvimType));
    }
  }
}
