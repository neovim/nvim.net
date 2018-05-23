using System;
using System.Linq;
using System.Reflection;

namespace NvimClient
{
  internal static class EnumUtil
  {
    /// <summary>
    ///   Gets an attribute on an enum field value.
    /// </summary>
    /// <typeparam name="T">The type of the attribute to get.</typeparam>
    /// <param name="enumValue">The enum value.</param>
    /// <returns>
    /// The attribute of type T that exists on the enum value.
    /// </returns>
    public static T GetAttribute<T>(Enum enumValue) where T : Attribute
    {
      var type       = enumValue.GetType();
      var valueName  = Enum.GetName(type, enumValue);
      var memberInfo = type.GetMember(valueName).First();
      return memberInfo.GetCustomAttribute<T>();
    }
  }
}
