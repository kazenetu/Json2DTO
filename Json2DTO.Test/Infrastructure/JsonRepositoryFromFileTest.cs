using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure;
using Infrastructure.JsonProperties;

namespace Json2DTO.Test.Infrastructure;

/// <summary>
/// JSON読み込みリポジトリのテスト:FromFile
/// </summary>
public class JsonRepositoryFromFileTest: IDisposable
{
    /// <summary>
    /// 生成したファイルのパス
    /// </summary>
    private string CreatedFilePath = "";

    /// <summary>
    /// Teardown
    /// </summary>
    public void Dispose()
    {
        // 生成したファイルが存在しない場合は終了
        if (string.IsNullOrEmpty(CreatedFilePath)) return;
        if(!File.Exists(CreatedFilePath)) return;

        //生成したファイルを削除
        File.Delete(CreatedFilePath);
    }

    /// <summary>
    /// JSONファイル作成
    /// </summary>
    /// <param name="json">JSON文字列</param>
    /// <param name="fileName">ファイル名</param>
    private void CreateFile(string json, string fileName)
    {
        var directoryPath = "fromFile";

        // フォルダの確認と生成
        if(!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }
        CreatedFilePath = Path.Combine(directoryPath, fileName);

        // ファイル出力
        File.WriteAllText(CreatedFilePath, json);
    }

    [Fact]
    public void SuccessShortClassName()
    {
        var json = @"{
            ""prop_string"" : ""string""
        }";
        var rootClassName = "a";
        CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        var classesEntity = repository.CreateClassEntityFromFile(CreatedFilePath);

        var rootClass = classesEntity.RootClass;
        Assert.Equal("A", rootClass.Name);
        Assert.Equal(1, rootClass.Properties.Count);

        var index = 0;
        Assert.Equal("prop_string", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.String, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);
    }
}