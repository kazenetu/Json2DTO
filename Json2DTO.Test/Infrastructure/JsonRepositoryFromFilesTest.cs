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

    [Fact]
    public void ExceptionDirectoryPathNull()
    {
        string? directoryPath = null;

        var repository = new JsonRepository();
        #pragma warning disable
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntityFromFiles(directoryPath));
        Assert.Equal($" is not directory", ex.Message);
    }

    [Fact]
    public void ExceptionDirectoryPathNotExist()
    {
        var directoryPath = "Dummy";

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntityFromFiles(directoryPath));
        Assert.Equal($"Dummy is not directory", ex.Message);
    }

    [Fact]
    public void ExceptionDirectoryNotExistsFiles()
    {
        Directory.CreateDirectory(CreatedDirectoryPath);

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntityFromFiles(CreatedDirectoryPath));
        Assert.Equal($"{CreatedDirectoryPath} is not file", ex.Message);
    }

    [Fact]
    public void ExceptionJsonError()
    {
        var json = @"{""prop_string""}";
        var rootClassName = "JsonError";
        CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntityFromFiles(CreatedDirectoryPath));
        Assert.Equal($"JSON parse error:{json}", ex.Message);
    }

    [Fact]
    public void ExceptionNoProperty()
    {
        var json = @"{
        }";
        var rootClassName = "NoProperty";
        CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntityFromFiles(CreatedDirectoryPath));
        Assert.Equal($"JSON elements none:{json}", ex.Message);
    }

    [Fact]
    public void ExceptionInnerClassNoProperty()
    {
        var json = @"{
            ""propObjct"" : 
            {
                ""prop_string"" : ""string""
                ,""propSubObjct"":{}
            }
        }";
        var rootClassName = "InnerClassNoProperty";
        CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntityFromFiles(CreatedDirectoryPath));
        var errorJson = "{}";
        Assert.Equal($"JSON elements none:{errorJson}", ex.Message);
    }

    [Fact]
    public void ExceptionIsString()
    {
        var json = @"{
            ""prop_string"" : ""string""
        }";
        var rootClassName = "SuccessSingleFile";
        CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        Assert.Equal(false, repository.IsJsonString(CreatedDirectoryPath));
    }

    [Fact]
    public void ExceptionIsFilePath()
    {
        var json = @"{
            ""prop_string"" : ""string""
        }";
        var rootClassName = "SuccessSingleFile";
        CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        Assert.Equal(false, repository.IsFilePath(CreatedDirectoryPath));
    }

    [Fact]
    public void SuccessIsDirectoryPath()
    {
        var json = @"{
            ""prop_string"" : ""string""
        }";
        var rootClassName = "SuccessSingleFile";
        CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        Assert.Equal(true, repository.IsDirectoryPath(CreatedDirectoryPath));
    }

    [Fact]
    public void SuccessSingleFile()
    {
        var json = @"{
            ""prop_string"" : ""string""
        }";
        var rootClassName = "SuccessSingleFile";
        CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        var classesEntities = repository.CreateClassEntityFromFiles(CreatedDirectoryPath);
        Assert.Equal(1, classesEntities.Count);

        var rootClass = classesEntities[0].RootClass;
        Assert.Equal("SuccessSingleFile", rootClass.Name);
        Assert.Equal(1, rootClass.Properties.Count);

        var index = 0;
        Assert.Equal("prop_string", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.String, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);
    }

    [Fact]
    public void SuccessMultiFile()
    {
        var json1 = @"{
            ""prop_string"" : ""string""
        }";
        var rootClassName1 = "fileA";
        CreateFile(json1, $"{rootClassName1}.json");

        var json2 = @"{
            ""prop_string"" : ""string""
            , ""propNumber"":10
        }";
        var rootClassName2 = "fileB";
        CreateFile(json2, $"{rootClassName2}.json");

        var repository = new JsonRepository();
        var classesEntities = repository.CreateClassEntityFromFiles(CreatedDirectoryPath);
        Assert.Equal(2, classesEntities.Count);

        var sortedClassesEntities = classesEntities.OrderBy(classes => classes.RootClass.Name).ToList();

        // fileA
        var rootClass = sortedClassesEntities[0].RootClass;
        Assert.Equal("FileA", rootClass.Name);
        Assert.Equal(1, rootClass.Properties.Count);

        var index = 0;
        Assert.Equal("prop_string", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.String, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);

        // fileB
        rootClass = sortedClassesEntities[1].RootClass;
        Assert.Equal("FileB", rootClass.Name);
        Assert.Equal(2, rootClass.Properties.Count);

        index = 0;
        Assert.Equal("prop_string", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.String, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);

        index = 1;
        Assert.Equal("propNumber", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Decimal, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);
    }
}