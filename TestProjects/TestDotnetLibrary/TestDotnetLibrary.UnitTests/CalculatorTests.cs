namespace TestDotnetLibrary.UnitTests
{
    [TestClass]
    public class CalculatorTests
    {
        [TestMethod]
        public void TestAdd()
        {
            // Arrange
            var arg1 = 1;
            var arg2 = 2;
            var expectedResult = 3;

            var calculator = new Calculator();

            // Act
            var result = calculator.Add(arg1, arg2);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}