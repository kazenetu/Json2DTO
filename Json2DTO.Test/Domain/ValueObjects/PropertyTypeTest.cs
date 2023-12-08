using Domain.ValueObjects;

namespace Json2DTO.Test.Domain.ValueObjects;

/// <summary>
/// プロパティ型ValueObjectのテスト
/// </summary>
public class PropertyTypeTest
{
    [Fact]
    public void ExceptionTypeObject()
    {
        var isList = false;
        var type = typeof(Object);
        var ex = Assert.ThrowsAny<ArgumentException>(() => new PropertyType(type, isList));
        Assert.Equal($"{nameof(type)}({type.Name}) is null", ex.Message);
    }

    [Fact]
    public void ExceptionTypeObjectList()
    {
        var isList = true;
        var type = typeof(Object);
        var ex = Assert.ThrowsAny<ArgumentException>(() => new PropertyType(type, isList));
        Assert.Equal($"{nameof(type)}({type.Name}) is null", ex.Message);
    }

    [Fact]
    public void ExceptionClassNoMinus()
    {
        var isList = false;
        var classNo = -1;
        var ex = Assert.ThrowsAny<ArgumentException>(() => new PropertyType(classNo, isList));
        Assert.Equal($"{nameof(classNo)} is negative value", ex.Message);
    }

    [Fact]
    public void ExceptionClassNoMinusList()
    {
        var isList = true;
        var classNo = -1;
        var ex = Assert.ThrowsAny<ArgumentException>(() => new PropertyType(classNo, isList));
        Assert.Equal($"{nameof(classNo)} is negative value", ex.Message);
    }

    [Fact]
    public void SuccessTypeString()
    {
        var isList = false;
        var type = typeof(string);
        PropertyType propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.String, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }

    [Fact]
    public void SuccessTypeDecimal()
    {
        var isList = false;
        var type = typeof(decimal);
        var propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.Decimal, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }

    [Fact]
    public void SuccessTypeBool()
    {
        var isList = false;
        var type = typeof(bool);
        var propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.Bool, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }

    [Fact]
    public void SuccessTypeNullable()
    {
        var isList = false;
        var type = typeof(Nullable);
        var propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.Null, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }

    [Fact]
    public void SuccessTypeStringList()
    {
        var isList = true;
        var type = typeof(string);
        PropertyType propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.String, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }

    [Fact]
    public void SuccessTypeDecimalList()
    {
        var isList = true;
        var type = typeof(decimal);
        var propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.Decimal, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }

    [Fact]
    public void SuccessTypeBoolList()
    {
        var isList = true;
        var type = typeof(bool);
        var propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.Bool, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }

    [Fact]
    public void SuccessTypeNullableList()
    {
        var isList = false;
        var type = typeof(Nullable);
        var propertyType = new PropertyType(type, isList);
        Assert.Equal(PropertyType.Kinds.Null, propertyType.Kind);
        Assert.Equal(string.Empty, propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }

    [Fact]
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
    public void SuccessClassNo1()
    {
        var isList = false;
        var classNo = 1;
        var propertyType = new PropertyType(classNo, isList);
        Assert.Equal(PropertyType.Kinds.Class, propertyType.Kind);
        Assert.Equal("InnerClass", propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }

    [Fact]
    public void SuccessClassNo2()
    {
        var isList = false;
        var classNo = 2;
        var propertyType = new PropertyType(classNo, isList);
        Assert.Equal(PropertyType.Kinds.Class, propertyType.Kind);
        Assert.Equal("InnerClassA", propertyType.ClassName);
        Assert.Equal(isList, propertyType.IsList);
    }

    [Fact]
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
