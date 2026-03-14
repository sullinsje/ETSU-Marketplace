# Test plan, tests performed (and test types), and analysis reports

## Test Plan
This plan covers automated functional testing of the web interface and performance testing of the backend services.

Objectives:
- Ensure core user workflows (Login, Listing Creation, Search) are functional across different browser sessions.
- Identify the breaking point of the API under concurrent user load.
- Verify data integrity between the UI and the database.

## Tests Performed & Test Types
A. UI Testing (Selenium)\
We utilized Selenium WebDriver to simulate user interactions.
- Test Case 01: User Authentication. Verified that users can register with an @etsu.edu email and log in successfully.
- Test Case 02: Item Listing Creation. Verified that a student can upload an item image, set a price, and save it to the marketplace.
- Test Case 03: Search & Filter. Tested the responsiveness of the search bar when filtering for "Leases" vs. "Items."

B. Performance & Load Testing (Locust)\
We used Locust to simulate "Rush Hour" traffic on the ETSU Marketplace
- Test Type: Load Testing.
- Scenario: 50 to 100 concurrent users performing GET requests on an api endpoint.
- Test Type: Stress Testing.
- Scenario: Incrementing users until the response time exceeds 2000ms.
