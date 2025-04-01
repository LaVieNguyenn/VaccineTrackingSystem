# Child Vaccination Schedule Tracking System

## Overview
This project is a **Child Vaccination Schedule Tracking System** built using **ASP.NET Core MVC** with a three-tier architecture: UI, Business Logic (BLL), and Data Access (DAL). It is designed to manage and track vaccination schedules for children at a vaccination center, ensuring that every child receives the appropriate vaccines at the right time.

## Features
- **Guest:**
  - Browse information about the vaccination center, vaccination services, pricing, and vaccination guides.
  - View public information without signing in.
  - Sign up to become a Customer.
- **Customer:**
  - Register an account and add child(ren)'s profiles.
  - View recommended vaccination schedules for their child(ren) based on age.
  - Schedule vaccination appointments for individual vaccines or packages.
  - Receive notifications and reminders about upcoming vaccinations.
  - Record vaccination reactions (if any) and provide feedback on services.
  - Manage payment and cancellation policies for appointments.
- **Staff:**
  - Record vaccination results and update vaccination records during appointments.
  - Manage the daily vaccination schedule at the center.
- **Admin:**
  - Manage users, services, vaccination schedules, and center information.
  - Configure system settings, view dashboards, and generate reports.
  - Oversee all transactions including payments and feedback.

## Architecture
The project follows a layered architecture:

- **UI Layer:**  
  Implements ASP.NET Core MVC Views and Controllers for the presentation layer.

- **Business Logic Layer (BLL):**  
  Contains services that encapsulate the business rules and logic. It communicates with the Data Access Layer (DAL) via repositories.

- **Data Access Layer (DAL):**  
  Uses Data Access Objects (DAO) with Dapper to communicate with Microsoft SQL Server. This layer implements a Generic Repository pattern where applicable to promote code reuse and maintainability.

## Database Design
The system utilizes Microsoft SQL Server with the following key tables:

- **Users:**  
  Stores account details for all roles (Guest, Customer, Staff, Admin).

- **Children:**  
  Contains profiles of children linked to Customer accounts (one-to-many relationship with Users).

- **Vaccines:**  
  Stores information about different vaccines, including production details when necessary.

- **Appointments:**  
  Records actual vaccination appointment bookings made by customers.

- **VaccineSchedules:**  
  Maintains the recommended vaccination schedule for each child.  
  *Note: This table guides the upcoming vaccinations based on age, independent of the actual booked appointments.*

- **VaccinationRecords:**  
  Captures the results and details of vaccinations performed (including adverse reactions).

- **Payments:**  
  Manages payment transactions and statuses for each appointment.

- **Feedback:**  
  Collects ratings and comments provided by customers after vaccination services.

- **Services:**  
  Contains details about the vaccination services offered (e.g., single vaccination, package vaccination, booster, etc.) along with status indicators.

- **Notifications:**  
  Handles sending reminders and notifications to customers about upcoming vaccinations.

- **CenterInfo:**  
  Stores information about the vaccination center, including contact details, descriptions, and service guides.

- **Roles:**  
  Contains predefined roles to manage permissions (Guest, Customer, Staff, Admin).

## Installation and Setup
1. **Clone the Repository:**
   ```bash
   git clone <repository-url>
   ```
2. **Database Setup:**
   - Create a new database in Microsoft SQL Server.
   - Execute the provided SQL scripts to create tables and seed initial data.

3. **Configuration:**
   - Update `appsettings.json` with your connection string:
     ```json
     "ConnectionStrings": {
         "DefaultConnection": "Server=YOUR_SERVER;Database=YourDatabase;User Id=YOUR_USER;Password=YOUR_PASSWORD;"
     }
     ```

4. **Run the Application:**
   - Open the solution in Visual Studio.
   - Build and run the application.
   - The default startup page will load.

## Usage
- **Customer Registration:**  
  Customers can register a new account and input their child’s details through a dedicated registration form.

- **Login:**  
  Users can log in using their credentials (for instance, using phone number and password).

- **Appointment Management:**  
  Customers can view the recommended vaccination schedule, book appointments, and manage payments for their bookings.

- **Administrative Functions:**  
  Admins and Staff have additional dashboards and management tools to oversee operations at the vaccination center.

## Technologies Used
- **ASP.NET Core MVC**
- **Dapper** for data access
- **Microsoft SQL Server** for database management
- **Bootstrap** for responsive UI design
- **C#**

## Contributing
Contributions are welcome! Feel free to fork the repository and submit pull requests for new features, bug fixes, or improvements.

## License
This project is licensed under the **MIT License**.

## Contact
For any questions or support, please contact [your.email@example.com].

---
