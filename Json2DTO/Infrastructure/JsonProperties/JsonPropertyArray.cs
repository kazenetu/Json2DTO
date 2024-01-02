using System.Text.Json;
using Domain.ValueObjects;

namespace Infrastructure.JsonProperties;

/// <summary>
/// JSONプロパティ：Array
/// </summary>
public class JsonPropertyArray : IJsonProperty
{
    /// <summary>
    /// キー名
    /// </summary>
    /// <returns>JsonValueKind</returns>
    public JsonValueKind GetKeyName()
    {
        return JsonValueKind.Array;
    }

    /// <summary>
    /// Jsonプロパティ結果の取得
    /// </summary>
    /// <param name="element">対象インスタンス</param>
    /// <param name="innerClassNo">インナークラス番号</param>
    /// <returns>Jsonプロパティ結果</returns>
    public JsonPropertyResult GetJsonPropertyResult(JsonProperty element, int innerClassNo)
    {
        var innerClassJson = string.Empty;
        Type? propertyType = null;
        var arrayIndex = 0;
        while (arrayIndex < element.Value.GetArrayLength())
        {
            var ValueKind = element.Value[arrayIndex].ValueKind;

            // クラス作成
            if (ValueKind == JsonValueKind.Object)
            {
                // インナークラス番号をインクリメント
                innerClassNo++;

                // インナークラス用JSON文字列を格納
                innerClassJson = element.Value[arrayIndex].ToString();

                break;
            }
            propertyType = GetPropertyType(element.Value[arrayIndex]);
            break;
        }

        // プロパティ生成
        PropertyValueObject prop;
        if (string.IsNullOrEmpty(innerClassJson))
        {
            // Typeチェック
            if (propertyType is null) throw new Exception($"{nameof(propertyType)} is null");

            // 値プロパティ
            prop = new PropertyValueObject(element.Name, new PropertyType(propertyType, true));
        }
        else
        {
            // インナークラス
            prop = new PropertyValueObject(element.Name, new PropertyType(innerClassNo, true));
        }

        return new JsonPropertyResult(prop, innerClassJson, innerClassNo);
    }

    /// <summary>
    /// プロパティのC#の型を取得する
    /// </summary>    
    /// <param name="element">対象インスタンス</param>
    /// <returns>C#の型</returns>
    protected static Type GetPropertyType(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => typeof(string),
            JsonValueKind.Number => typeof(decimal),
            JsonValueKind.True => typeof(bool),
            JsonValueKind.False => typeof(bool),
            JsonValueKind.Null => typeof(Nullable),
            _ => throw new Exception($"{element.ValueKind} is can not use")
        };
    }
}