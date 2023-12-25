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
    public void ExceptionFilePathNull()
    {
        string? filePath = null;

        var repository = new JsonRepository();
        #pragma warning disable
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntityFromFile(filePath));
        Assert.Equal($" is not exixts", ex.Message);
    }

    [Fact]
    public void ExceptionFilePathEmpty()
    {
        var filePath = string.Empty;

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntityFromFile(filePath));
        Assert.Equal($" is not exixts", ex.Message);
    }

    [Fact]
    public void ExceptionFilePatNotExist()
    {
        var filePath = "Dummy";

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntityFromFile(filePath));
        Assert.Equal($"Dummy is not exixts", ex.Message);
    }

    [Fact]
    public void ExceptionJsonError()
    {
        var json = @"{
            ""prop_string""
        }";
        var rootClassName = "JsonError";
        CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntityFromFile(CreatedFilePath));
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
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntityFromFile(CreatedFilePath));
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
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntityFromFile(CreatedFilePath));
        var errorJson = "{}";
        Assert.Equal($"JSON elements none:{errorJson}", ex.Message);
    }

    [Fact]
    public void Success()
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
        CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        var classesEntity = repository.CreateClassEntityFromFile(CreatedFilePath);

        var rootClass = classesEntity.RootClass;
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

    [Fact]
    public void SuccessInneerClass()
    {
        var json = @"{
            ""propObjct"" : 
            {
                ""propObjString"":""propObjString""
            }
            , ""propNumber"":10
        }";

        var rootClassName = "inneerClass";

        CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        var classesEntity = repository.CreateClassEntityFromFile(CreatedFilePath);

        var rootClass = classesEntity.RootClass;
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
        Assert.Equal(1, classesEntity.InnerClasses.Count);

        var innerClass = classesEntity.InnerClasses[0];
        Assert.Equal("InnerClass", innerClass.Name);
        Assert.Equal(1, innerClass.Properties.Count);

        index = 0;
        Assert.Equal("propObjString", innerClass.Properties[index].Name);
        Assert.Equal(string.Empty, innerClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.String, innerClass.Properties[index].Type?.Kind);
        Assert.Equal(false, innerClass.Properties[index].Type?.IsList);
    }

    [Fact]
    public void SuccessInneerClassNest()
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

        CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        var classesEntity = repository.CreateClassEntityFromFile(CreatedFilePath);

        var rootClass = classesEntity.RootClass;
        Assert.Equal("RootClass", rootClass.Name);
        Assert.Equal(1, rootClass.Properties.Count);

        var index = 0;
        Assert.Equal("propObjct", rootClass.Properties[index].Name);
        Assert.Equal("InnerClass", rootClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Class, rootClass.Properties[index].Type?.Kind);
        Assert.Equal(false, rootClass.Properties[index].Type?.IsList);

        // InnerClass
        Assert.Equal(2, classesEntity.InnerClasses.Count);

        // InnerClassA
        var innerClass = classesEntity.InnerClasses[0];
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

        // InneerClassNest
        innerClass = classesEntity.InnerClasses[1];
        Assert.Equal("InneerClassNest", innerClass.Name);
        Assert.Equal(1, innerClass.Properties.Count);

        index = 0;
        Assert.Equal("propSubObjct", innerClass.Properties[index].Name);
        Assert.Equal("InnerClassA", innerClass.Properties[index].PropertyTypeClassName);
        Assert.Equal(PropertyType.Kinds.Class, innerClass.Properties[index].Type?.Kind);
        Assert.Equal(false, innerClass.Properties[index].Type?.IsList);
    }
}