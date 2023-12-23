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
    /// ディレクトリ内のJSONファイルを読み込んでClass情報リストを返す
    /// </summary>
    /// <param name="filePath">JSONファイル</param>
    /// <returns>Classエンティティリスト</returns>
    public IReadOnlyList<ClassesEntity> CreateClassEntityFromFiles(string directoryPath)
    {
        // パラメータチェック
        if(!Directory.Exists(directoryPath)) throw new ArgumentException($"{directoryPath} is not directory");

        var files = Directory.EnumerateDirectories(directoryPath);
        if (files.Any()) throw new ArgumentException($"{directoryPath} is not file");

        // すべてのファイルの解析結果をリストに追加
        var result = new List<ClassesEntity>();
        foreach(var filePath in files)
        {
            result.Add(CreateClassEntityFromFile(filePath));
        }
        return result;
    }

    /// <summary>
    /// JSONファイルを読み込んでClass情報を返す
    /// </summary>
    /// <param name="filePath">JSONファイル</param>
    /// <returns>Classエンティティ</returns>
    public ClassesEntity CreateClassEntityFromFile(string filePath)
    {
        // パラメータチェック
        if(!File.Exists(filePath)) throw new ArgumentException($"{filePath} is not exixts");

        // ファイル読み込み
        var json = File.ReadAllText(filePath);

        // 文字列として読み取り
        var rootClassName = Path.GetFileNameWithoutExtension(filePath);
        return CreateClassEntityFromString(json, rootClassName);
    }

    /// <summary>
    /// JSON文字列を読み込んでClass情報を返す
    /// </summary>
    /// <param name="json">JSO文字列</param>
    /// <returns>Classエンティティ</returns>
    /// <param name="rootClassName">ルートクラス名</param>
    public ClassesEntity CreateClassEntityFromString(string json, string rootClassName)
    {
        // パラメータチェック
        if (string.IsNullOrEmpty(json)) throw new ArgumentException($"{nameof(json)} is null");
        if (string.IsNullOrEmpty(rootClassName)) throw new ArgumentException($"{nameof(rootClassName)} is null");
        if(rootClassName.Length > 1)
        {
            rootClassName = $"{rootClassName.Substring(0, 1).ToUpper()}{rootClassName.Substring(1)}";
        }
        else
        {
            rootClassName = rootClassName.Substring(0, 1).ToUpper();
        }

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
        // JSON文字列をパース
        JsonDocument jsonDocument;
        try
        {
            jsonDocument = JsonDocument.Parse(json);
        }
        catch
        {
            throw new ArgumentException($"JSON parse error:{json}");
        }

        // Classインスタンス設定
        var classEntity = ClassEntity.Create(className);

        // ルート要素と子要素配列を取得
        var rootElement = jsonDocument.RootElement;
        var elements = rootElement.EnumerateObject();

        // 要素がない場合はエラーとする
        if (!elements.Any()) throw new ArgumentException($"JSON elements none:{json}");

        // JSON文字列解析・変換
        foreach (var element in elements)
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