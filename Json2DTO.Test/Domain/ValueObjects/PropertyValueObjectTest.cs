using Domain.ValueObjects;

namespace Json2DTO.Test.Domain.ValueObjects;

/// <summary>
/// プロパティValueObjectのテスト
/// </summary>
public class PropertyValueObjectTest
{
    [Fact]
    public void ExceptionNameNull()
    {
        var proprtyType = new PropertyType(typeof(string), false);
        #pragma warning disable
        var ex = Assert.ThrowsAny<ArgumentException>(() => new PropertyValueObject(null, proprtyType));
        Assert.Equal("name is null", ex.Message);
    }

    [Fact]
    public void ExceptionNameEmpty()
    {
        var proprtyType = new PropertyType(typeof(string), false);
        string name = string.Empty;

        var ex = Assert.ThrowsAny<ArgumentException>(() => new PropertyValueObject(name, proprtyType));
        Assert.Equal($"{nameof(name)} is null", ex.Message);
    }

    [Fact]
    public void SuccessPropertyTypeString()
    {
        var proprtyType = new PropertyType(typeof(string), false);
        var propertyValueObject = new PropertyValueObject("Name", proprtyType);
        Assert.Equal("Name", propertyValueObject.Name);
        Assert.Equal(string.Empty, propertyValueObject.PropertyTypeClassName);
    }

    [Fact]
    public void SuccessPropertyTypeDecimal()
    {
        var proprtyType = new PropertyType(typeof(string), false);
        var propertyValueObject = new PropertyValueObject("Name", proprtyType);
        Assert.Equal("Name", propertyValueObject.Name);
        Assert.Equal(string.Empty, propertyValueObject.PropertyTypeClassName);
    }

    [Fact]
    public void SuccessPropertyTypeBool()
    {
        var proprtyType = new PropertyType(typeof(bool), false);
        var propertyValueObject = new PropertyValueObject("Name", proprtyType);
        Assert.Equal("Name", propertyValueObject.Name);
        Assert.Equal(string.Empty, propertyValueObject.PropertyTypeClassName);
    }

    [Fact]
    public void SuccessPropertyTypeNullable()
    {
        var proprtyType = new PropertyType(typeof(Nullable), false);
        var propertyValueObject = new PropertyValueObject("Name", proprtyType);
        Assert.Equal("Name", propertyValueObject.Name);
    }

    [Fact]
    public void SuccessClassNo1()
    {
        var classNo = 1;
        PropertyType proprtyType = new PropertyType(classNo , false);
        var propertyValueObject = new PropertyValueObject("Name", proprtyType);
        Assert.Equal("Name", propertyValueObject.Name);
        Assert.Equal("InnerClass", propertyValueObject.PropertyTypeClassName);
    }

    [Fact]
    public void SuccessClassNo2()
    {
        var classNo = 2;
        PropertyType proprtyType = new PropertyType(classNo , false);
        var propertyValueObject = new PropertyValueObject("Name", proprtyType);
        Assert.Equal("Name", propertyValueObject.Name);
        Assert.Equal("InnerClassA", propertyValueObject.PropertyTypeClassName);
    }
}
