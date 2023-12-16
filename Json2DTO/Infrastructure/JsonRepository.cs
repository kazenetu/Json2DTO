using System.Text.Json;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.JsonProperties;

namespace Infrastructure;

/// <summary>
/// JSON読み込みリポジトリ
/// </summary>
public class JsonRepository : IJsonRepository
{
    /// <summary>
    /// Jsonプロパティリスト
    /// </summary>
    /// <typeparam name="IJsonProperty">Jsonプロパティインターフェイス</typeparam>
    private static readonly IReadOnlyList<IJsonProperty> JsonProperties = new List<IJsonProperty>()
    {
        new JsonPropertyObject(),
        new JsonPropertyArray(),
        new JsonPropertyString(),
        new JsonPropertyNumber(),
        new JsonPropertyTrue(),
        new JsonPropertyFalse(),
        new JsonPropertyNull()
    };


    /// <summary>
    /// JSONファイルを読み込んでClass情報を返す
    /// </summary>
    /// <param name="filePath">JSONファイル</param>
    /// <returns>Classエンティティ</returns>
    public ClassesEntity CreateClassEntityFromFile(string filePath)
    {
        var result = string.Empty;

        // ファイル読み込み
        result = File.ReadAllText(filePath);

        // 文字列として読み取り
        var rootClassName = Path.GetFileNameWithoutExtension(filePath);
        rootClassName = $"{rootClassName.Substring(0, 1).ToUpper()}{rootClassName.Substring(1)}";
        return CreateClassEntityFromString(result, rootClassName);
    }

    /// <summary>
    /// JSON文字列を読み込んでClass情報を返す
    /// </summary>
    /// <param name="json">JSO文字列</param>
    /// <returns>Classエンティティ</returns>
    /// <param name="rootClassName">ルートクラス名</param>
    public ClassesEntity CreateClassEntityFromString(string json, string rootClassName)
    {
        rootClassName = $"{rootClassName.Substring(0, 1).ToUpper()}{rootClassName.Substring(1)}";
        var classesEntity = ClassesEntity.Create(rootClassName);

        // JSON文字列読み込み
        JsonParse(json, rootClassName, ref classesEntity, 0);

        return classesEntity;
    }

    /// <summary>
    /// インスタンス生成
    /// </summary>
    /// <param name="json">JSON文字列</param>
    /// <param name="className">クラス名</param>
    /// <param name="classesEntity">集約エンティティ</param>
    /// <returns>クラスエンティティ インスタンス</returns>
    private ClassEntity JsonParse(string json, string className, ref ClassesEntity classesEntity, int innerClassNo)
    {
        return ProcessJsonDocument(json, className, ref classesEntity, innerClassNo);
    }

    /// <summary>
    /// JsonDocumentで構造を解析する
    /// </summary>    
    /// <param name="json">JSON文字列</param>
    /// <param name="className">クラス名</param>
    /// <param name="classesEntity">集約エンティティ</param>
    /// <param name="innerClassNo">インナークラス番号</param>
    private ClassEntity ProcessJsonDocument(string json, string className, ref ClassesEntity classesEntity, int innerClassNo)
    {
        // Classインスタンス設定
        var classEntity = ClassEntity.Create(className);

        var jsonDocument = JsonDocument.Parse(json);
        var rootElement = jsonDocument.RootElement;
        foreach (var element in rootElement.EnumerateObject())
        {
            var jsonProperty = JsonProperties.Where(item => item.GetKeyName() == element.Value.ValueKind).FirstOrDefault();
            if (jsonProperty is null) throw new Exception($"{element.Value.ValueKind} is can not use");

            // プロパティ追加
            var jsonPropertyResult = jsonProperty.GetJsonPropertyResult(element, innerClassNo);
            if (className == classesEntity.Name)
            {
                classesEntity.AddRootProperty(jsonPropertyResult.PropertyValueObject);
            }
            else
            {
                classEntity.AddProperty(jsonPropertyResult.PropertyValueObject);
            }

            // インナークラス追加
            innerClassNo = jsonPropertyResult.innerClasssNo;
            if (!string.IsNullOrEmpty(jsonPropertyResult.InnerClassJson))
            {
                classesEntity.AddInnerClass(JsonParse(jsonPropertyResult.InnerClassJson, jsonPropertyResult.PropertyValueObject.PropertyTypeClassName, ref classesEntity, innerClassNo));
            }
        }

        return classEntity;
    }
}