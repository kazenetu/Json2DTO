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
    private string _createdDirectoryPath = "fromFiles";

    /// <summary>
    /// Teardown
    /// </summary>
    public void Dispose()
    {
        if(!Directory.Exists(_createdDirectoryPath)) return;

        // ファイル削除
        var files = Directory.GetFiles(_createdDirectoryPath);
        foreach (var file in files)
        {
            File.Delete(file);
        }
    }

    #region "Common:paramNullorEmpty"
    [Fact(DisplayName="ExceptionTest:stringParam is null"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
    public void ExceptionTargetParamNull()
    {
        string? target = null;

        var repository = new JsonRepository();
        #pragma warning disable
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(target));
        Assert.Equal($"target is null", ex.Message);
    }

    [Fact(DisplayName="ExceptionTest:stringParam is Empty"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
    public void ExceptionTargetParamEmpty()
    {
        var target = string.Empty;

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(target));
        Assert.Equal($"target is null", ex.Message);
    }
    #endregion


    #region "jsonString"
    [Fact(DisplayName="ExceptionTest:rootClassName is null"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
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

    [Fact(DisplayName="ExceptionTest:rootClassName is Empty"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
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

    [Fact(DisplayName="ExceptionTest:Json is error"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
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

    [Fact(DisplayName="ExceptionTest:Json is none"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
    public void ExceptionJsonParamNoProperty()
    {
        var json = @"{
        }";
        var rootClassName = "AA";

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(json, rootClassName));
        Assert.Equal($"JSON elements none:{json}", ex.Message);
    }

    [Fact(DisplayName="ExceptionTest:Json innerClass is none"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
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

    [Fact(DisplayName="Test:JsonString"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
    public void SuccessIsJsonString_JsonString()
    {
        var target = @"{
            ""prop_string"" : ""string""
        }";

        var repository = new JsonRepository();
        Assert.Equal(true, repository.IsJsonString(target));
    }

    [Fact(DisplayName="Test:JsonFile"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
    public void SuccessIsJsonStringFile()
    {
        var target = "file.json";

        var repository = new JsonRepository();
        Assert.Equal(false,repository.IsJsonString(target));
    }

    [Fact(DisplayName="Test:JsonDirectory"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
    public void SuccessIsJsonStringDirectory()
    {
        var target = "files";

        var repository = new JsonRepository();
        Assert.Equal(false, repository.IsJsonString(target));
    }

    [Fact(DisplayName="Test:JsonString properties"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
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

    [Fact(DisplayName="Test:JsonString short className"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
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

    [Fact(DisplayName="Test:JsonString inner class"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
    public void SuccessJsonStringInnerClass()
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

    [Fact(DisplayName="Test:JsonString inner class nest"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
    public void SuccessJsonStringInnerClassNest()
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
    [Fact(DisplayName="ExceptionTest:File path not exist"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
    public void ExceptionFilePathNotExist()
    {
        var filePath = "Dummy";

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(filePath));
        Assert.Equal($"target is not exists", ex.Message);
    }

    [Fact(DisplayName="ExceptionTest:File Json parse error"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
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

    [Fact(DisplayName="ExceptionTest:File Json none"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
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

    [Fact(DisplayName="ExceptionTest:File Json innerClass none"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
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

    [Fact(DisplayName="Test:File short className"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
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

    [Fact(DisplayName="Test:File innerClass"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
    public void SuccessFileInnerClass()
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

    [Fact(DisplayName="Test:File innerClass nest"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
    public void SuccessFileInnerClassNest()
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
    [Fact(DisplayName="ExceptionTest:Directory path exists"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
    public void ExceptionDirectoryPathNotExist()
    {
        var directoryPath = "Dummy";

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(directoryPath));
        Assert.Equal($"target is not exists", ex.Message);
    }

    [Fact(DisplayName="ExceptionTest:Directory path exists files"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
    public void ExceptionDirectoryNotExistsFiles()
    {
        Directory.CreateDirectory(_createdDirectoryPath);

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(_createdDirectoryPath));
        Assert.Equal($"{_createdDirectoryPath} is not file", ex.Message);
    }

    [Fact(DisplayName="ExceptionTest:Directory Json error"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
    public void ExceptionDirectoryJsonError()
    {
        var json = @"{""prop_string""}";
        var rootClassName = "JsonError";
        CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(_createdDirectoryPath));
        Assert.Equal($"JSON parse error:{json}", ex.Message);
    }

    [Fact(DisplayName="ExceptionTest:Directory no properties"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
    public void ExceptionDirectoryNoProperty()
    {
        var json = @"{
        }";
        var rootClassName = "NoProperty";
        CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(_createdDirectoryPath));
        Assert.Equal($"JSON elements none:{json}", ex.Message);
    }

    [Fact(DisplayName="ExceptionTest:Directory innerClass no properties"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
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
        var ex = Assert.ThrowsAny<ArgumentException>(() => repository.CreateClassEntity(_createdDirectoryPath));
        var errorJson = "{}";
        Assert.Equal($"JSON elements none:{errorJson}", ex.Message);
    }

    [Fact(DisplayName="Test:Directory 1file"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
    public void SuccessDirectorySingleFile()
    {
        var json = @"{
            ""prop_string"" : ""string""
        }";
        var rootClassName = "SuccessSingleFile";
        CreateFile(json, $"{rootClassName}.json");

        var repository = new JsonRepository();
        var classesEntities = repository.CreateClassEntity(_createdDirectoryPath);
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

    [Fact(DisplayName="Test:Directory 2files"), Trait("Category", "Infrastructure:JsonRepositoryTest")]
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
        var classesEntities = repository.CreateClassEntity(_createdDirectoryPath);
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

    /// <summary>
    /// JSONファイル作成
    /// </summary>
    /// <param name="json">JSON文字列</param>
    /// <param name="fileName">ファイル名</param>
    /// <returns>生成したファイルのパス</returns>
    private string CreateFile(string json, string fileName)
    {
        // フォルダの確認と生成
        if(!Directory.Exists(_createdDirectoryPath))
        {
            Directory.CreateDirectory(_createdDirectoryPath);
        }
        var filePath = Path.Combine(_createdDirectoryPath, fileName);

        // ファイル出力
        File.WriteAllText(filePath, json);

        // 生成ファイルパスを返す
        return filePath;
    }
}