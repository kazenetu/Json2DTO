using Domain.Entities;
using Domain.ValueObjects;

namespace Json2DTO.Test.Domain;

/// <summary>
/// クラスエンティティのテスト
/// </summary>
public class ClassEntityTest
{
    [Fact]
    public void ExceptionClassNameNull()
    {
        string className = null;
        var ex = Assert.ThrowsAny<ArgumentException>(() => ClassEntity.Create(className));
        Assert.Equal("className is null", ex.Message);
    }

    [Fact]
    public void ExceptionClassNameEmpty()
    {
        string className = string.Empty;
        var ex = Assert.ThrowsAny<ArgumentException>(() => ClassEntity.Create(className));
        Assert.Equal("className is null", ex.Message);
    }

    [Fact]
    public void SuccessPropertiesNone()
    {
        string className = "ClassName";
        var classEntity = ClassEntity.Create(className);
        Assert.Equal("ClassName", classEntity.Name);
        Assert.Equal(0, classEntity.Properties.Count);
    }
}
