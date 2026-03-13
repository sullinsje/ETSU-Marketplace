# Change Management & Bug Tracking Process
## Bug Tracking Process 
We currently log our bugs and issues inside of our Discord server, our primary form of communication. However, we plan to 
utilize GitHub Issues as our primary bug tracker. This ensures that every bug is documented, assigned, and linked to the 
specific code change that fixes it.
### Reporting: 
When a bug is found (e.g., "Avatar swaps on listing creation"), an Issue is created with:
- How to trigger the bug.
- What should happen vs. what did happen.
- Preferably screenshots of the failure.
- Issue tag (e.g., bug, high-priority, UI/UX).
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
- Merge: Once approved and all tests (Selenium/Locust) pass, the code is merged into main.
