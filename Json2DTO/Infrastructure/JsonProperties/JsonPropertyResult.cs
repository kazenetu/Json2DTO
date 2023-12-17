using Domain.ValueObjects;

namespace Infrastructure.JsonProperties;

/// <summary>
/// JsonPropertyの戻り値
/// </summary>
/// <param name="PropertyValueObject">プロパティ値オブジェクト</param>
/// <param name="InnerClassJson">インナークラス用JSON文字列</param>
public record JsonPropertyResult(PropertyValueObject PropertyValueObject, string InnerClassJson, int innerClasssNo);