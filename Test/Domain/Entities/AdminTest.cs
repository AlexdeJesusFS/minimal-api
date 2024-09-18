using minimal_api.Domain.Entities;

namespace Test.Domain.Entities;

[TestClass]
public class AdminTest
{
    [TestMethod]
    public void TestGetSetProperties()
    {
        //arrange
        var adm = new Admin();
        
        //act
        adm.Id = 1;
        adm.Email = "test@test.com";
        adm.Password = "test";
        adm.Rule = "Adm";

        //assert
        Assert.AreEqual(1, adm.Id);
        Assert.AreEqual("test@test.com", adm.Email);
        Assert.AreEqual("test", adm.Password);
        Assert.AreEqual("Adm", adm.Rule);
    }
}
