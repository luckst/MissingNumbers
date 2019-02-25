namespace PuntosColombia.MissingNumbers.Domain.Test
{
    using PuntosColombia.MissingNumbers.API.Controllers;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SaludoTest
    {
        private SaludoController sc = new SaludoController();
        [TestMethod]
        public void SaludoBasico()
        {
            string resultado = sc.Get();

            Assert.AreEqual(resultado, "Hola a todos!");
        }
    }
}
