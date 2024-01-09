using Domain.Entities;
using Domain.Interfaces;
using Domain.ValueObjects;
using Infrastructure;

namespace Json2DTO.Test.Application.Stub;

/// <summary>
/// JSON読み込みリポジトリのスタブ
/// </summary>
public class JsonRepositoryStub : IJsonRepository
{
    public enum Mode
    {
        None,
        JsonString,
        FilePath,
        DirectoryPath
    };

    /// <summary>
    /// 結果の状態
    /// </summary>
    public static Mode IsResult { set;get; } = Mode.None;

    /// <summary>
    /// Json文字列か否か
    /// </summary>
    /// <param name="target">確認対象文字列</param>
    /// <returns>Json文字列/以外</returns>
    public bool IsJsonString(string target)
    {
        return IsResult == Mode.JsonString;
    }

    /// <summary>
    /// ファイルパスか否か
    /// </summary>
    /// <param name="target">確認対象文字列</param>
    /// <returns>ファイルパス/以外</returns>
    public bool IsFilePath(string target)
    {
        return IsResult == Mode.FilePath;
    }

    /// <summary>
    /// ディレクトリパスか否か
    /// </summary>
    /// <param name="target">確認対象文字列</param>
    /// <returns>ディレクトリパス/以外</returns>
    public bool IsDirectoryPath(string target)
    {
        return IsResult == Mode.DirectoryPath;
    }

    /// <summary>
    /// ディレクトリ内のJSONファイルを読み込んでClass情報リストを返す
    /// </summary>
    /// <param name="filePath">JSONファイル</param>
    /// <returns>Classエンティティリスト</returns>
    public IReadOnlyList<ClassesEntity> CreateClassEntityFromFiles(string directoryPath)
    {
        var classInstances = new List<ClassesEntity>();

        var classInstance1 = ClassesEntity.Create("rootClassName");
        var proprtyType1 = new PropertyType(typeof(string), false);
        var propertyValueObject1 = new PropertyValueObject("Name", proprtyType1);
        classInstance1.AddRootProperty(propertyValueObject1);
        classInstances.Add(classInstance1);

        var classInstance2 = ClassesEntity.Create("rootClassNameA");
        var proprtyType2 = new PropertyType(typeof(string), false);
        var propertyValueObject2 = new PropertyValueObject("snake_name", proprtyType2);
        classInstance2.AddRootProperty(propertyValueObject2);
        classInstances.Add(classInstance2);

        return classInstances;
    }

    /// <summary>
    /// JSONファイルを読み込んでClass情報を返す
    /// </summary>
    /// <param name="filePath">JSONファイル</param>
    /// <returns>Classエンティティ</returns>
    public ClassesEntity CreateClassEntityFromFile(string filePath)
    {
        var classInstance = ClassesEntity.Create("rootClassName");
        var proprtyType = new PropertyType(typeof(string), false);
        var propertyValueObject = new PropertyValueObject("Name", proprtyType);
        classInstance.AddRootProperty(propertyValueObject);

        return classInstance;
    }

    /// <summary>
    /// JSON文字列を読み込んでClass情報を返す
    /// </summary>
    /// <param name="json">JSO文字列</param>
    /// <returns>Classエンティティ</returns>
    /// <param name="rootClassName">ルートクラス名</param>
    public ClassesEntity CreateClassEntityFromString(string json, string rootClassName)
    {
        var classInstance = ClassesEntity.Create(rootClassName);
        var proprtyType = new PropertyType(typeof(string), false);
        var propertyValueObject = new PropertyValueObject("Name", proprtyType);
        classInstance.AddRootProperty(propertyValueObject);

        return classInstance;
    }

}