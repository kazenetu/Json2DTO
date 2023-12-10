using Domain.ValueObjects;

namespace Domain.Entities;

/// <summary>
/// クラス集約エンティティ
/// </summary>
public class ClassesEntity
{
    /// <summary>
    /// 非公開インナークラスリスト
    /// </summary>
    /// <returns>非公開インナークラスリスト</returns>
    public List<ClassEntity> innerClasses = new();

    /// <summary>
    /// 読み取り用インナークラスリスト
    /// </summary>
    /// <returns>読み取り用インナークラスリスト</returns>
    public IReadOnlyList<ClassEntity> InnerClasses
    {
        get => innerClasses;
    }

    /// <summary>
    /// ルートクラス
    /// </summary>
    /// <returns>ルートクラス</returns>
    private ClassEntity? rootClass = null;

    /// <summary>
    /// 読み取り用ルートクラス
    /// </summary>
    /// <returns>読み取り用ルートクラス</returns>
    public ClassEntity RootClass
    {
        get
        {
            var newInstance = ClassEntity.Create(rootClass is null ? string.Empty : rootClass.Name);
            foreach (var prop in rootClass!.Properties)
            {
                newInstance.AddProperty(prop);
            }
            return newInstance;
        }
    }

    /// <summary>
    /// ルートクラスのクラス名を返す
    /// </summary>
    /// <returns>ルートクラス</returns>
    public string Name
    {
        get => rootClass?.Name ?? "RootClass";
    }

    /// <summary>
    /// 非公開コンストラクタ
    /// </summary>
    private ClassesEntity()
    {
    }

    /// <summary>
    /// ルートクラスのプロパティ追加
    /// </summary>
    /// <param name="Property">追加対象</param>
    public void AddRootProperty(PropertyValueObject Property)
    {
        // ルートクラス存在チェック
        if (rootClass is null) throw new Exception($"{nameof(rootClass)} is null");

        // プロパティ追加
        rootClass?.AddProperty(Property);
    }

    /// <summary>
    /// インナークラスの追加
    /// </summary>
    /// <param name="innerClass">追加対象</param>
    public void AddInnerClass(ClassEntity innerClass)
    {
        // 入力チェック
        if (innerClass is null) throw new ArgumentException($"{nameof(innerClass)} is null");

        // インナークラスリストに追加
        innerClasses.Add(innerClass!);
    }

    /// <summary>
    /// インスタンス生成
    /// </summary>
    /// <param name="rootClassName">ルートクラス名</param>
    /// <returns>クラス集約エンティティ インスタンス</returns>
    public static ClassesEntity Create(string rootClassName)
    {
        // 入力チェック
        if (rootClassName is null) throw new ArgumentException($"{nameof(rootClassName)} is null");

        // インスタンスを返す
        var result = new ClassesEntity()
        {
            rootClass = ClassEntity.Create(rootClassName!)
        };

        return result;
    }
}