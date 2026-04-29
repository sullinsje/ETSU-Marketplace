## Linked Files and Folders

- [Sprints Folder](https://github.com/sullinsje/ETSU-Marketplace/tree/main/Final/Sprints)
- [Architectural Design PNG](https://github.com/sullinsje/ETSU-Marketplace/blob/main/Final/ArchitecturalDesign.png)
- [ERD PNG](https://github.com/sullinsje/ETSU-Marketplace/blob/main/Final/ETSU_Marketplace_ERD(1).png)
- [UI/UX Design PDF](https://github.com/sullinsje/ETSU-Marketplace/blob/main/Final/UI_UX%20Design.pdf)

---

## Product Vision

Our vision for the ETSU Marketplace is to create a secure, campus-only platform where students can confidently buy and sell items within a trusted community. The application focuses on reducing scams, improving accessibility, and simplifying peer-to-peer transactions by restricting access to university users and providing essential marketplace features like listings, search, and messaging.

---

## Project Goals and Release Plan

### Project Goals

The goal of the ETSU Marketplace is to provide a safe, university-focused app where students can buy and sell items with other students. We aim to have the app create a trusted environment, reduce the risk of scams, and make it simple for users to browse, post, and manage listings.

### Primary Goals

- Enable students to create, edit, and manage listings.
- Allow for searching and filtering tools for easier browsing.
- Allow direct communication between buyers and sellers.
- Have a simple and consistent UI.

### Release Plan

The project's release was structured around the midterm and midterm 2/final submission, so we had 2 iterations. The first consisted of the core features of user authentication, CRUD operations for listings, searching, filtering, sorting, and image uploads.

The second one included the completion of the messaging, favoriting, improving the listing details, adding bug reports, metrics, monitoring, CI pipelines, and the Docker setup.

---

## Team Membership

- Jabari Mitchell
- Jacob Sullins
- Palwasha Newell
- Nathanael Wolfe

---

## Development Environment

### Core Framework & Runtime

- Runtime: .NET 10.0
- Primary Framework: ASP.NET Core MVC
- UI Engine: Razor Pages

### Programming Languages

- Backend: C# 13
- Frontend: HTML5, CSS3, JavaScript ES6+
- Scripting/Automation: TypeScript

### Data & Persistence

- Database: SQLite
- ORM: Microsoft Entity Framework Core
- Storage: Local File System, abstracted through a service for image uploads

### DevOps

- Containerization: Docker
- Automation: GitHub Actions

### Testing & Quality Assurance

- Functional Testing: Playwright
- Load/Performance Testing: Locust

### Development Tools & Infrastructure

- IDE: Visual Studio Code with C# Dev Kit
- Version Control: Git
- Architecture: x86-64
- Compatibility: Windows, Linux, and macOS

---

## Deployment Environment

### GitHub Container Registry

When development branches are pulled into main and pass all tests, a GitHub Action containerizes the application with Docker and pushes it to GitHub Container Registry, a platform for hosting Docker containers.

### Production Server

An Amazon Web Services EC2 instance running Ubuntu acts as the production server.

- It has an Elastic IP for consistent access through HTTP and SSH.
- A `compose.yaml` file sits on the server and pulls the latest application image from GHCR, along with images for monitoring tools and NGINX.
- The images are run as containers over the necessary ports.
- On boot, using `crontab`, it utilizes a deployment script called `deploy.sh` that cleans up the previous containers and uses `compose.yaml` to pull fresh ones and run them.
- An NGINX container runs on the server as a reverse proxy, forwarding HTTP requests from the Elastic IP to the application's domain: `http://etsumarketplace.xyz`.

### Running Your Own Instance of ETSU Marketplace

- The server configuration can be placed on any Ubuntu server utilizing the scripts in the repository directory `ETSU-Marketplace/server/`.
- Docker is required for the application to run and can be installed using the `setup.sh` script in `ETSU-Marketplace/server/`.
- Following the Docker installation, ensure that these files inside of `ETSU-Marketplace/server/` are placed in the `/home/ubuntu/` directory on an Ubuntu server:
  - `deploy.sh`
  - `compose.yaml`
  - `nginx.conf`
  - `prometheus.yml`
  - A `.env` file containing a GitHub Access Token for the ETSU Marketplace repository
- Modify `nginx.conf` to point to your domain if you have one. If not, a custom `nginx.conf` will likely be needed.

---

## Detailed Design Document

### Summary

ETSU Marketplace is a web application designed for East Tennessee State University students. It provides a platform for students to post listings to sell their items or request lease takeovers. The system focuses on ensuring users are people with registered ETSU emails and that listings belong to these people.

### Architecture

The application utilizes a monolithic layered architecture using ASP.NET Core.

- Presentation Layer: ASP.NET Core MVC
- Business Logic Layer: Resides in the Services folder; utilizes a repository pattern for managing listings and users and a custom FileStorageService for managing images
- Data Access Layer: Entity Framework Core with SQLite

### Data Model

#### Core Entities

- ApplicationUser: Custom ASP.NET Identity User with 1:1 Image and 1:N Listings relationships
- Listing: Base class with common fields between listing types, with 1:N Images and N:1 User relationships
- ItemListing: Inherits Listing and includes custom fields of Category and Condition
- LeaseListing: Inherits Listing and includes custom fields of Address, LeaseStartDate, LeaseEndDate, and more
- Image: Shared table for paths to avatars and listing photos, with N:1 Listing optional and 1:1 User optional relationships
- Chat Message: Messages associated with a specific Listing using an N:1 Listing relationship

### Technical Specifications

#### Repository Architecture

The system utilizes a Generic Repository Pattern, where services of a type of listing inherit a base repository class. This follows object-oriented programming practices.

- Interfaces for repositories define the expected CRUD operations specific to a listing type.
- Access to the repositories is achieved through the base repository.
- Specific functionality is determined through the type specified, such as in `ListingController()`.

#### Image Management

- All image paths, including avatars and listing images, are stored in a single Images table.
- Listing photos and avatars have cascade delete rules, meaning they are deleted if the associated listing or user is deleted.
- Images are stored on the host's disk in the `wwwroot/images/` folder by the `IFileStorageService`.

#### Security & Authorization

- Identity: Microsoft Identity with custom claims for first and last names.
- Resource Authorization: Controller-level checks ensure `Listing.UserId == CurrentUser.Id` before allowing edit or delete actions.
- Input Sanitization: ViewModels are used to prevent over-posting attacks.

#### CI/CD

- Continuous Integration: GitHub Actions automatically builds the code and runs all tests against the codebase following a pull request to main. If it passes, the branch is integrated into main.
- Continuous Deployment: Following successful testing and integration, the application code is automatically containerized and published to GitHub Container Registry.

---

## Coding Standards

### Naming Conventions

Give variables and methods self-descriptive names. This reduces the need to provide detailed comments in code. Code should be easy to read and understand.

### Commenting

Use self-descriptive code over detailed comments. Add comments when necessary or when leaving notes for later. An area where comments may be useful is a class summary in important classes. Examples include controllers and repositories, which provide the core functionality of the application.

### Modularity

We are using C#, an object-oriented language. Utilize its ability to contain smaller functions in logically similar classes. This will primarily be in the Services folder where the business logic resides.

Remember the pillars of OOP:

- Abstraction
- Encapsulation
- Inheritance
- Polymorphism

The repository layer is utilizing a Generic Repository Pattern. Inheritance and polymorphism are key here to encourage DRY code.

### Formatting

Use the format shortcuts for the programming language's VS Code formatter extensions. This helps keep clean indents and whitespace in code.

### Checks in Code

Make sure to provide checks in your code to prevent unexpected errors. Examples include checking sign-in status and ensuring variables are not null.

---

## Documentation Standards

Our documentation standards for this project involved using the Midterm and Final folders in our GitHub repository to organize our major project artifacts. The documents have clear headings, consistent formatting, and easily readable text. Within the codebase, our documentation consists of XML comments that summarize the functions of the interfaces, controllers, and different services.

---

## Definition of Ready and Done

### Definition of Ready

A user story or task is considered ready when:

- The requirement is clearly written and understandable.
- Acceptance criteria are defined and testable.
- Any dependencies are identified.
- The task is small enough to be completed within a sprint.

### Definition of Done

A user story or task is considered done when:

- The implementation is complete and functions as expected.
- All acceptance criteria are met.
- The application runs without errors.
- Changes have been committed and pushed to the repository.

---

## Change Management and Bug Tracking Process

### Bug Tracking Process

Bugs are tracked using GitHub Issues. There are two ways of reporting bugs.

#### Method 1

When a bug is found, such as "Avatar swaps on listing creation," an issue is created with:

- How to trigger the bug.
- What should happen versus what actually happened.
- Screenshots of the failure, when possible.
- Issue tag, such as bug, high-priority, or UI/UX.

#### Method 2

Within the ETSU Marketplace application, users can use the Bug Report page, found in the footer, to access the form.

The form includes:

- Title
- How to trigger the bug
- What should happen versus what actually happened

On form submission, a GitHub Issue is created in the repository for developers to pick up.

### Resolution

- A developer claims an issue.
- The developer fixes the issue.

### Change Management: Pull Requests

To maintain the main branch's stability, do not push code directly to the main branch. All changes follow the Branch-and-Review model.

### Workflow

- Branches: Every new feature or bug fix must happen on a developer's branch.
- Pull Requests: Once the work is complete, a pull request is opened to merge the branch into main.
- Review: The code is reviewed for logic errors, redundancy, and adherence to UI/UX standards.
- Merge: Once approved and all automated tests pass, the code is merged into main and deployed to GitHub Container Registry.

---

## Version Management

### Docker and GitHub Container Registry

- GitHub Actions, on a successful workflow run, uses Docker to automatically containerize the application and upload it to GitHub Container Registry.
- Due to the frequency of commits and versions and the nature of DevOps, versions are not inherently tracked.
- This application sits on a server, so versioning is not a major priority.
- For documentation purposes, GitHub Container Registry tracks the versions using a hash.
- Specific versions can be installed by their hash.

---

## Test Plan, Tests Performed, and Analysis Reports

### Test Plan

This plan covers automated functional testing of the web interface, testing application builds, and static code analysis.

### Objectives

- Ensure core user workflows, including login, listing creation, and search, are functional across different browser sessions.
- Ensure pushed code does not have security risks, major logical errors, or similar issues.

### Tests Performed and Test Types

#### UI Testing: Playwright

Playwright was used to simulate user interactions.

- Test Case 01: User Authentication. Verified that users can register with an `@etsu.edu` email and log in successfully.
- Test Case 02: Item Listing Creation. Verified that a student can upload an item image, set a price, and save it to the marketplace.
- Test Case 03: Search and Filter. Tested the responsiveness of the search bar when filtering for leases versus items.

#### Static Code Analysis: CodeQL

CodeQL is a tool that can be added to a GitHub repository as a GitHub Action. When adding the workflow to the repository, it automatically detects the languages used in the repository and performs static code analysis on every file in the repository. It also detects packages with known issues.

#### Application Builds: GitHub Actions and Docker Compose

In several testing environments, the application is built to ensure it compiles without issue. Docker Compose is then used to create a new container from the source code. If the Compose or build fails, the application does not build properly, which indicates syntax or logical errors.

### Analysis Reports

Since all testing is done through GitHub Actions, all analysis reports can be found in the Insights tab in the ETSU Marketplace repository.

---

## Test Automation

### Pipeline

Push to dev branch → pull request to main branch → GitHub Actions, including Playwright UI, Docker Compose, CodeQL, and related checks → pulled to main on success.

Every pull request will undergo a GitHub Actions check. Please do not bypass the tests. The option to submit your pull request should be greyed out until the tests pass. Following the tests' success, you should be able to submit your request.

### Dependencies for Writing Playwright Tests

- Node.js v24.14.1
- Playwright Test for VS Code extension

Inside the VS Code search bar, run:

```text
>Test: Install Playwright
```

Select the following options:

- Chromium
- Firefox
- WebKit
- GitHub Actions

Reference: [Playwright Getting Started Guide](https://playwright.dev/docs/getting-started-vscode)

### Recording a Test

To learn how to record a test, refer to the [Playwright Test Generator Guide](https://playwright.dev/docs/codegen#recording-a-test).

Ensure your tests are in the `e2e` directory. This is where the Playwright Tests action pulls the UI tests from.

---

## DevOps Implementation

### DevOps First Way

The DevOps First Way is implemented through the project's automated building and testing. Changes are pushed through GitHub, then tested with GitHub Actions, and must pass required status checks before being merged into main. Docker also supports consistent deployment.

### DevOps Second Way

The DevOps Second Way is implemented through the bug report system. Users are able to submit bug reports from the app, and those reports automatically create GitHub Issues, creating a feedback loop from users to the development team.

### DevOps Third Way

The DevOps Third Way is implemented with monitoring and continuous improvement. Prometheus collects the app's metrics, including request counters, listing creation totals, bug report totals, and listing creation duration.

Grafana visualizes those metrics in a dashboard so the team can see the system's behavior and make improvements based on usage data.
