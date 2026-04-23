# Detailed Design Document

## Summary
ETSU Marketplace is a web application designed for East Tennessee State University students. It provides a platform for 
students to post listings to sell their items or request lease takeovers. The system focuses on ensuring users are people
with registered ETSU emails and that listings belong to these people. 

## Architecture
The application utilizes a monolithic layered architecture using ASP.NET Core
- Presentation Layer: ASP.NET Core MVC 
- Business Logic Layer: Resides in our Services folder; utilizes a repository pattern for managing listings and users
  and a custom FileStorageService for managing images
- Data Access Layer: Entity Framework Core with SQLite

## Data Model
### Core Entities
- ApplicationUser: Custom ASP.NET Identity User (1:1 Image, 1:N Listings)
- Listing (Base): Common fields between listing types (1:N Images, N:1 User)
- ItemListing: Inherits Listing, custom fields of Category and Condition
- LeaseListing: Inherits Listing, custom fields of Address, LeaseStartDate, LeaseEndDate, etc.
- Image: Shared table for paths to Avatars and Listing photos (N:1 Listing (optional), 1:1 User (optional))
- Chat Message: Messages associated with a specific Listing (N:1 Listing)

## Technical Specifications
### Repository Architecture
The system utilizes a Generic Repository Pattern, where services of a type of listing inherit a base repository class; this is in line with Object Oriented Programming practices
- We have interfaces for repositories to define the contract for CRUD operations specific to a listing type
- Access to the repositories is achieved through the base repository; specific functionality is determined through the type specified, for example, ListingController<ItemListing>()
### Image Management
- Centralized Image Table: All image paths (avatars and listing images) are stored in a single Images table
- Listing Photos and Avatars have Cascade Delete rules, meaning they are deleted if the associated listing or user is deleted
- Images are stored on the host's disk in the `wwwroot/images/` folder, put there by our IFileStorageService
### Security & Authorization
- Identity: Microsoft Identity with custom claims for First/Last names.
- Resource Authorization: Controller-level checks ensure `Listing.UserId == CurrentUser.Id` before allowing Edit or Delete actions.
- Input Sanitization: ViewModels are used to prevent Over-Posting attacks.
### CI/CD
- Continuous Integration: GitHub Actions automatically builds the code and runs all tests against the codebase following a pull request to main. If it passes, the branch is integrated to main
- Continuous Deployment: Following successful testing and integration, the application code is then automatically containerized and published to GitHub Container Registry
