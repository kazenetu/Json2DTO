namespace Appplication.Commands;

/// <summary>
/// C#用コマンドクラス
/// </summary>
/// <param name="NameSpace">名前空間</param>
/// <param name="RootPath">出力ルートパス</param>
/// <param name="RootClassName">ルートクラス名(ファイル名)</param>
/// <param name="IndentSpaceCount">インデントスペース数</param>
/// <param name="Prefix">固定プレフィックス</param>
/// <param name="Suffix">固定サフィックス</param>
public record CSharpCommand(string NameSpace, string RootPath, string RootClassName, int IndentSpaceCount = 2, string Prefix = "", string Suffix = "") : CommonCommand(Prefix, Suffix);