﻿using MarleneCollectionXmlTool.Domain.Helpers;
using MarleneCollectionXmlTool.Domain.Helpers.Providers;
using Xunit;

namespace MarleneCollectionXmlTool.Domain.Tests.Helpers;

public class EnvironmentVariableHelpersTests
{
    [Theory]
    [InlineData(23)]
    [InlineData(2.1)]
    public void GetEnvironmentVariableOrDefault_ShouldReturnParsedValue_GivenDecimalValue(decimal value)
    {
        //Arrange
        var environementName = "EnvironmentVariableName";
        Environment.SetEnvironmentVariable(environementName, value.ToString());

        //Act
        var result = EnvironmentVariableValueProvider.GetEnvironmentVariableOrDefault<decimal>(environementName);

        //Assert
        Assert.Equal(value, result);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void GetEnvironmentVariableOrDefault_ShouldReturnParsedValue_GivenBoolValue(bool value)
    {
        //Arrange
        var environementName = "EnvironmentVariableName";
        Environment.SetEnvironmentVariable(environementName, value.ToString());

        //Act
        var result = EnvironmentVariableValueProvider.GetEnvironmentVariableOrDefault<bool>(environementName);

        //Assert
        Assert.Equal(value, result);
    }
}
