using System.Text.Json;

namespace Infrastructure.JsonProperties;

/// <summary>
/// Jsonプロパティインターフェイス
/// </summary>
public interface IJsonProperty
{
    /// <summary>
    /// キー名
    /// </summary>
    /// <returns>JsonValueKind</returns>
    JsonValueKind GetKeyName();

    /// <summary>
    /// Jsonプロパティ結果の取得
    /// </summary>
    /// <param name="element">対象インスタンス</param>
    /// <param name="innerClassNo">インナークラス番号</param>
    /// <returns>Jsonプロパティ結果</returns>
    JsonPropertyResult GetJsonPropertyResult(JsonProperty element, int innerClassNo);

}