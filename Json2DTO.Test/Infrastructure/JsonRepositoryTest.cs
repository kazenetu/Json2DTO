using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure;
using Infrastructure.JsonProperties;

namespace Json2DTO.Test.Infrastructure;

/// <summary>
/// JSON読み込みリポジトリのテスト
/// </summary>
public class JsonRepositoryTest: IDisposable
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
    /// <returns>生成したファイルのパス</returns>
    private string CreateFile(string json, string fileName)
    {
        // フォルダの確認と生成
        if(!Directory.Exists(CreatedDirectoryPath))
        {
            Directory.CreateDirectory(CreatedDirectoryPath);
        }
        var filePath = Path.Combine(CreatedDirectoryPath, fileName);

        // ファイル出力
        File.WriteAllText(filePath, json);

        // 生成ファイルパスを返す
        return filePath;
    }

    #region "Common:paramNullorEmpty"
    [Fact]
    public void ExceptionTargetParamNull()
    {
        string? target = null;

        var repository = new JsonRepository();
        #pragma warning disable
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(target));
        Assert.Equal($"target is null", ex.Message);
    }

    [Fact]
    public void ExceptionTargetParamEmpty()
    {
        var target = string.Empty;

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(target));
        Assert.Equal($"target is null", ex.Message);
    }
    #endregion


    #region "jsonString"
    [Fact]
    public void ExceptionRootClassNameParamNull()
    {
        var json = @"{
            ""prop_string"" : ""string""
            , ""propNumber"":10
            , ""prop_Date"":""2022/01/01 10:11:12""
            , ""PropTrue"":true
            , ""propFalse"":false
            , ""propNull"":null
            , ""propArray"":[1,2,3]
        }";
        string? rootClassName = null;

        var repository = new JsonRepository();
        #pragma warning disable
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(json, rootClassName));
        Assert.Equal($"{nameof(rootClassName)} is null", ex.Message);
    }

    [Fact]
    public void ExceptionRootClassNameParamEmpty()
    {
        var json = @"{
            ""prop_string"" : ""string""
            , ""propNumber"":10
            , ""prop_Date"":""2022/01/01 10:11:12""
            , ""PropTrue"":true
            , ""propFalse"":false
            , ""propNull"":null
            , ""propArray"":[1,2,3]
        }";
        var rootClassName = string.Empty;

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(json, rootClassName));
        Assert.Equal($"{nameof(rootClassName)} is null", ex.Message);
    }

    [Fact]
    public void ExceptionJsonParamError()
    {
        var json = @"{
            ""prop_string""
        }";
        var rootClassName = "AA";

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(json, rootClassName));
        Assert.Equal($"JSON parse error:{json}", ex.Message);
    }

    [Fact]
    public void ExceptionJsonParamNoProperty()
    {
        var json = @"{
        }";
        var rootClassName = "AA";

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(json, rootClassName));
        Assert.Equal($"JSON elements none:{json}", ex.Message);
    }

    [Fact]
    public void ExceptionJsonParamInnerClassNoProperty()
    {
        var json = @"{
            ""propObjct"" : 
            {
                ""prop_string"" : ""string""
                ,""propSubObjct"":{}
            }
        }";
        var rootClassName = "AA";

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(json, rootClassName));
        var errorJson = "{}";
        Assert.Equal($"JSON elements none:{errorJson}", ex.Message);
    }

    [Fact]
    public void SuccessJsonString()
    {
        var json = @"{
            ""prop_string"" : ""string""
            , ""propNumber"":10
            , ""prop_Date"":""2022/01/01 10:11:12""
            , ""PropTrue"":true
            , ""propFalse"":false
            , ""propNull"":null
            , ""propArray"":[1,2,3]
        }";
        var rootClassName = "rootClass";

        var repository = new JsonRepository();
        var classesEntities = repository.CreateClassEntity(json, rootClassName);
        Assert.Equal(1, classesEntities.Count);

        var rootClass = classesEntities[0].RootClass;
        Assert.Equal("RootClass", rootClass.Name);
        Assert.Equal(7, rootClass.Properties.Count);

        var index = 0;
        Assert.Equal("prop_string", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.String, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);

        index = 1;
        Assert.Equal("propNumber", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Decimal, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);

        index = 2;
        Assert.Equal("prop_Date", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.String, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);

        index = 3;
        Assert.Equal("PropTrue", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Bool, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);

        index = 4;
        Assert.Equal("propFalse", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Bool, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);

        index = 5;
        Assert.Equal("propNull", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Null, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);

        index = 6;
        Assert.Equal("propArray", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Decimal, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(true, rootClass.Properties[index].Type?.IsList);
    }

    [Fact]
    public void SuccessJsonStringShortClassName()
    {
        var json = @"{
            ""prop_string"" : ""string""
        }";
        var rootClassName = "a";

        var repository = new JsonRepository();
        var classesEntities = repository.CreateClassEntity(json, rootClassName);
        Assert.Equal(1, classesEntities.Count);

        var rootClass = classesEntities[0].RootClass;
        Assert.Equal("A", rootClass.Name);
        Assert.Equal(1, rootClass.Properties.Count);

        var index = 0;
        Assert.Equal("prop_string", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.String, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);
    }

    [Fact]
    public void SuccessJsonStringInneerClass()
    {
        var json = @"{
            ""propObjct"" : 
            {
                ""propObjString"":""propObjString""
            }
            , ""propNumber"":10
        }";

        var rootClassName = "rootClass";

        var repository = new JsonRepository();
        var classesEntities = repository.CreateClassEntity(json, rootClassName);
        Assert.Equal(1, classesEntities.Count);

        var rootClass = classesEntities[0].RootClass;
        Assert.Equal("RootClass", rootClass.Name);
        Assert.Equal(2, rootClass.Properties.Count);

        var index = 0;
        Assert.Equal("propObjct", rootClass.Properties[index].Name);
        Assert.Equal("InnerClass", rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Class, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);

        index = 1;
        Assert.Equal("propNumber", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Decimal, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);

        // InnerClass
        Assert.Equal(1, classesEntities[0].InnerClasses.Count);

        var innerClass = classesEntities[0].InnerClasses[0];
        Assert.Equal("InnerClass", innerClass.Name);
        Assert.Equal(1, innerClass.Properties.Count);

        index = 0;
        Assert.Equal("propObjString", innerClass.Properties[index].Name);
        Assert.Equal(string.Empty, innerClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.String, innerClass.Properties[index].Type?.Kind);
        Assert.Equal(false, innerClass.Properties[index].Type?.IsList);
    }

    [Fact]
    public void SuccessJsonStringInneerClassNest()
    {
        var json = @"{
            ""propObjct"" : 
            {
                ""propSubObjct"":
                {
                    ""propString"" : ""string""
                    , ""propNumber"":10
                    , ""propDate"":""2022/01/01 10:11:12""
                    , ""propTrue"":true
                    , ""propFalse"":false
                    , ""propNull"":null
                    , ""propArray"":[1,2,3]
                }
            }
        }";
        var rootClassName = "rootClass";

        var repository = new JsonRepository();
        var classesEntities = repository.CreateClassEntity(json, rootClassName);
        Assert.Equal(1, classesEntities.Count);

        var rootClass = classesEntities[0].RootClass;
        Assert.Equal("RootClass", rootClass.Name);
        Assert.Equal(1, rootClass.Properties.Count);

        var index = 0;
        Assert.Equal("propObjct", rootClass.Properties[index].Name);
        Assert.Equal("InnerClass", rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Class, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);

        // InnerClass
        Assert.Equal(2, classesEntities[0].InnerClasses.Count);

        // InnerClassA
        var innerClass = classesEntities[0].InnerClasses[0];
        Assert.Equal("InnerClassA", innerClass.Name);
        Assert.Equal(7, innerClass.Properties.Count);

        index = 0;
        Assert.Equal("propString", innerClass.Properties[index].Name);
        Assert.Equal(string.Empty, innerClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.String, innerClass.Properties[index].Type?.Kind);
        Assert.Equal(false, innerClass.Properties[index].Type?.IsList);

        index = 1;
        Assert.Equal("propNumber", innerClass.Properties[index].Name);
        Assert.Equal(string.Empty, innerClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Decimal, innerClass.Properties[index].Type?.Kind);
        Assert.Equal(false, innerClass.Properties[index].Type?.IsList);

        index = 2;
        Assert.Equal("propDate", innerClass.Properties[index].Name);
        Assert.Equal(string.Empty, innerClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.String, innerClass.Properties[index].Type?.Kind);
        Assert.Equal(false, innerClass.Properties[index].Type?.IsList);

        index = 3;
        Assert.Equal("propTrue", innerClass.Properties[index].Name);
        Assert.Equal(string.Empty, innerClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Bool, innerClass.Properties[index].Type?.Kind);
        Assert.Equal(false, innerClass.Properties[index].Type?.IsList);

        index = 4;
        Assert.Equal("propFalse", innerClass.Properties[index].Name);
        Assert.Equal(string.Empty, innerClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Bool, innerClass.Properties[index].Type?.Kind);
        Assert.Equal(false, innerClass.Properties[index].Type?.IsList);

        index = 5;
        Assert.Equal("propNull", innerClass.Properties[index].Name);
        Assert.Equal(string.Empty, innerClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Null, innerClass.Properties[index].Type?.Kind);
        Assert.Equal(false, innerClass.Properties[index].Type?.IsList);

        index = 6;
        Assert.Equal("propArray", innerClass.Properties[index].Name);
        Assert.Equal(string.Empty, innerClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Decimal, innerClass.Properties[index].Type?.Kind);
        Assert.Equal(true, innerClass.Properties[index].Type?.IsList);

        // InnerClass
        innerClass = classesEntities[0].InnerClasses[1];
        Assert.Equal("InnerClass", innerClass.Name);
        Assert.Equal(1, innerClass.Properties.Count);

        index = 0;
        Assert.Equal("propSubObjct", innerClass.Properties[index].Name);
        Assert.Equal("InnerClassA", innerClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Class, innerClass.Properties[index].Type?.Kind);
        Assert.Equal(false, innerClass.Properties[index].Type?.IsList);
    }

    #endregion

    #region "file"
    [Fact]
    public void ExceptionFilePatNotExist()
    {
        var filePath = "Dummy";

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(filePath));
        Assert.Equal($"target is not exists", ex.Message);
    }

    [Fact]
    public void ExceptionFileJsonError()
    {
        var json = @"{
            ""prop_string""
        }";
        var rootClassName = "JsonError";
        var createdFilePath = CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(createdFilePath));
        Assert.Equal($"JSON parse error:{json}", ex.Message);
    }

    [Fact]
    public void ExceptionFileNoProperty()
    {
        var json = @"{
        }";
        var rootClassName = "NoProperty";
        var createdFilePath = CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(createdFilePath));
        Assert.Equal($"JSON elements none:{json}", ex.Message);
    }

    [Fact]
    public void ExceptionFileInnerClassNoProperty()
    {
        var json = @"{
            ""propObjct"" : 
            {
                ""prop_string"" : ""string""
                ,""propSubObjct"":{}
            }
        }";
        var rootClassName = "InnerClassNoProperty";
        var createdFilePath = CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(createdFilePath));
        var errorJson = "{}";
        Assert.Equal($"JSON elements none:{errorJson}", ex.Message);
    }

    [Fact]
    public void SuccessFile()
    {
        var json = @"{
            ""prop_string"" : ""string""
            , ""propNumber"":10
            , ""prop_Date"":""2022/01/01 10:11:12""
            , ""PropTrue"":true
            , ""propFalse"":false
            , ""propNull"":null
            , ""propArray"":[1,2,3]
        }";
        var rootClassName = "Success";
        var createdFilePath = CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        var classesEntities = repository.CreateClassEntity(createdFilePath);
        Assert.Equal(1, classesEntities.Count);

        var rootClass = classesEntities[0].RootClass;
        Assert.Equal("Success", rootClass.Name);
        Assert.Equal(7, rootClass.Properties.Count);

        var index = 0;
        Assert.Equal("prop_string", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.String, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);

        index = 1;
        Assert.Equal("propNumber", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Decimal, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);

        index = 2;
        Assert.Equal("prop_Date", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.String, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);

        index = 3;
        Assert.Equal("PropTrue", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Bool, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);

        index = 4;
        Assert.Equal("propFalse", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Bool, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);

        index = 5;
        Assert.Equal("propNull", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Null, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);

        index = 6;
        Assert.Equal("propArray", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Decimal, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(true, rootClass.Properties[index].Type?.IsList);
    }

    [Fact]
    public void SuccessFileShortClassName()
    {
        var json = @"{
            ""prop_string"" : ""string""
        }";
        var rootClassName = "a";
        var createdFilePath = CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        var classesEntities = repository.CreateClassEntity(createdFilePath);
        Assert.Equal(1, classesEntities.Count);

        var rootClass = classesEntities[0].RootClass;
        Assert.Equal("A", rootClass.Name);
        Assert.Equal(1, rootClass.Properties.Count);

        var index = 0;
        Assert.Equal("prop_string", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.String, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);
    }

    [Fact]
    public void SuccessFileInneerClass()
    {
        var json = @"{
            ""propObjct"" : 
            {
                ""propObjString"":""propObjString""
            }
            , ""propNumber"":10
        }";

        var rootClassName = "inneerClass";

        var createdFilePath = CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        var classesEntities = repository.CreateClassEntity(createdFilePath);
        Assert.Equal(1, classesEntities.Count);

        var rootClass = classesEntities[0].RootClass;
        Assert.Equal("InneerClass", rootClass.Name);
        Assert.Equal(2, rootClass.Properties.Count);

        var index = 0;
        Assert.Equal("propObjct", rootClass.Properties[index].Name);
        Assert.Equal("InnerClass", rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Class, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);

        index = 1;
        Assert.Equal("propNumber", rootClass.Properties[index].Name);
        Assert.Equal(string.Empty, rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Decimal, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);

        // InnerClass
        Assert.Equal(1, classesEntities[0].InnerClasses.Count);

        var innerClass = classesEntities[0].InnerClasses[0];
        Assert.Equal("InnerClass", innerClass.Name);
        Assert.Equal(1, innerClass.Properties.Count);

        index = 0;
        Assert.Equal("propObjString", innerClass.Properties[index].Name);
        Assert.Equal(string.Empty, innerClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.String, innerClass.Properties[index].Type?.Kind);
        Assert.Equal(false, innerClass.Properties[index].Type?.IsList);
    }

    [Fact]
    public void SuccessFileInneerClassNest()
    {
        var json = @"{
            ""propObjct"" : 
            {
                ""propSubObjct"":
                {
                    ""propString"" : ""string""
                    , ""propNumber"":10
                    , ""propDate"":""2022/01/01 10:11:12""
                    , ""propTrue"":true
                    , ""propFalse"":false
                    , ""propNull"":null
                    , ""propArray"":[1,2,3]
                }
            }
        }";
        var rootClassName = "inneerClassNest";

        var createdFilePath = CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        var classesEntities = repository.CreateClassEntity(createdFilePath);
        Assert.Equal(1, classesEntities.Count);

        var rootClass = classesEntities[0].RootClass;
        Assert.Equal("InneerClassNest", rootClass.Name);
        Assert.Equal(1, rootClass.Properties.Count);

        var index = 0;
        Assert.Equal("propObjct", rootClass.Properties[index].Name);
        Assert.Equal("InnerClass", rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Class, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);

        // InnerClass
        Assert.Equal(2, classesEntities[0].InnerClasses.Count);

        // InnerClassA
        var innerClass = classesEntities[0].InnerClasses[0];
        Assert.Equal("InnerClassA", innerClass.Name);
        Assert.Equal(7, innerClass.Properties.Count);

        index = 0;
        Assert.Equal("propString", innerClass.Properties[index].Name);
        Assert.Equal(string.Empty, innerClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.String, innerClass.Properties[index].Type?.Kind);
        Assert.Equal(false, innerClass.Properties[index].Type?.IsList);

        index = 1;
        Assert.Equal("propNumber", innerClass.Properties[index].Name);
        Assert.Equal(string.Empty, innerClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Decimal, innerClass.Properties[index].Type?.Kind);
        Assert.Equal(false, innerClass.Properties[index].Type?.IsList);

        index = 2;
        Assert.Equal("propDate", innerClass.Properties[index].Name);
        Assert.Equal(string.Empty, innerClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.String, innerClass.Properties[index].Type?.Kind);
        Assert.Equal(false, innerClass.Properties[index].Type?.IsList);

        index = 3;
        Assert.Equal("propTrue", innerClass.Properties[index].Name);
        Assert.Equal(string.Empty, innerClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Bool, innerClass.Properties[index].Type?.Kind);
        Assert.Equal(false, innerClass.Properties[index].Type?.IsList);

        index = 4;
        Assert.Equal("propFalse", innerClass.Properties[index].Name);
        Assert.Equal(string.Empty, innerClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Bool, innerClass.Properties[index].Type?.Kind);
        Assert.Equal(false, innerClass.Properties[index].Type?.IsList);

        index = 5;
        Assert.Equal("propNull", innerClass.Properties[index].Name);
        Assert.Equal(string.Empty, innerClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Null, innerClass.Properties[index].Type?.Kind);
        Assert.Equal(false, innerClass.Properties[index].Type?.IsList);

        index = 6;
        Assert.Equal("propArray", innerClass.Properties[index].Name);
        Assert.Equal(string.Empty, innerClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Decimal, innerClass.Properties[index].Type?.Kind);
        Assert.Equal(true, innerClass.Properties[index].Type?.IsList);

        // InnerClass
        innerClass = classesEntities[0].InnerClasses[1];
        Assert.Equal("InnerClass", innerClass.Name);
        Assert.Equal(1, innerClass.Properties.Count);

        index = 0;
        Assert.Equal("propSubObjct", innerClass.Properties[index].Name);
        Assert.Equal("InnerClassA", innerClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Class, innerClass.Properties[index].Type?.Kind);
        Assert.Equal(false, innerClass.Properties[index].Type?.IsList);
    }
    #endregion

    #region "directory"
    [Fact]
    public void ExceptionDirectoryPathNotExist()
    {
        var directoryPath = "Dummy";

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(directoryPath));
        Assert.Equal($"target is not exists", ex.Message);
    }

    [Fact]
    public void ExceptionDirectoryNotExistsFiles()
    {
        Directory.CreateDirectory(CreatedDirectoryPath);

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(CreatedDirectoryPath));
        Assert.Equal($"{CreatedDirectoryPath} is not file", ex.Message);
    }

    [Fact]
    public void ExceptionDirectoryJsonError()
    {
        var json = @"{""prop_string""}";
        var rootClassName = "JsonError";
        CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(CreatedDirectoryPath));
        Assert.Equal($"JSON parse error:{json}", ex.Message);
    }

    [Fact]
    public void ExceptionDirectoryNoProperty()
    {
        var json = @"{
        }";
        var rootClassName = "NoProperty";
        CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(CreatedDirectoryPath));
        Assert.Equal($"JSON elements none:{json}", ex.Message);
    }

    [Fact]
    public void ExceptionDirectoryInnerClassNoProperty()
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
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(CreatedDirectoryPath));
        var errorJson = "{}";
        Assert.Equal($"JSON elements none:{errorJson}", ex.Message);
    }

    [Fact]
    public void SuccessDirectorySingleFile()
    {
        var json = @"{
            ""prop_string"" : ""string""
        }";
        var rootClassName = "SuccessSingleFile";
        CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        var classesEntities = repository.CreateClassEntity(CreatedDirectoryPath);
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
    public void SuccessDirectoryMultiFile()
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
        var classesEntities = repository.CreateClassEntity(CreatedDirectoryPath);
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
    #endregion    
}