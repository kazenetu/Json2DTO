using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// JSON読み込みリポジトリインターフェース
/// </summary>
public interface IJsonRepository
{
    /// <summary>
    /// Json文字列か否か
    /// </summary>
    /// <param name="target">確認対象文字列</param>
    /// <returns>Json文字列/以外</returns>
    bool IsJsonString(string target);

    /// <summary>
    /// ファイルパスか否か
    /// </summary>
    /// <param name="target">確認対象文字列</param>
    /// <returns>ファイルパス/以外</returns>
    bool IsFilePath(string target);

    /// <summary>
    /// ディレクトリパスか否か
    /// </summary>
    /// <param name="target">確認対象文字列</param>
    /// <returns>ディレクトリパス/以外</returns>
    bool IsDirectoryPath(string target);

    /// <summary>
    /// Json文字列/ファイルパス/ディレクトリパスを読み込んで Classエンティティリストを返す
    /// </summary>
    /// <param name="target">対象文字列(Json文字列/ファイルパス/ディレクトリパス)</param>
    /// <param name="className">クラス名(Json文字列時のみ必須)</param>
    /// <returns>Classエンティティリスト</returns>
    IReadOnlyList<ClassesEntity> CreateClassEntity(string target, string className);

    /// <summary>
    /// ディレクトリ内のJSONファイルを読み込んでClass情報リストを返す
    /// </summary>
    /// <param name="filePath">JSONファイル</param>
    /// <returns>Classエンティティリスト</returns>
    IReadOnlyList<ClassesEntity> CreateClassEntityFromFiles(string directoryPath);

    /// <summary>
    /// JSONファイルを読み込んでClass情報を返す
    /// </summary>
    /// <param name="filePath">JSONファイル</param>
    /// <returns>Classエンティティ</returns>
    ClassesEntity CreateClassEntityFromFile(string filePath);

    /// <summary>
    /// JSON文字列を読み込んでClass情報を返す
    /// </summary>
    /// <param name="filePath">JSO文字列</param>
    /// <param name="rootClassName">ルートクラス名</param>
    /// <returns>Classエンティティ</returns>
    ClassesEntity CreateClassEntityFromString(string json, string rootClassName);
}
