# Finance Tracker App

A Blazor Server finance tracker for personal money management with a local-first architecture.

## 1) Project Overview

Finance Tracker App helps users:
- Track income and expenses
- Manage custom categories
- Manage savings goals and contributions
- View dashboard and analytics summaries
- Configure profile and preferences

## 2) Tech Stack

- ASP.NET Core Blazor Server (`net10.0`)
- Entity Framework Core + SQLite
- xUnit (unit testing)
- Firebase Auth integration (with configurable demo login switch)

## 3) Architecture

### Local-first data
- Transactions: SQLite (`TransactionService`)
- Categories: SQLite (`CategoryService`)
- Goals + Contributions: SQLite (`LocalGoalsStore`)

### Auth
- Firebase Auth service path is present (`AuthService`)
- Demo login is controlled by config (`Firebase:AllowDemoLogin`)

## 4) Core Features

- Authentication and route protection
- Dashboard with live totals from real transaction data
- Transactions CRUD
- Categories CRUD
- Goals CRUD + contributions
- Analytics (monthly income/expense/net + expense breakdown)
- Settings (currency, dashboard preferences, budget limit)

## 5) Routes

- `/` Dashboard
- `/login`, `/register`, `/logout`
- `/transactions`
- `/add-transaction`
- `/edit-transaction/{transactionId}`
- `/goals`
- `/categories`
- `/analytics`
- `/settings`

## 6) Setup Instructions

### Prerequisites
- .NET SDK 10

### Run locally
bash
cd FinanceTrackerApp
dotnet restore
dotnet build
dotnet run


The app starts on the URL shown in terminal (usually `https://localhost:xxxx`).

## 7) Configuration

In `FinanceTrackerApp/appsettings.json` and `FinanceTrackerApp/appsettings.Development.json`:

json
"Firebase": {
  "ApiKey": "...",
  "AuthDomain": "...",
  "ProjectId": "...",
  "StorageBucket": "...",
  "MessagingSenderId": "...",
  "AppId": "...",
  "DatabaseUrl": "",
  "AuthToken": "",
  "AllowDemoLogin": false
}


Notes:
- `AllowDemoLogin` should stay `false` for submission/production.
- `DatabaseUrl` is optional for local-first mode.

## 8) Testing

### Run tests
bash
dotnet test .\FinanceTrackerApp.Tests\FinanceTrackerApp.Tests.csproj


Current automated coverage includes:
- `TransactionService` dashboard summary calculations

## 9) Submission Checklist

- [x] Login/Register + protected routes
- [x] Dashboard connected to real data
- [x] Transactions CRUD (persistent)
- [x] Goals feature working
- [x] Categories CRUD
- [x] Analytics page with real data
- [x] Settings page working
- [x] Test project added and passing
- [x] Documentation (this README)

## 10) Known Limitations / Next Improvements

- Add more unit tests for `CategoryService`, `LocalGoalsStore`, and auth flows
- Add bUnit UI tests for major pages
- Add export/report features
- Add CI pipeline for build + test checks
