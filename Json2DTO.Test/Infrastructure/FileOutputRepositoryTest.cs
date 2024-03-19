using Domain.Commands;
using Domain.Entities;
using Domain.ValueObjects;
using Infrastructure;

namespace Json2DTO.Test.Infrastructure;

/// <summary>
/// ファイル出力リポジトリクラスのテスト
/// </summary>
public class FileOutputRepositoryTest: IDisposable
{
    /// <summary>
    /// 出力ファイルを格納したディレクトリパス
    /// </summary>
    private string _createdDirectoryPath = "outputFiles";

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

    [Fact(DisplayName="ExceptionTest:classInstance is null"), Trait("Category", "Infrastructure:FileOutputRepositoryTest")]
    public void ExceptionClassInstanceNull()
    {
        ClassesEntity? classInstance = null;

        int indentSpace = 2;

        var commandParams = new Dictionary<ParamKeys, string>
        {
            {ParamKeys.CS_NameSpace, "Test"},
            {ParamKeys.Prefix, "Prefix"},
            {ParamKeys.Suffix, "Suffix"},
        };

        var command = 
            new FileOutputCommand(_createdDirectoryPath, OutputLanguageType.CS, indentSpace, commandParams);

        var repository = new FileOutputRepository();
        #pragma warning disable
        var result  = repository.OutputResult(classInstance, command);

        Assert.Equal(false, result.Success);
        Assert.Equal(string.Empty, result.FileName);
        Assert.Equal(string.Empty, result.SourceCode);
    }

    [Fact(DisplayName="ExceptionTest:classInstances is null"), Trait("Category", "Infrastructure:FileOutputRepositoryTest")]
    public void ExceptionClassInstancesNull()
    {
        List<ClassesEntity>? classInstance = null;

        int indentSpace = 2;

        var commandParams = new Dictionary<ParamKeys, string>
        {
            {ParamKeys.CS_NameSpace, "Test"},
            {ParamKeys.Prefix, "Prefix"},
            {ParamKeys.Suffix, "Suffix"},
        };

        var command = 
            new FileOutputCommand(_createdDirectoryPath, OutputLanguageType.CS, indentSpace, commandParams);

        var repository = new FileOutputRepository();
        #pragma warning disable
        var results  = repository.OutputResults(classInstance, command);

        Assert.Equal(1, results.Count);
        
        var result = results[0];
        Assert.Equal(false, result.Success);
        Assert.Equal(string.Empty, result.FileName);
        Assert.Equal(string.Empty, result.SourceCode);
    }

    [Fact(DisplayName="ExceptionTest:commmand.RootPath is null"), Trait("Category", "Infrastructure:FileOutputRepositoryTest")]
    public void ExceptionClassCommmandRootPathNull()
    {
        string rootClassName = "TestRootClass";
        var classInstance = ClassesEntity.Create(rootClassName);
        var proprtyType = new PropertyType(typeof(string), false);
        var propertyValueObject = new PropertyValueObject("Name", proprtyType);
        classInstance.AddRootProperty(propertyValueObject);

        int indentSpace = 2;

         var commandParams = new Dictionary<ParamKeys, string>
        {
            {ParamKeys.CS_NameSpace, "Test"},
            {ParamKeys.Prefix, "Prefix"},
            {ParamKeys.Suffix, "Suffix"},
        };

        #pragma warning disable
        var command = 
            new FileOutputCommand(null, OutputLanguageType.CS, indentSpace, commandParams);

        var repository = new FileOutputRepository();
        #pragma warning disable
        var result  = repository.OutputResult(classInstance, command);

        Assert.Equal(false, result.Success);
        Assert.Equal(string.Empty, result.FileName);
        Assert.Equal(string.Empty, result.SourceCode);
    }

    [Fact(DisplayName="ExceptionTest:commandParams is null"), Trait("Category", "Infrastructure:FileOutputRepositoryTest")]
    public void ExceptionClassCommmandParamsNull()
    {
        string rootClassName = "TestRootClass";
        var classInstance = ClassesEntity.Create(rootClassName);
        var proprtyType = new PropertyType(typeof(string), false);
        var propertyValueObject = new PropertyValueObject("Name", proprtyType);
        classInstance.AddRootProperty(propertyValueObject);

        int indentSpace = 2;

        Dictionary<ParamKeys, string>? commandParams = null;

        var command = 
            new FileOutputCommand(_createdDirectoryPath, OutputLanguageType.CS, indentSpace, commandParams);

        var repository = new FileOutputRepository();
        #pragma warning disable
        var result  = repository.OutputResult(classInstance, command);

        Assert.Equal(false, result.Success);
        Assert.Equal(string.Empty, result.FileName);
        Assert.Equal(string.Empty, result.SourceCode);
    }

    [Fact(DisplayName="ExceptionTest:command is null"), Trait("Category", "Infrastructure:FileOutputRepositoryTest")]
    public void ExceptionClassCommmandNull()
    {
        string rootClassName = "TestRootClass";
        var classInstance = ClassesEntity.Create(rootClassName);
        var proprtyType = new PropertyType(typeof(string), false);
        var propertyValueObject = new PropertyValueObject("Name", proprtyType);
        classInstance.AddRootProperty(propertyValueObject);

        int indentSpace = 2;

         var commandParams = new Dictionary<ParamKeys, string>
        {
            {ParamKeys.CS_NameSpace, "Test"},
            {ParamKeys.Prefix, "Prefix"},
            {ParamKeys.Suffix, "Suffix"},
        };

        FileOutputCommand? command = null;

        var repository = new FileOutputRepository();
        #pragma warning disable
        var result  = repository.OutputResult(classInstance, command);

        Assert.Equal(false, result.Success);
        Assert.Equal(string.Empty, result.FileName);
        Assert.Equal(string.Empty, result.SourceCode);
    }

    [Fact(DisplayName="Test:output simple file"), Trait("Category", "Infrastructure:FileOutputRepositoryTest")]
    public void SuccessClassInstance()
    {
        string rootClassName = "TestRootClass";
        var classInstance = ClassesEntity.Create(rootClassName);
        var proprtyType = new PropertyType(typeof(string), false);
        var propertyValueObject = new PropertyValueObject("Name", proprtyType);
        classInstance.AddRootProperty(propertyValueObject);

        int indentSpace = 2;

         var commandParams = new Dictionary<ParamKeys, string>
        {
            {ParamKeys.CS_NameSpace, "Test"},
            {ParamKeys.Prefix, "Prefix"},
            {ParamKeys.Suffix, "Suffix"},
        };

        var command = 
            new FileOutputCommand(_createdDirectoryPath, OutputLanguageType.CS, indentSpace, commandParams);

        var repository = new FileOutputRepository();
        var result  = repository.OutputResult(classInstance, command);

        Assert.Equal(true, result.Success);

        var expectedFileName = Path.Combine(_createdDirectoryPath, "PrefixTestRootClassSuffix.cs");
        Assert.Equal(expectedFileName, result.FileName);

        var expectedSourceCode = 
        @"using System.Text;
using System.Text.Json.Serialization;

namespace Test
{
  public class PrefixTestRootClassSuffix
  {
    public string Name { set; get; } = string.Empty;
  }
}";
        Assert.Equal(expectedSourceCode, result.SourceCode);
        Assert.Equal(expectedSourceCode, ReadFile(result.FileName));
    }

    [Fact(DisplayName="Test:output innerClass"), Trait("Category", "Infrastructure:FileOutputRepositoryTest")]
    public void SuccessClassInstanceInnerClass()
    {
        string rootClassName = "TestClass";
        var classInstance = ClassesEntity.Create(rootClassName);

        var innerClass = ClassEntity.Create("InnerClass");
        var proprtyTypeInner = new PropertyType(typeof(decimal), true);
        var propertyValueObjectInner = new PropertyValueObject("Name", proprtyTypeInner);
        innerClass.AddProperty(propertyValueObjectInner);
        classInstance.AddInnerClass(innerClass);

        var proprtyType = new PropertyType(typeof(string), false);
        var propertyValueObject = new PropertyValueObject("prop_string", proprtyType);
        classInstance.AddRootProperty(propertyValueObject);

        var proprtyType1 = new PropertyType(0, false);
        var propertyValueObject1 = new PropertyValueObject("class", proprtyType1);
        classInstance.AddRootProperty(propertyValueObject1);


        int indentSpace = 2;

        var commandParams = new Dictionary<ParamKeys, string>();

        var command = 
            new FileOutputCommand(_createdDirectoryPath, OutputLanguageType.CS, indentSpace, commandParams);

        var repository = new FileOutputRepository();
        var result  = repository.OutputResult(classInstance, command);

        Assert.Equal(true, result.Success);

        var expectedFileName = Path.Combine(_createdDirectoryPath, "TestClass.cs");
        Assert.Equal(expectedFileName, result.FileName);

        var expectedSourceCode = 
        @"using System.Text;
using System.Text.Json.Serialization;

public class TestClass
{
  public class InnerClass
  {
    public List<decimal>? Name { set; get; }
  }

  [JsonPropertyName(""prop_string"")]
  public string PropString { set; get; } = string.Empty;

  [JsonPropertyName(""class"")]
  public InnerClass? Class { set; get; }
}
";
        Assert.Equal(expectedSourceCode, result.SourceCode);
        Assert.Equal(expectedSourceCode, ReadFile(result.FileName));
    }

    [Fact(DisplayName="Test:output 2files"), Trait("Category", "Infrastructure:FileOutputRepositoryTest")]
    public void SuccessClassInstances()
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
       
        int indentSpace = 4;

         var commandParams = new Dictionary<ParamKeys, string>
        {
            {ParamKeys.CS_NameSpace, "Test"},
        };

        var command = 
            new FileOutputCommand(_createdDirectoryPath, OutputLanguageType.CS, indentSpace, commandParams);

        var repository = new FileOutputRepository();
        var results  = repository.OutputResults(classInstances, command);

        var index = 0;
        string[] expectedFileNames = {
            "RootClassName",
            "RootClassNameA",
        };

        string[] expectedSourceCodes ={
        @"using System.Text;
using System.Text.Json.Serialization;

namespace Test
{
    public class rootClassName
    {
        public string Name { set; get; } = string.Empty;
    }
}",
@"using System.Text;
using System.Text.Json.Serialization;

namespace Test
{
    public class rootClassNameA
    {
        [JsonPropertyName(""snake_name"")]
        public string SnakeName { set; get; } = string.Empty;
    }
}",
    };

        // ファイルごとに確認
        foreach (var result in results.OrderBy(res => res.FileName))
        {
            Assert.Equal(true, result.Success);
            
            var expectedFileName = Path.Combine(_createdDirectoryPath, $"{expectedFileNames[index]}.cs");
            Assert.Equal(expectedFileName, result.FileName);

            Assert.Equal(expectedSourceCodes[index], result.SourceCode);
            Assert.Equal(expectedSourceCodes[index], ReadFile(result.FileName));
 
            index++;
        }
    }

    /// <summary>
    /// 作成済みファイルを開いてソースコード取得
    /// </summary>
    /// <param name="filePath">ファイルパス</param>
    /// <returns>ソースコード文字列</returns>
    private string ReadFile(string filePath)
    {
        return File.ReadAllText(filePath);
    }
}