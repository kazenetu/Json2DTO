namespace Appplication.Models;

/// <summary>
/// ソースコード変換結果
/// </summary>
/// <param name="Success">変換成功か否か</param>
/// <param name="FileName">出力ファイル名</param>
/// <param name="SourceCode">変換後のソースコード</param>
public record ConvertResultModel(bool Success, string FileName, string SourceCode = "");