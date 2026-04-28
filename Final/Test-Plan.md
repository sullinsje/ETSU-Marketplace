# Test plan, tests performed (and test types), and analysis reports

## Test Plan
This plan covers automated functional testing of the web interface, testing application builds, and static code analysis

## Objectives:
- Ensure core user workflows (Login, Listing Creation, Search) are functional across different browser sessions.
- Ensure pushed code does not have security risks, major logical errors, etc.

## Tests Performed & Test Types
### UI Testing (Playwright)
We utilized Playwright to simulate user interactions.
- Test Case 01: User Authentication. Verified that users can register with an @etsu.edu email and log in successfully.
- Test Case 02: Item Listing Creation. Verified that a student can upload an item image, set a price, and save it to the marketplace.
- Test Case 03: Search & Filter. Tested the responsiveness of the search bar when filtering for "Leases" vs. "Items."

### Static Code Analysis (CodeQL) 
CodeQL is a tool that can be added to a GitHub repository as a GitHub Action. When adding the workflow to the repository, it will 
automatically detect the languages used in the repository and perform a static code analysis on every file in the repository. It will
also detect packages with known issues

### Application Builds (GitHub Actions, Docker Compose, etc.)
In several testing environments, the application is built to ensure it is compiled without issue. Docker Compose is then used to create 
a new container from the source code. If the Compose or build fails, the application simply does not build properly, indicating syntax
or logical errors. 

## Analysis Reports
Since all testing is done through GitHub Actions, all analysis reports can be found in the insights tab in the ETSU Marketplace repository. 
