# SmartLead AI Platform

AI-powered lead management and customer automation platform built with .NET Blazor.

## Team Members

- Viszelle Chacon
- Julyanne Lee
- Fernando Antonio Caballero Cruz

# Features

- User authentication and company-based access
- Customer and lead management (CRUD)
- AI-assisted customer responses
- Dashboard analytics and follow-up tracking
- Multi-tenant architecture for company data isolation

# Getting Started

## Prerequisites

- .NET 10.0 SDK (or latest compatible version)
- Visual Studio 2022 or VS Code with the C# Dev Kit extension

## Installation & Setup

1. Clone the repository:

```bash
git clone [your-repository-url]
cd SmartLeadAI
```

2. Configure the database connection in appsettings.json.

3. Apply database migrations:

```bash
dotnet ef database update
```

4. Run the Application:

```bash
dotnet run
```

5. Open the application in your browser using the URL shown in the console output (or through your deployed cloud service URL)

## Project Structure

- Models (/Models): Contains the core domain entities (Company.cs, User.cs, Customer.cs, Interaction.cs).
  - Note: These files utilize Data Annotations (e.g., [Required], [StringLength]) for validation and navigation properties for efficient ORM queries.
- Pages (/Components/Pages): Razor components for the frontend.
  - Integration Note: Look for TODO: comments throughout these files to identify where specific API endpoints and backend services must be wired to dynamic data sources.
- Layout (/Components/Layout): Global navigation and theme definitions (MainLayout.razor, NavMenu.razor).
- Styles (/wwwroot/app.css): Global CSS variables, WCAG-compliant color palette, and typography configurations.

## Quick Start Use Guide

1. Registration: New users must register their company first. Once registered, employees can join the company via their unique CompanyId.

2. Dashboard: Upon login, the dashboard provides a high-level view of your CRM, including recent interactions and pending tasks.

3. Managing Leads:
   - Navigate to the Customers tab to view your current leads.
   - Select "Add Customer" to input new lead details.
   - Ensure all fields are filled, as the system enforces strict data integrity.

4. Logging Interactions: Use the Interaction interface within a Customer detail view to log calls, WhatsApp messages, or meetings. This ensures every touchpoint is tracked and linked to your User ID for future analytics.
