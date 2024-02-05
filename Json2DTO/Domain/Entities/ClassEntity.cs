using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// クラスエンティティ
/// </summary>
public class ClassEntity
{
    /// <summary>
    /// 名称
    /// </summary>
    /// <value>クラス名</value>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// プロパティリスト
    /// </summary>
    /// <returns>プロパティリスト</returns>
    public IReadOnlyList<PropertyValueObject> Properties { get { return _propertyies; } }

    /// <summary>
    /// 非公開コンストラクタ
    /// </summary>
    private ClassEntity()
    {
    }

    /// <summary>
    /// 非公開プロパティリスト
    /// </summary>
    /// <returns>非公開プロパティリスト</returns>
    private List<PropertyValueObject> _propertyies = new();

    /// <summary>
    /// プロパティ追加
    /// </summary>
    /// <param name="property">追加対象</param>
    public void AddProperty(PropertyValueObject property)
    {
        // 入力チェック
        if (property is null) throw new ArgumentException($"{nameof(property)} is null");

        // プロパティリスト追加
        _propertyies.Add(property);
    }

    /// <summary>
    /// インスタンス生成
    /// </summary>
    /// <param name="className">クラス名</param>
    /// <returns>クラスエンティティ インスタンス</returns>
    public static ClassEntity Create(string className)
    {
        // 入力チェック
        if (string.IsNullOrEmpty(className)) throw new ArgumentException($"{nameof(className)} is null");

        // インスタンスを返す
        return new ClassEntity()
        {
            Name = className
        };
    }
}