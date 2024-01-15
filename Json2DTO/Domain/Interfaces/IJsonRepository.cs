using Domain.Entities;

namespace Domain.Interfaces;

/// <summary>
/// JSON読み込みリポジトリインターフェース
/// </summary>
public interface IJsonRepository
{
    /// <summary>
    /// Json文字列/ファイルパス/ディレクトリパスを読み込んで Classエンティティリストを返す
    /// </summary>
    /// <param name="target">対象文字列(Json文字列/ファイルパス/ディレクトリパス)</param>
    /// <param name="className">クラス名(Json文字列時のみ必須)</param>
    /// <returns>Classエンティティリスト</returns>
    IReadOnlyList<ClassesEntity> CreateClassEntity(string target, string className);
}
