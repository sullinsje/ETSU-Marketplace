# Test Automation

## Pipeline
Push to dev branch -> pull request to main branch -> GitHub Actions (Playwright UI, dotnet build, and other tests) -> pulled to main on success

**Every pull request will undergo a GitHub Actions check. Please do not bypass the tests. The option to submit your pull request should be greyed out until the tests pass. Following the tests' success, you should be able to submit your request.**

## Dependencies (for writing tests)
- node.Js v24.14.1
- Playwright Test for VSCode (extension)
- Inside of VSCode search bar run:
  - `>Test: Install Playwright`
- Select the options:
  - `Chromium`
  - `Firefox`
  - `WebKit`
  - `GitHub Actions`

[Playwright Getting Started Guide](https://playwright.dev/docs/getting-started-vscode)

## Recording a test
To learn how to record a test, refer to the [Playwright Test Generator Guide](https://playwright.dev/docs/codegen#recording-a-test)

