# 🌻 K4S System Enhancement Project

> **Krumpin 4 Success (K4S)** is a community program for at-risk youth. This project enhances the K4S web system to support user management, service enrollment, attendance tracking, and role-based access tailored to administrators, instructors, and students.

---

## Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Technology Stack](#technology-stack)
- [System Roles](#system-roles)
- [Authentication & Security](#authentication--security)
- [Key Functional Modules](#key-functional-modules)
- [Getting Started](#getting-started)
- [Future Enhancements](#future-enhancements)
- [Team](#team)

---

## Overview

The K4S System Enhancement Project aims to improve service delivery for at-risk youth by implementing a robust role-based web platform. The application streamlines user registration, program enrollment, attendance tracking, and administrative oversight.

---

## Features

- ✅ Self-registration and user profile management  
- 🧑‍🏫 Instructor-led service and section management  
- 🗓️ Attendance tracking (individual and class-wide)  
- 🧮 Real-time performance metrics  
- 🔐 Secure password reset and account deletion  
- 📊 Administrative role and permission assignment  

---

## Our Technology Stack

- **Backend:** ASP.NET Core  
- **Frontend:** Razor Pages (ASP.NET)  
- **Database:** Azure SQL  
- **Development Tools:** Visual Studio, Azure DevOps  
- **Authentication:** ASP.NET Core Identity  

---

## System Roles

### Administrator

- Assign roles (User, Instructor, Admin)  
- Manage users (reset passwords, delete accounts)  

### Instructor

- Manage services and sections  
- Enroll students and track attendance  
- View and report class performance  

### General User (Student)

- Register and enroll in services  
- Track attendance and progress  
- Manage personal profile  

---

## Authentication & Security

- **Role-Based Authentication:** Implemented with ASP.NET Core Identity  
- **Policy-Based Authorization:** Restricts access based on roles  
- **Session Control:** Ensures single-submission logic

---

## Key Functional Modules

### User Registration

- Self-registration form with validation  
- Application status tracking  

### Service & Section Management

- Admins/instructors can create and manage offerings  
- Support for multiple sections per service  

### Attendance Tracking

- Weekly attendance calendar with multiple status options  
- Instructor view: bulk actions, real-time calculations  
- Student view: personal attendance statistics  

### Performance Metrics

- View individual and class-wide attendance percentages  
- Time-series tracking of student engagement  

---

## Getting Started

1. **Clone the repository**
   ```bash
   git clone https://github.com/YOUR_ORG/K4S-System.git
   cd K4S-System
   ```

2. **Configure the database**
   - Update the connection string in `appsettings.json` with your Azure SQL credentials.

3. **Run the application**
   - Open in Visual Studio and press `F5`  
   - Or use the command line:
     ```bash
     dotnet run
     ```

4. **Set up roles**
   - Use the admin dashboard to assign users to the correct roles.

---

## Future Enhancements

- Introduce a Manager Role for operational oversight  
- Implement Certificate Generation for program completion  
- Enable multi-admin access with unique credentials  
- Fully remove all plaintext credentials from any config/code  
- Streamline password recovery process for user independence  

---

## 👥 Team

- **Ann Ubaka**  
- **Brittany Lee**  
- **Sarah Wehrung**  

**Course:** CS 395SI – Software Engineering  
**Instructor:** Dr. Zeng  
**Semester:** Spring 2025  
**Team:** Group 3