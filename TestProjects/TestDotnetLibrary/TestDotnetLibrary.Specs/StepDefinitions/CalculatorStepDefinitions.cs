namespace TestDotnetLibrary.Specs.StepDefinitions
{
    [Binding]
    public sealed class CalculatorStepDefinitions
    {
        private readonly ScenarioContext _scenarioContext;

        public CalculatorStepDefinitions(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [Given("the first number is (.*)")]
        public void GivenTheFirstNumberIs(int number)
        {
            //TODO: implement arrange (precondition) logic
            // For storing and retrieving scenario-specific data see https://go.specflow.org/doc-sharingdata
            // To use the multiline text or the table argument of the scenario,
            // additional string/Table parameters can be defined on the step definition
            // method. 

            _scenarioContext.Set(number, "FirstNumber");
        }

        [Given("the second number is (.*)")]
        public void GivenTheSecondNumberIs(int number)
        {
            //TODO: implement arrange (precondition) logic

            _scenarioContext.Set(number, "SecondNumber");
        }

        [When("the two numbers are subtracted")]
        public void WhenTheTwoNumbersAreAdded()
        {
            //TODO: implement act (action) logic
            var firstNumber = _scenarioContext.Get<int>("FirstNumber");
            var secondNumber = _scenarioContext.Get<int>("SecondNumber");

            var calculator = new Calculator();
            var result = calculator.Subtract(firstNumber, secondNumber);

            _scenarioContext.Set(result, "Result");
        }

        [Then("the result should be (.*)")]
        public void ThenTheResultShouldBe(int result)
        {
            //TODO: implement assert (verification) logic
            var calculatedResult = _scenarioContext.Get<int>("Result");

            Assert.AreEqual(result, calculatedResult);
        }
    }
}