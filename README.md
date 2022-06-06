# privacy-budget

This is the backend for Privacy Budget, a privacy-conscious budgetting tool.

## Running the backend

`TODO`

## Running the tests

Run the following to view the test results:
```
dotnet test
```

### Viewing the coverage

1. First install the report generator globally:
```
dotnet tool install --global dotnet-reportgenerator-globaltool --version 5.1.9
```

2. Run the tests and open the coverage report:
```
dotnet test --collect:"XPlat Code Coverage"
reportgenerator "-reports:./PrivacyBudgetServertests/TestResults/<id>/coverage.cobertura.xml" "-targetdir:.coverage" -reporttypes:HTML;
```

3. Open `.coverage/index.html` in your browser
