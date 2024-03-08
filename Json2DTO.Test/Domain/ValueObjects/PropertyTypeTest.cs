using Domain.ValueObjects;

namespace Json2DTO.Test.Domain.ValueObjects;

/// <summary>
/// プロパティ型ValueObjectのテスト
/// </summary>
public class Domain:ValueObjects:PropertyTypeTest
{
    [Fact(DisplayName="ExceptionTest:Type is ErrorType"), Trait("Category", "Domain:ValueObjects:PropertyTypeTest")]
    public void ExceptionTypeObject()
    {
        var isList = false;
        var type = typeof(Object);
        var ex = Assert.ThrowsAny<ArgumentException>(() => new PropertyType(type, isList));
        Assert.Equal($"{nameof(type)}({type.Name}) is null", ex.Message);
    }

    [Fact(DisplayName="ExceptionTest:Type is ErrorType List"), Trait("Category", "Domain:ValueObjects:PropertyTypeTest")]
    public void ExceptionTypeObjectList()
    {
        var isList = true;
        var type = typeof(Object);
        var ex = Assert.ThrowsAny<ArgumentException>(() => new PropertyType(type, isList));
        Assert.Equal($"{nameof(type)}({type.Name}) is null", ex.Message);
    }

    [Fact(DisplayName="ExceptionTest:ClassNo is Minus"), Trait("Category", "Domain:ValueObjects:PropertyTypeTest")]
    public void ExceptionClassNoMinus()
    {
        var isList = false;
        var classNo = -1;
        var ex = Assert.ThrowsAny<ArgumentException>(() => new PropertyType(classNo, isList));
        Assert.Equal($"{nameof(classNo)} is negative value", ex.Message);
    }

    [Fact(DisplayName="ExceptionTest:ClassNo is Minus List"), Trait("Category", "Domain:ValueObjects:PropertyTypeTest")]
    public void ExceptionClassNoMinusList()
    {
        var isList = true;
        var classNo = -1;
        var ex = Assert.ThrowsAny<ArgumentException>(() => new PropertyType(classNo, isList));
        Assert.Equal($"{nameof(classNo)} is negative value", ex.Message);
    }

    [Fact(DisplayName="Test:Type is string"), Trait("Category", "Domain:ValueObjects:PropertyTypeTest")]
    public void SuccessTypeString()
    {
        var isList = false;
        var type = typeof(string);
        PropertyType propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.String, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }

    [Fact(DisplayName="Test:Type is decimal"), Trait("Category", "Domain:ValueObjects:PropertyTypeTest")]
    public void SuccessTypeDecimal()
    {
        var isList = false;
        var type = typeof(decimal);
        var propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.Decimal, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }

    [Fact(DisplayName="Test:Type is bool"), Trait("Category", "Domain:ValueObjects:PropertyTypeTest")]
    public void SuccessTypeBool()
    {
        var isList = false;
        var type = typeof(bool);
        var propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.Bool, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }

    [Fact(DisplayName="Test:Type is nullable"), Trait("Category", "Domain:ValueObjects:PropertyTypeTest")]
    public void SuccessTypeNullable()
    {
        var isList = false;
        var type = typeof(Nullable);
        var propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.Null, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }

    [Fact(DisplayName="Test:Type is string List"), Trait("Category", "Domain:ValueObjects:PropertyTypeTest")]
    public void SuccessTypeStringList()
    {
        var isList = true;
        var type = typeof(string);
        PropertyType propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.String, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }

    [Fact(DisplayName="Test:Type is decimal List"), Trait("Category", "Domain:ValueObjects:PropertyTypeTest")]
    public void SuccessTypeDecimalList()
    {
        var isList = true;
        var type = typeof(decimal);
        var propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.Decimal, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }

    [Fact(DisplayName="Test:Type is bool List"), Trait("Category", "Domain:ValueObjects:PropertyTypeTest")]
    public void SuccessTypeBoolList()
    {
        var isList = true;
        var type = typeof(bool);
        var propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.Bool, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }

    [Fact(DisplayName="Test:Type is nullable List"), Trait("Category", "Domain:ValueObjects:PropertyTypeTest")]
    public void SuccessTypeNullableList()
    {
        var isList = false;
        var type = typeof(Nullable);
        var propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.Null, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }

    [Fact(DisplayName="Test:Type is classNo(0)"), Trait("Category", "Domain:ValueObjects:PropertyTypeTest")]
    public void SuccessClassNo0()
    {
        var isList = false;
        var classNo = 0;
        var propertyType = new PropertyType(classNo, isList);
        Assert.Equal(PropertyType.Kinds.Class, propertyType.Kind);
        Assert.Equal("InnerClass", propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }

    [Fact]
    [Fact(DisplayName="Test:Type is classNo(1)"), Trait("Category", "Domain:ValueObjects:PropertyTypeTest")]
    public void SuccessClassNo1()
    {
        var isList = false;
        var classNo = 1;
        var propertyType = new PropertyType(classNo, isList);
        Assert.Equal(PropertyType.Kinds.Class, propertyType.Kind);
        Assert.Equal("InnerClass", propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }

    [Fact(DisplayName="Test:Type is classNo(2)"), Trait("Category", "Domain:ValueObjects:PropertyTypeTest")]
    public void SuccessClassNo2()
    {
        var isList = false;
        var classNo = 2;
        var propertyType = new PropertyType(classNo, isList);
        Assert.Equal(PropertyType.Kinds.Class, propertyType.Kind);
        Assert.Equal("InnerClassA", propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }

    [Fact(DisplayName="Test:Type is classNo(3)"), Trait("Category", "Domain:ValueObjects:PropertyTypeTest")]
    public void SuccessClassNo3()
    {
        var isList = false;
        var classNo = 3;
        var propertyType = new PropertyType(classNo, isList);
        Assert.Equal(PropertyType.Kinds.Class, propertyType.Kind);
        Assert.Equal("InnerClassB", propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }
}
