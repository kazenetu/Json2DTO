using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure;
using Infrastructure.JsonProperties;

namespace Json2DTO.Test.Infrastructure;

/// <summary>
/// JSON読み込みリポジトリのテスト:FromFiles
/// </summary>
public class JsonRepositoryFromFilesTest: IDisposable
{
    /// <summary>
    /// 生成したファイルを格納したディレクトリパス
    /// </summary>
    private string CreatedDirectoryPath = "fromFiles";

    /// <summary>
    /// Teardown
    /// </summary>
    public void Dispose()
    {
        if(!Directory.Exists(CreatedDirectoryPath)) return;

        // ファイル削除
        var files = Directory.GetFiles(CreatedDirectoryPath);
        foreach (var file in files)
        {
            File.Delete(file);
        }
    }

    /// <summary>
    /// JSONファイル作成
    /// </summary>
    /// <param name="json">JSON文字列</param>
    /// <param name="fileName">ファイル名</param>
    private void CreateFile(string json, string fileName)
    {
        // フォルダの確認と生成
        if(!Directory.Exists(CreatedDirectoryPath))
        {
            Directory.CreateDirectory(CreatedDirectoryPath);
        }
        var filePath = Path.Combine(CreatedDirectoryPath, fileName);

        // ファイル出力
        File.WriteAllText(filePath, json);
    }

}