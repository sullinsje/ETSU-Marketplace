# Change Management & Bug Tracking Process
## Bug Tracking Process 
Bugs are tracked using GitHub Issues. We have two ways of reporting bugs: 
### Method 1: 
When a bug is found (e.g., "Avatar swaps on listing creation"), an Issue is created with:
- How to trigger the bug.
- What should happen vs. what did happen.
- Preferably screenshots of the failure.
- Issue tag (e.g., bug, high-priority, UI/UX).
### Method 2: 
- Within the ETSU Marketplace application, use the Bug Report page (found at any pages' footer) to access the form
- Submit the form with the following information:
  - Title
  - How to trigger the bug.
  - What should happen vs. what did happen.
- On form submission, a GitHub Issue will be created in the repository for developers to pick up
### Resolution: 
- A developer claims an issue
- Developer fixes the issue

## Change Management (Pull Requests)
To maintain the main branch's stability, do not push code directly to the main branch. 
All changes follow the Branch-and-Review model.

### Workflow:
- Branches: Every new feature (e.g., "Lease Listings") or bug fix must happen on a developer's branch
- Pull Requests: Once the work is complete, a pull request is opened to merge the branch into main.
- Review: Please review the code for:
  - Logic errors
  - Redundancy
  - Adherence to UI/UX standards 
- Merge: Once approved and all automated tests pass, the code is merged into main and deployed to GitHub Container Registry
