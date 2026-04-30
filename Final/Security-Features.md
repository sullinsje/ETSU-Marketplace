# Security Features 

This document outlines the security features implemented within the system and the supporting infrastructure technologies that enhance its defensive posture.

## Core Security Implementation
Based on the system requirements, the following features are prioritized:

* **ETSU Email Auth:** Only verified ETSU accounts can access the system, creating a closed, trusted environment.
* **ASP.NET Identity:** Implementation of industry-standard identity management for secure authentication and account recovery.
* **Authorization Controls:** Robust Role-Based Access Control (RBAC) to manage permissions and ensure data privacy.
* **Input Validation:** Strict protection mechanisms against malicious input, mitigating risks like SQL injection and cross-site scripting (XSS).

## Security Enhancements via Infrastructure

### Docker
Docker provides **isolation**. By running the application in a container, the host system is shielded. It ensures that the application has only the minimum required resources and prevents a single compromised service from easily accessing the rest of the infrastructure.

### NGINX
NGINX serves as a **Reverse Proxy and Gatekeeper**. It provides a layer of anonymity for the backend servers and can be configured for rate limiting to protect against brute force and Denial of Service (DoS) attacks.

### CodeQL
CodeQL is a **Static Analysis Security Testing (SAST)** tool. It allows developers to query code like data, identifying complex security vulnerabilities and coding errors early in the development lifecycle (Shift-Left Security), ensuring that only secure code is deployed.
