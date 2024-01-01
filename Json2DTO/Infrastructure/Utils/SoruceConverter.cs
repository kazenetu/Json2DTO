using Domain.Commands;
using Domain.Entities;

namespace Infrastructure.Utils;

/// <summary>
/// ソースコンバーター
/// </summary>
internal static class SoruceConverter
{
    /// <summary>
    /// C#コードを作成する
    /// </summary>
    /// <param name="classInstance">Class集約エンティティ</param>
    /// <param name="indentLevel">インデントレベル</param>
    /// <param name="namespaceName">名前空間</param>
    /// <param name="indentSpaceCount">インデントスペース数</param>
    /// <param name="Prefix">固定プレフィックス</param>
    /// <param name="Suffix">固定サフィックス</param>
    /// <returns>C#コード</returns>
    public static string ToCsCode(ClassesEntity classInstance, int indentLevel = 0, string? namespaceName = null, int indentSpaceCount = 2, string Prefix = "", string Suffix = "")
    {
        // パラメータ設定
        var param = new Dictionary<ParamKeys, string>();
        if (!string.IsNullOrEmpty(namespaceName))
        {
            param.Add(ParamKeys.CS_NameSpace, namespaceName);
        }
        param.Add(ParamKeys.IndentSpaceCount, indentSpaceCount.ToString());
        param.Add(ParamKeys.Prefix, Prefix);
        param.Add(ParamKeys.Suffix, Suffix);

        var instance = CSConverter.Create(classInstance, param);
        return instance.Convert();
    }
}