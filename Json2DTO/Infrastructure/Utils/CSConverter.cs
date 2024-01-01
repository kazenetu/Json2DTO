using System.Text;
using Domain.Commands;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure.Extensions;

namespace Infrastructure.Utils;

/// <summary>
/// C#ソースコード変換クラス
/// </summary>
public class CSConverter : IConverter
{
    /// <summary>
    /// 集合クラスインスタンス フィールド
    /// </summary>
    private readonly ClassesEntity ClassInstance;

    /// <summary>
    /// パラメータ フィールド
    /// </summary>
    private readonly Dictionary<ParamKeys, string> Params;

    /// <summary>
    /// ルートクラスインスタンス フィールド
    /// </summary>
    private readonly ClassEntity RootClass;

    /// <summary>
    /// インデントのスペース数
    /// </summary>
    private int IndentSpaceCount = 2;

    /// <summary>
    /// 固定プレフィックス
    /// </summary>
    private readonly string Prefix = string.Empty;

    /// <summary>
    /// 固定サフィックス
    /// </summary>
    private readonly string Suffix = string.Empty;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="classInstance">集約クラス</param>
    /// <param name="param">パラメータ</param>
    /// <returns>インスタンス<returns>
    public CSConverter(ClassesEntity classInstance, Dictionary<ParamKeys, string> param)
    {
        ClassInstance = classInstance;
        Params = param;

        // インデントパラメータを設定
        if (Params.ContainsKey(ParamKeys.IndentSpaceCount))
            IndentSpaceCount = int.Parse(Params[ParamKeys.IndentSpaceCount]);

        // 固定プレフィックスを設定
        if (Params.ContainsKey(ParamKeys.Prefix))
            Prefix = Params[ParamKeys.Prefix];

        // 固定サフィックスを設定
        if (Params.ContainsKey(ParamKeys.Suffix))
            Suffix = Params[ParamKeys.Suffix];

        // Rootを取得
        RootClass = ClassInstance.RootClass;
    }

    /// <summary>
    /// インスタンス生成
    /// </summary>
    /// <param name="classInstance">集約クラス</param>
    /// <param name="param">パラメータ</param>
    /// <returns>インスタンス<returns>
    public static IConverter Create(ClassesEntity classInstance, Dictionary<ParamKeys, string> param)
    {

        return new CSConverter(classInstance, param);
    }

    /// <summary>
    /// コード変更処理
    /// </summary>
    /// <returns>ソースコード文字列</returns>
    public string Convert()
    {
        var result = new StringBuilder();
        // using設定
        result.AppendLine("using System.Text;");
        result.AppendLine("using System.Text.Json.Serialization;");
        result.AppendLine();

        // 名前空間取得
        var namespaceName = string.Empty;
        if (Params.ContainsKey(ParamKeys.CS_NameSpace)) namespaceName = Params[ParamKeys.CS_NameSpace];

        // インデントレベル
        var indentLevel = 0;

        if (!string.IsNullOrEmpty(namespaceName))
        {
            result.AppendLine($"namespace {namespaceName}");
            result.AppendLine("{");
            indentLevel++;
        }

        // クラス生成
        result.Append(GetRootClassString(indentLevel));

        if (!string.IsNullOrEmpty(namespaceName))
        {
            result.Append('}');
        }

        return result.ToString();
    }

    /// <summary>
    /// class文字列生成して返す
    /// </summary>
    /// <param name="indentLevel">インデントレベル</param>
    /// <returns>class文字列</returns>
    string GetRootClassString(int indentLevel = 0)
    {
        // ルートクラスを出力
        return GetClassString(RootClass, indentLevel);
    }

    /// <summary>
    /// クラスエンティティからclass文字列生成して返す
    /// </summary>
    /// <param name="classEntity">クラスエンティティインスタンス</param>
    /// <param name="indentLevel">インデントレベル</param>
    /// <returns>class文字列</returns>
    string GetClassString(ClassEntity classEntity, int indentLevel = 0)
    {
        var result = new StringBuilder();

        // インデント設定
        var levelSpace = new string('S', indentLevel * IndentSpaceCount).Replace("S", " ");
        result.AppendLine($"{levelSpace}public class {Prefix}{classEntity.Name}{Suffix}");
        result.AppendLine($"{levelSpace}{{");

        if (classEntity == RootClass)
        {
            // インナークラスのクラス文字列作成
            foreach (var classInstance in ClassInstance.InnerClasses)
            {
                result.AppendLine($"{GetClassString(classInstance, indentLevel + 1)}");
            }
        }

        // プロパティ文字列作成
        var isNewLine = false;
        foreach (var property in classEntity.Properties)
        {
            if (isNewLine) result.AppendLine();
            result.Append($"{GetPropertyString(property, indentLevel + 1)}");
            isNewLine = true;
        }

        result.AppendLine($"{levelSpace}}}");

        return result.ToString();
    }

    /// <summary>
    /// プロパティValueObjectからプロパティ文字列を作成して返す
    /// </summary>
    /// <param name="property">プロパティValueObject</param>
    /// <param name="indentLevel">インデントレベル</param>
    /// <returns>プロパティ文字列</returns>
    string GetPropertyString(PropertyValueObject property, int indentLevel)
    {
        var result = new StringBuilder();

        // インデント設定
        var levelSpace = new string('S', indentLevel * IndentSpaceCount).Replace("S", " ");

        // プロパティ文字列作成
        var (proptyBase, attribute) = GetPropertyBaseString(property);
        if (!string.IsNullOrEmpty(attribute))
        {
            result.Append($"{levelSpace}{attribute}");
        }
        result.AppendLine($"{levelSpace}public {proptyBase}");

        return result.ToString();
    }

    /// <summary>
    /// プロパティValueObjectからプロパティ文字列のベースを作成して返す
    /// </summary>
    /// <param name="property">プロパティValueObject</param>
    /// <returns>プロパティ文字列のベースと属性のタプル</returns>
    (string propertyBaseString, string attribute) GetPropertyBaseString(PropertyValueObject property)
    {
        // C#型取得
        var typeName = property.Type switch
        {
            { Kind: PropertyType.Kinds.String } => "string",
            { Kind: PropertyType.Kinds.Decimal } => "decimal",
            { Kind: PropertyType.Kinds.Bool } => "bool",
            { Kind: PropertyType.Kinds.Null } => "object",
            { Kind: PropertyType.Kinds.Class, IsList: true } => $"{Prefix}{property.PropertyTypeClassName}{Suffix}",
            { Kind: PropertyType.Kinds.Class, IsList: false } => $"{Prefix}{property.PropertyTypeClassName}{Suffix}?",
            _ => throw new Exception($"{nameof(property)} has no type set"),
        };

        // デフォルト値
        var defualtValue = property.Type?.Kind switch
        {
            PropertyType.Kinds.String => "string.Empty",
            PropertyType.Kinds.Null => "string.Empty",
            _ => String.Empty,
        };

        // Listの場合はNullableにする
        if (property.Type!.IsList)
        {
            typeName = $"List<{typeName}>?";
        }

        // デフォルト文字列設定
        var defualt = string.Empty;
        if (defualtValue is not "")
        {
            defualt = $" = {defualtValue};";
        }

        // C#のプロパティを設定
        var codeProprty = property.Name.ToCSharpNaming();

        // 属性追加確認
        var attribute = string.Empty;
        if (codeProprty != property.Name)
        {
            attribute = $"[JsonPropertyName(\"{property.Name}\")]" + Environment.NewLine;
        }

        return ($"{typeName} {codeProprty} {{ set; get; }}{defualt}", attribute);
    }
}