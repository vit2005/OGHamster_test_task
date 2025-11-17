using NUnit.Framework;
using UnityEngine;

public class MoneyProviderTests
{
    [SetUp]
    public void Setup()
    {
        PlayerPrefs.DeleteAll();
    }

    [Test]
    public void IncreaseMoney_AddsCorrectAmount()
    {
        var provider = new MoneyProvider();
        provider.Init();

        provider.IncreaseMoney(5);

        Assert.AreEqual(5, provider.Amount);
    }

    [Test]
    public void DecreaseMoney_DoesNotGoBelowZero()
    {
        var provider = new MoneyProvider();
        provider.Init();

        provider.DecreaseMoney(10);

        Assert.AreEqual(0, provider.Amount);
    }

    [Test]
    public void Save_PersistsData()
    {
        var provider = new MoneyProvider();
        provider.Init();
        provider.IncreaseMoney(42);
        provider.Save();

        var newProvider = new MoneyProvider();
        newProvider.Init();

        Assert.AreEqual(42, newProvider.Amount);
    }
}