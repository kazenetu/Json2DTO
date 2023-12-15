using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// JSON読み込みリポジトリインターフェース
/// </summary>
public interface IJsonRepository
{
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
