using System;
using System.Collections.Generic;
using System.Linq;

namespace TinyDIContainer
{
  /// <summary>
  /// DIコンテナ
  /// </summary>
  public static class DIContainer
  {
    /// <summary>
    /// コンテナ本体
    /// </summary>
    private static readonly Dictionary<string, Type> Dict = new Dictionary<string, Type>();

    /// <summary>
    /// 追加
    /// </summary>
    /// <typeparam name="U">インターフェイス</typeparam>
    /// <typeparam name="V">インターフェイスを継承したクラス</typeparam>
    public static void Add<U, V>()
      where U : class
      where V : class
    {
      var classType = typeof(V);
      var interfaceType = typeof(U);
      if (classType.IsClass && classType.GetInterfaces().Contains(interfaceType))
      {
        Dict.Add(interfaceType.FullName, classType);
        return;
      }
      throw new Exception($"{interfaceType.Name},{classType.Name} Is Combination error");
    }

    /// <summary>
    /// インスタンス生成
    /// </summary>
    /// <typeparam name="U">インターフェイス</typeparam>
    /// <returns>インターフェイスを継承したクラスインスタンス</returns>
    public static U CreateInstance<U>()
      where U : class
    {
      var keyName = typeof(U).FullName;
      if (Dict.ContainsKey(keyName))
      {
        var classType = Dict[keyName];
        return Activator.CreateInstance(classType) as U;
      }
      throw new Exception($"{typeof(U).Name} Is Not Exists");
    }

    /// <summary>
    /// インスタンス生成
    /// </summary>
    /// <param name="interfaceType">インターフェイスのType</type>
    /// <returns>インターフェイスを継承したobject</returns>
    public static object CreateInstance(Type interfaceType)
    {
      var keyName = interfaceType.FullName;
      if (Dict.ContainsKey(keyName))
      {
        var classType = Dict[keyName];
        return Activator.CreateInstance(classType);
      }
      throw new Exception($"{interfaceType.Name} Is Not Exists");
    }
  }
}
