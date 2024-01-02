namespace Domain.Commands;

/// <summary>
/// 出力言語タイプ
/// </summary>
/// <remarks>言語を増やす場合はアイテムを追加する</remarks>
public enum OutputLanguageType
{
    /// <summary>
    /// C#
    /// </summary>
    CS,
}

/// <summary>
/// 追加パラメータ
/// </summary>
/// <remarks>言語を増やす場合はアイテムを追加する</remarks>
public enum ParamKeys
{
    /// <summary>
    /// C#用名前空間
    /// </summary>
    CS_NameSpace,

    /// <summary>
    /// インデントスペース数
    /// </summary>
    IndentSpaceCount,

    /// <summary>
    /// 固定プレフィックス
    /// </summary>
    Prefix,

    /// <summary>
    /// 固定サフィックス
    /// </summary>
    Suffix,
}

/// <summary>
/// ファイル出力コマンドクラス
/// </summary>
/// <param name="RootPath">ファイル出力のルートパス</param>
/// <param name="LanguageType">出力言語タイプ</param>
/// <param name="IndentSpaceCount">インデントスペース数</param>
/// <param name="Params">追加パラメータ</param>
public record FileOutputCommand(string RootPath, OutputLanguageType LanguageType, int IndentSpaceCount, Dictionary<ParamKeys, string> Params);