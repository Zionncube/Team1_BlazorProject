## Team Project Options
- **Project Name/Idea:** Finance Tracker App
- **Target Audience:** Individuals (not a business app)
- **Main Features:** Dashboard, login/register, analytics, CRUD operations, user privacy, professional documentation
- **Page/Route List:** Dashboard, Login/Register, Analytics, (other pages and subpages to be decided as we go)
- **Data Models/Schemas:**
	- User: userId, email, passwordHash, createdAt
	- Transaction: transactionId, userId, amount, category, date, description, type (income/expense)
	- Category: categoryId, userId, name, color
	- (More models can be added as features expand)
- **Component Library (e.g., MudBlazor, Bootstrap):** Use provided components
- **Fonts & Typography:** Any
- **Color Scheme & Branding:** Glassmorphism style, light green as primary color
- **Icon Set/Image Style:** Images under glassmorphism style, maybe icons as needed
- **State Management Approach:** Use built-in Blazor state for simple data; consider Fluxor or Mediator for complex state management.
- **API/Backend Structure:** Use ASP.NET Core Web API for business logic and data access, integrated with Firebase for storage.
- **Database Technology:** Google Firebase
- **Authentication & Security:** Authentication required, privacy enforced (userId for user info)
- **Accessibility & Localization:** Follow WCAG guidelines for accessibility; plan for future localization support.
- **Testing Strategy:** Use xUnit for backend tests, bUnit for Blazor component tests, and manual UI testing for user flows.
- **Error Handling & Logging:** Implement global error handling in Blazor; use Serilog or similar for logging errors and important events.
- **Deployment/Hosting:** Great will handle hosting and deployment
- **Documentation Standards:** Everything must be documented professionally in the README
## Project Ideas & Decisions

*************************************************************************************


## Team Lead Questions, Explanations, and Examples (Advanced To-Do App)
## Project Ideas & Decisions

- What do we need to set up for collaboration?  
### Recommended Trello Board Lists
 - Backlog (ideas, future tasks)
 - To Do (tasks ready to start)
 - In Progress (tasks being worked on)
 - Review (tasks needing team review/testing)
 - Done (completed tasks)
 - Blocked (tasks with issues or dependencies)
	_Explanation:_ Setting up shared tools ensures everyone can contribute and track progress.

## Potential Trello features

User authentication and registration (login, signup, password reset)
Dashboard with personalized widgets and analytics
CRUD operations for core entities (tasks, products, posts, etc.)
Real-time notifications and messaging
Role-based access control (admin, user, guest)
Responsive design for mobile and desktop
Search, filter, and sort functionality
Data visualization (charts, graphs)
Integration with external APIs (weather, payments, etc.)
File upload and management
User profile management and settings
Activity log and audit trail
Multi-language support (localization)
Dark/light theme toggle
Error handling and logging
Automated testing and CI/CD deployment

2. Project Vision & Requirements
- What kind of application are we building and who is it for?  
	_Explanation:_ Defining the app and its audience helps us focus on the right features.  
	_Example:_ "We're building an advanced to-do app for busy students and professionals who need to organize tasks efficiently."
- What are the main features we want?  
	_Explanation:_ Listing features clarifies the project scope.  
	_Example:_ "Task creation, due dates, priority levels, recurring tasks, notifications, and calendar integration."

3. Blazor & .NET Basics
- Does everyone understand what Blazor is and how CRUD works?  
	_Explanation:_ Blazor lets us build interactive web UIs with C#, and CRUD means Create, Read, Update, Delete—basic data management.  
	_Example:_ "A Blazor page like Tasks.razor lets users add (Create), view (Read), edit (Update), or delete (Delete) tasks."
- What is user authentication and why do we need it?  
	_Explanation:_ Authentication verifies user identity so each user has their own data.  
	_Example:_ "Users must log in to see and manage their personal to-do lists."

4. Application Structure
- What pages/routes will our app have?  
	_Explanation:_ Each page in Blazor is a .razor file with a route (URL path).  
	_Example:_ "/tasks, /categories, /profile, /login"
- What data models/classes (schema) do we need?  
	_Explanation:_ A schema defines the structure of our data (like a blueprint for a database table or a C# class).  
	_Example:_ "A Task class with Title, Description, DueDate, Priority, CategoryId."
- Who will work on which part?  
	_Explanation:_ Assigning roles ensures everyone knows what to work on.  
	_Example:_ "Alice: frontend (UI), Bob: backend (API), Carol: authentication, Dave: notifications."

5. Design & User Experience
- What should our color scheme and branding look like?  
	_Explanation:_ Consistent colors and fonts make the app look professional.  
	_Example:_ "Use teal and white with Open Sans font for a clean, modern look."
- What fonts and typography will we use?  
	_Explanation:_ Fonts affect readability and the overall feel of the app.  
	_Example:_ "Use Open Sans for body text and Montserrat for headings."
- What icon set or image style will we use?  
	_Explanation:_ Icons and images should be consistent and match the app's style.  
	_Example:_ "Use Material Icons for all action buttons."
- What component library will we use (if any)?  
	_Explanation:_ Component libraries provide ready-made UI elements and speed up development.  
	_Example:_ "Use MudBlazor for modern, material-style components."
- How will we ensure accessibility and responsive design?  
	_Explanation:_ Accessibility means users with disabilities can use our app; responsive design means it works on all devices.  
	_Example:_ "Use high-contrast colors, keyboard navigation, and test the layout on phone, tablet, and desktop."

6. Architecture & Technology
- What state management approach will we use?  
	_Explanation:_ State management controls how data flows and is updated in the app.  
	_Example:_ "Use built-in Blazor state for simple data, and Fluxor for complex state."
- What API/backend structure will we use?  
	_Explanation:_ The backend provides data and business logic to the app.  
	_Example:_ "Use a RESTful Web API built with ASP.NET Core."
- What database technology will we use?  
	_Explanation:_ The database stores our app's data.  
	_Example:_ "Use SQL Server for production, SQLite for local development."
- How will we handle authentication and security?  
	_Explanation:_ Security protects user data and app integrity.  
	_Example:_ "Use ASP.NET Identity for authentication and role-based access."
- How will we handle error handling and logging?  
	_Explanation:_ Good error handling and logging help us find and fix issues quickly.  
	_Example:_ "Use Serilog for logging errors and important events."

7. Development Process
- How will we test our app for quality?  
	_Explanation:_ Testing ensures our app works as expected and is bug-free.  
	_Example:_ "Write unit tests for the Task model and integration tests for task creation."
- How often will we meet or check in?  
	_Explanation:_ Regular check-ins keep everyone on track.  
	_Example:_ "Weekly Zoom call every Monday at 7pm to review progress."
- How will we document our code and write user guides?  
	_Explanation:_ Documentation helps others understand and use our code.  
	_Example:_ "Add comments to code and write a README with setup instructions."

8. Deployment & Maintenance
- Where will we deploy our app and who will handle it?  
	_Explanation:_ Deployment puts our app online for users, and one person should lead deployment to avoid confusion.  
	_Example:_ "Deploy the to-do app to Azure App Service. Bob will handle deployment and monitor uptime."
- What are our documentation standards?  
	_Explanation:_ Clear documentation helps new team members and users understand the app.  
	_Example:_ "Follow Markdown style for docs and keep a 'docs' folder in the repo."

## Dashboard Card Ideas
- **Balance Overview Card:** Shows current total balance (income minus expenses). Tapping can link to a detailed balance/summary page.
- **Recent Transactions Card:** Displays a list of the latest transactions. Tapping can link to the full transactions page.
- **Income Card:** Shows total income for the month. Tapping can link to an income breakdown page.
- **Expenses Card:** Shows total expenses for the month. Tapping can link to an expenses breakdown page.
- **Category Spending Card:** Visualizes spending by category (e.g., Food, Transport). Tapping can link to analytics or category details.
- **Budget Status Card:** Shows how much of the monthly budget is used. Tapping can link to the budget management page.
- **Upcoming Bills Card:** Lists upcoming bills or due payments. Tapping can link to a bills/reminders page.
- **Tips/Insights Card:** Shows financial tips or insights based on user data. No link needed.

## Testing Bank Integration with Sandbox APIs

To simulate real bank integration without using actual bank accounts, we can use sandbox environments from financial API providers:

### 1. Plaid Sandbox
- Sign up for a free developer account at https://dashboard.plaid.com/signup
- Use the Plaid Sandbox environment to test linking accounts and importing transactions with dummy data
- Follow Plaid's documentation: https://plaid.com/docs/sandbox/
- Use provided test credentials and institutions to simulate the full user flow

### 2. Yodlee Sandbox
- Register for a Yodlee developer account at https://developer.yodlee.com/
- Access the Yodlee Sandbox to test account linking and data import with simulated financial data
- See Yodlee's Quickstart: https://developer.yodlee.com/docs/api/1.1/Quickstart_Guide
- Use test users and accounts provided by Yodlee for development

To use sandbox bank integrations like Plaid or Yodlee, you will need to write some code and follow their setup guides. Here’s what’s typically involved:

1. Sign up for a developer account (Plaid or Yodlee).
2. Get API keys from their dashboard.
3. Follow their documentation to install SDKs or use their REST APIs in your app.
4. Add code to your app to connect to the sandbox, display the  bank-link UI, and fetch dummy data.
5. Use their test credentials to simulate linking accounts and importing transactions.

These sandboxes allow you to build and test your app’s bank integration features safely and for free, using fake accounts and transactions.



======================================================================================================
During the meeting, must ask these questions in order

What is DONE?

Go through the board and mark completed tasks.

Question:

“What is fully working and tested?”



You want finished, not in-progress.

What is IN PROGRESS?

For each teammate:

“What exactly are you finishing this week?”

Then ask:

“What’s your estimated completion day?”

Example:

Login page → Wednesday
Firebase connection → Thursday
UI polish → Friday



(Deadlines make work real.)

 What is BLOCKING anyone?

Question:
“Is anything stopping you from finishing?”

Common blockers:

Confusion about code
Merge conflicts
Missing features
Time issues

my job:
Help remove blockers or reassign help.


Create a clear final checklist

After the meeting, summarize:

What must be done before submission:

 Login + signup pages complete
 Goals feature working
 Transactions working
 Firebase connected
 UI cleaned
 Final testing
 Deployment/demo ready

Then assign ownership:

Person A → Authentication
Person B → Database/Firebase
Person C → UI polish
You → Integration + testing

Introduce mini deadlines


“Core features done by Thursday”
“Testing + polish on Friday”




Daily 5-minute check-ins

Send a short daily message:

Quick daily check-in:

 What did you finish today?
 What are you working on next?
 Any blockers?

Let’s keep momentum — we’re close 





What to do if someone is behind

Say this:

Hey — I noticed your task is still in progress and we’re getting close to the deadline. Do you need help finishing it? We can split it or pair program if needed.



leadership script for the week


Make work visible
Assign ownership
Set mini deadlines
Check progress daily
Help unblock people



This week priority checklist**
1. Must-fix now (highest impact)
- Wire Firebase config (`DatabaseUrl`, `AuthToken`) and verify goals persist.
- Make transactions true CRUD (add/edit/delete + persistence), not in-memory only.
- Connect dashboard values to real transaction data.

2. Core feature completion
- Finish `Analytics` page with actual charts/data (`FinanceTrackerApp/Components/Pages/Analytics.razor:5` is placeholder).
- Finish `Categories` management CRUD (`FinanceTrackerApp/Components/Pages/Categories.razor:5`).
- Finish `Settings` with real account/preferences controls (`FinanceTrackerApp/Components/Pages/Settings.razor:5`).

3. Quality + submission readiness
- Add at least one test project (none exists yet).
- Replace demo auth model (plain password/in-memory) with secure auth path.
- Final README with setup, architecture, routes, and demo flow.
