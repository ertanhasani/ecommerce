This repository contains an E-commerce web application built with .Net Core and MVC architecture. The application allows users to browse, search, and purchase products online. It also includes an admin panel where administrators can manage products, orders, and users.

Prerequisites
Before running the application, you will need to have the following tools and technologies installed on your system:

.Net Core SDK
Visual Studio Code or any other IDE of your choice
SQL Server Express or another relational database management system
Installation
To install and run the application, follow these steps:

Clone the repository to your local machine using the following command:

bash
Copy code
git clone https://github.com/mrbanad/E-commerce-With-.Net-Core-MVC.git
Open the project in your preferred IDE.

In the appsettings.json file, modify the connection string to match your database configuration. By default, the application uses SQL Server Express and the connection string is as follows:

swift
Copy code
"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EcommerceDb;Trusted_Connection=True;MultipleActiveResultSets=true"
Open the Package Manager Console and run the following commands to create the database and apply the initial migration:

sql
Copy code
Add-Migration InitialCreate
Update-Database
Run the application by pressing F5 or using the command dotnet run in the terminal.

Open a web browser and go to https://localhost:5001 to view the application.

Usage
The application has two main sections: the storefront and the admin panel.

Storefront
The storefront allows users to browse and purchase products online. Users can:

Browse products by category or search for a specific product using the search bar.
View product details such as price, description, and images.
Add products to their cart and proceed to checkout.
View their order history and track the status of their orders.
Admin Panel
The admin panel is accessible only to users with administrator privileges. Administrators can:

Add, edit, and delete products and categories.
View and manage orders placed by users.
Manage user accounts and roles.
Technologies Used
The application was built using the following technologies:

.Net Core 3.1
Entity Framework Core 3.1
ASP.Net Core MVC
SQL Server Express
HTML, CSS, and JavaScript
Bootstrap 4
Contributions
Contributions to this project are welcome. To contribute, follow these steps:

Fork the repository.
Create a new branch for your feature or bug fix.
Make your changes and commit them to your branch.
Push your changes to your forked repository.
Submit a pull request to the original repository.
License
This project is licensed under the MIT License.

**ENJOY!**
> **Feel free to add any feature that you think it would be nice!**

Admin User: Admin@Admin.com
Customer User: Customer@Customer.com
Password: Password1
