using System.Text.Json;
using Domain.ValueObjects;

namespace Infrastructure.JsonProperties;

/// <summary>
/// JSONプロパティ：Object
/// </summary>
public class JsonPropertyObject : IJsonProperty
{
    /// <summary>
    /// キー名
    /// </summary>
    /// <returns>JsonValueKind</returns>
    public JsonValueKind GetKeyName()
    {
        return JsonValueKind.Object;
    }

    /// <summary>
    /// Jsonプロパティ結果の取得
    /// </summary>
    /// <param name="element">対象インスタンス</param>
    /// <param name="innerClassNo">インナークラス番号</param>
    /// <returns>Jsonプロパティ結果</returns>
    public JsonPropertyResult GetJsonPropertyResult(JsonProperty element, int innerClassNo)
    {
        // インナークラス番号をインクリメント
        innerClassNo++;

        // インナークラス用JSON文字列を格納
        var innerClassJson = element.Value.ToString();

        // インナークラス
        var prop = new PropertyValueObject(element.Name, new PropertyType(innerClassNo, false));

        return new JsonPropertyResult(prop, innerClassJson, innerClassNo);
    }
}