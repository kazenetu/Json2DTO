using Domain.ValueObjects;

namespace Json2DTO.Test.Domain.ValueObjects;

/// <summary>
/// プロパティValueObjectのテスト
/// </summary>
public class PropertyValueObjectTest
{
    [Fact(DisplayName="ExceptionTest:name is null"), Trait("Category", "Domain:ValueObjects:PropertyValueObjectTest")]
    public void ExceptionNameNull()
    {
        var proprtyType = new PropertyType(typeof(string), false);
        #pragma warning disable
        var ex = Assert.ThrowsAny<ArgumentException>(() => new PropertyValueObject(null, proprtyType));
        Assert.Equal("name is null", ex.Message);
    }

    [Fact(DisplayName="ExceptionTest:name is string.Empty"), Trait("Category", "Domain:ValueObjects:PropertyValueObjectTest")]
    public void ExceptionNameEmpty()
    {
        var proprtyType = new PropertyType(typeof(string), false);
        string name = string.Empty;

        var ex = Assert.ThrowsAny<ArgumentException>(() => new PropertyValueObject(name, proprtyType));
        Assert.Equal($"{nameof(name)} is null", ex.Message);
    }

    [Fact(DisplayName="Test:type is string"), Trait("Category", "Domain:ValueObjects:PropertyValueObjectTest")]
    public void SuccessPropertyTypeString()
    {
        var proprtyType = new PropertyType(typeof(string), false);
        var propertyValueObject = new PropertyValueObject("Name", proprtyType);
        Assert.Equal("Name", propertyValueObject.Name);
        Assert.Equal(string.Empty, propertyValueObject.PropertyTypeClassName);
    }

    [Fact(DisplayName="Test:type is decimal"), Trait("Category", "Domain:ValueObjects:PropertyValueObjectTest")]
    public void SuccessPropertyTypeDecimal()
    {
        var proprtyType = new PropertyType(typeof(string), false);
        var propertyValueObject = new PropertyValueObject("Name", proprtyType);
        Assert.Equal("Name", propertyValueObject.Name);
        Assert.Equal(string.Empty, propertyValueObject.PropertyTypeClassName);
    }

    [Fact(DisplayName="Test:type is bool"), Trait("Category", "Domain:ValueObjects:PropertyValueObjectTest")]
    public void SuccessPropertyTypeBool()
    {
        var proprtyType = new PropertyType(typeof(bool), false);
        var propertyValueObject = new PropertyValueObject("Name", proprtyType);
        Assert.Equal("Name", propertyValueObject.Name);
        Assert.Equal(string.Empty, propertyValueObject.PropertyTypeClassName);
    }

    [Fact(DisplayName="Test:type is nullable"), Trait("Category", "Domain:ValueObjects:PropertyValueObjectTest")]
    public void SuccessPropertyTypeNullable()
    {
        var proprtyType = new PropertyType(typeof(Nullable), false);
        var propertyValueObject = new PropertyValueObject("Name", proprtyType);
        Assert.Equal("Name", propertyValueObject.Name);
    }

    [Fact(DisplayName="Test:type is class(1)"), Trait("Category", "Domain:ValueObjects:PropertyValueObjectTest")]
    public void SuccessClassNo1()
    {
        var classNo = 1;
        PropertyType proprtyType = new PropertyType(classNo , false);
        var propertyValueObject = new PropertyValueObject("Name", proprtyType);
        Assert.Equal("Name", propertyValueObject.Name);
        Assert.Equal("InnerClass", propertyValueObject.PropertyTypeClassName);
    }

    [Fact(DisplayName="Test:type is class(2)"), Trait("Category", "Domain:ValueObjects:PropertyValueObjectTest")]
    public void SuccessClassNo2()
    {
        var classNo = 2;
        PropertyType proprtyType = new PropertyType(classNo , false);
        var propertyValueObject = new PropertyValueObject("Name", proprtyType);
        Assert.Equal("Name", propertyValueObject.Name);
        Assert.Equal("InnerClassA", propertyValueObject.PropertyTypeClassName);
    }
}
