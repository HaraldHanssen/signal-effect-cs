namespace SignalEffectTest;

[TestClass]
public class InternalTest
{
    [TestMethod]
    public void CanDeleteWithinAForEachLoop()
    {
        var map = new Dictionary<int, string>();
        for (var i = 0; i < 5; i++)
        {
            map.Add(i, string.Empty);
        }
        var count = 0;
        foreach (var (k, _) in map)
        {
            map.Remove(k);
            count++;
        }
        Assert.AreEqual(0, map.Count);
        Assert.AreEqual(5, count);
    }
}
