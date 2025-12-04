# Internet Safety Plan
An all-in-one, parent-focused platform for managing a healthy and safe internet environment at home.

[NOTE: This project was originally planned to be an app, but I since pivoted and created the [Internet Safety Plan website](https://internetsafetyplan.com). The purpose of this project is now just to highlight my development capabilities. See the [App Walkthrough](https://github.com/7CLewis/InternetSafetyPlan/wiki/App-Walkthrough) Wiki for more in-depth info on the app and each of its parts].

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
- Visual Studio 2022 or later
- SSMS v18 or later
- SQL Server Express
- Node.js v20 or later
- npm v9 or later
- WSL2 with newest versions of `buildah` & `podman` installed (`sudo apt install podman buildah -y` should do it for Ubuntu-based Linux)

## Backend Setup
- Set up a new SQL Server DB locally named `InternetSafetyPlanDev`. 
- Copy the connection string information into the `InternetSafetyPlan` connection string in the Infrastructure project's `appsettings.Development.json`.
  - Account used for connection string needs `db_datareader` and `db_datawriter` permissions on the DB.
- Open the `InternetSafetyPlan.Backend` Solution in Visual Studio
- Set `InternetSafetyPlan.Infrastructure` as the Startup Project.
- Open the Package Manager Console, and set the Infrastructure project as the default.
- Run the command `Script-Migration -From 0 -To Initial -Output "migrations\Initial.sql"`
  - Can leave off the `Output` arg if you don't want to save it (or change the directory you output to)
- Copy-paste the outputted SQL into your `InternetSafetyPlanDev` DB. This'll initialize the DB Schema with all tables, indices, and table relationships.
- Back in VS, set `InternetSafetyPlan.Api` as the Startup project, and run it (may need to accept SSL certs on first run)
- In a browser, navigate to https://localhost:7070/swagger/index.html
- Try out some of the endpoints with Swagger.

## IAM Setup
- Open WSL2 and navigate to the `src/auth/Keycloak` directory
- Run the command `./local-containers.sh`. This will build the Keycloak image based on the Dockerfile, create a pod, and run a container.
- In a browser, navigate to http://localhost:8080/ (may take a few minutes to start running)
- Log in with the credentials found in `src/auth/Keycloak/.env`.
- Navigate to the internet-safety Realm
- Create a User, and set a password for them in the Credentials tab.

Success! You now have a user you can use to log into the app once the frontend is running. 
- Note that Keycloak is a full Identity and Access Management platform. You can set up SSO with Microsoft, Google, etc. if you want, instead of having local, Keycloak-only users.

## Frontend Setup
- In a terminal with node and npm installed, navigate to `src/frontend`
- Run `npm i`
- Run `npm run dev`

# Usage
Once the backend, frontend, and Keycloak are running:
- Navigate to the frontend in your browser by going to http://localhost:5173/
- Log in using your Keycloak account.
- Set up your family.
- Add devices, set goals and action items, and get internet-safe!

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
