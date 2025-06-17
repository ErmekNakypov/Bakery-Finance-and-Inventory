# Bakery-Finance-and-Inventory

## 📌 Project Description

This is a web-based information system for automating operations in a confectionery production business. The system includes:

- **Inventory Management** – tracking quantities and values of raw materials and finished products.
- **Product Manufacturing** – automatically updates stock levels using SQL triggers.
- **Payroll Management** – calculates salaries based on employee participation in production, purchases, and sales.
- **Credit Tracking** – manages credits and repayments, including interest and penalties.
- **Purchasing & Sales** – records raw material purchases and finished product sales.
- **Reporting** – detailed and summary reports on manufacturing, sales, payroll, and credit payments, with filters by date and other parameters.

## ⚙️ Technologies Used

- **ASP.NET MVC** – for the client-side web application structure.
- **Entity Framework Core** – ORM for database access with LINQ support.
- **Microsoft SQL Server** – for storing all application data; includes triggers and stored procedures.
- **ASP.NET Identity** – for authentication and role-based authorization.
- **Clean Architecture** – layered project structure:

## 👤 User Roles

- **Administrator** – full access to system and reports.
- **Technologist** – manages manufacturing and inventory.
- **Accountant** – handles payroll and credits.
- **Director** – views all reports and analytics.
