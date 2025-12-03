# Internet Safety Plan
An all-in-one, parent-focused platform for managing a healthy and safe internet environment at home.

[NOTE: This project was originally planned to be an app, but I since pivoted and created the [Internet Safety Plan website](https://internetsafetyplan.com). The purpose of this project is now just to highlight my development capabilities.]

[NOTE 2: This repo was copied from repos in my Azure DevOps project (that unfortunately cannot be made public), hence the simplistic commit history. If you are an employer and would like to see the full commit history in AzDo, please reach out to me and I'll happily give you access to the project]. 

---

## Table of Contents
- [Introduction](#introduction)
- [Tech Stack](#tech-stack)
- [Coding Practices](#coding-practices)
- [Architecture Overview](#architecture-overview)
- [Build and Run](#build-and-run)
- [Usage](#usage)
- [Contributing](#contributing)
- [License](#license)
- [Contact](#contact)

---

# Introduction
The **Internet Safety Plan** is an all-in-one action plan for parents to track and manage the internet-safe environment they are establishing with their families. It features everything parents need to maintain their course and achieve their internet safety goals, including:

- Device-management lists and charts  
- Periodic check-ins on goals  
- Regular updates on new software, apps, and trends  
- Communication tips for raising kids to think critically and make wise online decisions  

# Tech Stack

- **Backend:** .NET 8
- **Frontend:** React  
- **Auth:** Keycloak  
- **Database:** MS SQL Server

---
# Coding Practices
TODO: Note good coding practice stuff I did like DDD, onion architecture, etc.

---

# Architecture Overview
- The **React frontend** handles the user interface for parents.
- The **.NET backend** manages business logic and DB connectivity
- **Keycloak** is an OIDC + OAuth 2.0 implementation that provides authentication, RBAC, and secure session management.

---

# Build and Run
## Prerequisites
- .NET 8 SDK  
- Node.js + npm  (TODO: Add versions)
- Docker or buildah + podman (TODO: Add versions; for running Keycloak locally)  

## Backend Setup


## Frontend Setup


## Keycloak Setup


# Usage
Once the backend, frontend, and Keycloak are running:
- Navigate to the frontend in your browser by going to [TODO: Add URL]
- Log in using your Keycloak account.
- Set up your family.
- Add devices, set goals, and start receiving check-ins.
- Explore the knowledge base and app/device guides.

# Contributing
Contributions are welcome!
Please open an issue or submit a pull request following the project’s coding style and guidelines.

# License
This project is licensed under a Non-Commercial License.
Use, modification, and distribution are allowed for non-commercial purposes only.
See the LICENSE file for details.

# Contact
Internet Safety Consulting, LLC
Website: https://internetsafetyconsulting.com
Email: [casey@internetsafetyconsulting.com](mailto:casey@internetsafetyconsulting.com)
